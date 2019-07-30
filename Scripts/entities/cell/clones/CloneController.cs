using System.Collections;
using System.Collections.Generic;
using Com.LuisPedroFonseca.ProCamera2D;
using Rewired;
using UnityEngine;

namespace com.egamesstudios.cell
{
    /// <summary>
    /// Component to manage clones from creation to death
    /// </summary>
    public class CloneController : MonoBehaviour
    {
        /// <summary>
        /// The parent player
        /// </summary>
        private CellController mainCell;
        /// <summary>
        /// The currently active player (Can be parent or clone)
        /// </summary>
        private CellController currentActive;
        /// <summary>
        /// Rewired player reference
        /// </summary>
        private Player player;

        /// <summary>
        /// Index for current active in list
        /// </summary>
        private int index;
        /// <summary>
        /// List of all active players
        /// </summary>
        private List<CellController> clones;
        /// <summary>
        /// Reference for specific type of clone
        /// </summary>
        public GameObject normal, jumper, melee_fighter, ranged_fighter, slinger, ground;
        [SerializeField]
        private SFXPlayer sfx;
        /// <summary>
        /// The UI component for clone creation
        /// </summary>
        [SerializeField]
        private CloneMenu cloneMenu;

        // Use this for initialization
        void Start()
        {
            mainCell = currentActive = GetComponent<CellController>();
            VariableContainer.variableContainer.currentActive = currentActive;

            player = mainCell.player;

            cloneMenu.gameObject.SetActive(false);

            clones = new List<CellController>
            {
                mainCell
            };
        }

        // Update is called once per frame
        private void Update()
        {
            for (int i = 0; i < clones.Count; i++)
            {
                CellController cellTemp = clones[i].GetComponent<CellController>();
                if (cellTemp.vars.isDead && cellTemp.vars.isClone && cellTemp.vars.activeState != State.CONTROL)
                {
                    DestroyInactiveClone(cellTemp);
                }
            }
        }
        void LateUpdate()
        {
            if(mainCell.vars.activeState == State.CLONING)
            {
                if(Input.GetKeyDown(KeyCode.B))
                {
                    mainCell.ChangeState(State.CONTROL);
                }
                if(mainCell.player.GetButtonDown("Confirm"))
                {
                    CreateClone(cloneMenu.CreateClone());
                }
            }
            if(mainCell.vars.activeState == State.CONTROL && mainCell.vars.activeClones < 5 && mainCell.vars.maxHealth > 20)
            {
                if (player.GetButtonDown("CreateClone"))
                {
                    EnterCloneMenu();
                }

            }

            if (mainCell.vars.activeClones > 0 && 
                currentActive.vars.grounded && currentActive.vars.activeState == State.CONTROL) 
            {
                if (player.GetButtonDown("SwapCloneRight"))
                    SwitchClone(true);
                else if (player.GetButtonDown("SwapCloneLeft"))
                    SwitchClone(false);
            }

            if(mainCell.vars.activeClones > 0 && currentActive.vars.isClone && player.GetButtonDown("KillClone"))
            {
                if (currentActive.vars.isGateClone)
                {
                    if ((currentActive.vars.isOnGate && mainCell.vars.isOnGate) || IsInRangeOfCell()) DestroyActiveClone();
                }
                else
                {
                    DestroyActiveClone();
                }
            }
            if(mainCell.vars.activeClones > 0 && currentActive.vars.isClone && currentActive.vars.isDead)
            {
                DestroyActiveClone();
            }
        }

        private bool IsInRangeOfCell()
        {
            var collider = currentActive.GetComponent<CircleCollider2D>();
            var hitInfo = Physics2D.CircleCastAll(collider.transform.position, collider.radius, Vector2.zero, 0);
            foreach(var info in hitInfo)
            {
                if (info.transform.gameObject.Equals(mainCell.gameObject)) return true;
            }
            return false;
        }

