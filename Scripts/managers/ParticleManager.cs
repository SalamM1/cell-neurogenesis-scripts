using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.egamesstudios.cell
{
    public class ParticleManager : SerializedMonoBehaviour
    {

        public static ParticleManager particleManager;
        [OdinSerialize]
        private Dictionary<FXType, GameObject[]> particles;

        void Awake()
        {
            if (particleManager == null)
                particleManager = this;
            else if (particleManager != this)
                Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
        }

        public void PlayParticle(FXType type, int index, Vector3 source)
        {
            try
            {
                Instantiate(particles[type][index], source, Quaternion.identity);
            } catch (Exception e)
            {
                Debug.LogWarning("Particles Failed: " + e);
            }
        }
    }
}

