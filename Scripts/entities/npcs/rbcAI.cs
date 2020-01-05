using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.egamesstudios.cell
{
    public class rbcAI : NPCAI, IInteractable
    {
        [SerializeField]
        private NPCState state;

        public float range;
        public float timer = 1.5f;
        private float TIMER;
        private float leftEdge, rightEdge;

    // Use this for initialization
        void Start()
        {
            state = NPCState.IDLE;

            leftEdge = transform.position.x - range;
            rightEdge = transform.position.x + range;

            TIMER = timer;
        }

        // Update is called once per frame
        void Update()
        {
            if(timer <= 0) {
                switch (state)
                {
                    case NPCState.IDLE:
                        MoveNPC();
                        break;
                    default:
                        break;
                }
                timer = TIMER;
            }
            else
            {
                timer -= Time.deltaTime;
            }
            
        }

        private void MoveNPC()
        {
            float newTarget;
           if(range <= 0)
            {
                return;
            }
           if (UnityEngine.Random.Range(0f, 1f) >= 0.5)
            {
                 newTarget = leftEdge;
            }
            else
            {
                 newTarget = rightEdge;
            }
            float moveX = UnityEngine.Random.Range(0f, 1.0f)*(newTarget - transform.position.x);
            transform.DOMoveX(moveX + transform.position.x, 0.2f, false).SetEase(Ease.Linear);

            if (moveX != 0)
            {
                if (moveX > 0)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                }
                else
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
            }
        }

        public void TriggerInteraction(Transform CellPosition)
        {
            if (CellPosition.position.x >= transform.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            state = NPCState.DIALOGUE;
        }

        public void EndInteraction()
        {
            state = NPCState.IDLE;
        }
    }
}

