using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour {
    public float rotateSpeed_X = 1f, rotateSpeed_Y = 1f, rotateSpeed_Z = 1f;
    public bool X, Y, Z;

	void Update () {
        float x = transform.rotation.x, y = transform.rotation.y, z = transform.rotation.z;
        
        if (X)
        {
            x = rotateSpeed_X * Time.deltaTime;
        }
        if (Y)
        {
            y = rotateSpeed_Y * Time.deltaTime;
        }
        if (Z)
        {
            z = rotateSpeed_Z * Time.deltaTime;
        }
        transform.Rotate(new Vector3(x, y, z));
    }
}
