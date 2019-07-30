using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.egamesstudios.cell
{
    public class RotatingPlatform : WindmillParent
    {
        [SerializeField, OnValueChanged("SetChildren")]
        private Vector3 fanScale;
        [Required, SerializeField]
        GameObject windmillChildTemplate;

        public override void SetChildren()
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                var temp = transform.GetChild(i);
                temp.transform.parent = null;
                DestroyImmediate(temp.gameObject);
            }
            for(int i = fanCount; i > 0; i--)
            Instantiate(windmillChildTemplate, transform).transform.localScale = fanScale;
        }
    }
}

