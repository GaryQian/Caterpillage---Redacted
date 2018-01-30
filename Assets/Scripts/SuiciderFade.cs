using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuiciderFade : MonoBehaviour {
    public float life = 5f;
    float startTime;
    public SpriteRenderer sr;
    public float initDelay = 0f;
    // Use this for initialization
    void Start() {
        Setup(life);
    }

    public void Setup(float l) {
        life = l;
        startTime = Time.time;
        Invoke("StartDie", initDelay);
    }

    private void Update() {
        if (sr == null) {
            Destroy(gameObject);
            return;
        }
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f -  (Time.time - startTime) / life);
    }

    public void StartDie() {
        Invoke("Die", life);
    }

    void Die() {
        Destroy(gameObject);
    }
}