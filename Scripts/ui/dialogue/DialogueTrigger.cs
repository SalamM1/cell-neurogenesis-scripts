using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.egamesstudios.cell
{
    public class DialogueTrigger : MonoBehaviour
    {

        public Dialogue dialogue;
        private Player player;
        private bool inStartRange, triggered;
        private IInteractable interactableObject;
        [SerializeField]
        private GameObject indicator;
        private const float detectionRange = 11f;
        private void Start()
        {
            interactableObject = GetComponent<IInteractable>();
            player = ReInput.players.GetPlayer(0);
        }
        private void Update()
        {
            if (player.GetButtonDown("Interact") && (inStartRange || triggered) && DialogueManager.dialogueManager.interactableState.Contains(VariableContainer.variableContainer.currentActive.vars.activeState))
            {
                TriggerDialogue();
            }
            if (triggered && CellDistance() > detectionRange)
            {
                DialogueManager.dialogueManager.EndDialogue();
                DisplayNext();
            }
        }

        public void TriggerDialogue()
        {
            if (DialogueManager.dialogueManager.isTriggered)
            {
                if (inStartRange)
                {
                    if (DialogueManager.dialogueManager.CompareCurrentDialogue(this))
                    {
                        DisplayNext();
                    }
                    else
                    {
                        StartDialogue();
                    }
                }
                else
                {
                    DisplayNext();
                }
            }
            else
            {
                StartDialogue();
            }
 
        }

        private void DisplayNext()
        {
            if (!DialogueManager.dialogueManager.DisplayNextSentence(this))
            {
                interactableObject.EndInteraction();
                triggered = false;
            }
        }

        private void StartDialogue()
        {
            DialogueManager.dialogueManager.StartDialogue(dialogue, this);
            triggered = true;
            interactableObject.TriggerInteraction(VariableContainer.variableContainer.currentActive.transform);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag.Equals("Cell") && collision.gameObject.Equals(VariableContainer.variableContainer.currentActive.gameObject))
            {
                inStartRange = true;
                indicator.SetActive(true);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.tag.Equals("Cell"))
            {
                inStartRange = false;
                indicator.SetActive(false);
            }
        }

        protected float CellDistance()
        {
            return (Vector3.Distance(VariableContainer.variableContainer.currentActive.transform.position, transform.position));
        }
    }

}
