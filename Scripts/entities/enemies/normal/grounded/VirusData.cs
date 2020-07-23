using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.egamesstudios.cell
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "Custom/Enemy/Flying/Virus", order = 2)]
    public class VirusData : FlyingData
    {
        [FoldoutGroup("Virus")]
        public GameObject projectile;
        [FoldoutGroup("Virus")]
        public Vector2 chargeSpeed;
    }
}
