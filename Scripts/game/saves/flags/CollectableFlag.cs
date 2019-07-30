using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.egamesstudios.cell
{
    public class CollectableFlag : MonoBehaviour
    {

        [SerializeField]
        private int ID;

        [SerializeField]
        private CollectableType type;

        // Use this for initialization
        void Start()
        {
            if((SaveManager.saveManager.activeGame.powerups[type])[ID])
            {
                gameObject.SetActive(false);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag.Equals("Cell"))
            {
                if(!collision.GetComponent<CellController>().vars.isClone)
                {   
                    if(type == CollectableType.WEAPON)
                    {
                        collision.GetComponent<CellController>().vars.hasGuitar = true;
                        gameObject.SetActive(false);
                    }
                    else
                    {
                        collision.GetComponent<CellController>().PlayAnimation(CellAnimation.PICKUP);
                        PlayPickupAnimation();
                        (SaveManager.saveManager.activeGame.powerups[type])[ID] = true;
                        gameObject.SetActive(false);
                    }

                }

            }
          
        }

        private void PlayPickupAnimation()
        {
         
        }
    }
}

