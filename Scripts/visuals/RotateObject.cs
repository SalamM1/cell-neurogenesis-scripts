using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour {
    [SerializeField]
    private float rotateSpeed_X, rotateSpeed_Y, rotateSpeed_Z;

	void Update () {
        transform.Rotate(new Vector3(rotateSpeed_X, rotateSpeed_Y, rotateSpeed_Z) * Time.deltaTime);
    }
}
