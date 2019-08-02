using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace com.egamesstudios.cell 
{
    public class HymirSave : MonoBehaviour, IInteractable
    {
        public void EndDialogueState()
        {
            
        }

        public void TriggerDialogueState(Transform CellPosition)
        {
            CellController mainCell = VariableContainer.variableContainer.mainCell;
            mainCell.vars.saveCheckpoint = transform.position;
            mainCell.vars.savedRoom = SceneManager.GetActiveScene().name;
            VariableContainer.variableContainer.currentActive.FullRecovery();
            SaveManager.saveManager.SaveGame();
        }
    }
}