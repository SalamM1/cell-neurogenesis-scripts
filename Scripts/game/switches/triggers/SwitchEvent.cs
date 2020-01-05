using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.egamesstudios.cell
{
    public abstract class ASwitchEvent : MonoBehaviour
    {
        protected bool inactive;
        public abstract void Trigger();
        public abstract void SetState(bool state);

        public void SetPermanent()
        {
            inactive = true;
        }

    }
}
