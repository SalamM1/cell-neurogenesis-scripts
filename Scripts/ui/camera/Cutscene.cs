using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace com.egamesstudios.cell {

    public class Cutscene : MonoBehaviour
    {
        public Transform cameraPoint;
        public float travelTime;
        public float holdTime;

        [PropertyRange(0,5)]
        public float zoom;

        private void Reset()
        {
            cameraPoint = transform;
            travelTime = 0.25f;
            holdTime = 2f;
        }
    }
}
