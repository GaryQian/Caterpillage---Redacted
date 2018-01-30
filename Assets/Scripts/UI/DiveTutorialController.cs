using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiveTutorialController : MonoBehaviour {

    public GameObject worm;
    public GameObject hand;
    public GameObject fire;
    RectTransform wormRect;
    RectTransform handRect;
    Image fireImage;
    public GameObject closeButton;
    public Image coneImg;

    bool canPush = false;
    float delay = 3f;

    float status;
    float rate = .3f;

	// Use this for initialization
	void Start () {
        status = 0f;
        wormRect = worm.GetComponent<RectTransform>();
        handRect = hand.GetComponent<RectTransform>();
        fireImage = fire.GetComponent<Image>();
        closeButton.SetActive(false);
        Invoke("CanPush", delay);
	}
	
	// Update is called once per frame
	void Update () {
        wormRect.localPosition = new Vector3(Mathf.Lerp(150, 0, status) * Util.xScale, Mathf.Lerp(200, 0, status * status) * Util.yScale, 0);
        wormRect.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(-15f, 60f, status * status) + 180f);
        handRect.localPosition = new Vector3(Mathf.Lerp(-300, -50, Mathf.Log10(status * 15)) * Util.xScale, Mathf.Lerp(50, -130, Mathf.Log10(status * 15)) * Util.yScale, 0);

        fireImage.color = new Color(1, 1, 1, status * status);

        coneImg.color = new Color(coneImg.color.r, coneImg.color.g, coneImg.color.b, Mathf.Sin(Time.time * 8f) * 0.2f + 0.25f);

        if (status > 1f) status = 0;
        else status += rate*Time.deltaTime;
	}

    void CanPush() {
        closeButton.SetActive(true);
        canPush = true;
    }

    public void ExitPressed() {
        if (!canPush) return;
        GetComponent<GoToTarget>().ChangeTarget(new Vector3(0, -1000, 0));
        Invoke("Die", 1f);
    }

    void Die() {
        Destroy(gameObject);
    }
}
