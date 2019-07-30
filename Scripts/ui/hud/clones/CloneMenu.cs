using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.egamesstudios.cell
{
    public class CloneMenu : MonoBehaviour
    {
        Player player;
        float x, y, angle;
        int newSelection, currentSelection;

        [SerializeField]
        private CloneMenuBox[] cloneBoxes = new CloneMenuBox[6];
        // Use this for initialization
        void Start()
        {
            player = ReInput.players.GetPlayer(0);
            currentSelection = 0;
                        cloneBoxes[currentSelection].SelectActive();
        }

        // Update is called once per frame
        void LateUpdate()
        {
            if (gameObject.activeInHierarchy)
            {
                if (VariableContainer.variableContainer.mainCell.transform.localScale.x < 0)
                {
                    this.transform.localScale = new Vector3(-1.5f, 1.5f, 1);
                }
                else
                {
                    this.transform.localScale = new Vector3(1.5f, 1.5f, 1);
                }

                UpdateBoxValidity();
                ScanInput();
            }
        }

        void UpdateBoxValidity()
        {
            CellVariables vars = VariableContainer.variableContainer.mainCell.vars;
            cloneBoxes[0].SetValidity(true);
            cloneBoxes[1].SetValidity(vars.hasDoubleJump || vars.hasWallJump);
            cloneBoxes[2].SetValidity(vars.hasGuitar);
            cloneBoxes[3].SetValidity(vars.hasGun);
            cloneBoxes[4].SetValidity(vars.hasSlingJump || vars.hasChargeDash);
            cloneBoxes[5].SetValidity(vars.hasGroundPound);
        }

        void ScanInput()
        {
            x = player.GetAxis("Camera X");
            y = player.GetAxis("Camera Y");
            angle = ((Mathf.Atan2(y, x)*Mathf.Rad2Deg)) + 30;
            if (angle <= -135 || angle >= 195) newSelection = 3; //D
            else if (angle >= 135 && angle <= 165) newSelection = 2; //C
            else if (angle >= 75 && angle <= 105) newSelection = 1; //B
            else if (angle >= 15 && angle <= 45) newSelection = 0; //A
            else if (angle >= -45 && angle <= -15) newSelection = 5; //F
            else if (angle >= -105 && angle <= -75) newSelection = 4; //E
            else newSelection = currentSelection;

            if (newSelection != currentSelection)
            {
                CloneMenuBox oldSelect = cloneBoxes[currentSelection];
                CloneMenuBox newSelect = cloneBoxes[newSelection];
                if (newSelect.canSelect && oldSelect.canSelect)
                {
                    currentSelection = newSelection;
                    newSelect.SelectActive();
                    oldSelect.DeselectActive();
                }
            }
            if(currentSelection == 0)
            {
                cloneBoxes[0].SelectActive();
            }
        }

        public CellType CreateClone()
        {
            if(currentSelection > 5)
            {
                currentSelection = 0;
            }
            int val = currentSelection;
            currentSelection = 0;
            return cloneBoxes[val].cloneDetails.type;
        }
    }
}

