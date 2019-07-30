using UnityEngine;
using System.Collections;

namespace com.egamesstudios.cell
{
    public abstract class ManualCutsceneClip : MonoBehaviour
    {
        public int sortOrder;
        protected ManualCutscenePlayer cutscenePlayer;
        public bool chainNextClip, playing;

        private void Awake()
        {
            cutscenePlayer = GetComponentInParent<ManualCutscenePlayer>();
        }

        public abstract bool TriggerClip();
        public virtual void EndClip()
        {
            if (chainNextClip) cutscenePlayer.CutsceneClips[sortOrder + 1].EndClip();
            playing = false;
        }
    }
}
