using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetSummaryManager : MonoBehaviour {

    // Use this for initialization
    public Text message;
    public Text timeText;
    public Text coinsText;

    public GoToTarget starGTT;
    public GoToTarget timeGTT;
    public GoToTarget coinsGTT;
    public GoToTarget GTT;

    public GameObject timeHolder;
    public GameObject coinHolder;
    public float startScale;
    float currTimeScale;
    float currCoinScale;

    float currCoins = 0;
    float targetCoins;
    float clearTime;
    string timestring;

    int stage = 0;

    string planetName;
    public int timeLeft = 4;
    public StarDisplayer starDisplayer;

    public AudioClip coinWinkClip;
    int prevCoins;
    float prevWink;

    // Use this for initialization
    void Start() {
        planetName = PlanetNameGenerator.NewName();

        timeHolder.SetActive(false);
        coinHolder.SetActive(false);
        stage = 0;
        Invoke("ShowCoins", 0.75f);
        //Invoke();
        UpdateText();
    }

    // Update is called once per frame
    void Update() {
        switch (stage) {
            case 0: {
                    // Stars Dropping
                    break;
                }
            case 1: {
                    // Coins
                    currCoinScale += (1f - currCoinScale) * 10f * Time.deltaTime;
                    coinHolder.transform.localScale = Vector3.one * currCoinScale;
                    break;
                }
            case 2: {
                    // Time
                    currTimeScale += (1f - currTimeScale) * 10f * Time.deltaTime;
                    timeHolder.transform.localScale = Vector3.one * currTimeScale;

                    currCoinScale += (1f - currCoinScale) * 10f * Time.deltaTime;
                    coinHolder.transform.localScale = Vector3.one * currCoinScale;
                    break;
                }
            case 3: {
                    // wait
                    break;
                }

        }
        if (stage == 1 || stage == 2 || stage == 3) {
            currCoins += (targetCoins - currCoins) * Time.deltaTime * 5f;
            coinsText.text = "" + (int)currCoins;
            if ((int)currCoins != prevCoins && Time.time - prevWink > 0.06f) {
                SoundManager.PlaySingleSfx(coinWinkClip, 1.5f);
                prevWink = Time.time;
            }
            prevCoins = (int)currCoins;
            Util.wm.bm.SetCoins(targetCoins - currCoins);
        }
        //else if (stage == 3) {
        //
        //}
    }

    void UpdateText() {
        clearTime = WorldManager.stats.currentPlanetStats.clearTime;
        if (clearTime < 7f) SocialManager.Instance.ReportAchievement("CgkIqKL72ZAZEAIQDg", "speeddemon");
        timestring = Util.wm.bm.SetTime(WorldManager.stats.currentPlanetStats.clearTime);
        timeText.text = timestring;
        targetCoins = CoinHandler.stagedCoins;
        coinsText.text = "" + (int)CoinHandler.stagedCoins;
        message.text = WorldManager.stats.currentPlanetStats.name + " Destroyed";
        timeLeft--;
        if (timeLeft < 0) {
            CancelInvoke("UpdateText");
        }
    }

    public void PauseButtonPress() {

    }

    void ShowTime() {
        stage++;
        coinsGTT.ChangeTarget(new Vector3(0, -210f, 0));
        timeHolder.SetActive(true);
        currTimeScale = startScale;
        timeHolder.transform.localScale = Vector3.one * startScale;

        Invoke("Finish", .75f);
    }

    void ShowCoins() {
        stage++;
        starGTT.ChangeTarget(new Vector3(0, 240f, 0));
        coinHolder.SetActive(true);
        currCoinScale = startScale;
        coinHolder.transform.localScale = Vector3.one * startScale;
        Invoke("ShowTime", .75f);
    }

    void Finish() {
        stage++;
        currCoins = targetCoins;
        Invoke("Exit", 2.5f);
        Invoke("Die", 3.2f);
    }

    void Exit() {
        starGTT.ChangeTarget(new Vector3(0, 1080f, 0));
        Invoke("ExitTime", 0.10f);
        coinsGTT.ChangeTarget(new Vector3(0, -1080f, 0));
    }

    void ExitTime() {
        timeGTT.ChangeTarget(new Vector3(0, 1000f, 0));
    }
    

    void Die() {
        Destroy(gameObject);
    }
}
