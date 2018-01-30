using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTrail : Suicider {

    public Vector3 target;
    public SpriteRenderer sr;
    public float start;

    public float damage;

    public GameObject ReflectPrefab;
    public bool customTrail = false;
    public Color color = Color.red;
    public float width = 0.3f;

    // Use this for initialization
    void Start () {
        Invoke("Die", life);
        Reposition(target);
        start = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
        sr.color = new Color(color.r, color.g, color.b, (life - Time.time + start) / life);
	}

    public bool Reposition(Vector3 targ) {
        Vector3 offset = targ - transform.position;
        offset = new Vector3(offset.x, offset.y, 0);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, offset, offset.magnitude, 1 << 10);
        float dist = hit.collider == null ? offset.magnitude : hit.distance;
        if (hit.collider == null) {
            WorldManager.wormHealth.Damage(damage);
            if (GameManager.wormStats.reflective) {
                GameObject reflection = Instantiate(ReflectPrefab, targ + new Vector3(0, 0, 1f), Quaternion.identity);
                reflection.GetComponent<BulletTrailReflect>().inDir = offset;
            }
        }
        transform.localScale = new Vector3(dist, width, 1);
        float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0, angle);
        return hit.collider != null;
    }
}
