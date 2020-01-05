using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.egamesstudios.cell
{

    [RequireComponent(typeof(MovingPlatform))]
	public class BeatBlockMovingPlatform : BeatBlockChild
	{

        protected override void SwitchToActive()
        {
            SetActive();
        }

        protected override void SwitchToInactive()
        {
            SetInactive();
            GetComponent<MovingPlatform>().ClearTargets();
        }

        public override void SetActive()
        {
            base.SetActive();
            var colliders = GetComponents<Collider2D>();
            colliders[0].enabled = true;
            colliders[1].enabled = true;
        }

        public override void SetInactive()
        {
            base.SetInactive();
            var colliders = GetComponents<Collider2D>();
            colliders[0].enabled = false;
            colliders[1].enabled = false;
        }
    }
}