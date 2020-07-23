using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace com.egamesstudios.cell
{
    public class MovingPlatform : PlatformCatcher
    {
        [FoldoutGroup("Vertical")]
        public float height = 10, speedY = 2f;
        [FoldoutGroup("Horizontal")]
        public float width = 10, speedX = 2f;

        [PropertyRange(0, 4)]
        [FoldoutGroup("Vertical")]
        [PropertyTooltip("0 - Start at Bottom" +
        "\n1 - Start at Midpoint going Up" +
        "\n2 - Start at Top" +
        "\n3 - Start at Midpoint going Down" +
        "\n4 - Start at Bottom")]
        public float startingPointY;

        [PropertyRange(0, 4)]
        [FoldoutGroup("Horizontal")]
        [PropertyTooltip("0 - Start at Left" +
        "\n1 - Start at Midpoint going Right" +
        "\n2 - Start at Right" +
        "\n3 - Start at Midpoint going Left" +
        "\n4 - Start at Left")]
        public float startingPointX;

        [FoldoutGroup("Vertical")]
        public bool vertical;
        [FoldoutGroup("Horizontal")]
        public bool horizontal;


        private float yCenter = 0f, xCenter = 0f;
        private float timeChangeX, timeChangeY;

        void Start()
        {
            yCenter = transform.position.y;
            xCenter = transform.position.x;
            startingPointY *= height / 2;
            startingPointX *= width / 2;

            timeChangeX = timeChangeY = 0;
        }

        void FixedUpdate()
        {
            Vector3 startPos = transform.position;
            transform.position = new Vector3(
                horizontal ? CalculatePosition(xCenter, transform.position.x, speedX, startingPointX, width, timeChangeX) : transform.position.x, 
                vertical   ? CalculatePosition(yCenter, transform.position.y, speedY, startingPointY, height, timeChangeY) : transform.position.y, 
                transform.position.z);
            Vector3 offset = transform.position - startPos;

            foreach (Transform target in targets)
            {
                if(target != null)
                {
                    target.position += offset;
                }
            }

            targets.RemoveAll(target => target == null);

            if (horizontal) timeChangeX += Time.fixedDeltaTime * CalculateTimeChange(xCenter, transform.position.x, speedX, width);
            if (vertical) timeChangeY += Time.fixedDeltaTime * CalculateTimeChange(yCenter, transform.position.y, speedY, height);
        }

        private float CalculatePosition(float center, float pos, float speed, float startPoint, float distance, float time)
        {
            return center + Mathf.PingPong(time + startPoint, distance) - (distance * 0.5f);
        }

        private float CalculateTimeChange(float center, float pos, float speed, float distance)
        {
            return Mathf.Min(speed * (3f - (Mathf.Abs(pos - center) * 5.9f) / distance), speed);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.white;
            Vector3 from = transform.position - new Vector3(horizontal ? width / 2f : 0, vertical ? height / 2f : 0);
            Vector3 to = transform.position + new Vector3(horizontal ? width / 2f : 0, vertical ? height / 2f : 0);
            Gizmos.DrawLine(from, to);

            bool isRed = (horizontal) ? (startingPointX >= 2 && startingPointX < 4) : (startingPointY >= 2 && startingPointY < 4);           
            Gizmos.color = (isRed) ? Color.red : Color.cyan;
            Gizmos.DrawSphere(transform.position + new Vector3(horizontal ? 
                (startingPointX > 2 ? ((4 - startingPointX) * width / 2) : startingPointX * width / 2) - width / 2 : 0, vertical ? 
                (startingPointY > 2 ? ((4 - startingPointY) * height / 2) : (startingPointY * height / 2)) - height / 2 : 0), 0.2f);
            Gizmos.color = Color.black;
            Gizmos.DrawWireCube(transform.position + new Vector3(horizontal ?
                (startingPointX > 2 ? ((4 - startingPointX) * width / 2) : startingPointX * width / 2) - width / 2 : 0, vertical ?
                (startingPointY > 2 ? ((4 - startingPointY) * height / 2) : (startingPointY * height / 2)) - height / 2 : 0), GetComponent<BoxCollider2D>().size);
        }
#if UNITY_EDITOR
        [Button("Set Max Left")]
        private void SetMaxL()
        {
            startingPointX = startingPointY = 0;
        }

        [Button("Set Max Right")]
        private void SetMaxRL()
        {
            startingPointX = startingPointY = 2;
        }

        [Button("Set Center -> Right")]
        private void SetCenterRight()
        {
            startingPointX = startingPointY = 1;
        }

        [Button("Set Center -> Left")]
        private void SetCenterLeft()
        {
            startingPointX = startingPointY = 3;
        }
#endif  

    }
}
