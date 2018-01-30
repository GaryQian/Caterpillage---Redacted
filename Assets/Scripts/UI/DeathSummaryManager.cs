using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathSummaryManager : MonoBehaviour {

    public Button continueButton;

    //Titles
    public Text mainTitle;
    public Text leftTitle;
    public Text rightTitle;

    //Descriptions
    public Text planetsDestroyedText;
    public Text buildingsEatenText;
    public Text damageTakenText;
    public Text damageDealtText;

    public Text citizensEatenText;
    public Text soldiersEatenText;
    public Text carsEatenText;
    public Text helicoptersEatenText;
    public Text satellitesEatenText;
    public Text tanksEatenText;
    public Text juggernautsEatenText;

    //Numbers
    public Text planetsDestroyed;
    public Text buildingsEaten;
    public Text damageTaken;
    public Text damageDealt;

    public Text citizensEaten;
    public Text soldiersEaten;
    public Text carsEaten;
    public Text helicoptersEaten;
    public Text satellitesEaten;
    public Text tanksEaten;
    public Text juggernautsEaten;

    public Text goldEarned;
    public Text starsEarned;

    float fontScale;

    void Start () {
        fontScale = Util.fontScale;

        mainTitle.fontSize = (int)(mainTitle.fontSize * fontScale);
        leftTitle.fontSize = (int)(leftTitle.fontSize * fontScale);
        rightTitle.fontSize = (int)(rightTitle.fontSize * fontScale);

        planetsDestroyedText.fontSize = (int)(planetsDestroyedText.fontSize * fontScale);
        buildingsEatenText.fontSize = (int)(buildingsEatenText.fontSize * fontScale);
        damageTakenText.fontSize = (int)(damageTakenText.fontSize * fontScale);
        damageDealtText.fontSize = (int)(damageDealtText.fontSize * fontScale);

        citizensEatenText.fontSize = (int)(citizensEatenText.fontSize * fontScale);
        soldiersEatenText.fontSize = (int)(soldiersEatenText.fontSize * fontScale);
        carsEatenText.fontSize = (int)(carsEatenText.fontSize * fontScale);
        helicoptersEatenText.fontSize = (int)(helicoptersEatenText.fontSize * fontScale);
        satellitesEatenText.fontSize = (int)(satellitesEatenText.fontSize * fontScale);
        tanksEatenText.fontSize = (int)(tanksEatenText.fontSize * fontScale);
        juggernautsEatenText.fontSize = (int)(juggernautsEatenText.fontSize * fontScale);


        planetsDestroyed.fontSize = (int)(planetsDestroyed.fontSize * fontScale);
        buildingsEaten.fontSize = (int)(buildingsEaten.fontSize * fontScale);
        damageTaken.fontSize = (int)(damageTaken.fontSize * fontScale);
        damageDealt.fontSize = (int)(damageDealt.fontSize * fontScale);

        citizensEaten.fontSize = (int)(citizensEaten.fontSize * fontScale);
        soldiersEaten.fontSize = (int)(soldiersEaten.fontSize * fontScale);
        carsEaten.fontSize = (int)(carsEaten.fontSize * fontScale);
        helicoptersEaten.fontSize = (int)(helicoptersEaten.fontSize * fontScale);
        satellitesEaten.fontSize = (int)(satellitesEaten.fontSize * fontScale);
        tanksEaten.fontSize = (int)(tanksEaten.fontSize * fontScale);
        juggernautsEaten.fontSize = (int)(juggernautsEaten.fontSize * fontScale);

        goldEarned.fontSize = (int)(goldEarned.fontSize * fontScale);
        starsEarned.fontSize = (int)(starsEarned.fontSize * fontScale);




        planetsDestroyed.text = "" + WorldManager.stats.prevPlanetStats.Count;
        buildingsEaten.text = "" + WorldManager.stats.buildingsEaten;
        damageTaken.text = "" + (int)WorldManager.stats.damageTaken;
        damageDealt.text = "" + (int)WorldManager.stats.damageDealt;

        citizensEaten.text = "" + WorldManager.stats.harmlessKilled;
        soldiersEaten.text = "" + WorldManager.stats.soldiersKilled;
        carsEaten.text = "" + WorldManager.stats.carsKilled;
        helicoptersEaten.text = "" + WorldManager.stats.helicoptersKilled;
        satellitesEaten.text = "" + WorldManager.stats.satellitesKilled;
        tanksEaten.text = "" + WorldManager.stats.tanksKilled;
        juggernautsEaten.text = "" + WorldManager.stats.generalsKilled;

        goldEarned.text = "" + (int)WorldManager.stats.TotalCoins();
        starsEarned.text = "" + (int)WorldManager.stats.TotalNewStars();
    }

    private void Update() {
        if (GameManager.state != Gamestate.Playing) {
            Die();
            Util.gm.CloseStatsScreen();
        }
    }

    public void ContinueButtonPress() {
        //if (!GameManager.settings.adsRemoved 
        //    && GameManager.plays > 1 
        //    && GameManager.plays % 2 == 0 
        //    && GameManager.maxLevel >= 5
        //    && AdManager.interstitial.IsLoaded()) {
        Invoke("Die", 0.5f);
        if (!GameManager.settings.adsRemoved
            && GameManager.maxLevel >= 5
            && Random.Range(0, 1f) < 0.8f
            && AdManager.interstitial.IsLoaded()) {

            if (Time.realtimeSinceStartup - AdManager.lastInterstitialTime > AdManager.interstitialDelay) {

                AdManager.interstitial.Show();
                AdManager.interstitial.Destroy();
                GameManager.adManager.RequestInterstitial();

                AdManager.lastInterstitialTime = Time.realtimeSinceStartup;
            }

        }

        Util.gm.CloseStatsScreen();
        SocialManager.Instance.ReportAll();
        WorldManager.pg.ErasePlanet();
        Die();
    }

    void Die() {
        Destroy(gameObject);
    }
}
