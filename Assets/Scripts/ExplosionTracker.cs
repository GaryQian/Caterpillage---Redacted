using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionTracker : MonoBehaviour {

    public static Dictionary<ExplosionTracker, Vector2> set;
    public static int explodeCount;
    bool added = false;
	// Use this for initialization
	void Start () {
        if (!added) Add();
        explodeCount++;
	}

    public void Add() {
        added = true;
        if (set == null) {
            set = new Dictionary<ExplosionTracker, Vector2>();
        }
        set.Add(this, transform.position);
    }


    private void OnDestroy() {
        if (set.ContainsKey(this)) set.Remove(this);
        explodeCount--;
    }

    public static void Reset() {
        set = new Dictionary<ExplosionTracker, Vector2>();
        explodeCount = 0;
    }
}
