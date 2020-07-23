using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.egamesstudios.cell
{
    public class VirusRH1Death : EnemyDeathFunc
    {
        [SerializeField]
        private GameObject cellToSwitchOn;

        public override void OnDeath()
        {
            Debug.Log("Death detected");
            cellToSwitchOn.SetActive(true);
        }
    }
}
