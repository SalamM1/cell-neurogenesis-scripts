using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace com.egamesstudios.cell
{
    public class HUDCloneIndicator : MonoBehaviour
    {
        public CellController clone;

        public Image glow;
        public Image indicator;

        public void UpdateClone(CellController clone)
        {
            this.clone = clone;
        }

        public void SetActive(bool active)
        {
            indicator.GetComponent<ScalingObject>().enabled = active;
            indicator.GetComponent<ScalingObject>().SetLowerBound();
            glow.gameObject.SetActive(active);
            
        }
    }
}
