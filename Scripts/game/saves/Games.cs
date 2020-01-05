using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Serialization;
using UnityEngine;

namespace com.egamesstudios.cell
{
    [Serializable]
	public class Games
	{
        public Dictionary<CollectableType, bool[]> powerups;  //ID based
        public Dictionary<string, RoomSaveData> roomFlags;
        public bool[][] worldFlags;
        public bool[][] compendiumFlags;
        public bool[] unlockedHats;
        public CellData cellData;
        //public Shop[] shopStock; //ID based
        private string version;

        public static readonly int HEALTH_INCREMENT = 10;
        public static readonly int ENERGY_INCREMENT = 20;
        public static readonly int GUITAR_INCREMENT = 4;
        public static readonly int GUN_INCREMENT = 4;

        public Games()
        {
            version = "0.1.1";
            cellData = new CellData();
            powerups = new Dictionary<CollectableType, bool[]>();
            PopulatePowerups();
        }

        public void PopulateGameFlags()
        {
            worldFlags = new bool[4][];
            for (int i = 0; i < 4; i++)
            {
                worldFlags[i] = new bool[48];
            }
            compendiumFlags = new bool[2][];
            compendiumFlags[0] = new bool[128];
            compendiumFlags[1] = new bool[128];

            roomFlags = new Dictionary<string, RoomSaveData>();

            var splitFile = new string[] { "\r\n", "\r", "\n", System.Environment.NewLine };
            string[] content = ((TextAsset)Resources.Load("newSaveData", typeof(TextAsset))).text.Split(splitFile, StringSplitOptions.None);
            foreach (String s in content)
            {
                string[] level = s.Split(new char[] { ' ' }, StringSplitOptions.None);
                int numOfLevels = Int32.Parse(level[1]);
                int numOfFlags = Int32.Parse(level[2]);

                for (int i = 1; i <= numOfLevels; i++)
                {
                    roomFlags.Add(level[0] + i, new RoomSaveData(numOfFlags, numOfFlags));
                }
            }            
        }

        private void PopulatePowerups()
        {
            powerups.Add(CollectableType.HEALTH, new bool[10]);
            powerups.Add(CollectableType.ENERGY, new bool[10]);
            powerups.Add(CollectableType.ORE, new bool[16]);
            powerups.Add(CollectableType.WEAPON, new bool[8]);
            powerups.Add(CollectableType.MEMORY, new bool[10]);
            powerups.Add(CollectableType.ATOM, new bool[92]);
            powerups.Add(CollectableType.ABILITY, new bool[10]);
            powerups.Add(CollectableType.GUITAR_UPGRADE, new bool[7]);
            powerups.Add(CollectableType.GUN_UPGRADE, new bool[7]);
        }

        public void LoadCellData(CellVariables vars)
        {
            foreach (CollectableType type in powerups.Keys)
            {
                bool[] boolArray = powerups[type];
                int combinedValue = 0;
                switch (type)
                {
                    case CollectableType.HEALTH:
                        combinedValue = 40 + boolArray.Count(c => c) * HEALTH_INCREMENT;
                        vars.maxHealth = vars.mainHealth = combinedValue;
                        break;
                    case CollectableType.ENERGY:
                        combinedValue = 100 + boolArray.Count(c => c) * ENERGY_INCREMENT;
                        vars.maxEnergy = vars.mainEnergy = combinedValue;
                        break;
                    case CollectableType.ABILITY:
                        // vars.hasGuitar = boolArray[0];
                        vars.hasGuitar = true;
                        vars.hasGun = boolArray[1];
                        vars.hasDoubleJump = boolArray[2];
                        vars.hasWallJump = boolArray[3];
                        vars.hasSlingJump = boolArray[4];
                        vars.hasGroundPound = boolArray[5];
                        vars.hasChargeDash = boolArray[6];
                        vars.hasTripleJump = boolArray[7];
                        break;
                    case CollectableType.GUITAR_UPGRADE:
                        combinedValue = 5 + boolArray.Count(c => c) * GUITAR_INCREMENT;
                        vars.guitarDamage = combinedValue;
                        break;
                    case CollectableType.GUN_UPGRADE:
                        combinedValue = 5 + boolArray.Count(c => c) * GUITAR_INCREMENT;
                        vars.gunDamage = combinedValue;
                        break;
                }
            }
            cellData.LoadCellData(vars);
        }

        public RoomSaveData GetRoomSaveData(string sceneName)
        {
            if (!roomFlags.ContainsKey(sceneName)) return null;
            return roomFlags[sceneName];
        }
        public void RegenerateEnemies()
        {
            roomFlags.Values.ToList().ForEach(x => x.RegenerateEnemies());
        }
    }

    [Serializable]
    public class RoomSaveData
    {
        public bool[] roomFlags;
        public bool[] enemyFlags;

        public RoomSaveData(int roomFlagCount, int enemyFlagCount)
        {
            roomFlags = new bool[roomFlagCount];
            enemyFlags = new bool[enemyFlagCount];
        }

        public void RegenerateEnemies()
        {
            enemyFlags = new bool[enemyFlags.Length];
        }
        public bool[] this[int index]
        {
            get
            {
                if (index == 0)
                {
                    return roomFlags;
                }
                else if (index == 1)
                {
                    return enemyFlags;
                }
                else return null;
            }
            set
            {
                if (index == 0)
                {
                    roomFlags = value;
                }
                else if (index == 1)
                {
                    enemyFlags = value;
                }
            }
        }
        public bool this[int index, int boolIndex]
        {
            get
            {
                if (index == 0)
                {
                    return roomFlags[boolIndex];
                }
                else if (index == 1)
                {
                    return enemyFlags[boolIndex];
                }
                else return false;
            }
            set
            {
                if (index == 0)
                {
                    roomFlags[boolIndex] = value;
                }
                else if (index == 1)
                {
                    enemyFlags[boolIndex] = value;
                }
            }
        }

        public static implicit operator bool(RoomSaveData roomSaveData)
        {
            return roomSaveData != null;
        }

    }
    public enum CollectableType
    {
        HEALTH = 0,
        ENERGY = 1,
        WEAPON = 2,
        ORE = 3,
        MEMORY = 4,
        ATOM = 5,
        ABILITY = 6,
        GUITAR_UPGRADE = 7,
        GUN_UPGRADE = 8,

    }
}
