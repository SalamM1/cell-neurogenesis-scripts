using UnityEngine;
using System.Collections;

namespace com.egamesstudios.cell
{
    public class CameraClip : ManualCutsceneClip
    {
        [SerializeField, Sirenix.OdinInspector.FoldoutGroup("Clip Details")]
        private Cutscene cutscene;

        public override void EndClip()
        {
            base.EndClip();
        }
        public override bool TriggerClip()
        {
            CutsceneManager.cutsceneManager.PlayControlledCutscene(cutscene);
            playing = true;
            return true;
        }

    }
}
