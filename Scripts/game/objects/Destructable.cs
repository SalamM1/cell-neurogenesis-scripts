using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.egamesstudios.cell
{
    public class Destructable : MonoBehaviour
    {
        [SerializeField]
        HitableType damagedBy;
        [SerializeField]
        int health;
        [SerializeField, ChildGameObjectsOnly]
        private ParticleSystem shakeParticles, destructionParticles;

        public void Damage(HitableType hitType, int damage)
        {
            if (damagedBy == HitableType.BOTH || damagedBy == hitType)
            {
                health -= damage;
                if (health <= 0)
                {
                    DestroyDestructable();
                }
                else
                {
                    ShakeParticles();
                }
            }
        }

        public void DestroyDestructable()
        {
            GetComponent<SpriteRenderer>().enabled = GetComponent<BoxCollider2D>().enabled = false;
            destructionParticles.Play();
            Destroy(gameObject, 1);
        }

        protected void ShakeParticles()
        {
            shakeParticles.Play();
            transform.DOShakePosition(0.5f, Vector3.right*0.25f, 7, 0).Play();
            GetComponent<SpriteRenderer>().DOColor(Color.magenta, 0.15f).SetLoops(2, LoopType.Yoyo).Play();
        }
    }
}
