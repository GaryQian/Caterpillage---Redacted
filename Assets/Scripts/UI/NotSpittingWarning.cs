using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotSpittingWarning : MonoBehaviour {

    int initialSpitCount;
    
	// Use this for initialization
	void Start () {
        initialSpitCount = WorldManager.stats.spitCount;
        InvokeRepeating("AttemptSuicide", 0, 2f);
	}
	
    void AttemptSuicide() {
        if (WorldManager.wormMotion.wormState != WormState.playing || WorldManager.wormMotion.planetTime > 10f && WorldManager.stats.spitCount-initialSpitCount >= 10) {
            Destroy(gameObject);
        }
    }
}
