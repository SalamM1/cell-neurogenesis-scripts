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
                        (SaveManager.saveManager.activeGame.powerups[type])[ID] = true;

                        try
                        {
                            GetComponent<Collider2D>().enabled = false;
                            GetComponent<HoveringItem>().enabled = false;
                            GetComponent<ScalingObject>().enabled = false;
                        } 
                        catch (Exception e)
                        {
                            Debug.LogWarning(e);
                        }
                        StartCoroutine(PlayPickupAnimation());
                    }
                }
            }         
        }

        private IEnumerator PlayPickupAnimation()
        {
            float theta = 90;
            transform.localScale = Vector3.one * 0.5f;
            GetComponent<SFXPlayer>().PlaySFX(0, 0.3f, 1.25f);
            while (theta >= -1350)
            {
                transform.position = new Vector3(Mathf.Cos(Mathf.Deg2Rad * theta), Mathf.Sin(Mathf.Deg2Rad * theta) - 0.15f, 0) * (1 - Mathf.Abs(theta / 1350)) 
                    + VariableContainer.variableContainer.mainCell.transform.position;
                theta -= Time.deltaTime * 890f;
                yield return new WaitForEndOfFrame();
            }
            switch (type)
            {
                case CollectableType.HEALTH:
                    VariableContainer.variableContainer.mainCell.UpgradeMaxHealth();
                    break;

                case CollectableType.ENERGY:
                    VariableContainer.variableContainer.mainCell.UpgradeMaxEnergy();
                    break;
            }
            GetComponent<SFXPlayer>().PlaySFX(1, 0.5f, 0.85f);
            Destroy(gameObject);
            yield return null;
        }
    }
}