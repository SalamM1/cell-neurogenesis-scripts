using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.egamesstudios.cell
{
    public class InteractablePickup : MonoBehaviour
    {
        [SerializeField]
        private Pickup itemStats;


        // Use this for initialization
        void Start()
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = itemStats.image;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag.Equals("Cell"))
            {
                switch (itemStats.type)
                {
                    case PICKUPS.HEALTH:
                        collision.gameObject.GetComponent<CellController>().RecoverHealth(itemStats.value);
                        break;
                    case PICKUPS.ENERGY:
                        collision.gameObject.GetComponent<CellController>().RecoverEnergy(itemStats.value);
                        break;
                    case PICKUPS.MONEY:
                        break;
                }
                Destroy(gameObject);
            }
        }
    }
}

