using Com.LuisPedroFonseca.ProCamera2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace com.egamesstudios.cell
{
    public class CameraManager : MonoBehaviour
    {
        public int PPU;
        public float screenSize;
        public float offsetX, offsetY;
        public PostProcessResources postProcessResources;
        public static CameraManager cameraManager;
        public ProCamera2D cam;
        public bool DEBUG = false;
        public float cloneTransitionTime;

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
                AddNewTarget(VariableContainer.variableContainer.mainCell.transform, instantTarget: true);
                InitializeCam();
            }
        }

        private void InitializeCam()
        {
            if (DEBUG) Destroy(cam.GetComponent<ProCamera2DNumericBoundaries>());

            var pixel = cam.GetComponent<ProCamera2DPixelPerfect>();
            if (!pixel) pixel = cam.gameObject.AddComponent<ProCamera2DPixelPerfect>();
            pixel.PixelsPerUnit = PPU;
            pixel.ViewportAutoScale = AutoScaleMode.None;
            pixel.SnapCameraToGrid = false;
            pixel.SnapMovementToGrid = true;

            var forward = cam.GetComponent<ProCamera2DForwardFocus>();
            if (!forward) forward = cam.gameObject.AddComponent<ProCamera2DForwardFocus>();
            forward.Progressive = false;
            forward.TransitionSmoothness = 14 / 48f;
            forward.RightFocus = 4 / 48f;
            forward.LeftFocus = 4 / 48f;
            forward.TopFocus = 2 / 48f;
            forward.BottomFocus = 4 / 48f;

           cam.UpdateScreenSize(screenSize);

            var postProcess = cam.GetComponent<PostProcessLayer>();
            if (!postProcess) (postProcess = cam.gameObject.AddComponent<PostProcessLayer>()).Init(postProcessResources);
            postProcess.volumeTrigger = cam.transform;
            postProcess.volumeLayer = ~0; //everything

            CutsceneManager.cutsceneManager.UpdateCinematics(cam.GetComponent<ProCamera2DCinematics>());
        }
        public void AddNewTarget(Transform transform, bool instantTarget = false)
        {
            float duration = instantTarget ? 0f : cloneTransitionTime;
            cam.RemoveAllCameraTargets(duration);
            cam.AddCameraTarget(transform, 1, 1, duration, new Vector2(offsetX, offsetY));
        }
    }
}

