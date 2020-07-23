using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.egamesstudios.cell
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "Custom/Enemy/Flying/Base", order = 1)]
    public class FlyingData : AEnemyData
    {
        [FoldoutGroup("Flying")]
        public float hoverDistance;
        [FoldoutGroup("Flying")]
        public float hoverScaling;
    }
}
