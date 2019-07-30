using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.egamesstudios.cell
{
    public class CloneGate : MonoBehaviour
    {
        [SerializeField]
        Transform locationA, locationB;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.tag.Equals("Cell"))
            {
                collision.GetComponent<CellController>().vars.isOnGate = true;
                collision.GetComponent<CellController>().vars.gateLocation =
                    Vector3.Distance(locationA.position, collision.transform.position) > Vector3.Distance(locationB.position, collision.transform.position) ?
                    locationA : locationB;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.tag.Equals("Cell"))
            {
                collision.GetComponent<CellController>().vars.isOnGate = false;
                collision.GetComponent<CellController>().vars.gateLocation = null;
            }
        }
    }
}
