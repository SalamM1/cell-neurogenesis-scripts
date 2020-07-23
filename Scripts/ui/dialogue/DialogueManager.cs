using Sirenix.Serialization;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace com.egamesstudios.cell
{
    public enum Language
    {
        _ENG,
        _ITA,
        _ARA,
    }
    public class DialogueManager : MonoBehaviour
    {

        public TextMeshProUGUI nameText;
        public TextMeshProUGUI dialogueText;
        [HideInInspector]
        public bool isTriggered;
        [SerializeField, EnumToggleButtons]
        private Language languageSeting;

        public static DialogueManager dialogueManager;

        [OdinSerialize]
        public DialogueWrapper[] loadedDialogue;
        private DialogueWrapper[] cutsceneText;
        private Queue<string> sentences;
        [SerializeField]
        private SFXPlayer sfx;
        public HashSet<State> interactableState;

        private Coroutine cutsceneTextBox;
        private Coroutine textTyping;

        private DialogueTrigger currentDialogueTarget;

        void Awake()
        {
            if (dialogueManager == null)
                dialogueManager = this;
            else if (dialogueManager != this)
                Destroy(gameObject);
            DontDestroyOnLoad(gameObject);

            sentences = new Queue<string>();
            interactableState = new HashSet<State>() { State.CONTROL , State.DIALOGUE};
        }

        /// <summary>
        /// Loads dialogue from a JSON file. File syntax: dialogue/sceneName/sceneName + languageSetting.JSON
        /// </summary>
        /// <param name="scene">The name of the scene loaded t</param>
        public void LoadDialogueFile(string scene)
        {
            try
            {
                string content = ((TextAsset)Resources.Load("dialogue/" + scene + "/" + scene + languageSeting.ToString(), typeof(TextAsset))).text;
                loadedDialogue = JsonHelper.FromJson<DialogueWrapper>(content);
            } catch (Exception e)
            {
                Debug.LogWarning(e);
            }
        }

        internal void StartDialogue(Dialogue dialogue, DialogueTrigger dialogueTrigger)
        {
            
            sentences.Clear();
            UIManager.uIManager.ChangeState(UIState.DIALOGUE);
            currentDialogueTarget = dialogueTrigger;
            isTriggered = true;
            nameText.text = loadedDialogue[dialogue.id][0];

            for (int i = 1; i < loadedDialogue[dialogue.id].dialogue.Length; i++)
            {
                sentences.Enqueue(loadedDialogue[dialogue.id][i]);
            }

            DisplayNextSentence(dialogueTrigger);
        }

        public bool DisplayNextSentence(DialogueTrigger dialogueTrigger)
        {
            if (!isTriggered || dialogueTrigger != currentDialogueTarget)
                return false;
            sfx.PlaySFX(0);
            if (sentences.Count == 0)
            {
                EndDialogue();
                return false;
            }

            if (textTyping != null)
            {
                StopCoroutine(textTyping);
                textTyping = null;
            }
            textTyping = StartCoroutine(TypeSentence(sentences.Dequeue()));
            return true;
        }

        public void EndDialogue()
        {
            StopAllCoroutines();
            dialogueText.text = "";
            nameText.text = "";
            ExitState();

            if(UIManager.uIManager.state == UIState.DIALOGUE)
            {
                UIManager.uIManager.ChangeState(UIState.INGAME);
            }
        }

        public void ExitState()
        {
            sentences.Clear();
            isTriggered = false;
            currentDialogueTarget = null;
        }

        internal void SetCutsceneDialogue(int dialogueID, string cutsceneName)
        {
            sentences.Clear();
            string content = ((TextAsset)Resources.Load("cutscene/" + cutsceneName + "/" + cutsceneName + languageSeting.ToString(), typeof(TextAsset))).text;
            cutsceneText = JsonHelper.FromJson<DialogueWrapper>(content);
            isTriggered = true;
            nameText.text = cutsceneText[dialogueID][0];

            for (int i = 1; i < cutsceneText[dialogueID].dialogue.Length; i++)
            {
                sentences.Enqueue(cutsceneText[dialogueID][i]);
            }
            if(cutsceneTextBox != null)
            {
                StopCoroutine(cutsceneTextBox);
                cutsceneTextBox = null;
            }
            StartCoroutine(UIManager.uIManager.UIDialogueBox(UIManager.uIManager.dialogue.GetComponentInChildren<Image>(), true));
            DisplayNextSentenceCutscene();
        }

        public bool DisplayNextSentenceCutscene()
        {
            sfx.PlaySFX(0);
            if (sentences.Count == 0)
            {
                EndDialogueCutscene();
                return true;
            }

            if (textTyping != null)
            {
                StopCoroutine(textTyping);
                textTyping = null;
            }
            textTyping = StartCoroutine(TypeSentence(sentences.Dequeue()));
            return false;
        }

        void EndDialogueCutscene()
        {
            StopAllCoroutines();
            cutsceneTextBox = StartCoroutine(UIManager.uIManager.UIDialogueBox(UIManager.uIManager.dialogue.GetComponentInChildren<Image>(), false));
            dialogueText.text = "";
            nameText.text = "";
            isTriggered = false;
        }

        public bool CompareCurrentDialogue(DialogueTrigger dialogueTrigger)
        {
            return dialogueTrigger == currentDialogueTarget;
        }

        IEnumerator TypeSentence(string s)
        {
            dialogueText.text = "";
            foreach (char c in s.ToCharArray())
            {
                dialogueText.text += c;
                yield return new WaitForSecondsRealtime(1/60);
            }
        }
        internal void ClearTextBoxes()
        {
            nameText.text = dialogueText.text = "";
        }
    }
}
