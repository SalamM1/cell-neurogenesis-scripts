using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using UnityEngine.SceneManagement;

namespace com.egamesstudios.cell
{
    public class SaveManager : SerializedMonoBehaviour
    {
        [SerializeField]
        private bool DEBUG;
        public static SaveManager saveManager;
        public GameObject autoSaveAnim;
        public RoomSaveData currentRoom;
        [FilePath]
        public string filePath;
        [SerializeField]
        const string fileName = "/CellSave";
        [SerializeField]
        private string fileExtension = ".neurodata";
        private int saveSlot;
        private Games[] gameList;
        [OdinSerialize, NonSerialized]
        public Games activeGame;

        void Awake()
        {
            if (saveManager == null)
                saveManager = this;
            else if (saveManager != this)
                Destroy(gameObject);
            DontDestroyOnLoad(gameObject);

            gameList = new Games[3];
            currentRoom = null;
            LoadGamesFromFile();
            //TEMP - Until the UI manager can call it
            try
            {
                SelectGame(0);
                LoadGame();
            } catch (Exception e)
            {
                Debug.Log(e.Message);
                InitManager();
                SelectGame(0);
                SaveGame();
            }
        }

        public void OnSceneChange(string sceneName)
        {
            currentRoom = activeGame.GetRoomSaveData(sceneName);
        }
        
        private void InitManager()
        {
            activeGame = new Games();
            activeGame.PopulateGameFlags();
            Debug.Log("New Game created");
        }

        public void SaveGame()
        {
            // autoSaveAnim.SetActive(true);
            CellController mainCell = VariableContainer.variableContainer ? VariableContainer.variableContainer.mainCell : FindObjectOfType<CellController>();
            activeGame.cellData.SaveCellData(mainCell.vars);
            activeGame.RegenerateEnemies();

            DataFormat dataFormat = DataFormat.JSON;
            byte[] bytes = SerializationUtility.SerializeValue(activeGame, dataFormat);
            using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.Write))
            using (var writer = new BinaryWriter(fs))
            {
                writer.Write(bytes);
            }
            //  autoSaveAnim.SetActive(false);
        }

        #region CellData
        public void LoadCellData()
        {
            activeGame.RegenerateEnemies();
            CellController mainCell = VariableContainer.variableContainer ? VariableContainer.variableContainer.mainCell : FindObjectOfType<CellController>();
            activeGame.LoadCellData(mainCell.vars);
        }
        #endregion

        #region Loading
        public void SelectGame(int slot)
        {
            saveSlot = slot;
            filePath = Application.persistentDataPath + fileName + saveSlot + fileExtension;
        }

        public void LoadGame()
        {
            activeGame = gameList[saveSlot];
            if (activeGame == null)
            {
                InitManager();
            }
            if (!DEBUG)
                LoadCellData();
        }

        private void LoadGamesFromFile()
        {
            for (int i = 0; i < 3; i++)
            {
                SelectGame(i);
                gameList[i] = LoadGameFile();
            }
        }

        private Games LoadGameFile()
        {
            DataFormat dataFormat = DataFormat.JSON;
            using (var fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read))
            using (var reader = new BinaryReader(fs))
            {
                return SerializationUtility.DeserializeValue<Games>(reader.BaseStream, dataFormat);
            }
        }
        #endregion

        #region Debugging
        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.P))
            {
                SaveGame();
                Debug.Log("Saving");
            }
            if(Input.GetKeyDown(KeyCode.O))
            {
                DeleteGame();
                Debug.Log("Deleted Save Successfuly");
            }
        }

        private void DeleteGame()
        {
            activeGame = null;
            DataFormat dataFormat = DataFormat.Binary;
            byte[] bytes = SerializationUtility.SerializeValue(activeGame, dataFormat);
            using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.Write))
            using (var writer = new BinaryWriter(fs))
            {
                writer.Write(bytes);
            }
        }
        #endregion
    }
}

