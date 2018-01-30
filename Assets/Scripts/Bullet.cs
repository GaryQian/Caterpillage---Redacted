using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public Rigidbody2D body;
    public GameObject ExplosionPrefab;
    public GameObject SplatPrefab;
    public float GravityStrength = 2f;
    int counter = 0;
    public Vector2 initV;
    public bool skipStamp = false;

    public float damage = 10f;

    public Texture2D StampTex;

    bool hitUnit = false;


    GameObject explosion;
    // Use this for initialization
    void Start () {
        body.velocity = initV;
        damage = GameManager.wormStats.spitLevel * GameManager.wormStats.spitDamageIncrement + GameManager.wormStats.startSpitDamage;
        hitUnit = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (counter % 3 == 0) {
            body.AddForce(new Vector2(-transform.position.x, -transform.position.y) * GravityStrength * 3);
            body.MoveRotation(AngleFromOffset(body.velocity));
        }
        counter++;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player" || other.tag == "Bullet") return;
        if (other.gameObject.layer == 12 || other.gameObject.layer == 14 || other.gameObject.layer == 19 || other.tag == "Character" || other.tag == "Helicopter") {
            other.GetComponent<Character>().Damage(damage);
            if (GameManager.wormStats.acidSpit) {
                other.GetComponent<Character>().ApplyAcid();
            }
            hitUnit = true;
                // Spawn Splat
        }
        Explode(other);
    }

    public void Explode(Collider2D other) {
        if (ExplosionPrefab != null && explosion == null) {
            explosion = Instantiate(ExplosionPrefab, transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360f)));
            if (!GameManager.wormStats.explosiveSpit || hitUnit || skipStamp) {
                explosion.GetComponent<Destructible2D.D2dExplosion>().Stamp = false;
            }
            else {
                bool close = false;
                //Debug.Log(ExplosionTracker.set.Count);
                foreach (ExplosionTracker exp in ExplosionTracker.set.Keys) {
                    //Debug.Log((ExplosionTracker.set[exp] - (Vector2)transform.position).magnitude);
                    if ((ExplosionTracker.set[exp] - (Vector2)transform.position).magnitude < .15f) {
                        close = true;
                        break;
                    }
                }
                if (!close) {
                    explosion.GetComponent<Destructible2D.D2dExplosion>().StampTex = StampTex;
                    if (other.gameObject.layer == 8) {
                        explosion.GetComponent<Destructible2D.D2dExplosion>().StampSize = new Vector2(.7f, .7f);
                    }
                    explosion.GetComponent<ExplosionTracker>().Add();
                }
                else {
                    explosion.GetComponent<Destructible2D.D2dExplosion>().Stamp = false;
                }
            }
            //Check if hit buildings or dirt
            if (!GameManager.wormStats.explosiveSpit && other.gameObject.transform.parent != null
                && SplatPrefab != null
                && (other.gameObject.layer == 8 || other.gameObject.layer == 10)) {
                GameObject splat = Instantiate(SplatPrefab, transform.position + (Vector3)body.velocity * 0.01f, Quaternion.Euler(0, 0, Random.Range(0, 360f)), other.gameObject.transform.parent.transform);
                splat.transform.localScale = Vector3.one * (other.gameObject.layer == 10 ? 1.5f : 3f);/// other.transform.localScale.x;
                splat.transform.localPosition = splat.transform.localPosition + new Vector3(0, 0, -0.001f);
            }
            if (GameManager.wormStats.explosiveSpit) explosion.transform.localScale = explosion.transform.localScale * 2f;
            explosion.GetComponent<Explosion>().hitObj = other.gameObject;
            
        }
        Destroy(gameObject);
    }

    float AngleFromOffset(Vector3 offset) {
        return Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
    }
}
