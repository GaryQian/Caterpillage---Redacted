using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoTilt : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player" || collision.gameObject.layer == 13) {
            GetComponent<Rigidbody2D>().freezeRotation = false;
            Destroy(this);
        }
    }
}
