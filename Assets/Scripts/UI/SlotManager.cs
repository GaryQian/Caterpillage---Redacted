using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotManager : MonoBehaviour {

    public GameObject slot1;
    public GameObject slot2;
    public GameObject slot3;
    public GameObject back;

    GoToTarget slot1GTT;
    GoToTarget slot2GTT;
    GoToTarget slot3GTT;
    GoToTarget backGTT;

    public AudioClip selectSlotClip;

    void Start() {
        slot1GTT = slot1.GetComponent<GoToTarget>();
        slot2GTT = slot2.GetComponent<GoToTarget>();
        slot3GTT = slot3.GetComponent<GoToTarget>();
        backGTT = back.GetComponent<GoToTarget>();
    }

    // Update is called once per frame
    void Update () {
        //transform.SetAsLastSibling();
    }

    public void Slot1Pressed() {
        bool wasEmpty = GameManager.slots.slots[1].isEmpty;
        GameManager.SelectSlot(1);
        Util.gm.OnPlayButtonPressed(wasEmpty);

        SoundManager.PlaySfx(selectSlotClip);
    }

    public void Slot2Pressed() {
        bool wasEmpty = GameManager.slots.slots[2].isEmpty;
        GameManager.SelectSlot(2);
        Util.gm.OnPlayButtonPressed(wasEmpty);

        SoundManager.PlaySfx(selectSlotClip);
    }

    public void Slot3Pressed() {
        bool wasEmpty = GameManager.slots.slots[3].isEmpty;
        GameManager.SelectSlot(3);
        Util.gm.OnPlayButtonPressed(wasEmpty);

        SoundManager.PlaySfx(selectSlotClip);
    }

    public void BackButtonPressed() {
        slot1GTT.targetPosition = new Vector3(Util.xScale * -600, Util.yScale * 1300, 0);
        slot2GTT.targetPosition = new Vector3(Util.xScale * -200, Util.yScale * 1300, 0);
        slot3GTT.targetPosition = new Vector3(Util.xScale * 200, Util.yScale * 1300, 0);
        backGTT.targetPosition = new Vector3(Util.xScale * 500, Util.yScale * 1300, 0);
        Invoke("Die", 0.5f);
        Util.gm.GetComponent<GameManager>().OnSlotBackButtonPressed();
    }

    void Die() {
        Destroy(gameObject);
    }
}
