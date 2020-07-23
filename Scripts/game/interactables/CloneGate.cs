using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.egamesstudios.cell
{
    public class CloneGate : MonoBehaviour
    {
        [SerializeField]
        Transform locationA, locationB;
        private bool inGate;
        private bool prevFrame;
        [Sirenix.OdinInspector.FoldoutGroup("Clones Allowed"), SerializeField]
        private bool normal, jumper, melee_fighter, ranged_fighter, slinger, ground;

#if UNITY_EDITOR
        [Sirenix.OdinInspector.Button]
        private void SetArrowDirection()
        {
            Transform childA = locationA.GetChild(0);
            Transform childB = locationB.GetChild(0);
            childA.localRotation = Quaternion.Euler(0, 0, (int) Vector2.SignedAngle(Vector2.right, childB.position - childA.position));
            childB.localRotation = Quaternion.Euler(0, 0, (int) Vector2.SignedAngle(Vector2.right, childA.position - childB.position));
        }
#endif

        private void Start()
        {
            inGate = false;
            SetState(inGate, locationA);
            SetState(inGate, locationB);
        }

        private void Update()
        {
            if (prevFrame != inGate)
            {
                SetState(inGate, locationA);
                SetState(inGate, locationB);
                prevFrame = inGate;
            }
        }

        private void SetState(bool state, Transform location)
        {
            location.GetChild(0).gameObject.SetActive(state);
            location.GetChild(1).gameObject.SetActive(state);
            if (state)
                location.GetComponentInChildren<ParticleSystem>().Play(true);
            else
                location.GetComponentInChildren<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.CompareTag("Cell"))
            {
                collision.GetComponent<CellController>().vars.isOnGate = true;
                collision.GetComponent<CellController>().vars.gateLocation =
                    Vector3.Distance(locationA.position, collision.transform.position) > Vector3.Distance(locationB.position, collision.transform.position) ?
                    locationA : locationB;
                collision.GetComponent<CellController>().vars.allowedClones = new bool[6]{ normal, jumper, melee_fighter, ranged_fighter, slinger, ground};
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.CompareTag("Cell"))
            {
                inGate = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Cell"))
            {
                inGate = false;
                collision.GetComponent<CellController>().vars.isOnGate = false;
                collision.GetComponent<CellController>().vars.gateLocation = null;
                collision.GetComponent<CellController>().vars.allowedClones = new bool[6] { true, true, true, true, true, true };
            }
        }
    }
}
