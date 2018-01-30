using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlapTutorialController : MonoBehaviour {

    public GameObject worm;
    public GameObject hand;
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
    void Start() {
        status = 0f;
        wormRect = worm.GetComponent<RectTransform>();
        handRect = hand.GetComponent<RectTransform>();
        closeButton.SetActive(false);
        Invoke("CanPush", delay);
    }

    // Update is called once per frame
    void Update() {
        wormRect.localPosition = new Vector3(Mathf.Lerp(100, -50, status) * Util.xScale, Mathf.Lerp(-100, 100, status * status) * Util.yScale, 0);
        wormRect.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(30f, -35f, status * status) + 180f);
        handRect.localPosition = new Vector3(Mathf.Lerp(-370, -200, Mathf.Log10(status * 15)) * Util.xScale, Mathf.Lerp(0, 150, Mathf.Log10(status * 15)) * Util.yScale, 0);

        //fireImage.color = new Color(1, 1, 1, status * status);

        coneImg.color = new Color(coneImg.color.r, coneImg.color.g, coneImg.color.b, Mathf.Sin(Time.time * 8f) * 0.2f + 0.25f);

        if (status > 1f) status = 0;
        else status += rate * Time.deltaTime;
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
