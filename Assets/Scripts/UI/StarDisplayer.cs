using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarDisplayer : MonoBehaviour {

    public Image star1;
    public Image star2;
    public Image star3;
    public Text twoTime;
    public Text threeTime;
    public GameObject starDropPref;

    public RectTransform starholder;

    public float startDelay;
    public float gapDelay;
    public int total = 3;
    public int newStars = 0;

    public int level;
    public bool ateGem;

    public AudioClip star1Clip;
    public AudioClip star2Clip;
    public AudioClip star3Clip;

    // Use this for initialization
    void Start () {
        twoTime.text = "";
        threeTime.text = "";
        switch(total) {
            case 1:
                Invoke("DropStar1", startDelay);
                twoTime.text = Util.TimeToText(StarCalculator.GetTwoThresh(level, ateGem));
                threeTime.text = Util.TimeToText(StarCalculator.GetThreeThresh(level, ateGem));
                break;
            case 2:
                Invoke("DropStar1", startDelay);
                Invoke("DropStar2", startDelay + gapDelay);
                threeTime.text = Util.TimeToText(StarCalculator.GetThreeThresh(level, ateGem));
                break;
            case 3:
                Invoke("DropStar1", startDelay);
                Invoke("DropStar2", startDelay + gapDelay);
                Invoke("DropStar3", startDelay + gapDelay * 2);
                break;
        }
	}

    void DropStar1() {
        GameObject starDrop = Instantiate(starDropPref, Vector3.zero, Quaternion.identity, starholder);
        starDrop.transform.localPosition = star1.transform.localPosition;
        SoundManager.PlaySingleSfx(star1Clip, .75f, 1);
    }

    void DropStar2() {
        GameObject starDrop = Instantiate(starDropPref, Vector3.zero, Quaternion.identity, starholder);
        starDrop.transform.localPosition = star2.transform.localPosition;
        SoundManager.PlaySingleSfx(star2Clip, .75f, 1);
    }

    void DropStar3() {
        GameObject starDrop = Instantiate(starDropPref, Vector3.zero, Quaternion.identity, starholder);
        starDrop.transform.localPosition = star3.transform.localPosition;
        SoundManager.PlaySingleSfx(star3Clip, .75f, 1);
    }
}
