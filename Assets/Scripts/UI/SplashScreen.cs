using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SplashScreen : MonoBehaviour {

    public float life;
    bool outactive = false;
    bool inactive = true;
    float startTime;

    public CanvasGroup cg;
    public Image logo;
    public float fadeOutDuration;
    public float fadeInDuration;
	// Use this for initialization
	void Start () {
        Invoke("Die", life);
        Invoke("BeginOut", life - fadeOutDuration);
        Invoke("EndIn", fadeInDuration);
        startTime = Time.realtimeSinceStartup;
	}
	
	// Update is called once per frame
	void Update () {

        if (inactive) {
            float t = (Time.realtimeSinceStartup - startTime) / fadeInDuration;
            logo.color = new Color(1f, 1f, 1f, Mathf.SmoothStep(0f, 1f, t));
        }
		if (outactive) {
            float t = (Time.realtimeSinceStartup - startTime) / fadeOutDuration;
            cg.alpha = Mathf.SmoothStep(1f, 0f, t);
        }
    }

    void BeginOut() {
        outactive = true;
        startTime = Time.realtimeSinceStartup;
    }

    void EndIn() {
        inactive = false;
    }

    void Die() {
        Destroy(gameObject);
    }
}
