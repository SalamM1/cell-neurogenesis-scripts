using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.egamesstudios.cell
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "Custom/Enemy/Base", order = 1)]
    public class AEnemyData : ScriptableObject
    {
        [FoldoutGroup("Data")]
        public int maxHealth, contactDamage, attackDamage;

        [FoldoutGroup("Data")]
        public bool canThaw, canBeKnockbacked;

        [FoldoutGroup("Data")]
        public float knockbackForce, speed, detectionRange, awareSpeedFactor;

        [FoldoutGroup("Data")]
        public EnemyType enemyType;

        [FoldoutGroup("Data")]
        public DropTable dropTable;

        [FoldoutGroup("Timers")]
        public float TIME_BETWEEN_ATTACKS, TIME_TO_THAW, TIME_TO_STUN, TIME_TO_BE_AWARE, TIME_TO_BE_IDLE, TIME_TO_KNOCKBACK, TIME_TO_ATTACK, TIME_TO_LOSE_CHASE,RANDOM_MOVE_TIMER;
    }
}

