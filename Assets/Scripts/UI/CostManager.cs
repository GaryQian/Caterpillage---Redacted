using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CostManager {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static float GetSpitterEvolveCost() {
        return 100f;
    }

    public static float GetWingsEvolveCost() {
        return 300f;
    }

    public static float GetShellEvolveCost() {
        return 150f;
    }

    public static float GetSpitterFeatureCost() {
        if (GameManager.wormStats.NextSpitterID() == -1) return 0;
        switch (GameManager.wormStats.NextSpitterID()) {
            case 0: return 400f;
            case 1: return 2000f;
            case 2: return 10000f;
            case 3: return 60000f;
        }
        return 1000 * GameManager.wormStats.NextSpitterID() + 500f;
    }

    public static float GetSpitterLevelCost() {
        return 300 * GameManager.wormStats.spitLevel + 100f + 15 * Mathf.Pow(1.5f, GameManager.wormStats.spitLevel);
    }

    public static float GetWingFeatureCost() {
        if (GameManager.wormStats.NextWingID() == -1) return 0;
        switch (GameManager.wormStats.NextWingID()) {
            case 0: return 1000f;
            case 1: return 400f;
            case 2: return 3000f;
            case 3: return 50000f;
        }
        return 300 * GameManager.wormStats.NextWingID() + 200f;
    }

    public static float GetWingLevelCost() {
        return 400 * GameManager.wormStats.wingsLevel + 200f + 7 * Mathf.Pow(1.4f, GameManager.wormStats.wingsLevel);
    }

    public static float GetShellFeatureCost() {
        if (GameManager.wormStats.NextShellID() == -1) return 0;
        switch (GameManager.wormStats.NextShellID()) {
            case 0: return 500f;
            case 1: return 2000f;
            case 2: return 15000f;
            case 3: return 50000f;
        }
        return 300 * GameManager.wormStats.NextShellID() + 200f;
    }

    public static float GetShellLevelCost() {
        return 300 * GameManager.wormStats.armorLevel + 100f + 12 * Mathf.Pow(1.5f, GameManager.wormStats.armorLevel);
    }

    public static float GetSizeCost() {
        return 100 * Mathf.Pow(2.1f, GameManager.wormStats.size) + 200f;
    }

    public static float GetSpeedCost() {
        return 10 * Mathf.Pow(1.55f, GameManager.wormStats.speed) + 150f + 600 * GameManager.wormStats.speed;
    }

}
