using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.egamesstudios.cell
{
    [CreateAssetMenu(fileName = "New Hat", menuName = "Custom")]
    public class HatContainer : ScriptableObject
    {
        string hatName;
        int hatID;
        public Sprite hatImage;
    }
}
