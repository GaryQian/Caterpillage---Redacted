using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour {

    public GameObject tipButton;
    /*public GameObject playButton;
    public GameObject planet1;

    Vector3 playButtonLocation;
    Vector3 planet1Location;
    Vector3 tipButtonLocation;

    RectTransform playButtonTrans;
    RectTransform planet1Trans;
    RectTransform tipButtonTrans;

    GoToTarget playButtonGTT;
    GoToTarget planet1GTT;

    public Image bgMidBG;
    public Image bgMidDec;
    public Image bgBotBG2;
    public Image bgBotBG1;
    public Image bgBotDec;
    public Image bgTopBG2;
    public Image bgTopBG1;
    public Image bgTopDec1;
    public Image bgTopDec2;

    Vector3 bgMidBGLocation;
    Vector3 bgMidDecLocation;
    Vector3 bgBotBG2Location;
    Vector3 bgBotBG1Location;
    Vector3 bgBotDecLocation;
    Vector3 bgTopBG2Location;
    Vector3 bgTopBG1Location;
    Vector3 bgTopDec1Location;
    Vector3 bgTopDec2Location;

    RectTransform bgMidBGTrans;
    RectTransform bgMidDecTrans;
    RectTransform bgBotBG2Trans; //bot
        Vector2 bgBotBG2AnchorMin;
        Vector2 bgBotBG2AnchorMax;
    RectTransform bgBotBG1Trans;
        Vector2 bgBotBG1AnchorMin;
        Vector2 bgBotBG1AnchorMax;
    RectTransform bgBotDecTrans;
        Vector2 bgBotDecAnchorMin;
        Vector2 bgBotDecAnchorMax;
    RectTransform bgTopBG2Trans; //top
        Vector2 bgTopBG2AnchorMin;
        Vector2 bgTopBG2AnchorMax;
    RectTransform bgTopBG1Trans;
        Vector2 bgTopBG1AnchorMin;
        Vector2 bgTopBG1AnchorMax;
    RectTransform bgTopDec2Trans;
        Vector2 bgTopDec2AnchorMin;
        Vector2 bgTopDec2AnchorMax;
    RectTransform bgTopDec1Trans;
        Vector2 bgTopDec1AnchorMin;
        Vector2 bgTopDec1AnchorMax;*/

    public GameObject chaosScene;
    public GameObject background;
    public Image skyColorBG;
    public GameObject screenButton;
    public GameObject audioHolder;

    public GameObject slotMenuPrefab;
    GameObject slotMenu;

    bool fadeSky = false;
    float fadeTime = 0;

    CanvasGroup group;
    bool ending = false;
    float opacity = 1;
    float fadeRate = 0.05f;

    bool showTip = true;

    public AudioClip tapToContClip;


    // Use this for initialization
    void Start () {
        /*Util.gm.SpawnBlackBG(); 

        playButtonLocation = playButton.GetComponent<RectTransform>().localPosition;
        planet1Location = planet1.GetComponent<RectTransform>().localPosition;
        playButtonGTT = playButton.GetComponent<GoToTarget>();
        planet1GTT = planet1.GetComponent<GoToTarget>();

        tipButtonTrans = tipButton.GetComponent<RectTransform>();
        tipButtonLocation = tipButtonTrans.localPosition;

        bgMidBGTrans = bgMidBG.GetComponent<RectTransform>();
        bgMidBGLocation = bgMidBGTrans.localPosition;
        bgMidDecTrans = bgMidDec.GetComponent<RectTransform>();
        bgMidDecLocation = bgMidDecTrans.localPosition;

        bgBotBG2Trans = bgBotBG2.GetComponent<RectTransform>(); //bot
        bgBotBG2Location = bgBotBG2Trans.localPosition;
        bgBotBG2AnchorMin = bgBotBG2Trans.anchorMin;
        bgBotBG2AnchorMax = bgBotBG2Trans.anchorMax;

        bgBotBG1Trans = bgBotBG1.GetComponent<RectTransform>();
        bgBotBG1Location = bgBotBG1Trans.localPosition;
        bgBotBG1AnchorMin = bgBotBG1Trans.anchorMin;
        bgBotBG1AnchorMax = bgBotBG1Trans.anchorMax;

        bgBotDecTrans = bgBotDec.GetComponent<RectTransform>();
        bgBotDecLocation = bgBotDecTrans.localPosition;
        bgBotDecAnchorMin = bgBotDecTrans.anchorMin;
        bgBotDecAnchorMax = bgBotDecTrans.anchorMax;

        bgTopBG2Trans = bgTopBG2.GetComponent<RectTransform>(); //top
        bgTopBG2Location = bgTopBG2Trans.localPosition;
        bgTopBG2AnchorMin = bgTopBG2Trans.anchorMin;
        bgTopBG2AnchorMax = bgTopBG2Trans.anchorMax;

        bgTopBG1Trans = bgTopBG1.GetComponent<RectTransform>();
        bgTopBG1Location = bgTopBG1Trans.localPosition;
        bgTopBG1AnchorMin = bgTopBG1Trans.anchorMin;
        bgTopBG1AnchorMax = bgTopBG1Trans.anchorMax;

        bgTopDec2Trans = bgTopDec2.GetComponent<RectTransform>();
        bgTopDec2Location = bgTopDec2Trans.localPosition;
        bgTopDec2AnchorMin = bgTopDec2Trans.anchorMin;
        bgTopDec2AnchorMax = bgTopDec2Trans.anchorMax;

        bgTopDec1Trans = bgTopDec1.GetComponent<RectTransform>();
        bgTopDec1Location = bgTopDec1Trans.localPosition;
        bgTopDec1AnchorMin = bgTopDec1Trans.anchorMin;
        bgTopDec1AnchorMax = bgTopDec1Trans.anchorMax;*/


        group = GetComponent<CanvasGroup>();
    }
	
	// Update is called once per frame
	void Update () {
        if (fadeSky) {
            skyColorBG.color = Color.Lerp(Color.blue, new Color(0, 40  / 255f, 150 / 255f), (Time.time-fadeTime) / 1f);
        }
        if (!fadeSky) {
            //skyColorBG.color = Color.Lerp(new Color(0, 30, 100), new Color(0, 61, 204), fadeTime);
            skyColorBG.color = Color.Lerp(new Color(0, 40 / 255f, 150 / 255f), Color.blue, (Time.time-fadeTime) / 1f);
        }

        if (ending) {
            opacity -= fadeRate;
            group.alpha = opacity;
        }
        /*if(showTip)
            Float(tipButtonTrans, tipButtonLocation, 5f, 2.8f);
        else
            Destroy(tipButton.gameObject);

        //BACKGROUND MOVING
        Float(bgMidBGTrans, bgMidBGLocation, 15f, 0.5f);
        Float(bgMidDecTrans, bgMidDecLocation, 10f, 0.8f);

        ScaleFloat(bgBotBG2Trans, bgBotBG2AnchorMin, bgBotBG2AnchorMax, 4, 0.02f, 0.9f);
        ScaleFloat(bgBotBG1Trans, bgBotBG1AnchorMin, bgBotBG1AnchorMax, 4, 0.05f, 0.7f);
        ScaleFloat(bgBotDecTrans, bgBotDecAnchorMin, bgBotDecAnchorMax, 3, 0.05f, 0.7f);

        ScaleFloat(bgTopBG2Trans, bgTopBG2AnchorMin, bgTopBG2AnchorMax, 2, 0.02f, 0.9f);
        ScaleFloat(bgTopBG1Trans, bgTopBG1AnchorMin, bgTopBG1AnchorMax, 2, 0.05f, 0.7f);
        ScaleFloat(bgTopDec2Trans, bgTopDec2AnchorMin, bgTopDec2AnchorMax, 2, 0.02f, 0.6f);
        ScaleFloat(bgTopDec1Trans, bgTopDec1AnchorMin, bgTopDec1AnchorMax, 1, 0.03f, 0.6f);*/
    }






    //Button stuff

    public void PlayButtonPress() {
        fadeSky = true;
        fadeTime = Time.time;
        screenButton.SetActive(false);
        chaosScene.GetComponent<GoToTarget>().ChangeTarget(new Vector3(0, -850, 0));
        background.GetComponent<GoToTarget>().ChangeTarget(new Vector3(0, 0, 0));
        audioHolder.GetComponent<GoToTarget>().ChangeTarget(new Vector3(960f, -80f, 0));
        slotMenu = Instantiate(slotMenuPrefab, Vector3.zero, Quaternion.identity, transform);
        /*playButtonGTT.targetPosition = new Vector3(Util.xScale*1800, Util.yScale * -300, 0);
        planet1GTT.targetPosition = new Vector3(Util.xScale * 50, Util.yScale * 100, 0);
        playButton.GetComponent<Button>().enabled = false;*/

        SoundManager.PlaySfx(tapToContClip);

    }   

    public void BackButtonPress() {
        fadeSky = false;
        fadeTime = Time.time;
        screenButton.SetActive(true);
        chaosScene.GetComponent<GoToTarget>().ChangeTarget(new Vector3(0, 0, 0));
        background.GetComponent<GoToTarget>().ChangeTarget(new Vector3(0, 300, 0));
        audioHolder.GetComponent<GoToTarget>().ChangeTarget(new Vector3(960f, 0, 0));
        /*playButton.GetComponent<Button>().enabled = true;
        playButtonGTT.targetPosition = playButtonLocation;
        planet1GTT.targetPosition = planet1Location;*/

        SoundManager.PlaySfx(tapToContClip);
    }

    public void Exit() {
        ending = true;
        
        Invoke("Die", 0.5f);
    }

    void Die() {
        Util.gm.DestroyBlackBG();
        Destroy(gameObject);
    }

    /*(void Float(RectTransform trans, Vector3 location, float amp, float rate) {
        float ph = Mathf.Sin(Time.realtimeSinceStartup * rate) * amp;
        trans.localPosition = location + new Vector3(0, ph, 0);
    }

    void ScaleFloat(RectTransform trans, Vector2 TAnchorMin, Vector2 TAnchorMax, int corner, float amp, float rate) {
        float scale = amp*Mathf.Sin(Time.realtimeSinceStartup * rate);
        switch (corner) {
            case 1:
                trans.anchorMin = new Vector2(scale + TAnchorMin.x, scale + TAnchorMin.y);
                break;
            case 2:
                trans.anchorMax = new Vector2(scale + TAnchorMax.x, TAnchorMax.y);
                trans.anchorMin = new Vector2(TAnchorMin.x, -scale + TAnchorMin.y);
                break;
            case 3:
                trans.anchorMax = new Vector2(scale + TAnchorMax.x, scale + TAnchorMax.y);
                break;
            case 4:
                trans.anchorMax = new Vector2(TAnchorMax.x, scale + TAnchorMax.y);
                trans.anchorMin = new Vector2(-scale + TAnchorMin.x, TAnchorMin.y);
                break;
        }
    }*/

    public void tipButtonPress() {
        showTip = false;
        Destroy(tipButton.gameObject);
        //GameManager.sm.save();
    }
}
