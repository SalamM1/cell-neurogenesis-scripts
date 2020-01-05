using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace com.egamesstudios.cell
{

	public class BaseBullet : Bullet
	{
        void Awake()
        {
            wallMask = LayerMask.GetMask("Platform", "MovingPlatform");
            generalMask = LayerMask.GetMask("Enemy", "Platform", "MovingPlatform");
            //Initialize Data here
            speed = 19.42f;
            hitObjects = new HashSet<GameObject>();            
        }

        public override void Shoot(CellController cell, Vector2 direction, float shotLength, float chargeRate)
        {
            travelDistance = shotLength;
            this.cell = cell;
            if (cell.vars.faceLeft)
            {
                speed *= -1;
            }

            if(cell.vars.equippedBullet == BulletType.CONE)
            {
                Vector2 cone1 = Vector2.zero;
                Vector2 cone2 = Vector2.zero;
                float rotation = 0;

                if(direction == Vector2.right)
                {
                    cone1 = new Vector2(0.9659f, -0.2588f);
                    cone2 = new Vector2(0.9659f, 0.2588f);
                }
                else if (direction == Vector2.left)
                {
                    cone1 = new Vector2(-0.9659f, -0.2588f);
                    cone2 = new Vector2(-0.9659f, 0.2588f);
                }
                else if (direction == Vector2.up)
                {
                    cone1 = new Vector2(-0.2588f, 0.9659f);
                    cone2 = new Vector2(0.2588f, 0.9659f);
                    rotation = -90;
                }
                else
                {
                    cone1 = new Vector2(0.2588f, -0.9659f);
                    cone2 = new Vector2(-0.2588f, -0.9659f);
                    rotation = 90;
                }

                (Instantiate(gameObject, transform.position, transform.rotation)).GetComponent<BaseBullet>().ShootBasic(cell, direction, shotLength, 0.8f);
                (Instantiate(gameObject, transform.position, Quaternion.Euler(new Vector3(0, 0, (rotation + 15) * (cell.vars.faceRight ? -1 : 1))))).GetComponent<BaseBullet>().ShootBasic(cell, cone1, shotLength, 0.55f);
                (Instantiate(gameObject, transform.position, Quaternion.Euler(new Vector3(0, 0, (rotation - 15) * (cell.vars.faceRight ? -1 : 1))))).GetComponent<BaseBullet>().ShootBasic(cell, cone2, shotLength, 0.55f);
                Destroy(gameObject);
            }
            else
            {
                damageMultiplier = (cell.vars.equippedBullet == BulletType.GHOST ? 0.8f : 1);
                hasShot = true;
            }          
        }

        private void ShootBasic(CellController cell, Vector2 direction, float shotLength, float chargeRate)
        {
            travelDistance = shotLength;
            this.cell = cell;
            if (cell.vars.faceLeft)
            {
                speed *= -1;
            }
            damageMultiplier = chargeRate;
            hasShot = true;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.gameObject.layer == 8 || collision.gameObject.layer == 12)
            {
                destroy = true;
            }

            switch(collision.tag)
            {
                case "Enemy":
                    if (!hitObjects.Contains(collision.gameObject))
                    {
                        collision.gameObject.GetComponent<AEnemyAI>().Damage((int)(cell.vars.gunDamage * damageMultiplier), transform);
                        hitObjects.Add(collision.gameObject);
                        if (cell.vars.equippedBullet != BulletType.GHOST) destroy = true;
                    }
                    break;
                case "Switch":
                    if(!hitObjects.Contains(collision.gameObject))
                    {
                        collision.gameObject.GetComponent<ASwitch>().TriggerSwitch(SwitchType.HITABLE, HitableSwitchType.GUN);
                    }
                    break;
            }
        }

    }
}