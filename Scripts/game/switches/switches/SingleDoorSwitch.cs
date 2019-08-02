using DG.Tweening;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.egamesstudios.cell
{
    public class SingleDoorSwitch : MonoBehaviour, IGuitarSwitch, IGunSwitch
    {
        SwitchFlag flag;
        [OdinSerialize]
        public DoorTrigger[] switchEvents;
        public Transform button;

        bool activated;
        public float angle;
        public float moveSpeed;

        private Cutscene[] scenes;
        private bool hasCutscene;

        void Start()
        {
            flag = GetComponentInParent<SwitchFlag>();
            if ((scenes = GetComponentsInChildren<Cutscene>()).Length > 0)
            {
                hasCutscene = true;
            }
        }

        public void TriggerSwitch()
        {
            if (!activated)
            {
                activated = true;
                foreach (DoorTrigger switchEvent in switchEvents)
                {
                    switchEvent.SetDoorState(true);
                    switchEvent.SetPermanent();
                }
                if(hasCutscene)
                {
                    StartCoroutine(CutsceneManager.cutsceneManager.PlayCutscene(scenes));
                }
                StartCoroutine(AnimateSwitch());
            }
        }

        IEnumerator AnimateSwitch()
        {
            yield return new WaitForSecondsRealtime(0.05f);
            float travelled = 0;
            while (travelled < angle)
            {
                button.transform.Rotate(Vector3.back * Time.deltaTime*moveSpeed*10);
                travelled += Time.deltaTime * moveSpeed * 10;
                yield return new WaitForEndOfFrame();
            }
            button.transform.rotation = Quaternion.Euler(0, 0, -angle);
            yield break;
        }
    }
}