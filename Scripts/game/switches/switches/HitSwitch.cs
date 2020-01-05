using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.egamesstudios.cell
{
    //Determine if a switch has a timer or not
    public enum HitableTriggerType
    {
        ONCE, TIMED
    }

    public class HitSwitch : ASwitch
    {
        [BoxGroup("Hitable")]
        public HitableSwitchType hitType;
        [BoxGroup("Hitable")]
        public float angle;
        [BoxGroup("Hitable")]
        public float moveSpeed;
        [BoxGroup("Hitable")]
        public Transform animatedPart;
        [BoxGroup("Hitable")]
        public HitableTriggerType triggerType;

        private void Reset()
        {
            type = SwitchType.HITABLE;
        }

        private void Start()
        {
            type = SwitchType.HITABLE;
        }

        protected override void TriggerSwitch(HitableSwitchType expectedHitType)
        {
            if (hitType != HitableSwitchType.BOTH && hitType != expectedHitType) return;

            if (flag) flag.SetFlag(true);
            foreach (ASwitchEvent switchEvent in switchEvents[index].events)
            {
                if (permanent)
                {
                    switchEvent.SetState(permanentState);
                    switchEvent.SetPermanent();
                }
                else
                {
                    switchEvent.Trigger();
                }
            }
            StartCoroutine(AnimateSwitch());
        }
        private IEnumerator AnimateSwitch()
        {
            yield return new WaitForSecondsRealtime(0.05f);
            float travelled = 0;
            while (travelled < angle)
            {
                animatedPart.transform.Rotate(Vector3.back * Time.deltaTime * moveSpeed * 10);
                travelled += Time.deltaTime * moveSpeed * 10;
                yield return new WaitForEndOfFrame();
            }
            animatedPart.transform.rotation = Quaternion.Euler(0, 0, -angle);
            yield break;
        }
    }
}
