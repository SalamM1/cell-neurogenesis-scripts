using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.egamesstudios.cell
{
    public class VirusAI : FlyingAI
    {
        [SerializeField]
        private GameObject projectile;
        private Vector2 chargeSpeed;

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
                    if(!castingAttack) chargeSpeed = (VariableContainer.variableContainer.currentActive.transform.position - transform.position).normalized * speed * 1.75f;
                    castingAttack = true;
                    rb.velocity = chargeSpeed;
                }
                //Ranged
                else
                {
                    GetComponent<SFXPlayer>().PlaySFX(1, 0.6f, 3f);
                    var proj = Instantiate<GameObject>(projectile, transform.position, Quaternion.Euler((VariableContainer.variableContainer.currentActive.transform.position - transform.position).normalized));
                    proj.GetComponent<Projectile>().Shoot(this, (VariableContainer.variableContainer.currentActive.transform.position - transform.position).normalized, attackDamage);
                    ChangeState(EnemyState.IDLE);
                }
            }
            else
            {
                timeToAttack -= Time.deltaTime;
            }
        }
    }
}
