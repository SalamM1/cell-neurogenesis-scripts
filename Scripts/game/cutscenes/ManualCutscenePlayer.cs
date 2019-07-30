using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace com.egamesstudios.cell
{
    [RequireComponent(typeof(EventFlag))]
    public class ManualCutscenePlayer : MonoBehaviour
    {
        [SerializeField]
        private ManualCutsceneClip[] cutsceneClips;
        [HideInInspector]
        public ManualCutsceneClip[] CutsceneClips { get { return cutsceneClips; } }
        private EventFlag flag;
        [SceneObjectsOnly, SerializeField]
        private GameObject trueFlag;
        private int index;
        private int indexOfLastClip;
        private bool canRecievePlayerInput, isPlaying, completed;

        private void Awake()
        {
           (cutsceneClips = GetComponentsInChildren<ManualCutsceneClip>()).OrderBy(a => a.sortOrder);
        }

        private void Start()
        {
            flag = GetComponent<EventFlag>();
            if(trueFlag) trueFlag.SetActive(flag.GetFlag());
            gameObject.SetActive(!flag.GetFlag());

        }

        private void Update()
        {
            if(isPlaying)
            {
                if (canRecievePlayerInput && Rewired.ReInput.players.GetPlayer(0).GetAnyButtonDown())
                {
                    NextClip();
                }
            }

        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.tag.Equals("Cell") && !isPlaying && !completed)
            {
                BeginCutscene();
            }
        }

        private void BeginCutscene()
        {
            UIManager.uIManager.ChangeState(UIState.CUTSCENE);
            VariableContainer.variableContainer.currentActive.ChangeState(State.CUTSCENE);
            isPlaying = true;
            index = indexOfLastClip = 0;
            cutsceneClips[index].TriggerClip();
            while (cutsceneClips[index].chainNextClip)
            {
                index++;
                if (index >= cutsceneClips.Length) return;
                cutsceneClips[index].TriggerClip();
            }
        }

        public void NextClip()
        {
            cutsceneClips[indexOfLastClip].EndClip();
            if (++index >= cutsceneClips.Length) EndCutscene();
            else
            {
                canRecievePlayerInput = cutsceneClips[index].TriggerClip();
                indexOfLastClip = index;
                while (cutsceneClips[index].chainNextClip)
                {
                    index++;
                    if (index >= cutsceneClips.Length) return;
                    canRecievePlayerInput = (cutsceneClips[index].TriggerClip() && canRecievePlayerInput);
                }               
            }
        }
        private void EndCutscene()
        {
            isPlaying = false;
            VariableContainer.variableContainer.currentActive.ChangeState(State.CONTROL);
            UIManager.uIManager.ChangeState(UIState.INGAME);
            CutsceneManager.cutsceneManager.EndControllCutscene();
            flag.SetFlag(true);
            if (trueFlag) trueFlag.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
