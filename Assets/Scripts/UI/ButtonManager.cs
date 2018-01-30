using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour {

    public Button pauseButton;
    public GameObject pauseMenu;

    GameObject menu;
    float scale;

    public Button inventoryButton;
    public Sprite inventoryBagFull;
    public Sprite inventoryBagEmpty;
    public Sprite inventoryBagOpened;
    public GameObject inventoryBG;
    public Image inventoryGlow;
    InventoryBGStretcher inventoryStretcher;
    public bool inventoryExpanded;
    int count;
    float destroyDelay = .11f;
    float spacing = 145f;
    

    public GameObject healthPotionButtonPrefab;
    public GameObject speedPotionButtonPrefab;
    public GameObject berserkPotionButtonPrefab;
    GameObject healthPotionButton;
    GameObject speedPotionButton;
    GameObject berserkPotionButton;

    public GameObject WavePrefab;

    public AudioClip healClip;
    public AudioClip speedClip;
    public AudioClip berserkClip;
    public AudioClip gulpClip;
    public AudioClip bagOpenClip;

    Image healthPotionImage;
    Image speedPotionImage;
    Image berserkPotionImage;


    Text healthPotionCount;
    Text speedPotionCount;
    Text berserkPotionCount;

    public bool pauseButtonEnabled = true;

    // Use this for initialization
    void Start () {
        inventoryExpanded = false;
        InvokeRepeating("UpdateConsumableReady", 0f, 0.25f);
        inventoryStretcher = inventoryBG.GetComponent<InventoryBGStretcher>();


        StartCoroutine(PulseInventoryGlow());
    }

    public void OnPauseButtonPress() {
        if (!pauseButtonEnabled) return;
        WorldManager.togglePause();
        if (WorldManager.paused) {
            menu = Instantiate(pauseMenu, Vector3.zero, Quaternion.identity, Util.gm.canvas.GetComponent<RectTransform>());
            menu.GetComponent<RectTransform>().SetAsLastSibling();
            //menu.GetComponent<RectTransform>().SetParent(GameObject.Find("Canvas").transform);
        }
        else {
            Destroy(menu);
        }
    }

    public void MakeLeftHandedMode() {
        inventoryButton.GetComponent<RectTransform>().anchorMin = new Vector2(0.91f, 0.7f);
        inventoryButton.GetComponent<RectTransform>().anchorMax = new Vector2(0.99f, 0.83f);

        inventoryGlow.GetComponent<RectTransform>().anchorMin = new Vector2(0.90f, 0.68f);
        inventoryGlow.GetComponent<RectTransform>().anchorMax = new Vector2(1f, 0.85f);

        inventoryBG.GetComponent<RectTransform>().anchorMin = new Vector2(0.935f, 0.73f);
        inventoryBG.GetComponent<RectTransform>().anchorMax = new Vector2(0.965f, 0.78f);
        inventoryStretcher.ToggleTarget(count);
    }

    public void MakeRightHandedMode() {
        inventoryButton.GetComponent<RectTransform>().anchorMin = new Vector2(0.01f, 0.7f);
        inventoryButton.GetComponent<RectTransform>().anchorMax = new Vector2(0.09f, 0.83f);

        inventoryGlow.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 0.68f);
        inventoryGlow.GetComponent<RectTransform>().anchorMax = new Vector2(0.1f, 0.85f);

        inventoryBG.GetComponent<RectTransform>().anchorMin = new Vector2(0.035f, 0.73f);
        inventoryBG.GetComponent<RectTransform>().anchorMax = new Vector2(0.065f, 0.78f);
        inventoryStretcher.ToggleTarget(count);
    }

    //CONSUMABLES ########################################################
    public void ToggleInventory() {
        SoundManager.PlaySfx(1.5f, bagOpenClip);
        if (inventoryExpanded) {
            inventoryExpanded = false;
            if (IsInventoryFull()) {
                inventoryButton.GetComponent<Image>().sprite = inventoryBagFull;
            }
            else {
                inventoryButton.GetComponent<Image>().sprite = inventoryBagEmpty;
            }
            if (healthPotionButton != null) {
                healthPotionButton.GetComponent<GoToTarget>().ChangeTarget(new Vector3(0, 0, 0));
                Invoke("DestroyHealthPotion", destroyDelay);
            }
            if (speedPotionButton != null) {
                speedPotionButton.GetComponent<GoToTarget>().ChangeTarget(new Vector3(0, 0, 0));
                Invoke("DestroySpeedPotion", destroyDelay);
            }
            if (berserkPotionButton != null) {
                berserkPotionButton.GetComponent<GoToTarget>().ChangeTarget(new Vector3(0, 0, 0));
                Invoke("DestroyBerserkPotion", destroyDelay);
            }
            inventoryStretcher.ToggleTarget(0);

        }
        else {
            inventoryExpanded = true;
            inventoryButton.GetComponent<Image>().sprite = inventoryBagOpened;
            UpdateConsumableReady();
            count = 0;
            if (GameManager.slots.slots[GameManager.slotId].progress.consumableCount[Progress.healthPotionId] > 0) {
                count++;
                healthPotionButton = Instantiate(healthPotionButtonPrefab, inventoryButton.transform);
                healthPotionButton.GetComponent<GoToTarget>().ChangeTarget(new Vector3(0, -spacing * count, 0), false);
                healthPotionCount = healthPotionButton.transform.GetChild(0).GetComponent<Text>();  
                healthPotionImage = healthPotionButton.GetComponent<Image>();
                healthPotionButton.name = "HealthPotionButton";
            }
            if (GameManager.slots.slots[GameManager.slotId].progress.consumableCount[Progress.speedPotionId] > 0) {
                count++;
                speedPotionButton = Instantiate(speedPotionButtonPrefab, inventoryButton.transform);
                speedPotionButton.GetComponent<GoToTarget>().ChangeTarget(new Vector3(0, -spacing * count, 0), false);
                speedPotionCount = speedPotionButton.transform.GetChild(0).GetComponent<Text>();
                speedPotionImage = speedPotionButton.GetComponent<Image>();
                speedPotionButton.name = "SpeedPotionButton";
            }
            if (GameManager.slots.slots[GameManager.slotId].progress.consumableCount[Progress.berserkPotionId] > 0) {
                count++;
                berserkPotionButton = Instantiate(berserkPotionButtonPrefab, inventoryButton.transform);
                berserkPotionButton.GetComponent<GoToTarget>().ChangeTarget(new Vector3(0, -spacing * count, 0), false);
                berserkPotionCount = berserkPotionButton.transform.GetChild(0).GetComponent<Text>();
                berserkPotionImage = berserkPotionButton.GetComponent<Image>();
                berserkPotionButton.name = "BerserkPotionButton";
            }
            UpdateConsumableReady();
            UpdateConsumableCount();
            inventoryStretcher.ToggleTarget(count);

            //inventoryGlow.color = new Color(inventoryGlow.color.r, inventoryGlow.color.g, inventoryGlow.color.b, 0);
        }
    }

    IEnumerator PulseInventoryGlow() {
        while (true) {
            if (!inventoryExpanded) inventoryGlow.color = new Color(inventoryGlow.color.r, inventoryGlow.color.g, inventoryGlow.color.b, 1f - (Mathf.Sin(Time.time * 6) + 1) / 2f);
            yield return null;
        }
    }

    public void UpdateClosedInventory() {
        if (inventoryExpanded) return;
        if (IsInventoryFull()) {
            inventoryButton.GetComponent<Image>().sprite = inventoryBagFull;
        }
        else {
            inventoryButton.GetComponent<Image>().sprite = inventoryBagEmpty;
        }
    }

    public void UpdateExpandedInventory() {
        if (inventoryExpanded) {
            if (healthPotionButton != null) {
                DestroyHealthPotion();
            }
            if (speedPotionButton != null) {
                DestroySpeedPotion();
            }
            if (berserkPotionButton != null) {
                DestroyBerserkPotion();
            }
            count = 0;
            if (GameManager.slots.slots[GameManager.slotId].progress.consumableCount[Progress.healthPotionId] > 0) {
                count++;
                healthPotionButton = Instantiate(healthPotionButtonPrefab, inventoryButton.transform);
                healthPotionButton.GetComponent<GoToTarget>().ChangeTarget(new Vector3(0, -spacing * count, 0), false);
                healthPotionCount = healthPotionButton.transform.GetChild(0).GetComponent<Text>();
                healthPotionImage = healthPotionButton.GetComponent<Image>();
            }
            if (GameManager.slots.slots[GameManager.slotId].progress.consumableCount[Progress.speedPotionId] > 0) {
                count++;
                speedPotionButton = Instantiate(speedPotionButtonPrefab, inventoryButton.transform);
                speedPotionButton.GetComponent<GoToTarget>().ChangeTarget(new Vector3(0, -spacing * count, 0), false);
                speedPotionCount = speedPotionButton.transform.GetChild(0).GetComponent<Text>();
                speedPotionImage = speedPotionButton.GetComponent<Image>();
            }
            if (GameManager.slots.slots[GameManager.slotId].progress.consumableCount[Progress.berserkPotionId] > 0) {
                count++;
                berserkPotionButton = Instantiate(berserkPotionButtonPrefab, inventoryButton.transform);
                berserkPotionButton.GetComponent<GoToTarget>().ChangeTarget(new Vector3(0, -spacing * count, 0), false);
                berserkPotionCount = berserkPotionButton.transform.GetChild(0).GetComponent<Text>();
                berserkPotionImage = berserkPotionButton.GetComponent<Image>();
            }
            UpdateConsumableReady();
            UpdateConsumableCount();
            inventoryStretcher.ToggleTarget(count);
        }
    }

    public void SpawnWave(Color c) {
        GameObject wv = Instantiate(WavePrefab, Util.canvas.transform);
        wv.GetComponent<WaveEffect>().c = c;
    }

    public void UseHealthPotion() {
        if (WorldManager.wormHealth.health/WorldManager.wormHealth.maxHealth >= 0.5f || WorldManager.wormMotion.wormState != WormState.playing) {
            return;
        }
        if (GameManager.slots.slots[GameManager.slotId].progress.consumableCount[Progress.healthPotionId] > 0) {
            GameManager.slots.slots[GameManager.slotId].progress.consumableCount[Progress.healthPotionId]--;
            WorldManager.consumableHandler.UseHealthPotion();
            SoundManager.PlaySfx(healClip);
            SoundManager.PlaySfx(1.5f, gulpClip);
        }
        if (GameManager.slots.slots[GameManager.slotId].progress.consumableCount[Progress.healthPotionId] == 0) {
            count--;
            inventoryStretcher.ToggleTarget(count);
            ShiftConsumables(1);
        }
        UpdateConsumableCount();
        SpawnWave(Color.green);
        //GameManager.sm.SaveCurrentSlot();
    }

    public void UseSpeedPotion() {
        if (WorldManager.consumableHandler.speedActive || WorldManager.wormMotion.wormState != WormState.playing) {
            return;
        }
        if (GameManager.slots.slots[GameManager.slotId].progress.consumableCount[Progress.speedPotionId] > 0) {
            GameManager.slots.slots[GameManager.slotId].progress.consumableCount[Progress.speedPotionId]--;
            WorldManager.consumableHandler.UseSpeedPotion();
            SoundManager.PlaySfx(speedClip);
            SoundManager.PlaySfx(1.5f, gulpClip);
        }
        if (GameManager.slots.slots[GameManager.slotId].progress.consumableCount[Progress.speedPotionId] == 0) {
            count--;
            inventoryStretcher.ToggleTarget(count);
            ShiftConsumables(2);
        }
        UpdateConsumableCount();
        SpawnWave(Color.blue);
        //GameManager.sm.SaveCurrentSlot();
    }

    public void UseBerserkPotion() {
        if (WorldManager.consumableHandler.berserkActive || WorldManager.wormMotion.wormState != WormState.playing) {
            return;
        }
        if (GameManager.slots.slots[GameManager.slotId].progress.consumableCount[Progress.berserkPotionId] > 0) {
            GameManager.slots.slots[GameManager.slotId].progress.consumableCount[Progress.berserkPotionId]--;
            WorldManager.consumableHandler.UseBerserkPotion();
            SoundManager.PlaySfx(berserkClip);
            SoundManager.PlaySfx(1.5f, gulpClip);
        }
        if (GameManager.slots.slots[GameManager.slotId].progress.consumableCount[Progress.berserkPotionId] == 0) {
            count--;
            inventoryStretcher.ToggleTarget(count);
            ShiftConsumables(3);
        }
        UpdateConsumableCount();
        SpawnWave(Color.yellow);
        //GameManager.sm.SaveCurrentSlot();
    }

    void UpdateConsumableCount() {
        if (healthPotionCount != null) {
            healthPotionCount.text = "x" + GameManager.slots.slots[GameManager.slotId].progress.consumableCount[Progress.healthPotionId];
        }
        if (speedPotionCount != null) {
            speedPotionCount.text = "x" + GameManager.slots.slots[GameManager.slotId].progress.consumableCount[Progress.speedPotionId];
        }
        if (berserkPotionCount != null) {
            berserkPotionCount.text = "x" + GameManager.slots.slots[GameManager.slotId].progress.consumableCount[Progress.berserkPotionId];
        }
    }

    bool IsInventoryFull() {
        if (GameManager.slots.slots[GameManager.slotId].progress.consumableCount[Progress.healthPotionId] > 0) {
            return true;
        }
        if (GameManager.slots.slots[GameManager.slotId].progress.consumableCount[Progress.speedPotionId] > 0) {
            return true;
        }
        if (GameManager.slots.slots[GameManager.slotId].progress.consumableCount[Progress.berserkPotionId] > 0) {
            return true;
        }
        return false;
    }

    void UpdateConsumableReady() {
        if (healthPotionButton != null) {
            if (WorldManager.wormHealth.health/WorldManager.wormHealth.maxHealth < 0.5f) {
                healthPotionImage.color = Color.white;
                healthPotionCount.color = Color.white;
            }
            else {
                healthPotionImage.color = Color.grey;
                healthPotionCount.color = Color.grey;
            }
        }
        if (speedPotionButton != null) {
            if (!WorldManager.consumableHandler.speedActive) {
                speedPotionImage.color = Color.white;
                speedPotionCount.color = Color.white;
            }
            else {
                speedPotionImage.color = Color.grey;
                speedPotionCount.color = Color.grey;
            }
        }
        if (berserkPotionButton != null) {
            if (!WorldManager.consumableHandler.berserkActive) {
                berserkPotionImage.color = Color.white;
                berserkPotionCount.color = Color.white;
            }
            else {
                berserkPotionImage.color = Color.grey;
                berserkPotionCount.color = Color.grey;
            }
        }
    }

    void ShiftConsumables(int whichOneZero) {
        switch (whichOneZero) {
            case 1: {
                    DestroyHealthPotion();
                    if (speedPotionButton != null) {
                        speedPotionButton.GetComponent<GoToTarget>().ChangeTarget(new Vector3(0, speedPotionButton.GetComponent<GoToTarget>().targetPosition.y/Util.yScale + spacing, 0));
                    }
                    if (berserkPotionButton != null) {
                        berserkPotionButton.GetComponent<GoToTarget>().ChangeTarget(new Vector3(0, berserkPotionButton.GetComponent<GoToTarget>().targetPosition.y / Util.yScale + spacing, 0));
                    }
                    break;
                }
            case 2: {
                    DestroySpeedPotion();
                    if (berserkPotionButton != null) {
                        berserkPotionButton.GetComponent<GoToTarget>().ChangeTarget(new Vector3(0, berserkPotionButton.GetComponent<GoToTarget>().targetPosition.y / Util.yScale + spacing, 0));
                    }
                    break;
                }
            case 3: {
                    DestroyBerserkPotion();
                    break;
                }
        }
    }

    void DestroyHealthPotion() {
        Destroy(healthPotionButton.gameObject);
    }
    void DestroySpeedPotion() {
        Destroy(speedPotionButton.gameObject);
    }
    void DestroyBerserkPotion() {
        Destroy(berserkPotionButton.gameObject);
    }
}
