using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.egamesstudios.cell
{
    public class WindmillChild : PlatformCatcher
    {
        private float angle;
        private WindmillParent windmillParent;

        public void Setup(WindmillParent windmillParent, float angle)
        {
            this.windmillParent = windmillParent;
            this.angle = angle;
            transform.localPosition = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle), 0) * windmillParent.radius;
        }

        public void MovePlatform()
        {
            angle += windmillParent.speed * Time.fixedDeltaTime;
            Vector3 startPos = transform.localPosition;
            transform.localPosition = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle), 0) * windmillParent.radius;
            Vector3 offset = transform.localPosition - startPos;

            foreach (Transform target in targets)
            {
                //target.position += offset;
                target.Translate(offset);
            }
        }
    }
}
