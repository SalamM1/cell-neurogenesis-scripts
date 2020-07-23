using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.egamesstudios.cell
{
    public class ParasiteAI : AEnemyAI
    {
        private bool cellDetected;

        protected override void OnStart()
        {
            cellDetected = false;
            ChangeState(EnemyState.IDLE);
        }

        protected override void WhileAware()
        {
            if (timeToBeAware <= 0)
            {
                if(CheckWall(speed * Vector2.right, 1))
                {
                    ReverseSpeed();
                }
                rb.velocity = new Vector2(speed, rb.velocity.y);
            }
            else
            {
                timeToBeAware -= Time.deltaTime;
            }
        }

        protected override void WhileDead()
        {
            ParticleManager.particleManager.PlayParticle(FXType.ENEMY, 0, transform.position);
            Destroy(gameObject);
        }

        protected override void OnDamage()
        {
            if (!cellDetected) ChangeState(EnemyState.AWARE);
            cellDetected = true;
            ReverseSpeed();
        }

        protected override void OnCollisionWithCell()
        {
            OnDamage();
        }

        private void ReverseSpeed()
        {
            //play particle
            //play SFX
            speed *= -1;
        }

        protected override void ExitState(EnemyState oldState)
        {
            switch (oldState)
            {
                case EnemyState.IDLE:
                    rb.velocity = new Vector2(0, 4);
                    break;
                case EnemyState.AWARE:
                    rb.velocity = new Vector2(0, rb.velocity.y);
                    timeToBeAware = enemyData.TIME_TO_BE_AWARE;
                    animator.ResetTrigger("chase");
                    break;
                case EnemyState.ACTION:
                    break;
                case EnemyState.FROZEN:
                    break;
                case EnemyState.STUNNED:
                    break;
                case EnemyState.DEAD:
                    break;
            }
        }
    }
}
