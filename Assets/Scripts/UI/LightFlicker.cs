using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightFlicker : MonoBehaviour {

    public float amp;
    public float rate;
    public float minOpacity;

    Image image;
    float random;
	// Use this for initialization
	void Start () {
        image = GetComponent<Image>();
        random = Random.Range(0, 1000);
    }
	
	// Update is called once per frame
	void Update () {
        //RectOffset = 1f - amp + (2 * amp * Mathf.PerlinNoise(Time.time, 0));
        image.color = new Color(image.color.r, image.color.g, image.color.b, minOpacity + amp/2f + (amp * (Mathf.PerlinNoise(Time.time*rate+random, 0)-0.5f)));
        //rect.localScale = new Vector3(1f - amp + (2 * amp * Mathf.PerlinNoise(Time.time, 0)), 1f - amp + (2 * amp * Mathf.PerlinNoise(Time.time, 0)), 1f - amp + (2 * amp * Mathf.PerlinNoise(Time.time, 0)));
	}
}
