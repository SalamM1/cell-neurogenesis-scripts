using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.egamesstudios.cell
{
    public class VariableContainer : MonoBehaviour
    {

        public static VariableContainer variableContainer;

        public List<CellController> cells;
        public CellController mainCell;
        public CellController currentActive;

        void Awake()
        {
            if (variableContainer == null)
                variableContainer = this;
            else if (variableContainer != this)
                Destroy(gameObject);
            DontDestroyOnLoad(gameObject);

            cells = new List<CellController>();
            cells.AddRange(FindObjectsOfType<CellController>());
            foreach (CellController c in cells)
            {
                if (!c.vars.isClone)
                {
                    mainCell = c;
                }
            }
            currentActive = mainCell;
        }
    }
}

