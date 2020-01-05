using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.egamesstudios.cell
{
    public class PressureWave : ElementalBullet
    {
        public override void Shoot(CellController cell, Vector2 direction, float shotLength, float chargeRate)
        {
            base.Shoot(cell, direction, shotLength, chargeRate);
            RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, direction, shotLength, wallMask);
            if (hitInfo) travelDistance = hitInfo.distance;
            hasShot = true;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            switch (collision.tag)
            {
                case "Enemy":
                    if (!hitObjects.Contains(collision.gameObject))
                    {
                        collision.gameObject.GetComponent<AEnemyAI>().Knockback(transform, 4);
                        hitObjects.Add(collision.gameObject);
                    }
                    break;
                case "Switch":
                    if (!hitObjects.Contains(collision.gameObject))
                    {
                        collision.gameObject.GetComponent<ASwitch>().TriggerSwitch(SwitchType.HITABLE, HitableSwitchType.GUN);
                    }
                    break;
                case "Block":
                    //Do the block stuff
                    break;
            }
        }
    }
}
