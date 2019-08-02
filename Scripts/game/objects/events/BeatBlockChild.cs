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
            active = !active;
            if(active)
                SwitchToActive();
            else
                SwitchToInactive();
		}

        protected abstract void SwitchToActive();
        protected abstract void SwitchToInactive();

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