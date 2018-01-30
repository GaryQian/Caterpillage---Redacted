using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracker : MonoBehaviour {

	// Use this for initialization
	void Start () {
        WorldManager.pg.AllSpawnedUnits.Add(gameObject);
	}

    private void OnDestroy() {
        WorldManager.pg.AllSpawnedUnits.Remove(gameObject);
    }

}
