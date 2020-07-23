using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.egamesstudios.cell
{
    public class WeightSwitch : ASwitch
    {
        [SerializeField, BoxGroup("Weight")]
        WeightSwitchType weightType;
        Animator anim;

        private void Reset()
        {
            type = SwitchType.WEIGHT;
        }

        private void Start()
        {
            type = SwitchType.WEIGHT;
            anim = GetComponentInChildren<Animator>();
        }

        protected override void TriggerSwitch(HitableType expectedHitType)
        {
            if (index == 1)
            {
                BoxCollider2D box = GetComponent<BoxCollider2D>();
                foreach (RaycastHit2D hitInfo in Physics2D.BoxCastAll(box.bounds.center, box.bounds.extents * 2, 0, Vector2.zero))
                {
                    if ((hitInfo.transform.CompareTag("Cell") && weightType == WeightSwitchType.LIGHT) || hitInfo.transform.CompareTag("Block")) return;
                }
            }

            AnimateSwitch();

            foreach (ASwitchEvent switchEvent in switchEvents[index].events)
            {
                switchEvent.Trigger();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            bool block = false;
            if (((collision.CompareTag("Cell") && weightType == WeightSwitchType.LIGHT) || (block = collision.CompareTag("Block"))) && index == 0)
            {
                if (block && flag)
                {
                    flag.SetFlag(true);
                }
                TriggerSwitch(SwitchType.WEIGHT);
            }
        }
        private void AnimateSwitch()
        {
            anim.SetTrigger("triggerSwitch");
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            bool block = false;
            if (((collision.CompareTag("Cell") && weightType == WeightSwitchType.LIGHT) || (block = collision.CompareTag("Block"))) && index == 1)
            {
                if (block && flag)
                {
                    flag.SetFlag(false);
                }
                TriggerSwitch(SwitchType.WEIGHT);
            }
        }
    }
}