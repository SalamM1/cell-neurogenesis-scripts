using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.egamesstudios.cell
{
    public class HUDCloneManager : MonoBehaviour
    {
        List<CellController> clones;
        HUDCloneIndicator[] HUDClone;

        private void Start()
        {
            HUDClone = GetComponentsInChildren<HUDCloneIndicator>();
            SyncClones();
        }

        public void SwitchToClone(int index)
        {
            for (int i = 0; i < HUDClone.Length; i++)
            {
                HUDClone[i].SetActive(i == index ? true : false);
            }
        }

        public void SyncClones()
        {
            if(clones == null) clones = VariableContainer.variableContainer.cells;
            int i;

            for (i = 0; i < clones.Count; i++)
            {
                if(clones[i] != null)
                {
                    HUDClone[i].UpdateClone(clones[i]);
                    HUDClone[i].gameObject.SetActive(true);
                } 
                else
                {
                    HUDClone[i].gameObject.SetActive(false);
                }
            }
            while (i < HUDClone.Length)
            {
                HUDClone[i].gameObject.SetActive(false);
                i++;
            }
    }
    }
}
