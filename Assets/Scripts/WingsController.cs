using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingsController : MonoBehaviour {
    public GameObject wing1;
    public GameObject wing2;

    public Sprite chickenWing;
    public Sprite wingSprite1;
    public Sprite wingSprite2;
    public Sprite butterflySprite;

    public Animator topAnimator;
    public Animator botAnimator;

    bool isButter;
    // Use this for initialization

    public float baseScale;
    public float incScale;
    float wingScale;
	void Start () {
		
	}
	
    public void Begin() {
        if (!GameManager.wormStats.hasWings) {
            wing1.SetActive(false);
            wing2.SetActive(false);
        }
        else {
            wing1.SetActive(true);
            wing2.SetActive(true);

            wingScale = baseScale + incScale * GameManager.wormStats.wingsLevel;
            wing1.transform.localScale = new Vector3(1, 1, 1) * wingScale;
            wing2.transform.localScale = new Vector3(1, -1, 1) * wingScale;

            isButter = false;
            if (GameManager.wormStats.butterfly) {
                wing1.GetComponent<SpriteRenderer>().sprite = butterflySprite;
                wing2.GetComponent<SpriteRenderer>().sprite = butterflySprite;
                isButter = true;
                topAnimator.SetBool("Butter", true);
                botAnimator.SetBool("Butter", true);
            }
            else if (GameManager.wormStats.doubleFlap) {
                wing1.GetComponent<SpriteRenderer>().sprite = wingSprite2;
                wing2.GetComponent<SpriteRenderer>().sprite = wingSprite2;
                topAnimator.SetBool("WingT", true);
                botAnimator.SetBool("WingB", true);
            }
            else if (GameManager.wormStats.dive) {
                wing1.GetComponent<SpriteRenderer>().sprite = wingSprite1;
                wing2.GetComponent<SpriteRenderer>().sprite = wingSprite1;
                Debug.Log("Setting wing bools");
                topAnimator.SetBool("WingT", true);
                botAnimator.SetBool("WingB", true);
            }
            else if (GameManager.wormStats.flap) {
                wing1.GetComponent<SpriteRenderer>().sprite = wingSprite1;
                wing2.GetComponent<SpriteRenderer>().sprite = wingSprite1;
                topAnimator.SetBool("WingT", true);
                botAnimator.SetBool("WingB", true);
            }
            else if (GameManager.wormStats.wingsLevel >= 2) {
                wing1.GetComponent<SpriteRenderer>().sprite = wingSprite1;
                wing2.GetComponent<SpriteRenderer>().sprite = wingSprite1;
                topAnimator.SetBool("WingT", true);
                botAnimator.SetBool("WingB", true);
            }
            else {
                wing1.GetComponent<SpriteRenderer>().sprite = chickenWing;
                wing2.GetComponent<SpriteRenderer>().sprite = chickenWing;
                topAnimator.SetBool("WingT", true);
                botAnimator.SetBool("WingB", true);
            }
        }
    }

    public void Flap() {
        topAnimator.SetBool("Underground", false);
        botAnimator.SetBool("Underground", false);

        topAnimator.SetTrigger("FlapT");
        botAnimator.SetTrigger("FlapB");
    }


    public void Dive() {
        topAnimator.SetBool("Diving", true);
        botAnimator.SetBool("Diving", true);
    }

    public void UnDive() {
        topAnimator.SetBool("Diving", false);
        botAnimator.SetBool("Diving", false);
    }

    public void AirToGround() {
        SetAnimType();
        topAnimator.SetBool("Underground", true);
        botAnimator.SetBool("Underground", true);

        UnDive();

    }

    public void GroundToAir() {
        topAnimator.SetBool("Underground", false);
        botAnimator.SetBool("Underground", false);
    }
    // Update is called once per frame
    
    public void SetAnimType() {
        if (isButter) {
            topAnimator.SetBool("Butter", true);
            botAnimator.SetBool("Butter", true);
        }
        else {
            topAnimator.SetBool("WingT", true);
            botAnimator.SetBool("WingB", true);
        }
    }
}
