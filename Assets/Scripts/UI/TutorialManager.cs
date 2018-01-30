using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour {

    public GameObject tutorialPrefab;
    public GameObject NotSpittingWarningPrefab;

    // Use this for initialization
    void Start () {
        
	}

    public void SpawnTutorialCheckers() {
        CancelInvoke("CheckGo");
        CancelInvoke("CheckSpitterUpgrade");
        CancelInvoke("CheckSpitterUpgrade");
        CancelInvoke("CheckHealth");

        if (GameManager.slots.slots[GameManager.slotId].progress.tutorialState == 0) {
            GameManager.slots.slots[GameManager.slotId].progress.tutorialState++;
        }

        switch (GameManager.slots.slots[GameManager.slotId].progress.tutorialState) {
            case 0: Invoke("CheckGo", 2f); break;
            case 1: Invoke("CheckSpitterUpgrade", 1f); break;
            case 2: Invoke("CheckSpitterUpgrade", 1f); break;
            case 3: Invoke("CheckHealth", 5f); break;
        }

    }

    void CheckGo() {
        if (GameManager.state != Gamestate.UpgradeMenu) return;
        if (GameManager.upgradeManager == null) return;
        if (GameManager.level >= 2) {
            GameManager.slots.slots[GameManager.slotId].progress.tutorialState++;
            SpawnTutorialCheckers();
            return;
        }
        if (GameManager.level == 1
            && !GemShopManager.IAPOpen
            && !GameManager.inGemShop) {
            GameObject tut = Instantiate(tutorialPrefab, Util.canvas.transform);
            tut.GetComponent<TutorialOverlay>().button = FindButtonWithName("PlayLevelButton");
            tut.GetComponent<TutorialOverlay>().instructionsStr = "Play!       ";
            tut.GetComponent<TutorialOverlay>().pause = false;
            return;
        }
        

        Invoke("CheckGo", 1f);
    }

    public void CheckSpittingWarning() {
        if (GameManager.state != Gamestate.Playing) return;
        if (GameManager.level > 20 || GameManager.level == 1) return;
        if (!GameManager.wormStats.hasSpit) return;
        if (WorldManager.wormMotion.wormState == WormState.playing 
            && WorldManager.wormMotion.planetTime > 10f 
            && WorldManager.stats.spitCount/WorldManager.wormMotion.planetTime < 0.3f) {
            Instantiate(NotSpittingWarningPrefab, Util.gm.canvas.transform);
            return;
        }
        Invoke("CheckSpittingWarning", 2f);
    }


    void CheckSpitterUpgrade() {
        if (GameManager.wormStats.hasSpit) return;
        if (GameManager.state == Gamestate.UpgradeMenu && GameManager.money >= CostManager.GetSpitterEvolveCost() && !GameManager.inGemShop && !GemShopManager.IAPOpen) {
            Invoke("SpawnSpitterSelectorTutorial", 0.75f);
            return;
        }
        Invoke("CheckSpitterUpgrade", 1f);

    }

    void SpawnSpitterSelectorTutorial() {
        if (GameManager.state != Gamestate.UpgradeMenu || GemShopManager.IAPOpen || GameManager.inGemShop) {
            CheckSpitterUpgrade();
            return;
        }
        GameObject tut = Instantiate(tutorialPrefab, Util.canvas.transform);
        tut.GetComponent<TutorialOverlay>().button = FindButtonWithName("SpitterButton");
        tut.GetComponent<TutorialOverlay>().instructionsStr = "Time to upgrade! Tap here to select spitter.";
        tut.GetComponent<TutorialOverlay>().endCall = OnSelectSpitter;
        tut.GetComponent<TutorialOverlay>().pause = false;
    }

    void SpawnBuySpitterTutorial() {
        if (GameManager.state != Gamestate.UpgradeMenu || GemShopManager.IAPOpen) {
            SpawnTutorialCheckers();
            return;
        }
        GameObject tut = Instantiate(tutorialPrefab, Util.canvas.transform);
        tut.GetComponent<TutorialOverlay>().button = FindButtonWithName("EvolveButton");
        tut.GetComponent<TutorialOverlay>().instructionsStr = "Tap Evolve to buy spitter! Tap in the air to spit.";
        tut.GetComponent<TutorialOverlay>().endCall = CheckHealth;
        tut.GetComponent<TutorialOverlay>().scale = 1.3f;
        tut.GetComponent<TutorialOverlay>().pause = false;
        //tut.GetComponent<TutorialOverlay>().endCall = OnOpenInventory;

    }

    public void OnSelectSpitter() {
        Invoke("SpawnBuySpitterTutorial", 0.001f);
    }


    void CheckHealth() {
        if (GameManager.slotId != -1) {
            if (GameManager.slots.slots[GameManager.slotId].progress.tutorialState != 3) return;
            if (WorldManager.wormMotion.wormState == WormState.playing) {
                if (WorldManager.wormHealth.health / WorldManager.wormHealth.maxHealth < 0.35f && WorldManager.stats.currentPlanetStats.Destruction() < DifficultyScaler.destructionReq * 0.8f) {
                    if (!Util.wm.buttonManagerMember.inventoryExpanded) {
                        GameObject tut = Instantiate(tutorialPrefab, Util.canvas.transform);
                        tut.GetComponent<TutorialOverlay>().button = FindButtonWithName("InventoryButton");
                        tut.GetComponent<TutorialOverlay>().instructionsStr = "Low health! Tap the bag to open your inventory.";
                        tut.GetComponent<TutorialOverlay>().endCall = OnOpenInventory;
                        tut.GetComponent<TutorialOverlay>().badButtons = new List<Button>();
                        tut.GetComponent<TutorialOverlay>().badButtons.Add(FindButtonWithName("PauseButton"));
                    }
                    else {
                        GameManager.slots.slots[GameManager.slotId].progress.tutorialState++;
                        OnOpenInventory();
                    }
                    return;
                }
            }
        }
        Invoke("CheckHealth", 1f);
    }

    void SpawnUseHealthPotTutorial() {
        if (GameManager.slots.slots[GameManager.slotId].progress.consumableCount[Progress.healthPotionId] <= 1) {
            GameManager.slots.slots[GameManager.slotId].progress.tutorialState++;
            return;
        }
        GameObject tut = Instantiate(tutorialPrefab, Util.canvas.transform);
        tut.GetComponent<TutorialOverlay>().button = FindButtonWithName("HealthPotionButton");
        tut.GetComponent<TutorialOverlay>().instructionsStr = "Tap the health potion to use. Buy more in the Gem Shop!";
        tut.GetComponent<TutorialOverlay>().badButtons = new List<Button>();
        tut.GetComponent<TutorialOverlay>().badButtons.Add(FindButtonWithName("SpeedPotionButton"));
        tut.GetComponent<TutorialOverlay>().badButtons.Add(FindButtonWithName("InventoryButton"));
        //tut.GetComponent<TutorialOverlay>().endCall = OnOpenInventory;

    }

    public void OnOpenInventory() {
        Invoke("SpawnUseHealthPotTutorial", 0.5f);
    }

    Button FindButtonWithName(string n) {
        foreach (var g in GameObject.FindGameObjectsWithTag("TutorialButton")) {
            if (g.name == n) {
                return g.GetComponent<Button>();
            }
        }
        return null;
    }




}
