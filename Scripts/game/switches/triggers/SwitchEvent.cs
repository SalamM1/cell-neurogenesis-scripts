using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.egamesstudios.cell
{
    public abstract class ASwitchEvent : MonoBehaviour
    {
        protected bool inactive;
        public abstract void Trigger();

        public void SetPermanent()
        {
            inactive = true;
        }

    }
}
