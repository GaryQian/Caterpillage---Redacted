using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum cutsceneState {BEGIN, DROOL, END};

public class CutsceneManager : MonoBehaviour {

    public GameObject droolPrefab;
    public GameObject caterpillar;
    public GameObject planet;
    public GameObject bg;
    public Text text;
    Image caterpillarImage;
    float minScale = 0.5f;
    float scaleRate = 0.55f;
    cutsceneState state;
    float colorScale = 1f;
    float colorRate = 1f;

    public float fadeDuration;
    public float startTime;

    public CanvasGroup cg;
	// Use this for initialization
	void Start () {
        state = cutsceneState.BEGIN;
        text.text = "So hungry...";
        caterpillarImage = caterpillar.GetComponent<Image>();
        Invoke("BeginDrooling", 4f);
        StartCoroutine(FadeIn());
    }
	
	// Update is called once per frame
	void Update () {
		if (caterpillar.transform.localScale.x > minScale && state == cutsceneState.DROOL) {
            caterpillar.transform.localScale = new Vector3(caterpillar.transform.localScale.x - scaleRate * Time.deltaTime, caterpillar.transform.localScale.y - scaleRate * Time.deltaTime, 0);
            if (scaleRate > 0.7f*Time.deltaTime) scaleRate -= 0.7f*Time.deltaTime; //deceleration
        }
        
        if (colorScale > 0 && state == cutsceneState.DROOL) {
            caterpillarImage.color = Color.Lerp(Color.white, new Color(0.78f, 0.88f, 0.8f), colorScale);
            colorScale -= Time.deltaTime*colorRate;
        }
    }

    public void BeginDrooling() {
        if (state == cutsceneState.BEGIN) {
            state = cutsceneState.DROOL;
            Instantiate(droolPrefab, caterpillar.transform);
            planet.GetComponent<GoToTarget>().ChangeTarget(new Vector3(800, -900, 0));
            caterpillar.GetComponent<GoToTarget>().ChangeTarget(new Vector3(-450, 100, 0));
            bg.GetComponent<GoToTarget>().ChangeTarget(new Vector3(-200, 50, 0));
            text.text = "Ooo food...";
            Invoke("EndCutscene", 5f);
        }
    }

    public void NextStatePressed() {
        Debug.Log("Skipping!");
        switch (state) {
            case cutsceneState.BEGIN:
                BeginDrooling();
                break;
            case cutsceneState.DROOL:
                EndCutscene();
                break;

        }
    }

    IEnumerator FadeOut() {
        GameManager.Instance.StartGame("RANDOM");
        startTime = Time.unscaledTime;
        while (Time.unscaledTime - startTime < fadeDuration) {
            cg.alpha = 1f - (Time.unscaledTime - startTime) / fadeDuration;
            yield return null;
        }
        Destroy(gameObject);
    }

    IEnumerator FadeIn() {
        startTime = Time.unscaledTime;
        cg.alpha = 0;
        while (Time.unscaledTime - startTime < fadeDuration) {
            cg.alpha = (Time.unscaledTime - startTime) / fadeDuration;
            yield return null;
        }
    }

    public void EndCutscene() {
        StartCoroutine(FadeOut());
    }
}
