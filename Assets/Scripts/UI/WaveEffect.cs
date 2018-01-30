using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveEffect : MonoBehaviour {
    float dir = 1;
    public float speed = 2000f;
    public Color c;

	// Use this for initialization
	void Start () {
		if (GameManager.settings.leftHandedMode) {
            transform.localPosition = new Vector3(1200 * Util.xScale, 0, 0);
            dir = -1;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, 1f);
        }
        else {
            transform.localPosition = new Vector3(-1200 * Util.xScale, 0, 0);
            dir = 1;
        }
        GetComponent<Image>().color = c;
	}
	
	// Update is called once per frame
	void Update () {
        transform.localPosition = new Vector3(transform.localPosition.x + speed * Time.deltaTime * dir, 0, 0);
	}
}
