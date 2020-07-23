using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.egamesstudios.cell
{
    public class HealthCounter : MonoBehaviour
    {
        public Image hpBarMask;
        private Image hpBarTile;
        public TextMeshProUGUI hpValue;

        public Image epBarMask;
        private Image epBarTile;
        public TextMeshProUGUI epValue;

        public Image hpBarBorder;
        public Image epBarBorder;

        private readonly float healthWidthMin = 162f; //width for 20hp
        private readonly float healthWidthBase = 18f; //per 10 hp after 20, or 20 energy after 100
        private readonly float healthBarDiff = 16f;
        private readonly float healthTextDiff = 389f;

        private readonly float energyWidthMin = 198f;
        private readonly float energyTextDiff = 389f;

        private void Start()
        {
            hpBarTile = hpBarMask.GetComponentsInChildren<Image>()[1];
            epBarTile = epBarMask.GetComponentsInChildren<Image>()[1];
        }
        void Update()
        {
            hpValue.text = "" + VariableContainer.variableContainer.mainCell.vars.mainHealth;
            hpBarMask.fillAmount = (float)VariableContainer.variableContainer.mainCell.vars.mainHealth / VariableContainer.variableContainer.mainCell.vars.maxHealth;
            epValue.text = "" + VariableContainer.variableContainer.mainCell.vars.mainEnergy;
            epBarMask.fillAmount = (float)VariableContainer.variableContainer.mainCell.vars.mainEnergy / VariableContainer.variableContainer.mainCell.vars.maxEnergy;

            var hpTemp = healthWidthMin + Mathf.Max(0, healthWidthBase * (VariableContainer.variableContainer.mainCell.vars.maxHealth - 20) / 10);
            hpBarBorder.rectTransform.sizeDelta = new Vector2(hpTemp, hpBarBorder.rectTransform.sizeDelta.y);
            hpBarMask.rectTransform.sizeDelta = hpBarTile.rectTransform.sizeDelta = new Vector2(hpTemp - healthBarDiff, hpBarMask.rectTransform.sizeDelta.y);
            hpValue.rectTransform.localPosition = new Vector3(hpTemp - healthTextDiff, hpValue.rectTransform.localPosition.y, 0);

            var epTemp =energyWidthMin + Mathf.Max(0, healthWidthBase * (VariableContainer.variableContainer.mainCell.vars.maxEnergy - 100) / 20);
            epBarBorder.rectTransform.sizeDelta = new Vector2(epTemp, epBarBorder.rectTransform.sizeDelta.y);
            epBarMask.rectTransform.sizeDelta = epBarTile.rectTransform.sizeDelta = new Vector2(epTemp - healthBarDiff, epBarMask.rectTransform.sizeDelta.y);
            epValue.rectTransform.localPosition = new Vector3(epTemp - energyTextDiff, epValue.rectTransform.localPosition.y, 0);
        }
    }
}

