using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.egamesstudios.cell
{
    public class ScaleInOut : MonoBehaviour
    {
        [SerializeField]
        private Vector3 originalScale;
        [SerializeField]
        private float speed;

        public void FlipX()
        {
            originalScale.x *= -1;
        }

        public void SetX(int x)
        {
            originalScale.x = Mathf.Abs(originalScale.x) * x;
        }

        public IEnumerator Scale(bool appear, bool enableDisable = false)
        {
            if(originalScale.x < 0)
            {
                if (appear)
                {
                    if (enableDisable) gameObject.SetActive(true);
                    while (transform.localScale.x > originalScale.x)
                    {
                        transform.localScale -= new Vector3(Time.deltaTime * speed, Time.deltaTime * -speed, 0);
                        if (transform.localScale.x < originalScale.x) transform.localScale = originalScale;
                        yield return new WaitForEndOfFrame();
                    }
                }
                else
                {
                    while (transform.localScale.x < 0)
                    {
                        transform.localScale += new Vector3(Time.deltaTime * speed, Time.deltaTime * -speed, 0);
                        yield return new WaitForEndOfFrame();
                    }
                    transform.localScale = new Vector3(0, 0, 1);
                    if (enableDisable) gameObject.SetActive(false);
                }
            }
            else
            {
                if (appear)
                {
                    if (enableDisable) gameObject.SetActive(true);
                    while (transform.localScale.x < originalScale.x)
                    {
                        transform.localScale += new Vector3(Time.deltaTime * speed, Time.deltaTime * speed, 0);
                        if (transform.localScale.x > originalScale.x) transform.localScale = originalScale;
                        yield return new WaitForEndOfFrame();
                    }
                }
                else
                {
                    while (transform.localScale.x > 0)
                    {
                        transform.localScale -= new Vector3(Time.deltaTime * speed, Time.deltaTime * speed, 0);
                        yield return new WaitForEndOfFrame();
                    }
                    transform.localScale = new Vector3(0, 0, 1);
                    if (enableDisable) gameObject.SetActive(false);
                }
            }

        }
    }
}