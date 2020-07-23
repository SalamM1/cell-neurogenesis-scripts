using DG.Tweening;
using Rewired;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace com.egamesstudios.cell
{
    public class StartMenuButton : MonoBehaviour
    {
        [SerializeField]
        private GameObject settingsMenu;
        private TextMeshProUGUI text;

        // Update is called once per frame
        public void Highlight(bool activate)
        {
            if (!text) text = GetComponent<TextMeshProUGUI>();

            if (activate)
            {
                text.rectTransform.DOScale(1.25f, 0.35f).SetLoops(-1, LoopType.Yoyo).SetUpdate(true).Play();
            }
            else
            {
                DOTween.Kill(text.rectTransform);
                text.rectTransform.localScale = Vector3.one;
            }
        }
        public void OnConfirm(StartMenu menuCalling)
        {
            settingsMenu.SetActive(true);
            menuCalling.gameObject.SetActive(false);
        }

        public virtual void OnConfirm(VideoSettings videoSetting)
        {

        }
    }
}
