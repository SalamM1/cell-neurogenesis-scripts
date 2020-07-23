using UnityEngine;
using System.Collections;

namespace com.egamesstudios.cell
{
    public class MusicClip : ManualCutsceneClip
    {
        [SerializeField, Sirenix.OdinInspector.FoldoutGroup("Clip Details")]
        private MusicData musicData;

        public override void EndClip()
        {
            base.EndClip();
        }
        public override bool TriggerClip()
        {
            playing = true;
            MusicController.musicController.PlayAudio(musicData);
            return true;
        }

    }
}
