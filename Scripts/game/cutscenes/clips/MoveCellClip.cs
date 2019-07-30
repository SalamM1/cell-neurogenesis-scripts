using UnityEngine;
using System.Collections;

namespace com.egamesstudios.cell
{
    public class MoveCellClip : MoveObjectClip
    {
        
        private void Update()
        {
            if (!playing) return;
            if (reachedPoint) cutscenePlayer.NextClip();
            else
            {
                VariableContainer.variableContainer.currentActive.SetCutsceneSpeed(isRight);
                float x = VariableContainer.variableContainer.currentActive.transform.position.x;
                if (x > positionToMoveTo.position.x - 0.2f && x < positionToMoveTo.position.x + 0.2f)
                {
                    VariableContainer.variableContainer.currentActive.SetCutsceneSpeed(isRight, true);
                    reachedPoint = true;
                }
            }

        }
        public override bool TriggerClip()
        {
            playing = true;
            isRight = positionToMoveTo.position.x > VariableContainer.variableContainer.currentActive.transform.position.x;
            return false;
        }
    }
}
