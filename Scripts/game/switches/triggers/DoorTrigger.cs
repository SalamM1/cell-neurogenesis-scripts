using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.egamesstudios.cell
{

    [RequireComponent(typeof(Door))]
    public class DoorTrigger : ASwitchEvent
    {
        [ShowInInspector]
        bool isOpen;
        public float moveSpeed = 0.1f;
        public float switchDoorDelay = 0.5f;
        private Door door;

        private void Awake()
        {
            this.door = GetComponent<Door>();
        }

        public override void Trigger()
        {
            if (inactive) return;
            if(isOpen) StartCoroutine(DoorLoop(-moveSpeed));
            else StartCoroutine(DoorLoop(moveSpeed));
            isOpen = !isOpen;
        }

        public override void SetState(bool open)
        {
            if (isOpen == open || inactive) return;
            else Trigger();
        }

        IEnumerator DoorLoop(float moveSpeed)
        {
            yield return new WaitForSecondsRealtime(switchDoorDelay);
            for (float travel = 0; travel < door.size; travel += this.moveSpeed)
            {
                transform.Translate(door.direction.GetVector()*moveSpeed);
                yield return new WaitForSeconds(0.02f);
            }
            yield return new WaitForSeconds(0.8f);
        }
    }
}