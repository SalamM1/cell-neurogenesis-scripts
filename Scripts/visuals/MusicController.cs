using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.egamesstudios.cell
{
    public class MusicController : MonoBehaviour
    {
        [SerializeField, PropertyRange(0, 1)]
        private float volumeMax;
        private AudioSource audioSource;
        
        [ShowInInspector]
        private MusicData[] musicData;
        private MusicData currentPlaying;

        public static MusicController musicController;

        void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.volume = volumeMax;

            if (musicController == null)
                musicController = this;
            else if (musicController != this)
                Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
        }

        public void Update()
        {
            if (currentPlaying.loopLength > 0 && currentPlaying.loopThreshold > 0)
            {
                if (audioSource.timeSamples >= currentPlaying.loopThreshold * currentPlaying.audioClip.frequency)
                {
                    audioSource.timeSamples -= Mathf.RoundToInt(currentPlaying.loopLength * currentPlaying.audioClip.frequency);
                }
            }
        }

        public void LoadAudio(MusicData[] musicData)
        {
            this.musicData = musicData;
        }

        //Play from currently loaded Music List
        public bool PlayAudio(int index)
        {
            if (currentPlaying == null)
            {
                currentPlaying = musicData[index];
                audioSource.clip = currentPlaying.audioClip;
                audioSource.Play();
                return true;
            }
            else if (index >= musicData.Length)
                return false;
            else if (musicData[index].id == currentPlaying.id)
                return false;
            StartCoroutine(StartNewMusic(musicData[index]));
            return true;
        }

        //Play music not in music list, eg. for a cutscene
        public bool PlayAudio(MusicData musicDataToPlay)
        {
            if (currentPlaying == null)
            {
                currentPlaying = musicDataToPlay;
                audioSource.clip = currentPlaying.audioClip;
                audioSource.Play();
                return true;
            }
            else if (currentPlaying.id == musicDataToPlay.id)
            {
                return false;
            }
            StartCoroutine(StartNewMusic(musicDataToPlay));
            return true;
        }

        //Play the default music in the list
        public bool PlayDefaultRoomMusic()
        {
            return PlayAudio(0);
        }

        //Coroutine to fade music in/out
        private IEnumerator StartNewMusic(MusicData newAudio)
        {
            while (audioSource.volume > 0)
            {
                audioSource.volume -= Time.deltaTime*2;
                yield return new WaitForEndOfFrame();
            }

            audioSource.Stop();
            currentPlaying = newAudio;
            audioSource.clip = currentPlaying.audioClip;
            audioSource.timeSamples = 0;
            audioSource.Play();

            while (audioSource.volume < volumeMax)
            {
                audioSource.volume += Time.deltaTime*2;
                yield return new WaitForEndOfFrame();
            }
            audioSource.volume = volumeMax;
        }
    }
}