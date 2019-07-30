using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.egamesstudios.cell
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PushableBlock : MonoBehaviour
    {

        public float speed, frictionFactor;
        private Rigidbody2D rb2d;
        [SerializeField]
        private SFXPlayer sfx;

        // Use this for initialization
        void Start()
        {
            rb2d = GetComponent<Rigidbody2D>();
            sfx = GetComponent<SFXPlayer>();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

