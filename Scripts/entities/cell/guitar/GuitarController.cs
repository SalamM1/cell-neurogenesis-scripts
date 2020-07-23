using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.egamesstudios.cell
{
    public class GuitarController : MonoBehaviour
    {
        /// <summary>
        /// The centre transform for the guitar hitbox.
        /// </summary>
        public Transform guitar_centre;
        /// <summary>
        /// Reference to parent
        /// </summary>
        private CellController cell;
        /// <summary>
        /// The time between attacks
        /// </summary>
        [SerializeField, Tooltip("The time between attacks")]
        private readonly float TIMER = 0.4f;
        /// <summary>
        /// The length of the attack/swing
        /// </summary>
        [SerializeField, Tooltip("The length of the attack/swing")]
        private readonly float ATTACK_LENGTH = 0.2f;
        /// <summary>
        /// The maximum amount of charge allowed
        /// </summary>
        [SerializeField, Tooltip("The maximum amount of charge allowed")]
        private readonly float CHARGE_MAX = 1.0f;
        /// <summary>
        /// Updatable timer to check time between attacks
        /// </summary>
        private float timer;
        [SerializeField]
        private SFXPlayer sfx;
        [SerializeField]
        private Sprite guitarImage;
        [SerializeField]
        private GameObject guitarImageObj;
        private HashSet<GameObject> hitObjects;

        /// <summary>
        /// Updatable charge value used to determine charge multiplier
        /// </summary>
        private float chargeValue;
        /// <summary>
        /// Multiplier to increase damage based on length of a precharge
        /// </summary>
        private float chargeMultiplier;
        /// <summary>
        /// If true, currently swinging
        /// </summary>
        private bool attacking;
        /// <summary>
        /// Updatable timer to check time between attacks
        /// </summary>
        private float attackLength;

        /// <summary>
        /// The size of the hitbox
        /// </summary>
        public Vector2 boxSize;
        
        void Start()
        {
            hitObjects = new HashSet<GameObject>();
            cell = GetComponentInParent<CellController>();
            timer = TIMER;
        }

        void Update()
        {
            //increment cooldown timer and timer for ongoing attack
            if (timer > 0) timer -= Time.deltaTime * (cell.vars.swingSpeedModifier);
            if (attacking) attackLength -= Time.deltaTime;
            if (attackLength <= 0)
            {
                attacking = false;
                guitarImageObj.transform.localScale = new Vector3(1, 1, 1);
                attackLength = ATTACK_LENGTH;
            }
            //Check if Cell is in control and can input attacks
            if(cell.vars.activeState == State.CONTROL)
            {
                //Increase charge on button being held down; can do while on cooldown. Scales and plays a particle effect
                if (cell.player.GetButton("Guitar") && !attacking)
                {
                    chargeValue = Mathf.Min(chargeValue + Time.deltaTime, CHARGE_MAX);
                    chargeMultiplier = (chargeValue / (CHARGE_MAX));
                    guitarImageObj.transform.localScale = new Vector3(chargeMultiplier*0.4f + 1, chargeMultiplier*0.4f + 1, 1);
                    chargeMultiplier++;
                    // TODO: Add color and particle updater, add sounds
                }

                //On release, do the attack
                if (cell.player.GetButtonUp("Guitar") || cell.player.GetButtonDown("Guitar"))
                {
                    if (timer <= 0)
                    {
                        Attack();
                    }
                }
            }
            

            //If Cell is currently attacking, then do the boxcast magic on each frame, saving enemies hit in the current attack
            if(attacking)
            {
                guitarImageObj.GetComponent<SpriteRenderer>().sprite = null;
                RaycastHit2D[] hitInfo = Physics2D.CircleCastAll((Vector2)guitar_centre.position, boxSize.x*0.5f*chargeMultiplier, Vector2.zero);
                int damage = (int)(cell.vars.guitarDamage * chargeMultiplier);
                foreach (RaycastHit2D h in hitInfo)
                {
                    GameObject go = h.collider.gameObject;
                    if (!hitObjects.Contains(go))
                        switch (go.tag)
                        {
                            case "Enemy":
                                go.GetComponent<AEnemyAI>().Damage(damage, cell.transform);
                                cell.Knockback(go.transform, cell.vars.pushbackModifier, true, 0.1f);
                                break;
                            case "Switch":

                                if (go.GetComponent<ASwitch>() != null)
                                {
                                    go.GetComponent<ASwitch>().TriggerSwitch(SwitchType.HITABLE, HitableType.GUITAR);
                                }
                                break;
                            case "Projectile":
                            // TODO: Handle Projectile Parrying
                            case "Destructable":
                                if (go.GetComponent<Destructable>() != null)
                                {
                                    go.GetComponent<Destructable>().Damage(HitableType.GUITAR, damage);
                                }
                                break;
                            default:
                                break;
                        }
                    hitObjects.Add(go);
                }
                timer = TIMER;
            }
            else
            {
                guitarImageObj.GetComponent<SpriteRenderer>().sprite = guitarImage;
            }
        }
        //Resets charge value, updates animations, sets attacking state, and clears attack buffer
        public void Attack()
        {
            hitObjects.Clear();
            cell.UpdateAnimations("attack");
            cell.UpdateAnimations("swing");
            chargeValue = 0;
            attacking = true;
        }

        void OnDrawGizmosSelected()
        {
            //Draws Hitbox
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(guitar_centre.position, boxSize.x*chargeMultiplier*0.5f);
        }
    }
}