using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace com.egamesstudios.cell
{
    public class WindmillParent : MonoBehaviour
    {

        [SerializeField]
        public float radius, speed, startAngle;
        [OnValueChanged("SetChildren", true)]
        public int fanCount;
        [ChildGameObjectsOnly]
        private List<WindmillChild> fans;

        // Start is called before the first frame update
        void Start()
        {
            if (fanCount == 0) fanCount = transform.childCount;
            float angleIncrement = 360/ fanCount;
            fans = new List<WindmillChild>(fanCount);
            foreach (WindmillChild fan in transform.GetComponentsInChildren<WindmillChild>())
            {
                fans.Add(fan);
                fan.Setup(this, startAngle);
                fan.transform.position = transform.position;
                startAngle += angleIncrement;
            }
        }

        public virtual void SetChildren()
        {

        }
        // Update is called once per frame
        void FixedUpdate()
        {
            for (int i = fanCount - 1; i >= 0; i--)
            {
                fans[i].MovePlatform();
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}
