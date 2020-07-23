using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace com.egamesstudios.cell
{
    public class EventFlag : MonoBehaviour
    {
        [SerializeField]
        protected int ID;
        [SerializeField]
        protected bool flag;

        private void Awake()
        {
            LoadFlag();
        }
        protected void LoadFlag()
        {
            if (SaveManager.saveManager.currentRoom)
                flag = SaveManager.saveManager.currentRoom[0,ID];
        }

        protected void LoadFlag(string sceneName)
        {
            flag = SaveManager.saveManager.activeGame.roomFlags[sceneName][0,ID];
        }

        public void SetFlag(bool flagState)
        {
            SaveManager.saveManager.currentRoom[0,ID] = flagState;
            flag = flagState;
        }

        public void SetFlag(bool flagState, string sceneName)
        {
            SaveManager.saveManager.activeGame.roomFlags[sceneName][0,ID] = flagState;
            flag = flagState;
        }

        public bool GetFlag()
        {
            return flag;
        }

#if UNITY_EDITOR
        [Sirenix.OdinInspector.Button]
        public void PrintFlagIDs()
        {
            Debug.ClearDeveloperConsole();
            foreach (EventFlag compFlag in FindObjectsOfType<EventFlag>())
            {
                Debug.Log(compFlag.ID);
            }
        }

#endif
    }
}

