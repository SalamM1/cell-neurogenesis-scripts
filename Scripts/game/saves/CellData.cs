using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.egamesstudios.cell
{
    [System.Serializable]
    public class CellData
    {
        public string savedRoom;
        public Vector2 savedCheckpointCoordinates;
        public int[] stats;

        public CellData()
        {
            stats = new int[6] { 0, 0, 0, 0, 0, 0};
            savedRoom = "RH1";
            savedCheckpointCoordinates = Vector2.zero;
        }

        public void LoadCellData(CellVariables vars)
        {
            //vars.oreCollected?
            //vars.oreUsed?
            //vars.hatID = stats[2];
            vars.equippedGun = (GunType) stats[3];
            vars.equippedBullet = (BulletType)stats[4];
            //vars.equippedGuitar = (GuitarType)stats[5];

            vars.saveCheckpoint = savedCheckpointCoordinates;
            vars.checkpoint = savedCheckpointCoordinates;
            vars.savedRoom = savedRoom;
        }

        public void SaveCellData(CellVariables vars)
        {
            //vars.oreCollected?
            //vars.oreUsed?
            //stats[2] = vars.hatID;
            stats[3] = (int)vars.equippedGun;
            stats[4] = (int)vars.equippedBullet;
            //stats[5] = (int)vars.equippedGuitar;
            Debug.Log("Saved Cell Data");
            savedCheckpointCoordinates = vars.saveCheckpoint;
            savedRoom = vars.savedRoom;
        }
    }
}
