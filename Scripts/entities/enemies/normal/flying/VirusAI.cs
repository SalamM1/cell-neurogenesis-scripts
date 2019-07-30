using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.egamesstudios.cell
{
    public class VirusAI : FlyingAI
    {
        protected override void WhileDead()
        {
            ParticleManager.particleManager.PlayParticle(FXType.ENEMY, 0, transform.position);
            Destroy(gameObject);
        }

        protected override void WhileAction()
        {
            if(timeToAttack <= 0)
            {
                UpdateAnimations("attackFinish");
                //Melee
                if (attackID == 0)
                {
                    if (!castingAttack) rb.velocity = (VariableContainer.variableContainer.currentActive.transform.position - transform.position).normalized * speed * 1.75f;
                    castingAttack = true;
                }
                //Ranged
                else
                {
                    Debug.Log("pew pew");
                    EnterState(EnemyState.IDLE);
                }
            }
            else
            {
                timeToAttack -= Time.deltaTime;
                UpdateAnimations("attack" + attackID);
            }
        }
    }
}
