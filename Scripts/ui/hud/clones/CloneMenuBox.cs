using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace com.egamesstudios.cell
{
    public class CloneMenuBox : MonoBehaviour
    {

        [SerializeField]
        private Sprite inactiveBox, activeBox, selectedBox, defaultPicture;
        [SerializeField]
        private SpriteRenderer box, icon;
        [SerializeField]
        public CloneMenuContainer cloneDetails;

        public bool canSelect;
        private bool selected;

        // Use this for initialization
        void Start()
        {
            icon.sprite = cloneDetails.image;
            canSelect = true;
            SetValidity(false);
        }

        // Update is called once per frame
        void Update()
        {
            if(selected)
            {

            }
        }

        public void SelectActive()
        {
            box.sprite = selectedBox;
        }

        public void DeselectActive()
        {
            box.sprite = activeBox;
        }

        public void SetValidity(bool validity)
        {
            if(validity != canSelect)
            {
                canSelect = validity;
                box.sprite = validity ? activeBox : inactiveBox;
                icon.sprite = validity ? cloneDetails.image : defaultPicture;
            } 
        }
    }
}

