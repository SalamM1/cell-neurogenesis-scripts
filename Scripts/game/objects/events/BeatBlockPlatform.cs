using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.egamesstudios.cell 
{
	public class BeatBlockPlatform : BeatBlockChild
	{
        protected override void SwitchToActive()
        {
            SetActive();
        }

        protected override void SwitchToInactive()
        {
            SetInactive();
        }


        public override void SetActive()
        {
            base.SetActive();
            GetComponent<Collider2D>().enabled = true;
        }

        public override void SetInactive()
        {
            base.SetInactive();
            GetComponent<Collider2D>().enabled = false;
        }
    }
}