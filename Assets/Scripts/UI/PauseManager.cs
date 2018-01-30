using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour {

    public GameObject gray;
    public Button resumeButton;
    public Button optionsButton;
    public Button exitButton;
    public GameObject optionsPrefab;
    public GameObject mainButtons;

    GameObject menu;
    bool optionsPressed;
    float scale;
    GameObject optionsMenu = null;

    // Use this for initialization
    void Start() {
        optionsPressed = false;
        scale = Screen.width / 1920f;
        SoundManager.EndLaser();
    }

    // Update is called once per frame
    void Update() {
        if (optionsMenu == null) return;
        if (optionsPressed && optionsMenu.GetComponent<CanvasGroup>().alpha < 1) {
            optionsMenu.GetComponent<CanvasGroup>().alpha += 0.08f;
        }
        else if (!optionsPressed && optionsMenu.GetComponent<CanvasGroup>().alpha > 0) {
            optionsMenu.GetComponent<CanvasGroup>().alpha -= 0.08f;
        }
        else if (!optionsPressed && optionsMenu.GetComponent<CanvasGroup>().alpha <= 0) {
            Destroy(optionsMenu);
        }
    }

    public void OnResumeButtonPress() {
        WorldManager.buttonManager.pauseButtonEnabled = true;
        WorldManager.togglePause();
        Destroy(gameObject);


    }

    public void OnOptionsButtonPress() {
        SmoothMotion sm = mainButtons.GetComponent<SmoothMotion>();
        if (optionsPressed) {
            optionsPressed = false;
            //unsetup
            sm.startPos = new Vector3(0, 250*scale, 0);
            sm.endPos = new Vector3(0, 0, 0);
            sm.begin();
        }
        else {
            optionsPressed = true;
            //setup
            sm.startPos = new Vector3(0, 0, 0);
            sm.endPos = new Vector3(0, 250*scale, 0);
            sm.begin();

            //spawn options
            optionsMenu = Instantiate(optionsPrefab, Vector3.zero, Quaternion.identity, GetComponent<RectTransform>());
            RectTransform oRT = optionsMenu.GetComponent<RectTransform>();
            oRT.SetAsLastSibling();
            oRT.localPosition = new Vector3(0, -150 * Util.yScale, 0);
            optionsMenu.GetComponent<CanvasGroup>().alpha = 0;
        }
    }

    public void OnExitButtonPress() {
        OnResumeButtonPress();
        WorldManager.wormHealth.Die(true);
    }
}