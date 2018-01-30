using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour {
    Vector3 startAngle;
    //public float breakForce;
    //public float breakTorque;

    //public Texture2D[] sprites;
    public Destructible2D.D2dDestructible d;

    //Rigidbody2D body;


    public bool isOrig = false;
	// Use this for initialization
	void Start () {
        GetComponent<Rigidbody2D>().freezeRotation = true;
        startAngle = transform.eulerAngles;
        //Invoke("AllowRotate", 0.75f);
        InvokeRepeating("CountAsBroken", 2f, Random.Range(0.5f, 0.75f));

        transform.localScale = new Vector3(transform.localScale.x * Mathf.Sign(Random.Range(-1f, 1f)), transform.localScale.y, 1);


        //body = GetComponent<Rigidbody2D>();
        //GetComponent<Destructible2D.D2dDestructible>().MainTex = sprites[Random.Range(0, sprites.Length)];
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void AllowRotate() {
        GetComponent<Rigidbody2D>().freezeRotation = false;
        transform.eulerAngles = startAngle;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        CountAsBroken();
    }

    public void CountAsBroken() {
        if (isOrig) {
            if (d.AlphaCount < d.OriginalAlphaCount * .75f) {
                WorldManager.stats.EatBuilding();
                isOrig = false;
                WorldManager.wormHealth.Feed(5f);
            }
        }
    }

    //private void OnBecameVisible() {
    //    body.simulated = true;
    //}
    //
    //private void OnBecameInvisible() {
    //    if (body.velocity.sqrMagnitude < 1f) body.simulated = false;
    //}
}
