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
        private bool triggered;
        private IInteractable interactableObject;
        [SerializeField]
        private GameObject indicator;
        
        
        public void TriggerDialogue()
        {
            if (DialogueManager.dialogueManager.isTriggered)
            {
                DialogueManager.dialogueManager.DisplayNextSentence();
                if (!DialogueManager.dialogueManager.isTriggered)
                {
                    interactableObject.EndInteraction();
                }
            }
            else
            {
                DialogueManager.dialogueManager.StartDialogue(dialogue);
                interactableObject.TriggerInteraction(VariableContainer.variableContainer.currentActive.transform);
            }
 
        }

        private void Start()
        {
            interactableObject = GetComponent<IInteractable>();
            player = ReInput.players.GetPlayer(0);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag.Equals("Cell") && collision.gameObject.Equals(VariableContainer.variableContainer.currentActive.gameObject))
            {
                triggered = true;
                indicator.SetActive(true);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.tag.Equals("Cell"))
            {
                triggered = false;
                indicator.SetActive(false);
            }
        }

        private void Update()
        {
            if (player.GetButtonDown("Interact") && triggered &&  DialogueManager.dialogueManager.interactableState.Contains(VariableContainer.variableContainer.currentActive.vars.activeState))
            {
                TriggerDialogue();
            }
        }
    }

}
