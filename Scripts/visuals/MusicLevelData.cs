using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.egamesstudios.cell
{
    public class MusicLevelData : MonoBehaviour
    {
        [SerializeField]
        MusicData[] musicData;

        private void Awake()
        {
            MusicController.musicController.LoadAudio(musicData);
            MusicController.musicController.PlayAudio(0);
        }
    }
}
