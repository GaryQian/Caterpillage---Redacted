using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tentacle : MonoBehaviour {

    public Vector3 target;
    public GameObject targetObj;
    public GameObject parent;
    public SpriteRenderer sr;
    float start;
    public float life;
    public Vector2 inDir;

    public float extendSpeed = 0.35f;
    public float retractSpeed = 0.55f;
    public float extendTime;
    bool extending;
    public float yScale = 1f;

    float dist = 0;

    int layerStore;

    public float damage;
    // Use this for initialization
    void Start() {
        int layerMask = 1 << 12 + 1 << 14 + 1 << 15 + 1 << 10;
        //Invoke("Die", life);
        start = Time.time;
        extendTime = 0;
        extending = true;

        Reposition();

    }

    // Update is called once per frame
    void Update() {
        if (targetObj == null || parent == null) {
            Destroy(gameObject);
            return;
        }
        Reposition();
        extendTime += Time.deltaTime;
        if (extending && extendTime > extendSpeed) {
            extending = false;
            extendTime = 0;
            targetObj.GetComponent<Rigidbody2D>().isKinematic = true;
            //set layer to pulling to avoid collisions
            layerStore = targetObj.layer;
            targetObj.layer = 18;
            //dist = 
        }
        if (!extending && extendTime > retractSpeed) {
            Die();
        }
    }


    void Reposition() {
        transform.position = new Vector3(parent.transform.position.x, parent.transform.position.y, -0.5f);
        float ratio = extending ? extendTime / extendSpeed * dist : (retractSpeed - extendTime) / retractSpeed;
        if (extending) {
            Vector3 offset = new Vector3(targetObj.transform.position.x - transform.position.x, targetObj.transform.position.y - transform.position.y, 0);
            float angle = Util.AngleFromOffset(offset);
            dist = offset.magnitude;
            transform.eulerAngles = new Vector3(0, 0, angle);
        }
        else {
            Vector3 pos = transform.position + Util.Vector3FromAngle(transform.eulerAngles.z).normalized * ratio * dist;
            targetObj.transform.position = new Vector3(pos.x, pos.y, targetObj.transform.position.z);
        }
        transform.localScale = new Vector3(ratio * dist, yScale, 1f);
    }

    void Die() {
        targetObj.layer = layerStore;
        Destroy(targetObj.GetComponent<TentacleMarker>());
        targetObj.GetComponent<Rigidbody2D>().isKinematic = false;
        targetObj.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        targetObj.GetComponent<Character>().Damage(damage);
        Destroy(gameObject);
    }

    private void OnDestroy() {
        if (targetObj != null) {
            targetObj.layer = layerStore;
        }
    }
}
