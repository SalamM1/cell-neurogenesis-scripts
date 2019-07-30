using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Com.LuisPedroFonseca.ProCamera2D;
using UnityEngine;

namespace com.egamesstudios.cell 
{

	public class CutsceneManager : MonoBehaviour 
	{
        public static CutsceneManager cutsceneManager;

        private ProCamera2DCinematics camCin;

        private float counter;
        void Awake()
        {
            if (cutsceneManager == null)
                cutsceneManager = this;
            else if (cutsceneManager != this)
                Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
        }

        public IEnumerator PlayCutscene(Cutscene[] scene)
        {
            counter = camCin.EndDuration;
            VariableContainer.variableContainer.currentActive.ChangeState(State.CUTSCENE);
            foreach (Cutscene c in scene)
            {
                camCin.AddCinematicTarget(c.cameraPoint, c.travelTime, c.holdTime, c.zoom, EaseType.EaseOut);
                counter += c.travelTime + c.holdTime;
            }
            camCin.Play();

            yield return new WaitForSecondsRealtime(counter);
            camCin.RemoveAllCinematicTargets();
            VariableContainer.variableContainer.currentActive.ChangeState(State.CONTROL);
            yield return null;
        }

        public void PlayControlledCutscene(Cutscene scene)
        {
            camCin.AddCinematicTarget(scene.cameraPoint, scene.travelTime, -1, scene.zoom);
            if (!camCin.IsPlaying) camCin.Play();
            else camCin.GoToNextTarget();
        }

        public void EndControllCutscene()
        {
            camCin.Stop();
            camCin.RemoveAllCinematicTargets();
        }

        public void UpdateCinematics(ProCamera2DCinematics camCinNew)
        {
            camCin = camCinNew;
        }
    }   
}
