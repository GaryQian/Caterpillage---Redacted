using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RateUs : MonoBehaviour {

    public Text message;

    public GameObject star1;
    public GameObject star2;
    public GameObject star3;
    public GameObject star4;
    public GameObject star5;

    public Sprite filledStarSprite;

    // Use this for initialization
    void Start () {
		if (GameManager.settings.rated) {
            Destroy(gameObject);
            return;
        }
	}

    public void Star1Press() {
        message.text = "Thanks!";
        star1.GetComponent<Image>().sprite = filledStarSprite;
        Invoke("Exit", 0.8f);
    }

    public void Star2Press() {
        message.text = "Thanks!";
        star1.GetComponent<Image>().sprite = filledStarSprite;
        star2.GetComponent<Image>().sprite = filledStarSprite;
        Invoke("Exit", 0.8f);
    }

    public void Star3Press() {
        message.text = "Thanks!";
        star1.GetComponent<Image>().sprite = filledStarSprite;
        star2.GetComponent<Image>().sprite = filledStarSprite;
        star3.GetComponent<Image>().sprite = filledStarSprite;
        Invoke("Exit", 0.8f);
    }

    public void Star4Press() {
        message.text = "Thanks!";
        star1.GetComponent<Image>().sprite = filledStarSprite;
        star2.GetComponent<Image>().sprite = filledStarSprite;
        star3.GetComponent<Image>().sprite = filledStarSprite;
        star4.GetComponent<Image>().sprite = filledStarSprite;
        Application.OpenURL("http://google.com/");
        GoToSite();
        Invoke("Exit", 0.8f);
    }

    public void Star5Press() {
        message.text = "Thanks!";
        star1.GetComponent<Image>().sprite = filledStarSprite;
        star2.GetComponent<Image>().sprite = filledStarSprite;
        star3.GetComponent<Image>().sprite = filledStarSprite;
        star4.GetComponent<Image>().sprite = filledStarSprite;
        star5.GetComponent<Image>().sprite = filledStarSprite;
        GoToSite();
        Invoke("Exit", 0.8f);
    }

    void GoToSite() {
        GameManager.settings.rated = true;
        GameManager.sm.SaveSettings();
#if UNITY_ANDROID
        Application.OpenURL("https://play.google.com/store/apps/details?id=ninja.qian.caterpillage");
#endif
#if UNITY_IOS
        Application.OpenURL("https://itunes.apple.com/app/id1338595162");
#endif
    }

    public void Exit() {
        Invoke("Die", 0.8f);
        GetComponent<GoToTarget>().ChangeTarget(new Vector3(0, -1000f, 0));
    }

    void Die() {
        Destroy(gameObject);
    }

}
