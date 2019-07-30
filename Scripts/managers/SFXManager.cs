using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace com.egamesstudios.cell
{
    public enum FXType
    {
        ENEMY,
        OBJECT,
        DIALOGUE,
        CHARACTER,
    }

    public class SFXManager : SerializedMonoBehaviour
    {
        public static SFXManager sfxManager;
        [OdinSerialize]
        private Dictionary<FXType, AudioClip[]> sfxClips;

        void Awake()
        {
            if (sfxManager == null)
                sfxManager = this;
            else if (sfxManager != this)
                Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
        }

        public void PlaySFX(FXType type, int index, AudioSource source)
        {
           
            if (index < sfxClips[type].Length && index >= 0)
            {
                source.clip = sfxClips[type][index];
                source.PlayOneShot(sfxClips[type][index]);
            }
        }
    }


}
