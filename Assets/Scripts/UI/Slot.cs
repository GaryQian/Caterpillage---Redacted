using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Slot : MonoBehaviour {

    public int slot;

    public Text title;
    public Text newGame;
    public Text planetNumber;
    public GameObject starDisplay;
    public Text starAmount;
    public GameObject resetButton;

    public GameObject DeleteConfirmationPrefab;

    public bool isNewGame;

    // Use this for initialization
    void Start () {
        SetSlot();
	}

    public void SetSlot() {
        title.gameObject.SetActive(true);
        switch (slot) {
            case 1:
                title.text = "SLOT 1";
                break;
            case 2:
                title.text = "SLOT 2";
                break;
            case 3:
                title.text = "SLOT 3";
                break;
        }
        if (GameManager.slots.slots[slot] == null) GameManager.slots.slots[slot] = new SlotData(slot);
        if (GameManager.slots.slots[slot].isEmpty) {
            newGame.gameObject.SetActive(true);
            resetButton.gameObject.SetActive(false);
            planetNumber.gameObject.SetActive(false);
            starDisplay.SetActive(false);
        }
        else {
            newGame.gameObject.SetActive(false);
            resetButton.gameObject.SetActive(true);
            planetNumber.gameObject.SetActive(true);
            starDisplay.SetActive(true);
            planetNumber.text = "" + GameManager.slots.slots[slot].progress.maxLevel;
            starAmount.text = "" + GameManager.slots.slots[slot].progress.TotalStars();
        }
    }

    public void DeleteSlotPressed() {
        GameObject conf = Instantiate(DeleteConfirmationPrefab, Util.canvas.transform);
        conf.GetComponent<DeleteConfirmation>().slot = this;
    }

    public void DeleteSlot() {
        GameManager.slots.slots[slot] = new SlotData(slot);

        SetSlot();

        GameManager.sm.SaveSlot(slot);
    }
	
}
