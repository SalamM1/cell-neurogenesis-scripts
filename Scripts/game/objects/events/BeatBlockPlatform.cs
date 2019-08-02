using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.egamesstudios.cell 
{
	public class BeatBlockPlatform : BeatBlockChild
	{
        private Collider2D attachedCollider;

        private void Awake()
        {
            attachedCollider = GetComponent<Collider2D>();
        }
        protected override void SwitchToActive()
        {
            GetComponentInChildren<SpriteRenderer>().sprite = activeSprite;
            attachedCollider.enabled = true;
        }

        protected override void SwitchToInactive()
        {
            GetComponentInChildren<SpriteRenderer>().sprite = inactiveSprite;
            attachedCollider.enabled = false;
        }

        [ButtonGroup]
        public void SetActive()
        {
            active = true;
            GetComponentInChildren<SpriteRenderer>().sprite = activeSprite;
            attachedCollider = GetComponent<Collider2D>();
            attachedCollider.enabled = true;
        }
        [ButtonGroup]
        public void SetInactive()
        {
            active = false;
            GetComponentInChildren<SpriteRenderer>().sprite = inactiveSprite;
            attachedCollider = GetComponent<Collider2D>();
            attachedCollider.enabled = false;
        }
    }
}