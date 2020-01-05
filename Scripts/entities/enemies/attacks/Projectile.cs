using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.egamesstudios.cell
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField]
        private float speed;
        protected int damage;

        private bool destroy;
        private Rigidbody2D rb2d;

        private float travelled, travelDistance;

        private void Awake()
        {
            rb2d = GetComponent<Rigidbody2D>();
            rb2d.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

            if (travelDistance <= 0 || travelDistance > 60)
            {
                travelDistance = 60;
            }
        }

        public void Shoot(AEnemyAI enemySource, Vector2 angle, int damage)
        {
            rb2d.velocity = angle * speed;
            this.damage = damage;
        }

        private void FixedUpdate()
        {
            if (!destroy)
            {
                travelled += speed * Time.fixedDeltaTime;
                if (travelled > travelDistance || travelled < -travelDistance)
                {
                    destroy = true;
                }
            }
            else
            {
                    Destroy(gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.GetComponent<CellController>() != null)
            {
                collision.gameObject.GetComponent<CellController>().TakeHit(transform, damage, DamageType.ENEMY);
                destroy = true;
            }
            if(collision.gameObject.layer == 8)
            {
                destroy = true;
            }
        }
    }
}
