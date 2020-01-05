using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.egamesstudios.cell 
{
    [RequireComponent(typeof(Block))]
	public class BeatBlock : BeatBlockChild
	{

        protected override void SwitchToActive()
        {
            SetActive();
        }

        protected override void SwitchToInactive()
        {
            SetInactive();
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<Block>().ClearTargets();
        }


        public override void SetActive()
        {
            base.SetActive();
            GetComponent<Rigidbody2D>().isKinematic = false;
            var colliders = GetComponents<Collider2D>();
            colliders[0].enabled = true;
            colliders[1].enabled = true;
        }

        public override void SetInactive()
        {
            base.SetInactive();
            GetComponent<Rigidbody2D>().isKinematic = true;
            var colliders = GetComponents<Collider2D>();
            colliders[0].enabled = false;
            colliders[1].enabled = false;
        }
    }
}