using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class BarManager : MonoBehaviour {

    public GameObject healthBar;

    public GameObject healthDmgInd;
    public float indDelay = 0.5f;

    public GameObject healthGray;
    public GameObject healthShine1;
    public GameObject healthShine2;
    public GameObject healthTickPref;
    public GameObject healthWrapper;

    public GameObject destructionBar;
    public GameObject destructionGray;

    public Text coins;
    public Text timer;
    public Text diffMult;
    public Text streakMult;

    public Text planetName;

    float scale;

    List<GameObject> ticks;

    // Use this for initialization
    void Start () {
        coins.text = "" + GameManager.money;
        scale = Screen.width / 1920f;
        SetHealthTicks(500);
    }

    public void SetCoins(float c) {
        if (coins == null) return;
        coins.text = "" + (int)c;
    }

    public string SetTime(float t) {
        int min = (int)(t / 60);
        int sec = (int)(t % 60);
        if (sec < 10) {
            timer.text = "" + min + ":0" + sec;
        }
        else {
            timer.text = "" + min + ":" + sec;
        }
        return timer.text;
    }

    public void SetMultipliers(float diff, float streak) {
        diffMult.text = "x" + diff;
        streakMult.text = "x" + streak;
    }
    
    public void SetPlanetName() {
        planetName.text = WorldManager.stats.currentPlanetStats.name + " (" + WorldManager.stats.currentPlanetStats.level + ")";
    }

    public void SetHealthTicks(float maxHealth) {
        if (ticks != null) {
            foreach (GameObject t in ticks) {
                Destroy(t);
            }
        }
        ticks = new List<GameObject>();
        for (float i = 1; i < maxHealth / 100; i++) {
            GameObject healthTick = Instantiate(healthTickPref, Vector3.zero, Quaternion.identity, GetComponent<RectTransform>());
            healthTick.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            healthTick.GetComponent<RectTransform>().anchorMin = new Vector3(
                0.2396875f + (i * 0.5198334f / (maxHealth / 100)),
                healthTick.GetComponent<RectTransform>().anchorMin.y);
            healthTick.GetComponent<RectTransform>().anchorMax = new Vector3(
                0.2396875f + 0.0016875f + (i * 0.5198334f / (maxHealth / 100)),
                healthTick.GetComponent<RectTransform>().anchorMax.y);
            ticks.Add(healthTick);
        }
    }

    public void SetHealthBar(float health, float maxHealth) {
        if (healthBar == null) return;
        float percent = Mathf.Min(health / maxHealth, 1f);
        if (healthBar != null) healthBar.GetComponent<RectTransform>().localScale = new Vector3(percent, 1, 1);
        if (1 >= percent && percent >= .5) {
            healthBar.GetComponent<Graphic>().color = Color.Lerp(Color.yellow, Color.green, (percent-0.5f)*2) * 0.8f + new Color(0,0,0,255);
        }
        else if (.5 > percent && percent >= 0) {
            healthBar.GetComponent<Graphic>().color = Color.Lerp(Color.red, Color.yellow, percent*2) * 0.8f + new Color(0, 0, 0, 255);
        }
    }

    public void SetHealthInd(float health, float maxHealth) {
        float percent = Mathf.Max(Mathf.Min(health / maxHealth, 1f), 0);
        if (healthBar != null) healthDmgInd.GetComponent<RectTransform>().localScale = new Vector3(percent, 1, 1);
    }

    public void SetDestructionBar(float destruction, float maxDestruction) {
        destructionBar.GetComponent<RectTransform>().localScale = new Vector3((destruction / maxDestruction), 1, 1);
    }
}
