﻿using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.egamesstudios.cell
{
    public class FlyingAI : AEnemyAI
    {
        protected float scaling;
        protected Vector2 cellDirection, randomMoveSpeed;
        protected bool hoverRangeScaled;

        protected override void OnStart()
        {
            rb.gravityScale = 0;
            scaling = 1;
        }

        protected override void WhileIdle()
        {
            rb.velocity = Vector2.zero;
            if (CellDetectionDistance() <= enemyData.detectionRange)
            {
                if (CastRayToCell())
                {
                    ChangeState(EnemyState.AWARE);
                }
            }
        }

        protected override void WhileAware()
        {
            if (timeToBeAware <= 0)
            {
                if(timeBetweenAttacks <= 0)
                {
                    timeBetweenAttacks = enemyData.TIME_BETWEEN_ATTACKS;
                    if (CellDistance() <= ((FlyingData)enemyData).hoverDistance * scaling && CellDistance() >= ((FlyingData)enemyData).hoverDistance * scaling * 0.5f && Random.Range(0.0f, 1.0f) >= 0.6f)
                    {
                        attackID = 0;
                    }
                    else
                    {
                        attackID = 1;
                    }
                    ChangeState(EnemyState.ACTION);
                }
                else
                {
                    timeBetweenAttacks -= Time.deltaTime;
                    if (CastRayToCell())
                    {
                        if (CellDistance() <= ((FlyingData)enemyData).hoverDistance * scaling && CellDistance() >= ((FlyingData)enemyData).hoverDistance * scaling * 0.5f)
                        {
                            if (!hoverRangeScaled)
                            {
                                scaling = ((FlyingData)enemyData).hoverScaling;
                                hoverRangeScaled = true;
                            }
                            if (randomMoveTimer <= 0)
                            {
                                randomMoveSpeed = (new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f))) * enemyData.speed * 0.1f;
                                randomMoveTimer = enemyData.RANDOM_MOVE_TIMER;
                            }
                            else
                            {
                                randomMoveTimer -= Time.deltaTime;
                            }
                            rb.velocity = randomMoveSpeed;
                        }
                        else
                        {
                            if (hoverRangeScaled)
                            {
                                scaling = 1;
                                hoverRangeScaled = false;
                            }
                            cellDirection = (GetHoverPoint(true, true) - transform.position).normalized;
                            rb.velocity = cellDirection * enemyData.speed;
                        }
                    }
                    else
                    {
                        if (timeToLoseChase <= 0)
                        {
                            ChangeState(EnemyState.IDLE);
                        }
                        else
                        {
                            rb.velocity = cellDirection * enemyData.speed;
                            timeToLoseChase -= Time.deltaTime;
                        }
                    }
                }      
            }
            else
            {
                timeToBeAware -= Time.deltaTime;
            }
        }

        protected override void ExitState(EnemyState oldState)
        {
            switch (oldState)
            {
                case EnemyState.IDLE:
                    break;
                case EnemyState.AWARE:
                    timeToBeAware = enemyData.TIME_TO_BE_AWARE;
                    timeToLoseChase = enemyData.TIME_TO_LOSE_CHASE;
                    rb.velocity = Vector2.zero;
                    animator.ResetTrigger("chase");
                    break;
                case EnemyState.ACTION:
                    castingAttack = false;
                    timeToAttack = enemyData.TIME_TO_ATTACK;
                    rb.velocity = Vector2.zero;
                    animator.ResetTrigger("attackFinish");
                    break;
                case EnemyState.FROZEN:
                    break;
                case EnemyState.STUNNED:
                    break;
                case EnemyState.DEAD:
                    break;
            }
        }

        protected Vector3 GetHoverPoint(bool closest, bool right)
        {
            Vector3 direction = new Vector3(-0.5f, 0.866f, 0);
            if (right) direction = new Vector3(0.5f, 0.866f, 0);
            if(closest)
            {
                direction = (VariableContainer.variableContainer.currentActive.transform.position.x - transform.position.x > 0 ? new Vector3(-0.5f, 0.866f, 0) : new Vector3(0.5f, 0.866f, 0));
            }
            RaycastHit2D hitInfo = Physics2D.Raycast(VariableContainer.variableContainer.currentActive.transform.position, direction, ((FlyingData)enemyData).hoverDistance * scaling, LayerMask.GetMask("Platform", "MovingPlatform"));
            if (hitInfo) return hitInfo.point;
            return VariableContainer.variableContainer.currentActive.transform.position + (direction * ((FlyingData)enemyData).hoverDistance * scaling);
        }

        protected override void OnCollisionEnter2D(Collision2D collision)
        {
            base.OnCollisionEnter2D(collision);
            if ((collision.gameObject.layer == 8 || collision.gameObject.layer == 12) && castingAttack) ChangeState(EnemyState.IDLE);
        }
    }
}
