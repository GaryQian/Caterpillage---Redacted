using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour {

    public Button soundButton;
    public Button musicButton;
    public Button handButton;
    public Button homeButton;
    public Button closeButton;

    public Sprite soundOnSprite;
    public Sprite soundOffSprite;
    public Sprite musicOnSprite;
    public Sprite musicOffSprite;
    public Sprite rightHandSprite;
    public Sprite leftHandSprite;

    Image myImageComponent;

	// Use this for initialization
	void Start () {
        if (GameManager.settings.soundOn) soundButton.image.sprite = soundOnSprite;
        else soundButton.image.sprite = soundOffSprite;
        if (GameManager.settings.musicOn) musicButton.image.sprite = musicOnSprite;
        else musicButton.image.sprite = musicOffSprite;
        if (GameManager.settings.leftHandedMode && handButton != null) handButton.image.sprite = leftHandSprite;
        else if (handButton != null) handButton.image.sprite = rightHandSprite;
    }

    public void DoneButtonPress() {
        GameManager.upgradeManager.optionsPressed = false;
        Destroy(gameObject);
    }

    public void SoundButtonPress() {
        if (GameManager.settings.soundOn) {
            soundButton.image.sprite = soundOffSprite;
            GameManager.settings.soundOn = false;
        }
        else {
            soundButton.image.sprite = soundOnSprite;
            GameManager.settings.soundOn = true;
        }
        GameManager.sm.SaveSettings();
    }

    public void MusicButtonPress() {
        if (GameManager.settings.musicOn) {
            musicButton.image.sprite = musicOffSprite;
            GameManager.settings.musicOn = false;
        }
        else {
            musicButton.image.sprite = musicOnSprite;
            GameManager.settings.musicOn = true;
        }
        GameManager.sm.SaveSettings();
        SoundManager.instance.UpdateMusicOnOff();
    }

    public void HandButtonPress() {
        if (GameManager.settings.leftHandedMode) {
            handButton.image.sprite = rightHandSprite;
            GameManager.settings.leftHandedMode = false;
            Util.gm.buttonHolder.GetComponent<ButtonManager>().MakeRightHandedMode();
        }
        else {
            handButton.image.sprite = leftHandSprite;
            GameManager.settings.leftHandedMode = true;
            Util.gm.buttonHolder.GetComponent<ButtonManager>().MakeLeftHandedMode();
        }
        GameManager.sm.SaveSettings();
    }

    public void HomeButtonPress() {
        Util.gm.LoadTitlePage();
        GameManager.upgradeManager.GetComponent<GoToTarget>().speed = 10f;
        Destroy(Util.gm.upgradeMenu);

        SoundManager.instance.ChangeMusic(MusicType.title);
    }
}
