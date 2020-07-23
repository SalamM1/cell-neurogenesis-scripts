using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.egamesstudios.cell
{
    public class LockedEvent : MonoBehaviour
    {
        [SerializeField]
        ASwitchEvent[] events;
        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.CompareTag("Cell"))
            {
                if (VariableContainer.variableContainer.mainCell.UseKey(KeyType.NORMAL))
                {
                    for (int i = 0; i < events.Length; i++)
                    {
                        events[i].Trigger();
                        events[i].SetPermanent();
                    }
                    Destroy(gameObject);
                }
            }
        }
    }

}