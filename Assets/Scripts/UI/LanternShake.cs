using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanternShake : MonoBehaviour {

    public GameObject lantern1;
    public GameObject lantern2;
    public GameObject light1;
    public GameObject light2;

    public Sprite lanternOn;
    public Sprite lanternOff;

    bool lantern1On = true;
    bool lantern2On = true;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        lantern1.transform.localRotation = Quaternion.Euler(0, 0, Mathf.Sin(Time.time*1.5f)*3f);
        lantern2.transform.localRotation = Quaternion.Euler(0, 0, Mathf.Sin(Time.time * 1.5f+1f) * 3f);
    }

    public void Lantern1Tap() {
        if (lantern1On) {
            lantern1On = false;
            lantern1.GetComponent<Image>().sprite = lanternOff;
            light1.gameObject.SetActive(false);
        }
        else {
            lantern1On = true;
            lantern1.GetComponent<Image>().sprite = lanternOn;
            light1.gameObject.SetActive(true);
        }
        
    }

    public void Lantern2Tap() {
        if (lantern2On) {
            lantern2On = false;
            lantern2.GetComponent<Image>().sprite = lanternOff;
            light2.gameObject.SetActive(false);
        }
        else {
            lantern2On = true;
            lantern2.GetComponent<Image>().sprite = lanternOn;
            light2.gameObject.SetActive(true);
        }

    }
}
