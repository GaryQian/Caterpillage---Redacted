using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTrailReflect : MonoBehaviour {
    public Vector3 target;
    public SpriteRenderer sr;
    public float start;
    public float life;
    public Vector2 inDir;
    public float dist;

    public float damage;
    // Use this for initialization
    void Start () {
        int layerMask = 1 << 12 + 1 << 14 + 1 << 15 + 1 << 10;
        Invoke("Die", life);
        start = Time.time;

        Vector2 dir = Vector2.Reflect(inDir, Util.Vector2FromAngle(WorldManager.Worm.transform.eulerAngles.z + 90f));
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 5f, layerMask);
        dist = hit.collider == null ? dist : hit.distance;

        transform.localScale = new Vector3(dist, 0.3f, 1);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0, angle);

        if (hit.collider != null && hit.collider.GetComponent<Character>() != null) {
            hit.collider.GetComponent<Character>().Damage(damage);
        }

    }

    // Update is called once per frame
    void Update() {
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, (life - Time.time + start) / life);
    }

    //public bool Reposition(Vector3 targ) {
    //    Vector3 offset = targ - transform.position;
    //    offset = new Vector3(offset.x, offset.y, 0);
    //    RaycastHit2D hit = Physics2D.Raycast(transform.position, offset, offset.magnitude, 1 << 10);
    //    float dist = hit.collider == null ? offset.magnitude : hit.distance;
    //    if (hit.collider == null) WorldManager.wormHealth.Damage(5);
    //    transform.localScale = new Vector3(dist, 0.3f, 1);
    //    float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
    //    transform.eulerAngles = new Vector3(0, 0, angle);
    //    return hit.collider != null;
    //}

    void Die() {
        Destroy(gameObject);
    }
}
