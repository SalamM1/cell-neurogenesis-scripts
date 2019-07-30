using Com.LuisPedroFonseca.ProCamera2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.egamesstudios.cell
{
    public class CameraManager : MonoBehaviour
    {
        public int PPU;
        public float screenSize;
        public float offsetX, offsetY;

        public static CameraManager cameraManager;
        public ProCamera2D cam;
        public bool DEBUG = false;

        void Awake()
        {
            if (cameraManager == null)
                cameraManager = this;
            else if (cameraManager != this)
                Destroy(gameObject);
            DontDestroyOnLoad(gameObject);

        }

        public void OnSceneLoaded()
        {
            foreach(ProCamera2D cam2D in FindObjectsOfType<ProCamera2D>())
            {
                cam = cam2D;
                AddNewCell(VariableContainer.variableContainer.mainCell);
                InitializeCam();
            }
        }

        private void InitializeCam()
        {
            cam.UpdateScreenSize(screenSize);
            if (DEBUG) Destroy(cam.GetComponent<ProCamera2DNumericBoundaries>());

            var forward = cam.GetComponent<ProCamera2DForwardFocus>();
            forward.Progressive = true;
            forward.SpeedMultiplier = 3;
            forward.TransitionSmoothness = 0.208333333f;
            forward.RightFocus = 0.06f;
            forward.LeftFocus = 0.06f;
            forward.TopFocus = 0.09f;
            forward.BottomFocus = 0.16f;

            CutsceneManager.cutsceneManager.UpdateCinematics(cam.GetComponent<ProCamera2DCinematics>());
        }
        public void AddNewCell(CellController cell)
        {
            cam.RemoveAllCameraTargets();
            cam.AddCameraTarget(cell.transform, 1, 1, 0, new Vector2(offsetX, offsetY));
        }
    }
}

