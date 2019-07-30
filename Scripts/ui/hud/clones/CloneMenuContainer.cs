using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace com.egamesstudios.cell
{
    [CreateAssetMenu(fileName = "New CloneBox", menuName = "Custom/CloneBox")]
    public class CloneMenuContainer : ScriptableObject
    {
        public CellType type;
        [InlineEditor]
        public Sprite image;
    }
}
