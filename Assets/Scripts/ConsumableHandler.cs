using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableHandler : MonoBehaviour {

    //public bool healthLowEnough = false;
    public float speedDuration;
    public float berserkDuration;
    public float berserkMultiplier;

    public bool speedActive = false;
    public bool berserkActive = false;

    public SpriteRenderer speedSr;
    public SpriteRenderer berserkSr;

    public static ConsumableHandler Instance;

    float startBerserkTime;
    float startSpeedTime;
    float berserkRemainingTime;
    float speedRemainingTime;

    // Use this for initialization
    void Start() {
        DeactivateAll();
        Instance = this;
    }

    public void UseHealthPotion() {
        WorldManager.wormHealth.FillHealth();
    }

    public void UseSpeedPotion() {
        speedActive = true;
        WorldManager.wormMotion.EnableBoost();
        startSpeedTime = Time.time;
        CancelInvoke("DeactivateSpeed");
        Invoke("DeactivateSpeed", speedDuration);

        speedSr.color = new Color(speedSr.color.r, speedSr.color.g, speedSr.color.b, 1f);
    }

    public void UseBerserkPotion() {
        berserkActive = true;
        Character.berserkMultiplier = 2f;
        startBerserkTime = Time.time;
        CancelInvoke("DeactivateBerserk");
        Invoke("DeactivateBerserk", berserkDuration);

        berserkSr.color = new Color(berserkSr.color.r, berserkSr.color.g, berserkSr.color.b, 1f);
    }

    public void DeactivateSpeed() {
        speedActive = false;
        WorldManager.wormMotion.DisableBoost();
        speedRemainingTime = speedDuration;
        CancelInvoke("DeactivateSpeed"); //Cancel just in case called from somwhere else;

        speedSr.color = new Color(speedSr.color.r, speedSr.color.g, speedSr.color.b, 0);
    }

    public void DeactivateBerserk() {
        berserkActive = false;
        Character.berserkMultiplier = 1f;
        berserkRemainingTime = berserkDuration;
        CancelInvoke("DeactivateBerserk");

        berserkSr.color = new Color(berserkSr.color.r, berserkSr.color.g, berserkSr.color.b, 0);
    }

    public void DeactivateAll() {
        DeactivateSpeed();
        DeactivateBerserk();
    }

    public void PauseAllActive() {
        if (speedActive) {
            CancelInvoke("DeactivateSpeed");
            speedRemainingTime = Time.time - startSpeedTime;
        }
        if (berserkActive) {
            CancelInvoke("DeactivateBerserk");
            berserkRemainingTime = Time.time - startBerserkTime;
        }
        Invoke("ResumeAllActive", 40f);
    }

    public void ResumeAllActive() {
        if (speedActive) {
            Invoke("DeactivateSpeed", speedRemainingTime);
        }
        if (berserkActive) {
            Invoke("DeactivateBerserk", berserkRemainingTime);
        }
    }
}
