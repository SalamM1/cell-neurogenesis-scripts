using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.egamesstudios.cell
{
    public class ParasiteHole : MonoBehaviour
    {
        [SerializeField]
        private ParasiteAI connectedParasite;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.Equals(connectedParasite.gameObject))
            {
                Destroy(connectedParasite.gameObject);
            }
    }
    }

}