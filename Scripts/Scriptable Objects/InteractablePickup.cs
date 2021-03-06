﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.egamesstudios.cell
{
    public class InteractablePickup : MonoBehaviour
    {
        [SerializeField]
        private Pickup itemStats;
        private readonly static float CELL_DISTANCE = 2.25f; //1.5^2

        void Start()
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = itemStats.image;
        }

        private void Update()
        {
            Vector3 heading = VariableContainer.variableContainer.currentActive.transform.position - transform.position;
            if (heading.sqrMagnitude <= CELL_DISTANCE)
            {
                transform.position += (heading.normalized * Time.deltaTime * 5);
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag.Equals("Cell"))
            {
                switch (itemStats.type)
                {
                    case PICKUPS.HEALTH:
                        collision.gameObject.GetComponent<CellController>().RecoverHealth(itemStats.value);
                        GetComponent<SFXPlayer>().PlaySFX(0, 0.4f, 1.85f);
                        break;
                    case PICKUPS.ENERGY:
                        collision.gameObject.GetComponent<CellController>().RecoverEnergy(itemStats.value);
                        break;
                    case PICKUPS.MONEY:
                        VariableContainer.variableContainer.mainCell.UpdateMoney(itemStats.value);
                        break;
                }
                //playSFX
                //playParticle?
                Destroy(gameObject);
            }
        }
    }
}

