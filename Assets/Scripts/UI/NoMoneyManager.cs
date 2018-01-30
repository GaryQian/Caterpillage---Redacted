using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoMoneyManager : MonoBehaviour {

    public float speed;
    public float delay;
    RectTransform rt;
    SmoothMotion sm1;
    SmoothMotion sm2;
    Vector3 originalPos;

    // Use this for initialization
    void Start () {
        rt = GetComponent<RectTransform>();
        
        originalPos = rt.localPosition;

        sm1 = gameObject.AddComponent<SmoothMotion>();
        sm1.endPos = new Vector3(rt.localPosition.x, rt.localPosition.y - (100 * Util.yScale), rt.localPosition.z);
        sm1.duration = speed;
        sm1.begin();

        Invoke("Return", speed+delay);
    }

    void Return() {
        sm2 = gameObject.AddComponent<SmoothMotion>();
        sm2.autoSetStart = true;
        //sm2.startPos = rt.localPosition;
        sm2.endPos = originalPos;
        sm2.duration = speed;
        sm2.begin();
        Invoke("Die", speed);
    }

    void Die() {
        Destroy(gameObject);
    }
}
