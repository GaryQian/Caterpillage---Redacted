using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinusText : MonoBehaviour {

    public Text txt;
    public float amount;
    public float downSpeed;
    public float fadeSpeed;

	// Use this for initialization
	void Start () {

        txt.text = "-" + (int)amount;

        StartCoroutine(Die());

	}

    IEnumerator Die() {
        while (txt.color.a >= 0.05f) {
            txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, Mathf.Max(0, txt.color.a - fadeSpeed * Time.deltaTime));
            txt.transform.localPosition = txt.transform.localPosition - new Vector3(0, downSpeed * Time.deltaTime, 0);
            yield return null;
        }
        Destroy(gameObject);
    }
}
