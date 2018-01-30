using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util : MonoBehaviour {

    public static WorldManager wm;
    public static GameManager gm;
    public static Canvas canvas;
    public static float fontScale;
    public static float avgScale;

    public static float xScale;
    public static float yScale;

    public static float screenRatio;


    // Use this for initialization
    void Start () {
        screenRatio = (float)Screen.width / Screen.height;
        xScale = Screen.width / 1920f;
        yScale = Screen.height / 1080f;
        fontScale = (xScale + yScale) / 2f;
        avgScale = fontScale;

        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static float AngleFromOffset(Vector3 offset) {
        return Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
    }

    public static float AngleFromOffset(Vector2 offset) {
        return Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
    }

    public static Vector2 Vector2FromAngle(float angle) {
        return new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
    }

    public static Vector3 Vector3FromAngle(float angle) {
        return new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0);
    }

    public static string TimeToText(float t) {
        int min = (int)(t / 60);
        int sec = (int)(t % 60);
        if (sec < 10) {
            return "" + min + ":0" + sec;
        }
        else {
            return "" + min + ":" + sec;
        }
    }
}
