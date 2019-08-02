using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace com.egamesstudios.cell
{
	public class Waypoint : MonoBehaviour 
	{
        private void OnTriggerEnter2D(Collider2D collision)
        {
            CellController cell = null;
            if ((cell = collision.gameObject.GetComponent<CellController>()) != null)
            {
                cell.SetCheckpoint(transform.position);
            }
        }
    }
}