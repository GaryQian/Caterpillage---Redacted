using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

    public int RaycastCount = 5;
    public float RaycastRadius;
    public GameObject hitObj;
    public LayerMask Mask;
    public float forceStrength;

    public AudioClip explodeClip;


	// Use this for initialization
	void Start () {
        if (!GameManager.wormStats.explosiveSpit) return;
        //int Mask = LayerMask.GetMask("Characters", "Helicopter", "Satellites", "General");//1 << 12 + 1 << 14 + 1 << 15 + 1 << 19;
        var angleStep = 360.0f / RaycastCount;

        for (var i = 0; i < RaycastCount; i++) {
            var angle = i * angleStep;
            var direction = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
            var hits = Physics2D.RaycastAll(transform.position, direction, RaycastRadius, Mask);

            Character ch;
            //Debug.Log(hits.Length);
            foreach (var hit in hits) {
                var collider = hit.collider;

                // Make sure the raycast hit something, and that it wasn't a trigger
                if (collider != null && collider.isTrigger == false) {
                    ch = collider.GetComponent<Character>();
                    if (ch != null) {
                        if (collider.gameObject != hitObj) {
                            ch.Damage(WorldManager.wormMotion.damage * (GameManager.wormStats.spitLevel + 5f) / (GameManager.wormStats.maxSpitLevel + 5));

                            var rigidbody2D = collider.attachedRigidbody;

                            if (rigidbody2D != null) {
                                var force = direction * forceStrength;

                                rigidbody2D.AddForceAtPosition(force, hit.point);
                            }
                        }
                    }
                }
            }
        }

        float vol = 0.9f;
        if (GameManager.wormStats.tripleSpit) {
            vol = 0.6f;
        }
        if (GameManager.wormStats.quintupleSpit) {
            vol = 0.5f;
        }
        if (ExplosionTracker.explodeCount < 8) SoundManager.PlaySfx(vol, explodeClip);
    }
	
}
