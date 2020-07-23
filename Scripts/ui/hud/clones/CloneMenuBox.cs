using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace com.egamesstudios.cell
{
    public class CloneMenuBox : MonoBehaviour
    {

        [SerializeField]
        private CloneMenuContainer container;
        [SerializeField]
        private SpriteRenderer box, icon;

        public CellType type;
        public Sprite image;

        public bool canSelect;
        private bool selected;

        // Use this for initialization
        void Start()
        {
            icon.sprite = image;
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
            box.sprite = canSelect ? container.selectedBox : container.selectedInvalidBox;
            selected = true;
        }

        public void DeselectActive()
        {
            box.sprite = canSelect ? container.activeBox : container.inactiveBox;
            selected = false;
        }

        public void SetValidity(bool validity)
        {
            if(validity != canSelect)
            {
                canSelect = validity;
                box.sprite = validity ? container.activeBox : container.inactiveBox;
                icon.sprite = validity ? image : null; //container.defaultPicture;
            } 
        }
    }
}

