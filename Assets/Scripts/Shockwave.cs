using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shockwave : MonoBehaviour {

    public float maxDist;
    public float damage;

	// Use this for initialization
	void Start () {
        WorldManager.cameraFollow.Shake(10f, 0.4f, true);
        Vector3 offset = (Vector2)(WorldManager.Worm.transform.position - transform.position);
        if (offset.magnitude < maxDist) {

            RaycastHit2D hit = Physics2D.Raycast(transform.position, offset, Mathf.Min(maxDist, offset.magnitude), 1 << 10);
            if (hit.collider == null && !WorldManager.wormMotion.underground) {
                WorldManager.wormMotion.v += offset.normalized * 4f;
                WorldManager.wormHealth.Damage(damage);
            }
            else if (offset.magnitude < maxDist / 2f) {
                WorldManager.wormHealth.Damage(damage);
            }
        }
	}
	
}
