using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtParticle : MonoBehaviour {

    public SpriteRenderer sr;
	// Use this for initialization
	void Start () {
        Color pc = WorldManager.stats.currentPlanetStats.planetColor;
        sr.color = new Color(pc.r + Random.Range(-.3f, .3f), pc.g + Random.Range(-.3f, .3f), pc.b + Random.Range(-.3f, .3f));
        GetComponent<SuiciderFade>().Setup(GameManager.wormStats.size * 0.06f + 0.25f);
	}
}
