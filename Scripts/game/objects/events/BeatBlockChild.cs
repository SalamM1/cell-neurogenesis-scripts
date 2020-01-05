using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.egamesstudios.cell
{
	public abstract class BeatBlockChild : MonoBehaviour
	{
        [SerializeField]
        protected bool active;
        [SerializeField]
        protected Sprite inactiveSprite, activeSprite;

        public void SwitchActive()
		{
            if(!active)
                SwitchToActive();
            else
                SwitchToInactive();
		}

        protected abstract void SwitchToActive();
        protected abstract void SwitchToInactive();

        [ButtonGroup]
        public virtual void SetActive()
        {
            active = true;
            GetComponentInChildren<SpriteRenderer>().sprite = activeSprite;
        }

        [ButtonGroup]
        public virtual void SetInactive()
        {
            active = false;
            GetComponentInChildren<SpriteRenderer>().sprite = inactiveSprite;
        }

        public void Blink()
        {
            if (!active) return;
            StartCoroutine(BlinkSprite());
        }

        protected virtual IEnumerator BlinkSprite()
        {
            GetComponentInChildren<SpriteRenderer>().color = new Color(1, 1, 1, 0.33f);
            yield return new WaitForSeconds(0.3f);
            GetComponentInChildren<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        }
    }
}