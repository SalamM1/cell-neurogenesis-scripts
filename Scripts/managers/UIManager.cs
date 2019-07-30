using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace com.egamesstudios.cell
{
    public enum UIState
    {
        MAINMENU,
        STARTMENU,
        SELECTMENU,
        INGAME,
        TRANSITION,
        DIALOGUE,
        CUTSCENE,
    }

    public class UIManager : MonoBehaviour
    {
        public static UIManager uIManager;
        public UIState state = UIState.INGAME;
        public GameObject hud, dialogue, shop, startMenu, mainMenu, transition, selectMenu;
        private float timeScale, fadeTime;

        void Awake()
        {

            if (uIManager == null)
                uIManager = this;
            else if (uIManager != this)
                Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
        }

        public void ChangeState(UIState newState, bool dead = false)
        {
            fadeTime = dead ? 2 : 0;
            ExitState();
            EnterState(newState);
        }

        void ExitState()
        {
            switch (state)
            {
                case UIState.MAINMENU:
                    break;
                case UIState.STARTMENU:
                    break;
                case UIState.SELECTMENU:
                    break;
                case UIState.INGAME:
                    hud.SetActive(false);
                    break;
                case UIState.TRANSITION:
                    StartCoroutine(UIFade(transition.GetComponentInChildren<Image>(), false));
                    Time.timeScale = timeScale;
                    break;
                case UIState.DIALOGUE:
                    StartCoroutine(UIDialogueBox(dialogue.GetComponentInChildren<Image>(), false));
                    break;
                case UIState.CUTSCENE:
                    break;
            }
        }

        void EnterState(UIState newState)
        {
            state = newState;
            switch (state)
            {
                case UIState.MAINMENU:
                    break;
                case UIState.STARTMENU:
                    break;
                case UIState.SELECTMENU:
                    break;
                case UIState.INGAME:
                    hud.SetActive(true);
                    break;
                case UIState.TRANSITION:
                    StopAllCoroutines();
                    transition.SetActive(true);
                    StartCoroutine(UIFade(transition.GetComponentInChildren<Image>(), true));
                    timeScale = Time.timeScale;
                    Time.timeScale = 0;
                    break;
                case UIState.DIALOGUE:
                    StopAllCoroutines();
                    StartCoroutine(UIDialogueBox(dialogue.GetComponentInChildren<Image>(), true));
                    break;
                case UIState.CUTSCENE:                    
                    break;
            }
        }

        private IEnumerator UIFade(Image image, bool fadeIn)
        {
            // fade from opaque to transparent
            if (!fadeIn)
            {
                for (float i = 1; i >= 0; i -= Time.deltaTime*0.8f)
                {
                    image.color = new Color(1, 1, 1, i);
                    yield return null;
                }

                image.color = new Color(1, 1, 1, 0);
                transition.SetActive(false);
            }
            // fade from transparent to opaque
            else
            {
                yield return new WaitForSecondsRealtime(fadeTime);
                for (float i = 0; i <= 1; i += 0.003f + 0.003f*(4f-fadeTime))
                {
                    image.color = new Color(1, 1, 1, i);
                    yield return null;
                }
                image.color = new Color(1, 1, 1, 1);
            }
        }

        public IEnumerator UIDialogueBox(Image image, bool appear)
        {
            if(appear)
            {
                dialogue.SetActive(true);
                while(image.rectTransform.localScale.x < 1)
                {
                    image.rectTransform.localScale += new Vector3(Time.deltaTime*12, Time.deltaTime * 12, 0);
                    if(image.rectTransform.localScale.x > 1) image.rectTransform.localScale = Vector3.one;
                    yield return new WaitForEndOfFrame();
                }
            }
            else
            {
                while (image.rectTransform.localScale.x > 0)
                {
                    image.rectTransform.localScale -= new Vector3(Time.deltaTime*15, Time.deltaTime * 15, 0);
                    yield return new WaitForEndOfFrame();
                }
                image.rectTransform.localScale = new Vector3(0, 0, 1);
                dialogue.SetActive(false);
            }

        }

    }
}
