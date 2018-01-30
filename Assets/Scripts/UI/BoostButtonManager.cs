using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoostButtonManager : MonoBehaviour {

    public Button boostButton;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnBoostButtonDown() {
        WorldManager.wormMotion.EnableBoost();
    }

    public void OnBoostButtonUp() {
        WorldManager.wormMotion.DisableBoost();
    }
}
