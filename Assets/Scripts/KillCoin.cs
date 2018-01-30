using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillCoin : MonoBehaviour {

    public float time;
    public float riseSpeed;
    public float fadeSpeed;

    SpriteRenderer sr;

	// Use this for initialization
	void Start () {
        sr = GetComponent<SpriteRenderer>();
        Invoke("Die", time);

	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(transform.position.x, transform.position.y + riseSpeed * Time.deltaTime, -5f);
        sr.color = new Color(1, 1, 1, sr.color.a - fadeSpeed * Time.deltaTime);

	}

    void Die() {
        Destroy(gameObject);
    }
}
