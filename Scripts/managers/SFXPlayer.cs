using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.egamesstudios.cell
{
    [RequireComponent(typeof(AudioSource))]
    public class SFXPlayer : MonoBehaviour
    {
        [OdinSerialize]
        public AudioClip[] sfxClips;
        private AudioSource source;
        private int i;

        // Use this for initialization
        void Start()
        {
            source = GetComponent<AudioSource>();
        }

        public void PlaySFX(int index)
        {
            if (index < sfxClips.Length && index >= 0)
            {
          //      source.Stop();
                source.clip = sfxClips[index];
                source.PlayOneShot(sfxClips[index]);
            }
        }

        public void SetPitch(float val)
        {
            source.pitch = val;
        }
    }
}