        /// <summary>
        /// Sets player to cloning state and activates clone UI
        /// </summary>
        private void EnterCloneMenu()
        {
            mainCell.ChangeState(State.CLONING);
            cloneMenu.gameObject.SetActive(true);
        }

        /// <summary>
        /// Removes clone UI
        /// </summary>
        public void ExitCloneMenu()
        {
            cloneMenu.gameObject.SetActive(false);
        }

        /// <summary>
        /// General method to creating a clone
        /// </summary>
        /// <param name="cellType">Type of clone selected from clone UI</param>
        private void CreateClone(CellType cellType)
        {
            sfx.SetPitch(0.75f);
            sfx.PlaySFX(1);

            GameObject toInstantiate;
            switch (cellType)
            {
                case CellType.M_FIGHTER:
                    toInstantiate = melee_fighter;
                    break;
                case CellType.R_FIGHTER:
                    toInstantiate = ranged_fighter;
                    break;
                case CellType.JUMPER:
                    toInstantiate = jumper;
                    break;
                case CellType.SLINGER:
                    toInstantiate = slinger;
                    break;
                case CellType.GROUND:
                    toInstantiate = ground;
                    break;
                case CellType.NORMAL:
                default:
                    toInstantiate = normal;
                    break;
            }

            CellController newCell = (Instantiate<GameObject>(toInstantiate, mainCell.vars.isOnGate ? mainCell.vars.gateLocation.position + Vector3.up : mainCell.gameObject.transform.position, Quaternion.identity)).GetComponent<CellController>();
            mainCell.vars.activeClones++;

            mainCell.vars.maxHealth -= 20;
            mainCell.vars.mainHealth = mainCell.vars.mainHealth > mainCell.vars.maxHealth ? 
                mainCell.vars.maxHealth : mainCell.vars.mainHealth;

            index = 1;
            clones.Insert(index, newCell);
            VariableContainer.variableContainer.cells.Add(newCell);

            currentActive.ChangeState(State.INACTIVE);
            SetActiveAndCam(newCell);
            currentActive.vars.type = cellType;
            GivePowerups();
            currentActive.vars.isClone = true;
        }

        /// <summary>
        /// General function to switch control between clones
        /// </summary>
        /// <param name="right">If true, indexes + 1, if false, indexes - 1</param>
        private void SwitchClone(bool right)
        {
            sfx.SetPitch(1f);
            sfx.PlaySFX(0);
            index += right ? 1 : -1;
            if (index == clones.Count)
                index = 0;
            if (index < 0)
                index = clones.Count - 1;
            CellController newCell = clones[index];

            currentActive.ChangeState(State.INACTIVE);
            SetActiveAndCam(newCell);
        }

        /// <summary>
        /// Destroys a clone that is currently in control
        /// </summary>
        private void DestroyActiveClone()
        {
            index = 0;
            CellController cloneToKill = currentActive;
            CellController newCell = clones[index];
            SetActiveAndCam(newCell);
            DestroyShared(cloneToKill);
        }  

        /// <summary>
        /// Destroys a clone that is currently NOT in control
        /// </summary>
        /// <param name="cloneToKill">The desired clone to yeet</param>
        private void DestroyInactiveClone(CellController cloneToKill)
        {
            DestroyShared(cloneToKill);
            index = clones.IndexOf(currentActive);
        }

        /// <summary>
        /// Code shared between all types of clone destruction
        /// </summary>
        /// <param name="cloneToKill">The desired clone to yeet</param>
        private void DestroyShared(CellController cloneToKill)
        {
            sfx.SetPitch(1.3f);
            sfx.PlaySFX(1);
            clones.Remove(cloneToKill);
            VariableContainer.variableContainer.cells.Remove(currentActive);
            ReturnPowerups(cloneToKill);
            mainCell.vars.activeClones--;
            mainCell.vars.maxHealth += 20;
            mainCell.vars.mainHealth += cloneToKill.vars.isGateClone ? 20 : 0;
            Destroy(cloneToKill.gameObject);
        }

