using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMotion : MonoBehaviour {
    public Rigidbody2D body;
    Vector2 v;
    public int dir;
    public float walkForce = 3f;
	// Use this for initialization
	void Start () {
        NewWalk();
	}

    void NewWalk() {
        dir = Random.Range(0, 100f) < 50f ? -1 : 1;
        transform.localScale = new Vector3(transform.localScale.x * dir, transform.localScale.y, transform.localScale.z);
        Invoke("NewWalk", Random.Range(2f, 5f));
    }

    public void Fear(Vector3 pos) {
        float currAngle = AngleFromOffset(transform.position);
        float otherAngle = AngleFromOffset(pos);
        if (currAngle > otherAngle) {
            dir = -1;
        }
        else {
            dir = 1;
        }
        body.velocity = Vector2.zero;
        transform.localScale = new Vector3(transform.localScale.x * dir, transform.localScale.y, transform.localScale.z);
        body.velocity = Vector2.zero;
        ApplyForce(walkForce);
        /*float dist = Vector3.SqrMagnitude(pos - transform.position);
        if (Vector3.SqrMagnitude(pos - (transform.position + new Vector3(v.x, v.y, 0) * 0.1f)) < dist) {
            dir *= -1;
            Debug.Log("Turning");
        }
        CancelInvoke("NewWalk");
        Invoke("NewWalk", Random.Range(2f, 5f));*/
    }

    public void Stop() {
        Stop(Vector3.zero);
    }

    public void Stop(Vector3 pos) {
        float currAngle = AngleFromOffset(transform.position);
        float otherAngle = AngleFromOffset(pos);
        if (currAngle > otherAngle) {
            dir = -1;
        }
        else {
            dir = 1;
        }
        transform.localScale = new Vector3(transform.localScale.x * dir, transform.localScale.y, transform.localScale.z);
        dir = 0;
        CancelInvoke("NewWalk");
        Invoke("NewWalk", Random.Range(2f, 5f));
    }

    public void Move() {
        if (dir == 0) {
            CancelInvoke("NewWalk");
            NewWalk();
        }
    }

    // Update is called once per frame
    void FixedUpdate () {
        if (dir != 0) {
            ApplyForce(walkForce);
        }
	}

    void ApplyForce(float strength) {
        Vector3 normPos = transform.position.normalized;
        v = dir > 0 ? new Vector2(normPos.y, -normPos.x) : new Vector2(-normPos.y, normPos.x);
        v *= strength;
        body.AddForce(v);
    }

    float AngleFromOffset(Vector3 offset) {
        return Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
    }
}
