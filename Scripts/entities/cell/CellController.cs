using System;
using System.Collections;
using System.Collections.Generic;
using com.egamesstudios.cell;
using Rewired;
using Sirenix.OdinInspector;
using UnityEngine;

namespace com.egamesstudios.cell
{
    public enum CellAnimation
    {
        PICKUP,
        IDLE,
    }

    /// <summary>
    /// The type of damage inflicted used for determining different effects and damage scales.
    /// </summary>
    public enum DamageType
    {
        /// <summary>
        /// An enemy AI dealing damage during normal gameplay.
        /// </summary>
        ENEMY,
        /// <summary>
        /// A hazard dealing damage during normal gameplay.
        /// </summary>
        HAZARD,
        /// <summary>
        /// Forced damage that can occur at any point; usually in cutscenes or unavoidable attacks.
        /// </summary>
        FORCED,
    }

    [RequireComponent(typeof(Rigidbody2D), typeof(AudioSource), typeof(CircleCollider2D))]
    [RequireComponent(typeof(CellParticlePlayer))]
    /*
     * The primary class for character controlling
     * Contains all methods necessary for controlling Cell
     *
     * */
    public class CellController : MonoBehaviour
    {
        /// <summary>
        /// Variable container component.
        /// </summary>
        public CellVariables vars;

        /// <summary>
        /// Rewired player object used to read inputs.
        /// </summary>
        public Player player;
        private readonly int playerId = 0;
        /// <summary>
        /// General vector used as a buffer to set the rigidbody velocity for different states.
        /// </summary>
        [ShowInInspector]
        private Vector2 move;
        /// <summary>
        /// Main rigidbody attached to the player object.
        /// </summary>
        private Rigidbody2D rb2d;
        /// <summary>
        /// Guitar Weapon attached to this player object.
        /// </summary>
        public GuitarController guitar;
        /// <summary>
        /// Gun Weapon attached to this player object.
        /// </summary>
        public GunController gun;
        private SFXPlayer sfx;
        private CellParticlePlayer particles;

        void Awake()
        {
            player = ReInput.players.GetPlayer(playerId);

            move = new Vector2(0, 0);
            rb2d = this.GetComponent<Rigidbody2D>();
            guitar = GetComponentInChildren<GuitarController>();
            gun = GetComponentInChildren<GunController>();
            sfx = GetComponent<SFXPlayer>();
            particles = GetComponent<CellParticlePlayer>();

            DontDestroyOnLoad(gameObject);
            foreach (Animator a in GetComponentsInChildren<Animator>())
            {
                a.logWarnings = false;
            }
        }

        void Update()
        {
           // Debug.Log((int) (1.0f / Time.deltaTime)); //FPS counter quick
            if(vars.isDead)
            {
                if(!vars.isClone)
                {
                    GetComponent<CloneController>().KillAllClones();
                    UpdateAnimations();
                    SaveManager.saveManager.SaveGame();
                    StartCoroutine(LoadSceneManager.loadSceneManager.ResetCellToSave());
                    
                    vars.isDead = false;
                }
            } else
            {
                switch (vars.activeState)
                {
                    case State.CONTROL:
                        {
                            CalculateJump();
                            ScanInputs();
                            break;
                        }
                    case State.CHARGING:
                        {

                            if (!vars.isCharged && !vars.isLaunched)
                            {
                                CheckCharge();
                            }
                            else
                            {
                                CheckLaunch();
                            }

                            break;
                        }
                    case State.GROUND_POUND:
                        CheckGroundPound();
                        break;
                    case State.INACTIVE:
                        rb2d.velocity = new Vector2(0, rb2d.velocity.y);
                        break;
                    case State.TRANSITION:
                        transform.Translate(move * Time.unscaledDeltaTime);
                        break;
                    default:
                        break;
                }
                CleanupVariables();
                ManagePowerups();
                UpdateParticlesAndSound();
            }
            
        }

        private void FixedUpdate()
        {
            switch (vars.activeState)
            {
                case State.CONTROL:
                    {
                        CalculateMovement();
                        break;
                    }
                case State.CUTSCENE:
                    rb2d.velocity = new Vector2(move.x, rb2d.velocity.y);
                    break;
            }
                    
        }

