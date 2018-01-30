using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decor : MonoBehaviour {

    public SpriteRenderer sr;

	// Use this for initialization
	void Start () {
        sr.sprite = (Sprite)DecorManager.selectedDecor[Random.Range(0, DecorManager.selectedDecor.Count)];
        transform.position = transform.position + new Vector3(0, 0, 1);
	}
	
	private void OnDestroy() {
        WorldManager.stats.EatDecor();
    }
}
