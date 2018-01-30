using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissile : MonoBehaviour {

    public Vector3 v;
    public GameObject target;

    public GameObject ExplosionPrefab;
    GameObject explosion;
    public Texture2D StampTex;
    public Rigidbody2D body;
    public float speed;
    public float drag = 0.98f;

    public float damage = 20f;
    
	// Use this for initialization
	void Start () {
        v = Vector3.zero;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        Vector3 pos = transform.position + v * Time.fixedDeltaTime;
        body.MovePosition(pos);

        Vector2 accel = (target.transform.position - transform.position).normalized * speed * Time.fixedDeltaTime;
        body.MoveRotation(Util.AngleFromOffset(accel));
        v += (Vector3)accel;
        v *= drag;
	}

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            WorldManager.wormHealth.Damage(damage);
            Explode();
        }
        if (other.tag == "Bullet") {
            other.GetComponent<Bullet>().Explode(other);
            Explode();
        }
        if (other.gameObject.layer == 10) {
            Explode();
        }
    }

    void Explode() {
        if (ExplosionPrefab != null && explosion == null) {
            explosion = Instantiate(ExplosionPrefab, transform.position, Quaternion.Euler(0, 0,transform.localEulerAngles.z + 90f));
            //explosion.GetComponent<Destructible2D.D2dExplosion>().StampTex = StampTex;
            //if (!GameManager.wormStats.explosiveSpit) explosion.GetComponent<Destructible2D.D2dExplosion>().Stamp = false;
        }
        Destroy(gameObject);
    }
}
