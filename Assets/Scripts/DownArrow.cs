using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownArrow : MonoBehaviour {

    public SpriteRenderer sr;
    public float opacity;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.rotation = Quaternion.Euler(0, 0, Util.AngleFromOffset(transform.position) + 180f);
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, WorldManager.wormMotion.underground ? 0 : opacity);
        //sr.color = new Color(1f-WorldManager.stats.currentPlanetStats.planetColor.r, 1f - WorldManager.stats.currentPlanetStats.planetColor.g, 1f - WorldManager.stats.currentPlanetStats.planetColor.b, WorldManager.wormMotion.underground ? 0 : opacity);
    }
}
