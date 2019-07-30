using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace com.egamesstudios.cell
{

	public class Waypoint : MonoBehaviour 
	{
        public bool isAirPoint;
        public bool isLandPoint;
        public bool isCheckpoint;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            CellController cell = null;
            if ((cell = collision.gameObject.GetComponent<CellController>()) != null && isCheckpoint)
            {
                cell.SetCheckpoint(transform.position);
            }
        }
    }
}
