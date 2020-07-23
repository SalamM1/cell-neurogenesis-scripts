using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.egamesstudios.cell
{
    public class StartMenu : MonoBehaviour
    {
        [SerializeField]
        private StartMenuButton[] buttons;
        private int index;
        private Player player;
        private SFXPlayer sfxPlayer;

        // Start is called before the first frame update
        void Start()
        {
            index = 0;
            player = VariableContainer.variableContainer.mainCell.player;
            sfxPlayer = GetComponent<SFXPlayer>();
        }

        // Update is called once per frame
        void Update()
        {
            if (player.GetAxis("Camera Y") != 0 && player.GetAxisPrev("Camera Y") == 0)
            {
                if(player.GetAxis("Camera Y") > 0)
                {
                    TriggerButton(true);
                }
                else if (player.GetAxis("Camera Y") < 0)
                {
                    TriggerButton(false);
                }
            }

            if (player.GetButtonDown("Confirm"))
            {
                buttons[index].OnConfirm(this);
            }
        }

        private void TriggerButton(bool up)
        {
            sfxPlayer.PlaySFX(0, 0.6f, 1.2f);
            buttons[index].Highlight(false);
            if (!up)
            {
                index++;
                if (index >= buttons.Length) index = 0;
            } 
            else
            {
                index--;
                if (index < 0) index = buttons.Length - 1;
            }
            buttons[index].Highlight(true);
        }

        private void OnEnable()
        {
            buttons[index].Highlight(false);
            index = 0;
            buttons[index].Highlight(true);
        }
    }
}
