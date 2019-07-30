using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.egamesstudios.cell
{
    public class SwitchFlag : EventFlag
    {
        public GameObject open, closed;

        // Use this for initialization
        void Start()
        {
            closed.SetActive(!flag);
            open.SetActive(flag);
        }
    }
}

