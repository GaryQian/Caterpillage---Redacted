using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToTarget : MonoBehaviour {

    public Vector3 startPosition;
    public Vector3 targetPosition;
    Vector3 nextPosition;
    public float speed = 1f;

    public bool autosetStart;
    public bool autosetTarget;

    public bool hover = false;
    public float rate = 1f;
    public float amp = 1f;

    public float offset = 0f;

    public bool ignoreTimeScale = false;
    float lastTime = 0f;



    float timeElapsed = 0f;
	// Use this for initialization
	void Start () {
        targetPosition = new Vector3(Util.xScale * targetPosition.x, Util.yScale * targetPosition.y, targetPosition.z);
        startPosition = new Vector3(Util.xScale * startPosition.x, Util.yScale * startPosition.y, startPosition.z);
        //Debug.Log(targetPosition.y*Util.yScale);//#################################
        if (autosetStart) {
            startPosition = transform.localPosition;
        }
        if (autosetTarget) {
            targetPosition = transform.localPosition;
            transform.localPosition = startPosition;
        }

        //Debug.Log(targetPosition);
	}
	
	// Update is called once per frame
	void Update () {
        if (ignoreTimeScale) {
            
            if (hover) {
                float ph = -Mathf.Sin(Time.realtimeSinceStartup * rate + offset) * amp;
                nextPosition = (targetPosition - transform.localPosition - new Vector3(0, ph, 0)) * speed * Time.deltaTime + transform.localPosition;
                nextPosition = nextPosition + new Vector3(0, ph, 0);
            }
            else {
                nextPosition = (targetPosition - transform.localPosition) * speed * (Time.realtimeSinceStartup-lastTime) + transform.localPosition;
            }

            transform.localPosition = nextPosition;
            lastTime = Time.realtimeSinceStartup;
            return;
        }
        else {
            if (hover) {
                timeElapsed += Time.deltaTime;
                float ph = -Mathf.Sin(timeElapsed * rate + offset) * amp;
                nextPosition = (targetPosition - transform.localPosition - new Vector3(0, ph, 0)) * speed * Time.deltaTime + transform.localPosition;
                nextPosition = nextPosition + new Vector3(0, ph, 0);
            }
            else {
                nextPosition = (targetPosition - transform.localPosition) * speed * Time.deltaTime + transform.localPosition;
            }

            transform.localPosition = nextPosition;
        }
    }

    public void ChangeTarget(Vector3 targ, bool scale = true) {
        if (scale) {
            targetPosition = new Vector3(Util.xScale * targ.x, Util.yScale * targ.y, targetPosition.z);
            startPosition = new Vector3(Util.xScale * startPosition.x, Util.yScale * startPosition.y, startPosition.z);
        }
        else {
            targetPosition = new Vector3(targ.x, targ.y, targetPosition.z);
            startPosition = new Vector3(startPosition.x, startPosition.y, startPosition.z);
        }
    }
}
