using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialOverlay : MonoBehaviour {
    public Vector2 targetPosition;
    public string instructionsStr;
    public Button button;
    public UnityEngine.Events.UnityAction endCall;
    public List<Button> badButtons;
    public float scale = 0.5f;
    public bool pause = true;
    public float life = 10f;

    public Text instructionText;
    public GameObject holder;
    public GameObject instructionTextObj;

    float startTime;

	// Use this for initialization
	void Start () {
        startTime = Time.realtimeSinceStartup;
        if (button != null) {
            targetPosition = button.transform.position;
        }
        else {
            Destroy(gameObject);
            GameManager.slots.slots[GameManager.slotId].progress.tutorialState++;
            return;
        }

        if (badButtons != null) {
            foreach (var b in badButtons) {
                if (b != null) b.enabled = false;
            }
        }

        transform.localPosition = new Vector3(targetPosition.x, targetPosition.y);
        instructionText.text = instructionsStr;
        holder.transform.localScale = Vector3.one * scale;
        button.onClick.AddListener(CloseTutorial);
        if (targetPosition.x > Screen.width / 2f) {
            instructionTextObj.transform.localPosition = new Vector3(-instructionTextObj.transform.localPosition.x, instructionTextObj.transform.localPosition.y);
        }

        if (pause) WorldManager.ForcePause(true);

        Invoke("CloseTutorial", 8f);
	}

    void Update() {
        if (button == null) {
            CloseTutorial();
        }
        if (Time.realtimeSinceStartup - startTime > life) {
            CloseTutorial();
        }
    }

    void CloseTutorial() {
        if (pause) WorldManager.ForcePause(false);
        CancelInvoke("CloseTutorial");
        button.onClick.RemoveListener(CloseTutorial);
        if (badButtons != null) {
            foreach (var b in badButtons) {
                if (b != null) b.enabled = true;
            }
        }
        GameManager.slots.slots[GameManager.slotId].progress.tutorialState++;
        if (endCall != null) {
            endCall();
        }
        Destroy(gameObject);
    }

    
	
}
