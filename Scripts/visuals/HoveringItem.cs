using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoveringItem : MonoBehaviour {
    public float speed, distance;
    private float xPos, yPos;
	// Use this for initialization
	void Start () {
        xPos = transform.localPosition.x;
        yPos = transform.localPosition.y;
	}
	
	// Update is called once per frame
	void Update () {
       
        Vector3 basePos = transform.localPosition;
        Vector3 newPos =  new Vector3(basePos.x , yPos + Mathf.PingPong(Time.time*speed, distance) , 0);
        transform.localPosition = new Vector3(Calculator.CalcFixedPos(newPos.x), Calculator.CalcFixedPos(newPos.y), 0);
        
    }
}
