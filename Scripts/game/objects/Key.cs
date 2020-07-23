using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.egamesstudios.cell
{
    public class Key : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Cell"))
            {
                VariableContainer.variableContainer.mainCell.AddKey(KeyType.NORMAL);
                Destroy(gameObject);
            }
        }
    }

    public enum KeyType
    {
        NORMAL
    }
}
