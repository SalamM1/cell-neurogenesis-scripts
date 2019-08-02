using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace com.egamesstudios.cell
{
    public enum State
    {
        CONTROL,
        CUTSCENE,
        CLONING,
        CHARGING,
        GROUND_POUND,
        SHOPPING,
        INACTIVE,
        TRANSITION,
        DEAD,
        DIALOGUE,
    }

    public enum CellType
    {
        ORIGINAL,
        NORMAL,
        JUMPER,
        M_FIGHTER,
        R_FIGHTER,
        SLINGER,
        GROUND,
    }

    public class CellVariables : MonoBehaviour
    {
        public State activeState = State.CONTROL;
        public CellType type = CellType.ORIGINAL;

        [FoldoutGroup("General", 1)]
        [FoldoutGroup("Physics State", 0)]
        public bool grounded, hitGround;
        [FoldoutGroup("General")]
        [FoldoutGroup("Physics State")]
        public bool faceLeft, faceRight, aimUp, aimDown;

        [FoldoutGroup("Physics State")]
        [FoldoutGroup("Wall Jumping")]
        public bool wallSliding, wallSlidingLeft, wallSlidingRight, isWallJumping, wallJumpingLeft, wallJumpingRight;

        [FoldoutGroup("Jumping")]
        [FoldoutGroup("Physics State")]
        public bool enterJump, isJumping, endJump;
        [FoldoutGroup("Damage")]
        [FoldoutGroup("Physics State")]
        public bool isInvul, enterKnocked, isKnocked;
        [FoldoutGroup("Sling Jump")]
        [FoldoutGroup("Physics State")]
        public bool isCharged;
        [FoldoutGroup("Sling Jump")]
        [FoldoutGroup("Physics State")]
        public bool enterLaunch, isLaunched;
        [FoldoutGroup("Jumping")]
        [FoldoutGroup("Physics State")]
        public bool enterFalling, isFalling;
        [FoldoutGroup("General")]
        [FoldoutGroup("Physics State")]
        public bool isDead;

        [FoldoutGroup("Jumping")]
        public bool canDoubleJump;
        [FoldoutGroup("Jumping")]
        public bool canTripleJump;
        [FoldoutGroup("Cloning", 12)]
        [FoldoutGroup("Physics State")]
        public bool isClone, isGateClone, isOnGate;
        [FoldoutGroup("Cloning")]
        public Transform gateLocation;

        [FoldoutGroup("Jumping")]
        [FoldoutGroup("Powerups")]
        public bool hasDoubleJump;

        [FoldoutGroup("Jumping")]
        [FoldoutGroup("Powerups")]
        public bool hasTripleJump;

        [FoldoutGroup("Combat", 4)]
        [FoldoutGroup("Powerups")]
        public bool hasGuitar;

        [FoldoutGroup("Combat")]
        [FoldoutGroup("Powerups")]
        public bool hasGun;

        [FoldoutGroup("Wall Jumping", 6)]
        [FoldoutGroup("Powerups")]
        public bool hasWallJump;

        [FoldoutGroup("Powerups")]
        public bool hasSlingJump;

        [FoldoutGroup("Powerups", 2)]
        [FoldoutGroup("Speed Dash", 9)]
        public bool hasChargeDash;

        [FoldoutGroup("Powerups")]
        public bool hasGroundPound;

        [FoldoutGroup("General")]
        public int maxHealth, mainHealth, mainEnergy, maxEnergy, activeClones;

        [FoldoutGroup("Speed Modifiers", 3)]
        public float movement;
        [FoldoutGroup("Speed Modifiers")]
        public float speed;
        [FoldoutGroup("Speed Modifiers")]
        public float wallSlideSpeed, wallSlideSpeedModifier;
        [FoldoutGroup("Speed Modifiers")]
        public float swingSpeedModifier = 0;
        [FoldoutGroup("Speed Modifiers")]
        public float pushbackModifier;

        [FoldoutGroup("Jumping", 5)]
        public float jumpTime;
        [FoldoutGroup("Jumping")]
        public float timeInAir;
        [FoldoutGroup("Jumping")]
        public float jumpForce;

        [FoldoutGroup("Damage", 11)]
        public float knockbackTime;
        [FoldoutGroup("Damage")]
        public float knockbackForce;
        [FoldoutGroup("Damage")]
        public float invulTimer;

        [FoldoutGroup("Wall Jumping")]
        public float wallJumpTime;
        [FoldoutGroup("Wall Jumping")]
        public float wallJumpForce;

        [FoldoutGroup("Ground Pound", 7)]
        public float groundPoundTime;
        [FoldoutGroup("Ground Pound")]
        public float groundPoundForce;

        [FoldoutGroup("Sling Jump", 8)]
        public float chargeTime;
        [FoldoutGroup("Sling Jump")]
        public float chargeSpeed;

        [FoldoutGroup("General")]
        public Vector3 checkpoint;

        [FoldoutGroup("Combat")]
        public int guitarDamage, gunDamage;
        [FoldoutGroup("Combat")]
        public BulletType equippedBullet = BulletType.NORMAL;
        [FoldoutGroup("Combat")]
        public GunType equippedGun = GunType.NORMAL;

        [FoldoutGroup("Saves")]
        public string savedRoom;
        [FoldoutGroup("Saves")]
        public Vector2 saveCheckpoint;

        [FoldoutGroup("Saves")]
        public int coinCount;
    }
}

