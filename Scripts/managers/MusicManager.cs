using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.egamesstudios.cell
{
    public class MusicManager : MonoBehaviour
    {
        public static MusicManager MUSIC;

        void Awake()
        {
            if (MUSIC == null)
                MUSIC = this;
            else if (MUSIC != this)
                Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
        }
    }
}

