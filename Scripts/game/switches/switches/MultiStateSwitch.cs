using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.egamesstudios.cell
{
    public class MultiStateSwitch : SerializedMonoBehaviour, IGuitarSwitch, IGunSwitch
    {
        [ShowInInspector]
        private int index;
        [OdinSerialize, DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.ExpandedFoldout)]
        public Dictionary<int, ASwitchEvent[]> switches;
        [SerializeField, OnValueChanged("RefreshDictionary")]
        private int numberOfStates;
        [SerializeField, HideInInspector]
        private int maxIndex;

        [Button]
        private void RefreshDictionary()
        {
            if (switches == null) switches = new Dictionary<int, ASwitchEvent[]>();
            int difference = numberOfStates - switches.Count;
            if(difference < 0)
            {
                for(int i = switches.Count; i > numberOfStates - 1; i--)
                {
                    switches.Remove(i);
                }
            }
            else
            {
                for(int i = switches.Count; i < numberOfStates; i++)
                {
                    switches.Add(i, null);
                }
            }
            maxIndex = numberOfStates - 1;
        }
        public void TriggerSwitch()
        {
            index++;
            if (index > maxIndex) index = 0;
            StartCoroutine(AnimateSwitch());
            foreach(ASwitchEvent switchEvent in switches[index])
            {
                switchEvent.Trigger();
            }
        }

        private IEnumerator AnimateSwitch()
        {
            Debug.Log("AnImAtEd");
            yield return null;
        }
        private void Start()
        {
            index = -1;
        }
    }
}