        void LateUpdate()
        {
            CheckGrounded();
            if(vars.hasWallJump)
            {
                CheckSliding();
            }
            CheckDirection();
            CheckFalling();
            UpdateAnimations();
            vars.aimDown = (player.GetAxis("Camera Y") == -1);
            vars.aimUp = (player.GetAxis("Camera Y") == 1);
        }

        #region Update Methods
        /// <summary>
        /// Update method to check if the player object is grounded and stores in the variable in the container.
        /// </summary>
        private void CheckGrounded()
        {
            vars.hitGround = false;
            RaycastHit2D hit = Physics2D.CircleCast(transform.position,
                0.25f, new Vector2(0, -1), 0.57f, LayerMask.GetMask("Platform"));
            if (!vars.grounded && hit.collider != null)
            {
                vars.hitGround = true;
            }
            vars.grounded = (hit.collider);
            

        }

        /// <summary>
        /// Update method to check if player is currently sliding on a wall and stores updated variables in container.
        /// </summary>
        private void CheckSliding()
        {
            if (!vars.isWallJumping && !vars.isJumping)
            {
                RaycastHit2D hit = Physics2D.CircleCast(new Vector2(transform.position.x, transform.position.y),
                      0.1f, new Vector2(-1, 0), 0.57f, LayerMask.GetMask("Platform"));
                vars.wallSlidingLeft = (hit.collider != null && !vars.grounded) && vars.movement < 0;

                RaycastHit2D hit2 = Physics2D.CircleCast(new Vector2(transform.position.x, transform.position.y),
                    0.1f, new Vector2(1, 0), 0.57f, LayerMask.GetMask("Platform"));
                vars.wallSlidingRight = (hit2.collider != null && !vars.grounded) && vars.movement > 0;

                vars.wallSliding = (vars.wallSlidingLeft || vars.wallSlidingRight);
            }


            
        }
        /// <summary>
        /// Update method to check the direction player is facing based on localScale and stores updated variables in container.
        /// </summary>
        private void CheckDirection()
        {
            if(transform.localScale.x > 0)
                vars.faceRight = !(vars.faceLeft = true);
            if(transform.localScale.x < 0)
                vars.faceRight = !(vars.faceLeft = false);


        }

        /// <summary>
        /// Update method to check if player is currently falling down while out of a jump, charge, or ground pound and stores updated variables in container.
        /// </summary>
        private void CheckFalling()
        {
            vars.isFalling = (!vars.grounded && !vars.wallSliding && rb2d.velocity.y < 0
                && !vars.isWallJumping && vars.groundPoundTime <= 0);
        }

        /// <summary>
        /// Update method to determine the current status of player's charging ability.
        /// </summary>
        private void CheckCharge()
        {
            if (vars.chargeTime > 0)
            {
                if (player.GetButton("Charge"))
                {
                    rb2d.velocity = new Vector2(0, 0);
                    vars.chargeTime -= Time.deltaTime;
                }
                else
                {
                    ChangeState(State.CONTROL);
                }
            }
            else if (!vars.isCharged && rb2d.velocity.x == 0)
            {
                vars.isCharged = true;
            }
        }

        /// <summary>
        /// Update method to determine the status of a player's launch from the charge ability.
        /// </summary>
        private void CheckLaunch()
        {
            if (player.GetButtonUp("Charge"))
            {
                rb2d.velocity = new Vector2(vars.chargeSpeed * (SlingLeft() ? -1 : 1), vars.jumpForce);
                vars.isCharged = false;
                vars.isLaunched = true;
                sfx.PlaySFX(8);
                UpdateAnimations("idle");
                vars.chargeTime = 0.3f;
            }
            else if (!vars.isLaunched)
            {
                rb2d.velocity = new Vector2(0, 0);
            }
            if (vars.isLaunched)
            {
                if (vars.chargeTime > 0)
                {
                    vars.chargeTime -= Time.deltaTime;
                }
                else
                {
                    if (vars.grounded || vars.wallSliding)
                    {
                        ChangeState(State.CONTROL);
                    }
                }
            }
        }

