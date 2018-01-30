using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suicider : MonoBehaviour {
    public float life = 5f;
	// Use this for initialization
	void Start () {
        Invoke("Die", life);
	}
	
	void Die() {
        Destroy(gameObject);
    }
}
