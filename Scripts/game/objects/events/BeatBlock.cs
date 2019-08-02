using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.egamesstudios.cell 
{
    [RequireComponent(typeof(Block))]
	public class BeatBlock : BeatBlockChild
	{
        private Collider2D[] colliders;

        private void Awake()
        {
            colliders = GetComponents<Collider2D>();
        }
        protected override void SwitchToActive()
        {
            GetComponentInChildren<SpriteRenderer>().sprite = activeSprite;
            GetComponent<Rigidbody2D>().isKinematic = false;
            colliders[0].enabled = true;
            colliders[1].enabled = true;
        }

        protected override void SwitchToInactive()
        {
            GetComponentInChildren<SpriteRenderer>().sprite = inactiveSprite;
            GetComponent<Rigidbody2D>().isKinematic = true;
            colliders[0].enabled = false;
            colliders[1].enabled = false;
            GetComponent<Block>().ClearTargets();
        }

        [ButtonGroup]
        public void SetActive()
        {
            active = true;
            GetComponentInChildren<SpriteRenderer>().sprite = activeSprite;
            GetComponent<Rigidbody2D>().isKinematic = false;
            colliders = GetComponents<Collider2D>();
            colliders[0].enabled = true;
            colliders[1].enabled = true;
        }
        [ButtonGroup]
        public void SetInactive()
        {
            active = false;
            GetComponentInChildren<SpriteRenderer>().sprite = inactiveSprite;
            GetComponent<Rigidbody2D>().isKinematic = true;
            colliders = GetComponents<Collider2D>();
            colliders[0].enabled = false;
            colliders[1].enabled = false;
        }
    }
}