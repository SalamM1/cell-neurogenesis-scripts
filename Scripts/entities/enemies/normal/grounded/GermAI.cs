using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.egamesstudios.cell
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class GermAI : AEnemyAI {
        [SerializeField]
        private float jumpForce;

        protected override void WhileIdle()
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            if (CellDetectionDistance() <= detectionRange)
            {
                if(CastRayToCell())
                {
                    ChangeState(EnemyState.AWARE);
                }
            }
        }

        protected override void WhileAware()
        {
            timeToBeAware -= Time.deltaTime;

            if (timeToBeAware <= 0)
            {
                UpdateAnimations("chase");
                if (CellDetectionDistance() > detectionRange)
                {
                    ChangeState(EnemyState.IDLE);
                }
                else if (transform.position.x <= VariableContainer.variableContainer.currentActive.transform.position.x)
                {
                    rb.velocity = new Vector2(speed*awareSpeedFactor, rb.velocity.y);
                }
                else
                {
                    rb.velocity = new Vector2(-speed * awareSpeedFactor, rb.velocity.y);
                }
            }

        }

        protected override void WhileDead()
        {
            ParticleManager.particleManager.PlayParticle(FXType.ENEMY, 0, transform.position);
            Destroy(gameObject);
        }

        protected override void ExitState(EnemyState oldState)
        {
            switch (oldState)
            {
                case EnemyState.IDLE:
                    break;
                case EnemyState.AWARE:
                    rb.velocity = new Vector2(0, rb.velocity.y);
                    timeToBeAware = TIME_TO_BE_AWARE;
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