        /// <summary>
        /// Update method to determine the current status of player's ground pound ability.
        /// </summary>
        private void CheckGroundPound()
        {
            if (vars.groundPoundTime > 0) 
            {
                vars.groundPoundTime -= Time.deltaTime;
                rb2d.velocity = Vector2.up*8;
            }
            else
            {
                if(vars.grounded)
                {
                    GroundPoundEffector();
                }
                else
                {
                    rb2d.velocity = new Vector2(0, vars.groundPoundForce);
                }
            }
        }
        /// <summary>
        /// Called when player hits the ground from a ground pound ability.
        /// </summary>
        private void GroundPoundEffector()
        {
            ChangeState(State.CONTROL);
        }

        /// <summary>
        /// Determines the direction to throw the player from a charge ability. Works for both wall charging and ground charging.
        /// </summary>
        /// <returns> True for left, false for right</returns>
        private bool SlingLeft()
        {
            if(vars.wallSliding)
            {
                return vars.wallSlidingRight;
            } else
            {
                return vars.faceLeft;
            }
        }
        /// <summary>
        /// Update method to check player input for Jumping, Charging and Ground Pounding.
        /// </summary>
        private void ScanInputs()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }

            if (player.GetButtonDown("Jump") && vars.wallSliding && vars.hasWallJump)
            {
                vars.isWallJumping = true;
                if(vars.wallSlidingRight)
                {
                    vars.wallJumpingRight = true;
                }
                else if (vars.wallSlidingLeft)
                {
                    vars.wallJumpingLeft = true;
                }
                vars.wallSlidingLeft = vars.wallSlidingRight = vars.wallSliding = false;

                UpdateAnimations("wallJump");
                vars.wallJumpTime = 0.17f;

                if (vars.wallJumpingRight)
                {
                    rb2d.velocity = new Vector2(vars.wallJumpForce* -0.75f, vars.jumpForce * 2f);
                    
                }
                else if (vars.wallJumpingLeft)
                {
                    rb2d.velocity = new Vector2(vars.wallJumpForce * 0.75f, vars.jumpForce * 2f);
                }
            }
            if(player.GetButtonDown("Charge") && (vars.grounded || vars.wallSliding) && vars.hasSlingJump)
            {
                ChangeState(State.CHARGING);
                sfx.PlaySFX(7);
            }
            if (player.GetButtonDown("Charge") && (!vars.grounded && !vars.wallSliding) && vars.hasGroundPound)
            {
                ChangeState(State.GROUND_POUND);
            }
        }

        /// <summary>
        /// Update method to calculate the generic movement of a player, updates via buffer vector.
        /// </summary>
        private void CalculateMovement()
        {
            vars.movement = player.GetAxis("Horizontal") * vars.speed;

            if (vars.isKnocked)
            {
                move = rb2d.velocity;
            }
            else
            {
                if (vars.isWallJumping)
                {
                    if (vars.wallJumpingRight)
                        vars.movement = vars.wallJumpForce * -1;
                    else if (vars.wallJumpingLeft)
                        vars.movement = vars.wallJumpForce;
                }
                if(vars.wallSliding)
                {
                    rb2d.velocity = new Vector2(rb2d.velocity.x, vars.wallSlideSpeed*vars.wallSlideSpeedModifier);
                }
                move.x = vars.movement;
                move.y = rb2d.velocity.y;
            }

            rb2d.velocity = move;
        }

        /// <summary>
        /// Handles the physics and availability of the Jump/Double Jump/Triple Jump mechanic.
        /// </summary>
        private void CalculateJump()
        {
            if(player.GetButtonDown("Jump") && !vars.wallSliding) {
                if(vars.grounded && !vars.isKnocked)
                {
                    sfx.PlaySFX(1);
                    StartJump();
                }
                else if (vars.canDoubleJump && vars.hasDoubleJump && !vars.isKnocked && !vars.grounded && !vars.wallSliding)
                {
                    sfx.PlaySFX(2);
                    vars.canDoubleJump = false;
                    StartJump();
                }
                else if (vars.canTripleJump && vars.hasTripleJump && !vars.isKnocked && !vars.grounded && !vars.wallSliding)
                {
                    sfx.PlaySFX(3);
                    vars.canTripleJump = false;
                    StartJump();
                }
            }
            if (player.GetButton("Jump")) {
                if(vars.isJumping && !vars.isKnocked && (vars.timeInAir <= vars.jumpTime))
                {
                    rb2d.velocity = new Vector2(vars.movement, vars.jumpForce);
                    vars.timeInAir += Time.deltaTime;
                }
            }
            if (player.GetButtonUp("Jump")) {
                vars.isJumping = false;
            }
        }

        /// <summary>
        /// Shared code between all jump mechanics to reset the current timer of a jump
        /// </summary>
        private void StartJump()
        {
            UpdateAnimations("jump");
            vars.timeInAir = 0;
            rb2d.velocity = new Vector2(vars.movement, 0);
            vars.isJumping = true;
        }

        /// <summary>
        /// Update method to update generic animations for player object and children.
        /// </summary>
        private void UpdateAnimations()
        {
            if (vars.movement != 0)
            {
                if (vars.movement > 0)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                }
                else
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
            }

            foreach(Animator a in GetComponentsInChildren<Animator>())
            {
                if (a != null)
                {
                    a.SetFloat("speed", Mathf.Abs(vars.movement));
                    a.SetBool("grounded", vars.grounded);
                    a.SetBool("wallSliding", vars.wallSliding);
                    a.SetBool("wallJumping", vars.isWallJumping);
                    a.SetBool("falling", vars.isFalling);
                    a.SetBool("jumping", vars.isJumping);
                    a.SetBool("isDead", vars.isDead);
                }
            }
            
        }
        /// <summary>
        /// Animation updated used to update player object and its children to specific animation states.
        /// </summary>
        /// <param name="trigger"> The name of the trigger found in animators</param>
        public void UpdateAnimations(String trigger)
        {
            foreach (Animator a in GetComponentsInChildren<Animator>())
            {
                if (a != null)
                {
                    a.SetTrigger(trigger);
                }
            }
        }

        private void UpdateParticlesAndSound()
        {
            if(rb2d.velocity.x != 0 && vars.grounded)
            {
                sfx.PlaySFX(0);
               // particles.PlayParticle(CellParticleType.WALK, new Vector3(0, -0.5f, 0));
            }
            if(vars.wallSliding)
            {
                sfx.PlaySFX(4);
                particles.PlayParticle(CellParticleType.WALLSLIDE, new Vector3(vars.wallSlidingLeft ? -0.5f : 0.5f, 0, 0));
            }

        }

        /// <summary>
        /// Update method to reset booleans, timers, etc.
        /// </summary>
        private void CleanupVariables()
        {
            if(vars.grounded)
            {
                vars.canDoubleJump = true;
                vars.canTripleJump = true;
                vars.isWallJumping = false;
            }
            if(vars.wallSliding)
            {
                vars.canDoubleJump = true;
                vars.canTripleJump = true;
            }
            if(vars.wallJumpTime > 0)
            {
                vars.wallJumpTime -= Time.deltaTime;
            }
            else
            {
                vars.isWallJumping = vars.wallJumpingLeft = vars.wallJumpingRight = false;

            }
            if(vars.knockbackTime > 0)
            {
                vars.knockbackTime -= Time.deltaTime;
            }
            else
            {
                vars.isKnocked = false;
            }

        }

        /// <summary>
        /// Sets the active state for the Gun and Guitar
        /// </summary>
        private void ManagePowerups()
        {
            if(gun != null)
            {
                gun.gameObject.SetActive(vars.hasGun);
            }
            if(guitar != null)
            {
                guitar.gameObject.SetActive(vars.hasGuitar);
            }
        }


        #endregion
        #region Accessable Methods

        /// <summary>
        /// Public method to deal damage to player object, changes specific behaviour based on damage type
        /// </summary>
        /// <param name="source">Source transform of damage</param>
        /// <param name="damage">Amount of damage to deal</param>
        /// <param name="type">Type of damage source</param>
        public void TakeHit(Transform source, int damage, DamageType type)
        {
            switch(type)
            {
                case DamageType.FORCED:
                    sfx.PlaySFX(30);
                    DealDamage(source, damage);
                    break;
                case DamageType.HAZARD:
                    sfx.PlaySFX(29);
                    DealDamage(source, damage);
                    SendToCheckpoint();
                    break;
                case DamageType.ENEMY:
                    sfx.PlaySFX(28);
                    if (!vars.isInvul)
                    {
                        DealDamage(source, damage);
                    }
                    break;
                default:
                    break;
            }            
        }

        /// <summary>
        /// Shared code to commit damage to player and set appropriate states
        /// </summary>
        /// <param name="source">Source transform of damage</param>
        /// <param name="damage">Amount of damage to deal</param>
        private void DealDamage(Transform source, int damage)
        {
            if (vars.activeState == State.CUTSCENE) return;
            if (vars.activeState != State.INACTIVE) ChangeState(State.CONTROL);
            vars.mainHealth -= damage;
            if (vars.mainHealth <= 0)
            {
                UpdateAnimations("die");
                vars.isDead = true;
            }
            else
            {
                UpdateAnimations("takeHit");
                Knockback(source);
            }
            if(vars.isInvul)
            {
                StopCoroutine(DamagedState());
                foreach (SpriteRenderer spr in GetComponentsInChildren<SpriteRenderer>())
                {
                    spr.color = new Color(1f, 1f, 1f, 1f);
                }
            }
            StartCoroutine(DamagedState());
        }

        /// <summary>
        /// Sets player object to the damaged state and updated alpha of all child sprites to match state
        /// </summary>
        private IEnumerator DamagedState()
        {
            vars.isInvul = true;
            vars.invulTimer = 0;
            gameObject.layer = 14;
            while (vars.invulTimer < 1.8f)
            {
                foreach (SpriteRenderer spr in GetComponentsInChildren<SpriteRenderer>())
                {
                    spr.color = new Color(1f, 1f, 1f, spr.color.a == 0 ? 1 : 0);
                }
                vars.invulTimer += Time.deltaTime;
                yield return null;
            }
            vars.isInvul = false;
            gameObject.layer = 13;
            foreach (SpriteRenderer spr in GetComponentsInChildren<SpriteRenderer>())
            {
                spr.color = new Color(1f, 1f, 1f, 1f);
                UpdateAnimations("idle");
            }
            yield break;
        }
        /// <summary>
        /// Knockbacks the player object with predefined calculations
        /// </summary>
        /// <param name="source">Source transform of knockback</param>
        /// <param name="multiplier">Multiplier for knockback force</param>
        /// <param name="horizontalOnly">If true, will only knockback player horizontally</param>
        /// <param name="timer">Time to be locked in knockback state</param>
        public void Knockback(Transform source, float multiplier = 1, bool horizontalOnly = false, float timer = 0.2f)
        {
            float t = source.position.x - transform.position.x > 0 ? -1 : 1;
            vars.isKnocked = true;
            rb2d.velocity = new Vector2(vars.knockbackForce * t, horizontalOnly ? 0 : Mathf.Abs(vars.knockbackForce * 2 * t)) * multiplier;
            vars.knockbackTime = timer;
        }

        /// <summary>
        /// Sets the player's checkpoint stored in container to the given coordinates
        /// </summary>
        /// <param name="checkpoint">Positional coordinates for checkpoint</param>
        public void SetCheckpoint(Vector3 checkpoint)
        {
            vars.checkpoint = checkpoint;
        }
        /// <summary>
        /// Moves player to the checkpoint set in container
        /// </summary>
        public void SendToCheckpoint()
        {
            transform.position = vars.checkpoint;
        }

        public void PlayAnimation(CellAnimation animation)
        {
            UpdateAnimations(animation.ToString());
        }

        /// <summary>
        /// Recover player's health points by given amount
        /// </summary>
        /// <param name="value">Amount of health to recover</param>
        public void RecoverHealth(int value)
        {
            StartCoroutine(IncrementRecovery(value, true));
        }
        /// <summary>
        /// Recover player's energy points by given amount
        /// </summary>
        /// <param name="value">Amount of energy to recover</param>
        public void RecoverEnergy(int value)
        {
            StartCoroutine(IncrementRecovery(value, false));
        }
        /// <summary>
        /// Incremental function over time to recover health or energy
        /// </summary>
        /// <param name="value">Amount of health/energy to recover</param>
        /// <param name="health">If true, recoevers health. If false, recovers energy</param>
        /// <returns></returns>
        private IEnumerator IncrementRecovery(int value, bool health)
        {
            for(int i = 0; i < value; i++)
            {
                if(health)
                {
                    if (vars.mainHealth == vars.maxHealth)
                        yield break;
                    vars.mainHealth++;
                }
                else
                {
                    if (vars.mainEnergy == vars.maxEnergy)
                        yield break;
                    vars.mainEnergy++;
                }
                yield return new WaitForSecondsRealtime(0.03333f);
            }
           
        }

        public void UpdateMoney(int changeValue)
        {
            StartCoroutine(IncrementMoney(Math.Abs(changeValue), changeValue > 0));
        }

        private IEnumerator IncrementMoney(int value, bool positive)
        {
            for (int i = 0; i < value; i++)
            {
                if (vars.coinCount < 0)
                {
                    vars.coinCount = 0;
                    yield break;
                }
                if(positive)
                    vars.coinCount++;
                else
                    vars.coinCount--;
                yield return new WaitForSecondsRealtime(0.03333f);
            }

        }
        public void FullRecovery()
        {
            vars.mainHealth = vars.maxHealth;
            vars.mainEnergy = vars.maxEnergy;
        }
        /// <summary>
        /// Sets players speed during TRANSITION state
        /// </summary>
        /// <param name="movementDirection">Direction of transition</param>
        public void SetTransitionSpeed(Vector2 movementDirection)
        {
            move = new Vector2(Math.Abs(move.x)*movementDirection.x, Math.Abs(move.y) * movementDirection.y);
        }

        /// <summary>
        /// Sets players speed during CUTSCENE state
        /// </summary>
        /// <param name="isRight">If true, direction is right. If false, direction is left.</param>
        /// <param name="setZero">Resets speed to zero.</param>
        public void SetCutsceneSpeed(bool isRight, bool setZero = false)
        {
            vars.movement = isRight ? 1 : -1;
            if (setZero) vars.movement = 0;
            move.x =  vars.speed * vars.movement*0.5f;
        }

        #endregion
        #region State Transitions
        /// <summary>
        /// Changes the active state of the player object and uses "EnterState" and "ExitState" functions.
        /// </summary>
        /// <param name="state">The new state to enter.</param>
        public void ChangeState(State state)
        {
            ExitState(vars.activeState);
            EnterState(state);
        }

        /// <summary>
        /// Update methods for state transitions when exiting the current state
        /// </summary>
        /// <param name="state">The state to exit</param>
        private void ExitState(State state)
        {
            move = rb2d.velocity;
            switch (state)
            {
                case State.CONTROL:
                    rb2d.velocity = new Vector2(0, 0);
                    break;
                case State.CHARGING:
                    vars.isCharged = vars.isLaunched = false;                   
                    break;
                case State.CLONING:
                    GetComponent<CloneController>().ExitCloneMenu();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Update methods for state transitions when entering a new state
        /// </summary>
        /// <param name="state">New state to enter</param>
        private void EnterState(State state)
        {
            vars.activeState = state;
            switch (state)
            {
                case State.CONTROL:
                    UpdateAnimations("idle");
                    break;
                case State.CHARGING:
                    vars.chargeTime = 1.0f;
                    UpdateAnimations("charge");
                    break;
                case State.GROUND_POUND:
                    vars.groundPoundTime = 0.2275f*2;
                    UpdateAnimations("groundPound");
                    break;
                case State.CUTSCENE:
                    UpdateAnimations("idle");
                    move = new Vector2(0, rb2d.velocity.y);
                    rb2d.velocity = move;
                    break;
                case State.CLONING:
                    UpdateAnimations("idle");
                    break;
                case State.INACTIVE:
                    UpdateAnimations("idle");
                    vars.movement = 0;
                    break;
                case State.TRANSITION:
                    rb2d.velocity = move;
                    break;
                default:
                    break;
            }
        }
        #endregion
        #region Collision Handling
        private void OnCollisionEnter2D(Collision2D collision)
        {
            switch(vars.activeState)
            {
                default:
                    break;
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            switch (vars.activeState)
            {
                default:
                    break;
            }
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            switch (vars.activeState)
            {
                default:
                    break;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            switch (vars.activeState)
            {
                default:
                    break;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            switch (vars.activeState)
            {
                default:
                    break;
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            switch (vars.activeState)
            {
                default:
                    break;
            }
        }

        //Methods derived from Collisions
        #endregion

    }
}