using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.egamesstudios.cell
{
    public abstract class MoveObjectClip : ManualCutsceneClip
    {
        [SerializeField, Sirenix.OdinInspector.FoldoutGroup("Clip Details")]
        protected Transform positionToMoveTo;
        protected bool isRight, reachedPoint;
    }
}
