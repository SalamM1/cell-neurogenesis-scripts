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

        void Start()
        {
            yCenter = transform.position.y;
            xCenter = transform.position.x;
            startingPointY *= height / 2;
            startingPointX *= width / 2;
        }

        void FixedUpdate()
        {
            Vector3 startPos = transform.position;
            transform.position = new Vector3(horizontal ? (xCenter +
                        Mathf.PingPong(Time.timeSinceLevelLoad * speedX + startingPointX, width) - width / 2f) : transform.position.x, vertical ? (yCenter +
                        Mathf.PingPong(Time.timeSinceLevelLoad * speedY + startingPointY, height) - height / 2f) : transform.position.y, transform.position.z);
            Vector3 offset = transform.position - startPos;

            foreach (Transform target in targets)
            {
                target.position += offset;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.white;
            Vector3 from = transform.position - new Vector3(horizontal ? width / 2f : 0, vertical ? height / 2f : 0);
            Vector3 to = transform.position + new Vector3(horizontal ? width / 2f : 0, vertical ? height / 2f : 0);
            Gizmos.DrawLine(from, to);
            Gizmos.color = (startingPointY >= 2 && startingPointY < 4) ? Color.red : Color.cyan;
            Gizmos.DrawSphere(transform.position + new Vector3(horizontal ? (startingPointX > 2 ? ((4 - startingPointX) * width / 2) : startingPointX * width / 2) - width / 2 : 0, vertical ? (startingPointY > 2 ? ((4 - startingPointY) * height / 2) : (startingPointY * height / 2)) - height / 2 : 0), 0.2f);
        }


    }
}
