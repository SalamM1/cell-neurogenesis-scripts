using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace com.egamesstudios.cell
{
    public class SFXPlayer : MonoBehaviour
    {
        public AudioClip[] sfxClips;
        [SerializeField]
        private AudioMixerGroup mixer;

        public void PlaySFX(int index, float volume = 1f,  float pitch = 1f)
        {
            if (index < sfxClips.Length && index >= 0)
            {
                PlayClipAtPoint(sfxClips[index], transform.position, volume, pitch);
            }
        }

        private AudioSource PlayClipAtPoint(AudioClip clip, Vector3 position, float volume, float pitch)
        {
            var go = new GameObject("TempAudio");
            go.transform.position = position;
            var audioSource = go.AddComponent<AudioSource>();

            audioSource.clip = clip;
            audioSource.volume = volume;
            audioSource.pitch = pitch;
            audioSource.outputAudioMixerGroup = mixer;

            audioSource.Play();
            Destroy(go, clip.length);

            return audioSource;
        }
    }
}

