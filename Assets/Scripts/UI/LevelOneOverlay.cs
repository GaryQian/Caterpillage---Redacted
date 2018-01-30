using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelOneOverlay : MonoBehaviour {

	// Use this for initialization
	void Start () {
        InvokeRepeating("Check", 3f, 2f);
	}

    void Check() {
        if (WorldManager.wormMotion.wormState != WormState.playing) {
            Destroy(gameObject);
        }
    }
}
