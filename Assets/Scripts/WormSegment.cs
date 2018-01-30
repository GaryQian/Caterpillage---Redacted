using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormSegment : MonoBehaviour {
    public GameObject targ;
	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update() {
        float followAngle = Mathf.Atan((targ.transform.position.y - transform.position.y) / (targ.transform.position.x - transform.position.x));


        if (targ.transform.position.x - transform.position.x > 0) {
            followAngle += Mathf.PI;
        }

        float radius = 0.2f;
        float followSpeed = 2.5f;

        float x = transform.position.x;
        float y = transform.position.y;

        if (Mathf.Pow(targ.transform.position.x - transform.position.x, 2) + Mathf.Pow(targ.transform.position.y - transform.position.y, 2) > Mathf.Pow(radius + radius, 2)) {

            x = transform.position.x + ((targ.transform.position.x - transform.position.x) - 2 * (radius * Mathf.Cos(followAngle))) * followSpeed * Time.deltaTime;

            y = transform.position.y + ((targ.transform.position.y - transform.position.y) - 2 * (radius * Mathf.Sin(followAngle))) * followSpeed * Time.deltaTime;


        }

        float dist = Mathf.Sqrt(Mathf.Pow(transform.position.x - targ.transform.position.x, 2) + Mathf.Pow(transform.position.y - targ.transform.position.y, 2));

        x = ((transform.position.x - targ.transform.position.x) / (dist)) * (2 * radius) + targ.transform.position.x;

        y = ((transform.position.y - targ.transform.position.y) / (dist)) * (2 * radius) + targ.transform.position.y;

        transform.position = new Vector3(x, y, -0.9f);

    }
}
