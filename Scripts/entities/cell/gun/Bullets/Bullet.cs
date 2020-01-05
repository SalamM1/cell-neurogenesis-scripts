using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace com.egamesstudios.cell
{
    public enum BulletType
    {
        NORMAL,
        GHOST,
        CONE,
        CHARGE,
    }

    public abstract class Bullet : MonoBehaviour
    {
        protected int cost;
        [SerializeField]
        protected float travelDistance, timeToDestroy, damageMultiplier, speed;
        protected bool hasShot, destroy;
        protected HashSet<GameObject> hitObjects;
        [SerializeField]
        protected static LayerMask wallMask;
        [SerializeField]
        protected static LayerMask generalMask;
        protected CellController cell;
            
        [SerializeField]
        protected float travelled;
        private Rigidbody2D rb2d;

        public abstract void Shoot(CellController cell, Vector2 direction, float shotLength, float chargeRate);

        private void Start()
        {
            rb2d = GetComponent<Rigidbody2D>();
            rb2d.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }

        private void FixedUpdate()
        {           
            if (hasShot && !destroy)
            {
                rb2d.velocity = transform.right * speed;
                travelled += speed * Time.fixedDeltaTime;
                if (travelled > travelDistance || travelled < -travelDistance)
                {
                    destroy = true;
                }
            }
            if (destroy)
            {
                timeToDestroy -= Time.fixedDeltaTime;
                if (timeToDestroy <= 0)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
