using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace com.egamesstudios.cell
{

	public class LoadSceneManager : MonoBehaviour 
	{
        public static LoadSceneManager loadSceneManager;
        public int checkpointID;

        void Awake()
        {
            if (loadSceneManager == null)
                loadSceneManager = this;
            else if (loadSceneManager != this)
                Destroy(gameObject);
            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            CameraManager.cameraManager.OnSceneLoaded();
            DialogueManager.dialogueManager.LoadDialogueFile(scene.name);
            try
            {
                VariableContainer.variableContainer.mainCell.SetCheckpoint(checkpointID >= 0 ?
                    FindCheckPoint(checkpointID) :
                    (Vector3)VariableContainer.variableContainer.mainCell.vars.saveCheckpoint);
                VariableContainer.variableContainer.mainCell.SendToCheckpoint();
                UIManager.uIManager.ChangeState(UIState.INGAME);
                VariableContainer.variableContainer.mainCell.ChangeState(State.CONTROL);
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
            }
        }

        private Vector3 FindCheckPoint(int checkpointID)
        {
            foreach (SceneChanger point in FindObjectsOfType<SceneChanger>())
            {
                if (point.ID == checkpointID) return point.spawnHereOnEnter.position;
            }
            return (VariableContainer.variableContainer) ? VariableContainer.variableContainer.currentActive.transform.position : Vector3.zero;
        }

        public IEnumerator LoadLevel(int checkpointID, string connectedScene)
        {
            UIManager.uIManager.ChangeState(UIState.TRANSITION);
            VariableContainer.variableContainer.mainCell.GetComponent<CloneController>().KillAllClones();
            VariableContainer.variableContainer.mainCell.ChangeState(State.TRANSITION);
            SaveManager.saveManager.SaveGame();
            this.checkpointID = checkpointID;
            SaveManager.saveManager.OnSceneChange(connectedScene);
            yield return new WaitForSecondsRealtime(UIManager.uIManager.fadeTime + 0.1f);

            SceneManager.LoadScene(connectedScene);
        }

        public IEnumerator ResetCellToSave()
        {
            UIManager.uIManager.ChangeState(UIState.TRANSITION, true);
            VariableContainer.variableContainer.mainCell.ChangeState(State.DEAD);
            VariableContainer.variableContainer.mainCell.GetComponent<CloneController>().KillAllClones();
            VariableContainer.variableContainer.mainCell.FullRecovery();
            SaveManager.saveManager.SaveGame();
            yield return new WaitForSecondsRealtime(4.2f);


            //music reset
            checkpointID = -1;
            SaveManager.saveManager.OnSceneChange(VariableContainer.variableContainer.mainCell.vars.savedRoom);
            SceneManager.LoadScene(VariableContainer.variableContainer.mainCell.vars.savedRoom);
        }
    }
}
