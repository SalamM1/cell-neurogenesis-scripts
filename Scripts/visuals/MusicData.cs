using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

namespace com.egamesstudios.cell
{
    [CreateAssetMenu(fileName = "New MusicData", menuName = "Custom/Music Data")]
    public class MusicData : ScriptableObject
    {
        public AudioClip audioClip;
        public int id;
        public float loopThreshold;
        public float loopLength;
    }
}
