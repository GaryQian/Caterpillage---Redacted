using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodOverlay : MonoBehaviour {

    public Image img;
    public static float opacity;
    public float fadeRate;
    public float bloodRatio;
    public static float br;
    public float maxBlood;
    static float mb;

    public Image bgImg;
    public float bgMax;
	// Use this for initialization
	void Start () {
        img.color = Color.clear;
        opacity = 0;
        br = bloodRatio;
        mb = maxBlood;
	}
	
	// Update is called once per frame
	void Update () {
        if (GameManager.state != Gamestate.Playing) return;
        opacity = Mathf.Min(1f, Mathf.Max(0, opacity - fadeRate * Time.deltaTime));
        img.color = Color.Lerp(Color.clear, Color.white, opacity);

        bgImg.color = Color.Lerp(new Color(.9f, 0, 0, bgMax), new Color(.9f, 0, 0, 0), Mathf.Min(0.5f, WorldManager.wormHealth.health / WorldManager.wormHealth.maxHealth) * 2f);
    }

    public static void AddBlood(float amount) {
        opacity = Mathf.Min(mb, opacity + amount * br);
    }


}
