using UnityEngine;
using System.Collections;

namespace com.egamesstudios.cell
{
    public class TextClip : ManualCutsceneClip
    {
        [SerializeField, Sirenix.OdinInspector.FoldoutGroup("Clip Details")]
        private int dialogueID;
        private bool contNextFrame;

        private void Update()
        {
            if (!playing) return;
            var player = Rewired.ReInput.players.GetPlayer(0);
            if(contNextFrame)
            {
                contNextFrame = false;
                if (DialogueManager.dialogueManager.DisplayNextSentenceCutscene())
                {
                    cutscenePlayer.NextClip();
                    playing = false;
                }
            }
            if (player.GetButtonDown("Interact"))
            {
                contNextFrame = true;
            }

        }
        public override bool TriggerClip()
        {
            playing = true;
            DialogueManager.dialogueManager.SetCutsceneDialogue(dialogueID, cutscenePlayer.name);
            return false;
        }

        public override void EndClip()
        {
            base.EndClip();
            DialogueManager.dialogueManager.ClearTextBoxes();
        }
    }
}
