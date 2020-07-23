using DG.Tweening;
using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.egamesstudios.cell
{
    [RequireComponent(typeof(ScaleInOut))]
    public class CloneMenu : MonoBehaviour
    {
        Player player;
        bool xr, xl, yu, yd;
        int newSelection, currentSelection;

        [SerializeField]
        private CloneMenuBox[] cloneBoxes = new CloneMenuBox[6];
        [SerializeField]
        private GameObject selectionOutline;
        
        private SFXPlayer sfxPlayer;
        // Use this for initialization
        void Start()
        {
            player = ReInput.players.GetPlayer(0);
            sfxPlayer = GetComponent<SFXPlayer>();
            currentSelection = 0;
        }

        // Update is called once per frame
        void LateUpdate()
        {
            cloneBoxes[currentSelection].SelectActive();
            if (gameObject.activeInHierarchy)
            {
                if (VariableContainer.variableContainer.mainCell.transform.localScale.x < 0)
                {
                    //this.transform.localScale = new Vector3(-1f, 1f, 1);
                }
                else
                {
                    //this.transform.localScale = new Vector3(1f, 1f, 1);
                }

                UpdateBoxValidity();
                ScanInput();
            }
        }

        public void EnableMenu(bool enable)
        {
            if (enable)
            {
                gameObject.SetActive(true);
                transform.localScale = Vector3.forward;
            }

            StopAllCoroutines();
            GetComponent<ScaleInOut>().SetX(VariableContainer.variableContainer.mainCell.transform.localScale.x < 0 ? -1 : 1);
            StartCoroutine(GetComponent<ScaleInOut>().Scale(enable, true));
        }

        void UpdateBoxValidity()
        {
            CellVariables vars = VariableContainer.variableContainer.mainCell.vars;
            cloneBoxes[0].SetValidity(vars.allowedClones[0]);
            cloneBoxes[1].SetValidity((vars.hasDoubleJump || vars.hasWallJump) && vars.allowedClones[1]);
            cloneBoxes[2].SetValidity(vars.hasGuitar && vars.allowedClones[2]);
            cloneBoxes[3].SetValidity(vars.hasGun && vars.allowedClones[3] );
            cloneBoxes[4].SetValidity((vars.hasSlingJump || vars.hasChargeDash) && vars.allowedClones[4]);
            cloneBoxes[5].SetValidity(vars.hasGroundPound && vars.allowedClones[5]);
        }

        void ScanInput()
        {

            xr = player.GetButtonDown("Camera X");
            yu = player.GetButtonDown("Camera Y");
            xl = player.GetNegativeButtonDown("Camera X");
            yd = player.GetNegativeButtonDown("Camera Y");

            if (xr || xl)
            {
                newSelection = currentSelection + 3;
            }
            else if (yu || yd)
            {
                if (currentSelection == 0 || currentSelection == 3) newSelection = currentSelection + (yd ? +1 : +2);
                else if (currentSelection == 2 || currentSelection == 5) newSelection = currentSelection + (yd ? -2 : -1);
                else newSelection = currentSelection + (yd ? +1 : -1);
            }
            newSelection %= 6;

            if (newSelection != currentSelection)
            {
                CloneMenuBox oldSelect = cloneBoxes[currentSelection];
                CloneMenuBox newSelect = cloneBoxes[newSelection];
                currentSelection = newSelection;

                selectionOutline.transform.DOLocalMove(newSelect.transform.localPosition, 0.15f);
                sfxPlayer.PlaySFX(0, 1, 1);

                newSelect.SelectActive();
                oldSelect.DeselectActive();
            }
        }

        public CellType CreateClone()
        {
            return cloneBoxes[currentSelection].type;
        }

        public bool CanSelectClone()
        {
            return cloneBoxes[currentSelection].canSelect;
        }
    }
}

