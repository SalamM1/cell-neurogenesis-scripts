using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.egamesstudios.cell
{
    public class ElementalBullet : Bullet
    {
        void Awake()
        {
            wallMask = LayerMask.GetMask("Platform", "MovingPlatform");
            generalMask = LayerMask.GetMask("Enemy", "Platform", "MovingPlatform");
            //Initialize Data here
            speed = 19.42f;
            damageMultiplier = 0;
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
        }
    }
}