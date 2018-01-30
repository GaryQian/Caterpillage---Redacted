using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blinker : MonoBehaviour {

    public Animator anim;
    public float minDelay;
    public float maxDelay;

	// Use this for initialization
	void Start () {
        Invoke("Blink", Random.Range(minDelay, maxDelay));
	}
	
	
    void Blink() {
        switch (Random.Range(0, 3)) {
            case 0: anim.SetTrigger("blink1"); break;
            case 1: anim.SetTrigger("blink2"); break;
            case 2: anim.SetTrigger("blink3"); break;
        }
        Invoke("Blink", Random.Range(minDelay, maxDelay));
    }
}
