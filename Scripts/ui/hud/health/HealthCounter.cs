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
        [OdinSerialize]
        public Image hpBar;

        [OdinSerialize]
        public Text hpValue;
        [OdinSerialize]
        public Image epBar;

        [OdinSerialize]
        public Text epValue;
        void Update()
        {
            hpValue.text = "" + VariableContainer.variableContainer.mainCell.vars.mainHealth;
            hpBar.fillAmount = (float)VariableContainer.variableContainer.mainCell.vars.mainHealth / VariableContainer.variableContainer.mainCell.vars.maxHealth;
            epValue.text = "" + VariableContainer.variableContainer.mainCell.vars.mainEnergy;
            epBar.fillAmount = (float)VariableContainer.variableContainer.mainCell.vars.mainEnergy / VariableContainer.variableContainer.mainCell.vars.maxEnergy;
        }
    }
}

