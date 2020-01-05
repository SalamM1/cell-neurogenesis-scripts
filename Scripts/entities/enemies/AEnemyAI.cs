using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace com.egamesstudios.cell
{
    public enum EnemyState
    {
        IDLE, AWARE, ACTION, FROZEN, STUNNED, DEAD
    }
    [RequireComponent(typeof(Rigidbody2D))]
    /**
     * Base class to handle generic enemy AI
     */
    public abstract class AEnemyAI : MonoBehaviour
    {
        [FoldoutGroup("State")]
        protected bool alive;
        [FoldoutGroup("State"), SerializeField]
        protected bool canThaw, canBeKnockbacked, isKnocked, castingAttack;
        [FoldoutGroup("Data"), SerializeField]
        protected int enemyID, attackID, health, contactDamage, attackDamage;
        [FoldoutGroup("Data"), SerializeField]
        protected float speed, detectionRange, awareSpeedFactor;
        [FoldoutGroup("Timers"), SerializeField]
        protected float timeBetweenAttacks, timeToThaw, timeToStun, timeToBeAware, timeToBeIdle, timeToKnockback, timeToAttack;
        protected float TIME_BETWEEN_ATTACKS;
        protected float TIME_TO_THAW;
        protected float TIME_TO_STUN;
        protected float TIME_TO_BE_AWARE;
        protected float TIME_TO_BE_IDLE;
        protected float TIME_TO_KNOCKBACK;
        protected float TIME_TO_ATTACK;

        [HideInInspector]
        public Vector3 originalPosition;
        [SerializeField]
        private EnemyType enemyType;
        private LayerMask layerMask;

        protected Animator animator;
        protected Rigidbody2D rb;

        [SerializeField]
        [FoldoutGroup("Data")]
        protected DropTable dropTable;
        private static HashSet<State> validChaseStates;

        [SerializeField]
        [FoldoutGroup("State")]
        protected float knockbackForce, knockbackMultiplier;
        [ShowInInspector]
        private EnemyState state;

        // Use this for initialization
        void Awake()
        {
            if(validChaseStates == null)
            {
                validChaseStates = new HashSet<State>() { State.CONTROL, State.GROUND_POUND, State.CLONING, State.CHARGING };
            }
            if(SaveManager.saveManager.currentRoom && !SaveManager.saveManager.currentRoom[1,enemyID])
            {
                SetStats();
            }  
            else
            {
                alive = false;
                Destroy(gameObject);
            }
        }

        void Start()
        {
            this.originalPosition = transform.position;
            TIME_BETWEEN_ATTACKS = timeBetweenAttacks;
            TIME_TO_STUN = timeToStun;
            TIME_TO_THAW = timeToThaw;
            TIME_TO_BE_AWARE = timeToBeAware;
            TIME_TO_BE_IDLE = timeToBeIdle;
            TIME_TO_KNOCKBACK = timeToKnockback;
            TIME_TO_ATTACK = timeToAttack;
            OnStart();
        }

        // Update is called once per frame
        void Update()
        {
            if (!validChaseStates.Contains(VariableContainer.variableContainer.currentActive.vars.activeState))
            {
                if(state != EnemyState.IDLE)
                {
                    ChangeState(EnemyState.IDLE);
                }
                rb.velocity = Vector2.zero;
            }
            else
            {
                OnUpdate();
                switch (state)
                {
                    case EnemyState.IDLE:
                        WhileIdle();
                        break;
                    case EnemyState.AWARE:
                        WhileAware();
                        break;
                    case EnemyState.ACTION:
                        WhileAction();
                        break;
                    case EnemyState.FROZEN:
                        WhileFrozen();
                        break;
                    case EnemyState.STUNNED:
                        WhileStunned();
                        break;
                    case EnemyState.DEAD:
                        WhileDead();
                        break;
                }
            }
        }

        private void FixedUpdate()
        {
            if (isKnocked)
            {
                if (timeToKnockback < 0)
                {
                    foreach (SpriteRenderer spr in GetComponentsInChildren<SpriteRenderer>())
                    {
                        spr.color = Color.white;
                    }
                    isKnocked = false;
                    rb.velocity = Vector2.zero;
                    knockbackMultiplier = 1;
                }
                else
                {

                    rb.velocity = Vector2.right * knockbackForce * knockbackMultiplier;
                    timeToKnockback -= Time.fixedDeltaTime;
                }
            }
        }

        #region Update Methods
        protected virtual void OnStart()
        {

        }

        protected virtual void OnUpdate()
        {

        }

        protected virtual void WhileIdle()
        {

        }

        protected virtual void WhileAware()
        {

        }

        protected virtual void WhileAction()
        {

        }

        protected virtual void WhileFrozen()
        {

        }

        protected virtual void WhileStunned()
        {

        }

        protected virtual void WhileDead()
        {

        }

        protected virtual void SetStats()
        {
            state = EnemyState.IDLE;
            rb = GetComponent<Rigidbody2D>();
            alive = true;
            isKnocked = false;
            layerMask = ~LayerMask.GetMask("Enemy", "NPC", "Clone", "Default", "Collectable", "Switch");
            animator = GetComponentInChildren<Animator>();
            knockbackMultiplier = 1;
        }
        #endregion

        public void ChangeState(EnemyState newState)
        {
            ExitState(state);
            EnterState(newState);
        }

        protected virtual void EnterState(EnemyState newState)
        {
            switch (newState)
            {
                case EnemyState.IDLE:
                    UpdateAnimations("idle");
                    break;
                case EnemyState.AWARE:
                    UpdateAnimations("aware");
                    break;
                case EnemyState.ACTION:
                    UpdateAnimations("action");
                    UpdateAnimations("attack" + attackID);
                    break;
                case EnemyState.FROZEN:
                    UpdateAnimations("frozen");
                    break;
                case EnemyState.STUNNED:
                    UpdateAnimations("stunned");
                    break;
                case EnemyState.DEAD:
                    DropItems();
                    UpdateAnimations("dead");
                    alive = false;
                    SaveManager.saveManager.currentRoom[1,enemyID] = SaveManager.saveManager.activeGame.compendiumFlags[1][(int)enemyType] = true;
                    break;
            }
            state = newState;
        }

        protected abstract void ExitState(EnemyState oldState);

        protected bool CheckForGround(Vector2 offset)
        {
            var spr = GetComponentInChildren<SpriteRenderer>();
            RaycastHit2D hit = Physics2D.CircleCast(transform.position + (Vector3)offset,
                0.25f, new Vector2(0, -1), spr.bounds.extents.y, LayerMask.GetMask("Platform"));
            return (hit.collider);
        }

        protected void UpdateAnimations(string trigger)
        {
            animator.SetTrigger(trigger);
        }

        //Returns the absolute X or Y distance from Cell (whichever is higher)
        protected float CellDetectionDistance()
        {
            return Mathf.Max(Mathf.Abs(transform.position.x - VariableContainer.variableContainer.currentActive.transform.position.x),
                Mathf.Abs(transform.position.y - VariableContainer.variableContainer.currentActive.transform.position.y));
        }

        protected bool CastRayToCell()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, VariableContainer.variableContainer.currentActive.transform.position - transform.position, detectionRange, layerMask);
            if(hit) return hit.collider.gameObject.layer == 13 || hit.collider.gameObject.layer == 14;
            return false;
        }

        protected float CellDistance()
        {
            return (Vector3.Distance(VariableContainer.variableContainer.currentActive.transform.position, transform.position));
        }

        public void Damage(int damage)
        {
            ParticleManager.particleManager.PlayParticle(FXType.ENEMY, 1, transform.position);
            health -= damage;
            GetComponent<SFXPlayer>().PlaySFX(0, 0.5f, health <= 0 ? 0.75f : 1.75f);
            if (health <= 0)
            {
                ChangeState(EnemyState.DEAD);
            }
        }

        public void Damage(int damage, Transform source)
        {
            Damage(damage);
            Knockback(source);
            
        }

        public void Damage(int damage, Transform source, float knockbackMultiplier)
        {
            Damage(damage);
            Knockback(source, knockbackMultiplier);
        }

        public void Knockback(Transform source)
        {
            if(canBeKnockbacked)
            {
                float t = source.position.x - transform.position.x;
                t = t > 0 ? -1 : 1;
                foreach (SpriteRenderer spr in GetComponentsInChildren<SpriteRenderer>())
                {
                    spr.color = new Color(1, 0, 1);
                }
                knockbackForce = Mathf.Abs(knockbackForce) * t;
                isKnocked = true;
                timeToKnockback = TIME_TO_KNOCKBACK;
            }
        }

        public void Knockback(Transform source, float knockbackMultiplier)
        {
            this.knockbackMultiplier = knockbackMultiplier;
            Knockback(source);
        }

        protected void DropItems()
        {
            Vector3 randomVec;

            foreach (GameObject pickup in dropTable.pickups)
            {
                if (pickup)
                {
                    randomVec = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0) * 0.75f;
                    Instantiate(pickup, transform.position + randomVec, Quaternion.identity);
                }
            }
        }

        public bool IsGrounded()
        {
            var spr = GetComponentInChildren<SpriteRenderer>();
            RaycastHit2D hit = Physics2D.CircleCast(transform.position,
                0.25f, new Vector2(0, -1), spr.bounds.extents.y, LayerMask.GetMask("Platform"));
            return (hit.collider);
        }

        protected virtual void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.GetComponent<CellController>())
            {
                collision.gameObject.GetComponent<CellController>().TakeHit(transform, castingAttack ? attackDamage : contactDamage, DamageType.ENEMY);
                if (castingAttack) ChangeState(EnemyState.IDLE);
            }
        }
    }
}