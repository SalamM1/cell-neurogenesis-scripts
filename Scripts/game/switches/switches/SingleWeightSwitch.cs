using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.egamesstudios.cell
{
    public class SingleWeightSwitch : MonoBehaviour, IWeightSwitch
    {
        public WeightSwitchType type = WeightSwitchType.LIGHT;
        [OdinSerialize]
        public ASwitchEvent[] switchEvents;
        SwitchFlag flag;
        bool activated;
        private Animator anim;

        public Cutscene[] scene;
        private bool hasCutscene;

        void Start()
        {
            flag = GetComponentInParent<SwitchFlag>();

            if ((scene = GetComponentsInChildren<Cutscene>()).Length > 0)
            {
                hasCutscene = true;
            }
            anim = GetComponentInChildren<Animator>();
        }

        public void TriggerSwitch()
        {
            activated = !activated;
            foreach (ASwitchEvent switchEvent in switchEvents)
            {
                switchEvent.Trigger();
            }
            anim.SetBool("activated", activated);
        }

        public void ActivateSwitch()
        {
            if (hasCutscene)
            {
                StartCoroutine(CutsceneManager.cutsceneManager.PlayCutscene(scene));
            }
            TriggerSwitch();
        }

        public void DeactivateSwitch()
        {
            BoxCollider2D box = GetComponent<BoxCollider2D>();
            foreach (RaycastHit2D hitInfo in Physics2D.BoxCastAll(box.bounds.center, box.bounds.extents, 0, Vector2.zero))
            {
                if (hitInfo.transform.tag.Equals("Cell") || hitInfo.transform.tag.Equals("Block")) return;
            }
            TriggerSwitch();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            bool block = false;
            if (((collision.tag.Equals("Cell") && type == WeightSwitchType.LIGHT) || (block = collision.tag.Equals("Block"))) && !activated)
            {
                if (block && flag)
                {
                    flag.SetFlag(true);
                }
                ActivateSwitch();
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            bool block = false;
            if (((collision.tag.Equals("Cell") && type == WeightSwitchType.LIGHT) || (block = collision.tag.Equals("Block"))) && activated)
            {
                if (block && flag)
                {
                    flag.SetFlag(false);
                }
                DeactivateSwitch();
            }
        }
    }
}
