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
        [SerializeField, InlineEditor]
        protected AEnemyData enemyData;

        [FoldoutGroup("State")]
        protected bool alive;
        [FoldoutGroup("State"), SerializeField]
        protected bool isKnocked, castingAttack;
        [FoldoutGroup("Data"), SerializeField]
        protected int enemyID, attackID, health;

        [FoldoutGroup("Timers")]
        protected float timeBetweenAttacks, timeToThaw, timeToStun, timeToBeAware, timeToBeIdle, timeToKnockback, timeToAttack, timeToLoseChase, randomMoveTimer;
        protected float speed;

        [HideInInspector]
        public Vector3 originalPosition;
        private static LayerMask layerMask;

        protected Animator animator;
        protected Rigidbody2D rb;

        private static HashSet<State> validChaseStates;

        [SerializeField]
        [FoldoutGroup("State")]
        protected float knockbackMultiplier;
        [ShowInInspector]
        private EnemyState state;

        // Use this for initialization
        void Awake()
        {
            if(validChaseStates == null)
            {
                validChaseStates = new HashSet<State>() { State.CONTROL, State.GROUND_POUND, State.CLONING, State.CHARGING };
            }

            layerMask = ~LayerMask.GetMask("Enemy", "NPC", "Clone", "Default", "Collectable", "Switch");

            if (SaveManager.saveManager.currentRoom && !SaveManager.saveManager.currentRoom[1,enemyID])
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
            speed = enemyData.speed;
            timeBetweenAttacks = enemyData.TIME_BETWEEN_ATTACKS;
            timeToStun = enemyData.TIME_TO_STUN;
            timeToThaw = enemyData.TIME_TO_THAW;
            timeToBeAware = enemyData.TIME_TO_BE_AWARE;
            timeToBeIdle = enemyData.TIME_TO_BE_IDLE;
            timeToKnockback = enemyData.TIME_TO_KNOCKBACK;
            timeToAttack = enemyData.TIME_TO_ATTACK;
            timeToLoseChase = enemyData.TIME_TO_LOSE_CHASE;
            randomMoveTimer = 0;
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
            transform.GetChild(0).transform.localScale = speed <= 0 ? new Vector3(-1, 1, 1) : Vector3.one;

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

                    rb.velocity = Vector2.right * enemyData.knockbackForce * knockbackMultiplier;
                    timeToKnockback -= Time.fixedDeltaTime;
                }
            }
        }

        #region Update Methods
        protected virtual void OnStart()
        {

        }

        protected virtual void OnDamage()
        {

        }

        protected virtual void OnCollisionWithCell()
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
            health = enemyData.maxHealth;
            rb = GetComponent<Rigidbody2D>();
            alive = true;
            isKnocked = false;
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
                    SaveManager.saveManager.currentRoom[1,enemyID] = SaveManager.saveManager.activeGame.compendiumFlags[1][(int)enemyData.enemyType] = true;
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
            RaycastHit2D hit = Physics2D.Raycast(transform.position, VariableContainer.variableContainer.currentActive.transform.position - transform.position, enemyData.detectionRange, layerMask);
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
            else
            {
                OnDamage();
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
            if(enemyData.canBeKnockbacked)
            {
                float t = source.position.x - transform.position.x;
                t = t > 0 ? -1 : 1;
                foreach (SpriteRenderer spr in GetComponentsInChildren<SpriteRenderer>())
                {
                    spr.color = new Color(1, 0, 1);
                }
                knockbackMultiplier *= t;
                isKnocked = true;
                timeToKnockback = enemyData.TIME_TO_KNOCKBACK;
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

            foreach (ObjectAndValue pickup in enemyData.dropTable.pickups)
            {
                if (pickup != null)
                {
                    if (Random.Range(0f, 1f) <= pickup.probability)
                    {
                        for (int i = 0; i < pickup.quantity; i++)
                        {
                            randomVec = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0) * 0.75f;
                            Instantiate(pickup.gameObject, transform.position + randomVec, Quaternion.identity);
                        }
                    }
                }
            }

            foreach (EnemyDeathFunc func in GetComponentsInChildren<EnemyDeathFunc>())
            {
                func.OnDeath();
            }
        }

        public bool IsGrounded()
        {
            var spr = GetComponentInChildren<SpriteRenderer>();
            RaycastHit2D hit = Physics2D.CircleCast(transform.position,
                0.25f, new Vector2(0, -1), spr.bounds.extents.y, LayerMask.GetMask("Platform"));
            return (hit.collider);
        }

        protected bool CheckWall(Vector2 direction, float distance)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, LayerMask.GetMask("Platform"));
            return (hit.collider);
        }

        protected virtual void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.GetComponent<CellController>())
            {
                collision.gameObject.GetComponent<CellController>().TakeHit(transform, castingAttack ? enemyData.attackDamage : enemyData.contactDamage, DamageType.ENEMY);
                // if (castingAttack) ChangeState(EnemyState.IDLE);
                OnCollisionWithCell();
            }
        }
    }
}