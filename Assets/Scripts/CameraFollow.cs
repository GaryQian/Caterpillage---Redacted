using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraMode { active, stopped, lerpToTarg, hardFollow }

public class CameraFollow : MonoBehaviour {
    public GameObject Worm;
    
    public float origJuice = 1f;
    public float Juice;
    Camera cam;
    CameraMode mode = CameraMode.active;

    ////////////
    //Lerp Mode
    ////////////
    float duration;
    Vector2 initOffset;
    float elapsedTime;
    CameraMode nextMode;
    ////////////
    ////////////

    ////////////
    //Shake
    ////////////
    float shakeTime;
    float totalShakeTime;
    float shakeAmount;
    bool priority;
    ////////////
    ////////////
    // Use this for initialization
    void Start () {
        cam = GetComponent<Camera>();
        Juice = origJuice;
	}
	
	// Update is called once per frame
	void Update () {
        if (mode == CameraMode.stopped) return;
        Juice += (origJuice - Juice) * Time.deltaTime * 1f;
        Vector3 offset = Worm.transform.position - transform.position;
        switch (mode) {
            case CameraMode.active: {
                    transform.position = transform.position + offset * Juice * Time.deltaTime;
                    transform.position = new Vector3(transform.position.x, transform.position.y, -20f);
                    break;
                }
            case CameraMode.lerpToTarg: {
                    if (elapsedTime >= duration) {
                        JumpToWorm();
                        mode = nextMode;
                    }
                    transform.position = new Vector3(Mathf.SmoothStep(initOffset.x + WorldManager.Worm.transform.position.x, WorldManager.Worm.transform.position.x, elapsedTime / duration), Mathf.SmoothStep(initOffset.y + WorldManager.Worm.transform.position.y, WorldManager.Worm.transform.position.y, elapsedTime / duration), -20f);
                    elapsedTime += Time.deltaTime;
                    break;
                }
            case CameraMode.hardFollow: {
                    JumpToWorm();
                    break;
                }

        }
        if (shakeTime > 0) {
            float ratio = shakeTime / totalShakeTime;
            transform.position = transform.position + new Vector3(Random.Range(-shakeAmount, shakeAmount) * ratio, Random.Range(-shakeAmount, shakeAmount) * ratio, 0) * Time.deltaTime;
            shakeTime -= Time.deltaTime;
        }
	}

    public void Shake(float amount, float time, bool p=false) {
        //if (shakeTime > totalShakeTime / 2f) {
        //    if (amount < shakeAmount) shakeAmount += amount;
        //}
        if (p) {
            priority = true;
            shakeTime = time;
            totalShakeTime = time;
            shakeAmount = amount;
            Invoke("UnsetPriority", time);
        }
        else {
            if (!priority) {
                shakeTime = time;
                totalShakeTime = time;
                shakeAmount = amount;
            }
        }
    }

    void UnsetPriority() {
        priority = false;
    }

    public void Activate() {
        mode = CameraMode.active;
    }

    public void Deactivate() {
        mode = CameraMode.stopped;
    }

    public void SetMode(CameraMode m) {
        mode = m;
    }

    public void LerpToWorm(float duration, CameraMode nextmode=CameraMode.active) {
        mode = CameraMode.lerpToTarg;
        this.duration = duration;
        Vector3 offset = Worm.transform.position - transform.position;
        initOffset = new Vector3(offset.x, offset.y, 0);
        elapsedTime = 0;
        nextMode = nextmode;

    }

    public void SetJuice(float j) {
        Juice = j;
    }

    public void JumpToWorm() {
        transform.position = new Vector3(WorldManager.Worm.transform.position.x, WorldManager.Worm.transform.position.y, -20f);
    }

    public void JumpToPosition(Vector3 pos) {
        transform.position = new Vector3(pos.x, pos.y, -20f);
    }
}
