using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralAura : MonoBehaviour {

    public General gen;
    public Rigidbody2D body;

    private void FixedUpdate() {
        if (gen == null) {
            Destroy(gameObject);
            return;
        }
        body.MovePosition(gen.transform.position);
        body.MoveRotation(gen.transform.eulerAngles.z);
    }

    public void WormEntered() {
        if (gen.lastHitTime < Time.time) {
            gen.Hit();
        }
    }

}
