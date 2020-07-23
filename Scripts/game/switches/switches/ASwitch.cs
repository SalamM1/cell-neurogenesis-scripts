using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.egamesstudios.cell
{
    public abstract class ASwitch : MonoBehaviour
    {
        [System.Serializable]
        protected class SwitchEventArrayWrapper
        {
            public ASwitchEvent[] events;
        }
        [SerializeField]
        protected SwitchType type;
        protected SwitchFlag flag;

        [SerializeField]
        protected SwitchEventArrayWrapper[] switchEvents;
        [BoxGroup("General")]
        public bool permanent;
        [BoxGroup("General")]
        public bool permanentState;

        [BoxGroup("General")]
        [SerializeField, OnValueChanged("RefreshDictionary"), ValidateInput("ValidateStateValue", "Must be above 0; if Permanent, must be equal to 1, if Weight, must be equal to 2")]
        private int numOfStates;
        [ReadOnly, BoxGroup("General")]
        public int index;

        private Cutscene[] scenes;
        private bool hasCutscene;
        private bool activated;

        // Start is called before the first frame update
        void Awake()
        {
            index = 0;
            flag = GetComponentInParent<SwitchFlag>();
            if ((scenes = GetComponentsInChildren<Cutscene>()).Length > 0)
            {
                hasCutscene = true;
            }
        }

        public void TriggerSwitch(SwitchType expectedType, HitableType expectedHitType = HitableType.BOTH)
        {
            //Check if correct switch and if its active
            if (expectedType != type) return;
            if (activated) return;

            //Call Trigger
            TriggerSwitch(expectedHitType);

            //Play any cutscene
            if (hasCutscene)
            {
                StartCoroutine(CutsceneManager.cutsceneManager.PlayCutscene(scenes));
            }

            //Index, and make inactive if permanent
            index++;
            if (index >= switchEvents.Length) index = 0;
            if (index == 0 && permanent) activated = true;

        }

        protected abstract void TriggerSwitch(HitableType expectedHitType);

#if UNITY_EDITOR
        private bool ValidateStateValue(int value)
        {
            if (value <= 0) return false;
            if (type == SwitchType.WEIGHT && value != 2) return false;
            if (type == SwitchType.HITABLE && permanent && value != 1) return false;
            return true;
        }

        [Button, BoxGroup("General")]
        private void RefreshDictionary()
        {
            switchEvents = new SwitchEventArrayWrapper[numOfStates];
        }
#endif
    }

    public enum SwitchType
    {
        WEIGHT, HITABLE
    }

    //Heavy -> Blocks only
    //Light -> Cell can also activate
    public enum WeightSwitchType
    {
        LIGHT, HEAVY
    }

    public enum HitableType
    {
        GUN, GUITAR, BOTH
    }
}
