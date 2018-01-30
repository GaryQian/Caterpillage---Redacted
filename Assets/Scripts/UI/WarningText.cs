using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarningText : MonoBehaviour {

    public Color textColor;
    public string message;
    public float delay;
    public float fadeTime;

    Text text;
    bool fading = false;

	// Use this for initialization
	void Start () {
        text = GetComponent<Text>();
        text.text = message;
        text.color = textColor;
        Invoke("StartFade", delay);
        Invoke("Die", fadeTime + delay);
	}
	
	// Update is called once per frame
	void Update () {
		if (fading && text.color.a > 0) {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - 1f/fadeTime * Time.deltaTime);
        }
	}

    void StartFade() {
        fading = true;
    }

    void Die() {
        Destroy(gameObject);
    }
}
