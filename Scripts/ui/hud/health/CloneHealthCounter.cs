using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.egamesstudios.cell
{
    public class CloneHealthCounter : MonoBehaviour
    {
        public Image hpBarMask;
        public TextMeshProUGUI hpValue;

        private HUDCloneIndicator indicator;

        private void Start()
        {
            indicator = GetComponent<HUDCloneIndicator>();
        }

        private void Update()
        {
            if(indicator.gameObject.activeInHierarchy)
            {
                var vars = indicator.clone.vars;

                hpValue.text = "" + vars.mainHealth;
                hpBarMask.fillAmount = (float)vars.mainHealth / vars.maxHealth;
            }
        }
    }
}
