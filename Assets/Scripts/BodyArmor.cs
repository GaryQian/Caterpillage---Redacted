using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyArmor : MonoBehaviour {

    public Sprite armorSprite;
    public Sprite thornsSprite;
    public Sprite acidSprite;
    public Sprite reflectiveSprite;
    public Sprite tentacleSprite;

    SpriteRenderer sr;

	// Use this for initialization
	void Start () {
        sr = GetComponent<SpriteRenderer>();
        if (GameManager.wormStats.tentacles) {
            sr.sprite = tentacleSprite;
        }
        else if (GameManager.wormStats.reflective) {
            sr.sprite = reflectiveSprite;
        }
        else if (GameManager.wormStats.poison) {
            sr.sprite = acidSprite;
        }
        else if (GameManager.wormStats.thorns) {
            sr.sprite = thornsSprite;
        }
        else if (GameManager.wormStats.hasArmor) {
            sr.sprite = armorSprite;
        }
    }
}
