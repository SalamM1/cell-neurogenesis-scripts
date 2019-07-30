using UnityEngine;

public class CycleAlpha : MonoBehaviour {

    private SpriteRenderer spr;
    private float a = 0f;
    private bool add = true, sub = false;

    public float min, max, speed, r, g, b;

	// Use this for initialization
	void Start () {
        this.spr = GetComponent<SpriteRenderer>();
        a = min/255;
        spr.color = new Color(r/255, g/255, b/255, a);
    }
	
	// Update is called once per frame
	void Update () {
        if (a >= max/255) {
            add = false;
            sub = true;
        }
        else if (a <= min/255)
        {
            add = true;
            sub = false;
        }
        if (add)
        {
            a += 0.0005f*speed;
        }
        if (sub)
        {
            a -= 0.0005f*speed;
        }
        spr.color = new Color(r / 255, g / 255, b / 255, a);
    }
}