        /// <summary>
        /// Destroy all created clones and sets main player to active state
        /// </summary>
        public void KillAllClones()
        {
            if(currentActive.vars.isClone)
            {
                index = 0;
                SetActiveAndCam(mainCell);
            }
            int size = clones.Count;
            for (int i = 1; i < size; i++)
            {
                DestroyInactiveClone(clones[1]);
            }
        }

        /// <summary>
        /// Shared code to set a new player/clone as the new active clone/player, manages camera, states and references.
        /// </summary>
        /// <param name="cellToActive">The player/clone to set as the new active</param>
        private void SetActiveAndCam(CellController cellToActive)
        {
            CameraManager.cameraManager.AddNewCell(cellToActive);
            cellToActive.gameObject.layer = 13;
            currentActive.gameObject.layer = 16;
            currentActive = cellToActive;
            currentActive.ChangeState(State.CONTROL);
            VariableContainer.variableContainer.currentActive = currentActive;
        }

        /// <summary>
        /// Based on clone type, manages powerup handing mechanic from original to clone
        /// </summary>
        private void GivePowerups()
        {
            currentActive.vars.isGateClone = mainCell.vars.isOnGate;
            currentActive.vars.checkpoint = mainCell.vars.checkpoint;
            switch(currentActive.vars.type)
            {
                case CellType.M_FIGHTER:
                    currentActive.vars.hasGuitar = mainCell.vars.hasGuitar;
                    mainCell.vars.hasGuitar = false;
                    //Transfer Guitar stats
                    break;
                case CellType.R_FIGHTER:
                    currentActive.vars.hasGun = mainCell.vars.hasGun;
                    mainCell.vars.hasGun = false;
                    //Transfer Gun stats
                    break;
                case CellType.JUMPER:
                    currentActive.vars.hasDoubleJump = mainCell.vars.hasDoubleJump;
                    currentActive.vars.hasTripleJump = mainCell.vars.hasTripleJump;
                    mainCell.vars.hasDoubleJump = false;
                    mainCell.vars.hasTripleJump = false;
                    break;
                case CellType.SLINGER:
                    currentActive.vars.hasWallJump = mainCell.vars.hasWallJump;
                    currentActive.vars.hasSlingJump = mainCell.vars.hasSlingJump;
                    mainCell.vars.hasWallJump = false;
                    mainCell.vars.hasSlingJump = false;
                    break;
                case CellType.GROUND:
                    currentActive.vars.hasGroundPound = mainCell.vars.hasGroundPound;
                    currentActive.vars.hasChargeDash = mainCell.vars.hasChargeDash;
                    mainCell.vars.hasGroundPound = false;
                    mainCell.vars.hasChargeDash = false;
                    break;
            }
        }

        /// <summary>
        /// Based on clone type, manages powerup handing mechanic from clone to original
        /// </summary>
        private void ReturnPowerups(CellController cloneToKill)
        {
            switch (cloneToKill.vars.type)
            {
                case CellType.M_FIGHTER:
                    mainCell.vars.hasGuitar = cloneToKill.vars.hasGuitar;
                    break;
                case CellType.R_FIGHTER:
                    mainCell.vars.hasGun = cloneToKill.vars.hasGun;
                    break;
                case CellType.JUMPER:
                    mainCell.vars.hasDoubleJump = cloneToKill.vars.hasDoubleJump;
                    mainCell.vars.hasTripleJump = cloneToKill.vars.hasTripleJump;
                    break;
                case CellType.SLINGER:
                    mainCell.vars.hasWallJump = cloneToKill.vars.hasWallJump;
                    mainCell.vars.hasSlingJump = cloneToKill.vars.hasSlingJump;
                    break;
                case CellType.GROUND:
                    mainCell.vars.hasGroundPound = cloneToKill.vars.hasGroundPound;
                    mainCell.vars.hasChargeDash = cloneToKill.vars.hasChargeDash;
                    break;
            }
        }
    }
}

