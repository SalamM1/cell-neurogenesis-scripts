using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace com.egamesstudios.cell
{
    public enum GunType
    {
        NORMAL,
        ICE,
        ACID,
        PRESSURE,
    }

	public class GunController : MonoBehaviour 
	{
        //Equipped bullet from CellVars; saved because it is rare item
        public Transform gun_centre;
        public float shotRange;

        [FoldoutGroup("Bullet Type")]
        [InlineEditor]
        public GameObject basicBullet;
        [FoldoutGroup("Bullet Type")]
        [InlineEditor]
        public GameObject iceBeam;
        [FoldoutGroup("Bullet Type")]
        [InlineEditor]
        public GameObject pressureWave;
        [FoldoutGroup("Bullet Type")]
        [InlineEditor]
        public GameObject acidShot;

        [SerializeField]
        private readonly float CHARGE_MAX = 1.0f;

        private float chargeValue;
        private float chargeMultiplier;

        [SerializeField]
        private GameObject gunImageObj;

        private GameObject bullet;
        private Vector2 direction;
        private CellController cell;

        void Start () 
		{
            cell = GetComponentInParent<CellController>();
        }

        private void Update()
        {
           
            if (cell.vars.activeState == State.CONTROL)
            {
                if(cell.player.GetButton("Gun") && cell.vars.equippedBullet == BulletType.CHARGE && cell.vars.equippedGun == GunType.NORMAL)
                {
                    chargeValue = Mathf.Min(chargeValue + Time.deltaTime, CHARGE_MAX);
                    chargeMultiplier = (chargeValue / (CHARGE_MAX));
                    gunImageObj.transform.localScale = new Vector3(chargeMultiplier * 0.2f + 1, chargeMultiplier * 0.2f + 1, 1);
                    // TODO: Add color and particle updater, add sounds
                }

                if (cell.player.GetButtonUp("Gun"))
                {
                    chargeMultiplier++;
                    cell.UpdateAnimations("attack");
                    cell.UpdateAnimations("shoot");

                    Shoot();

                    chargeValue = 0;
                    chargeMultiplier = 0;
                    gunImageObj.transform.localScale = new Vector3(1, 1, 1);
                }
            }
        }

        void LateUpdate () 
		{
            float rotation = 0;
            direction = cell.vars.faceLeft ? Vector2.left : Vector2.right;

            if(cell.vars.aimUp)
            {
                rotation = -90;
                direction = Vector2.up;
            }
            else if(cell.vars.aimDown)
            {
                rotation = 90;
                direction = Vector2.down;
            }

            transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotation * (cell.vars.faceRight ? -1:1)));
        }

        public void Shoot()
        {
            var cooldown = FindObjectOfType<Bullet>();
            
            if (cooldown == null)
            {
                if(GetCost() <= cell.vars.mainEnergy)
                {
                    switch (cell.vars.equippedGun)
                    {
                        case GunType.NORMAL:
                            bullet = Instantiate(basicBullet, gun_centre.position, transform.rotation);
                            break;
                        case GunType.ACID:
                            bullet = Instantiate(acidShot, gun_centre.position, transform.rotation);
                            break;
                        case GunType.PRESSURE:
                            bullet = Instantiate(pressureWave, gun_centre.position, transform.rotation);
                            break;
                        case GunType.ICE:
                            bullet = Instantiate(iceBeam, gun_centre.position, transform.rotation);
                            break;
                    }
                    bullet.GetComponent<Bullet>().Shoot(cell, direction, shotRange, chargeMultiplier);
                    if(gunImageObj.transform.localScale.x > 1.15f) bullet.GetComponent<Bullet>().transform.localScale = gunImageObj.transform.localScale;
                    cell.vars.mainEnergy -= GetCost();
                }
            }
            
           
        }


        int GetCost()
        {
            int cost = 10;
            if (cell.vars.equippedGun == GunType.NORMAL)
            {
                switch (cell.vars.equippedBullet)
                {
                    case BulletType.NORMAL:
                        cost = 5;
                        break;
                    case BulletType.CHARGE:
                        cost = 10;
                        break;
                    case BulletType.CONE:
                    case BulletType.GHOST:
                        cost = 15;
                        break;
                }
            }
            else
            {
                cost = 25;
            }
            return cost;
        }
    }
}
