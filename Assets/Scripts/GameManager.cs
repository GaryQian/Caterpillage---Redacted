using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Gamestate { HomeMenu, UpgradeMenu, Playing, Paused, Dead, Stats }

public class GameManager : MonoBehaviour {

    public GameObject canvas;
    public GameObject SplashScreenPrefab;
    GameObject splashScreen;
    public GameObject UpgradeMenuPrefab;
    public GameObject upgradeMenu;
    public GameObject TitleMenuPrefab;
    GameObject titleMenu;
    public GameObject StatsScreenPrefab;
    GameObject statsScreen;
    public GameObject cutscenePrefab;
    GameObject cutscene;

    public static GameManager Instance;


    public TutorialManager tutorialManager;

    public GameObject pregramBlackBGPrefab;
    GameObject pregameBlackBG;

    public GameObject buttonHolder;

    public static WorldManager wm;
    public static SaveManager sm;
    public static SettingsData settings;
    public static GameObject camera;
    public static AdManager adManager;
    public static Purchaser purchaser;

    public static UpgradeManager upgradeManager;
    public static bool inGemShop = false;

    public static WormStats wormStats;
    public static int level = 1;
    public static int maxLevel = 1;
    public static int slotId = -1;
    public static Slots slots;
    public static int version;
    public static int plays;

    public static Gamestate state;

    public static float money = 1200000f;
    public static int gems = 0;

    public AudioClip startClip;


	// Use this for initialization
	void Start () {
        Instance = this;

        Application.targetFrameRate = 60;

        state = Gamestate.HomeMenu;

        sm = GetComponent<SaveManager>();
        camera = GameObject.Find("Main Camera");
        settings = new SettingsData();
        settings = sm.GetSettings();
        sm.SaveSettings();

        SoundManager.instance.UpdateMusicOnOff();

        adManager = GetComponent<AdManager>();
        purchaser = GetComponent<Purchaser>();

        version = sm.GetVersion();
        Debug.Log("SaveData version: " + version);
        slots = new Slots();
        for (int i = 1; i <= 3; i++) {
            slots.slots[i] = sm.LoadSlot(i);
            if (slots.slots[i] == null) {
                slots.slots[i] = new SlotData(i);
            }
        }

        splashScreen = Instantiate(SplashScreenPrefab, Vector3.zero, Quaternion.identity, canvas.transform);

        wormStats = new WormStats();

        plays = 0;

        Invoke("DisplayTitleMenu", 1.5f);
    }

    private void Update() {
        //////////////////////////////////
        ////////////////////////////////////
        //////////////////////////////////
        //////////////////////////////////
        //CHEATS
#if UNITY_EDITOR
        if (Input.GetKeyDown("m")) {
            GameManager.settings.hasCheated = true;
            GameManager.sm.SaveSettings();
            money += 100000;
        }
        if (Input.GetKeyDown("g")) {
            GameManager.settings.hasCheated = true;
            GameManager.sm.SaveSettings();
            slots.slots[slotId].progress.gems += 100000;
        }
        if (Input.GetKeyDown("h")) {
            GameManager.settings.hasCheated = true;
            GameManager.sm.SaveSettings();
            WorldManager.wormHealth.Feed(10000);
        }
#endif
        //////////////////////////////////
        ////////////////////////////////////
        //////////////////////////////////
        //////////////////////////////////
        //CHEATS
    }

    public void OnPlayButtonPressed(bool wasEmpty = false) {
        //Debug.Log("Play Button Pressed");
        if (!wasEmpty) {
            Invoke("DisplayUpgradeMenu", 0f);
        }
        else {
            LoadCutscene();
        }
        Invoke("DeleteTitleMenu", .75f);
        

    }

    public void LoadCutscene() {
        if (cutscene == null) {
            Destroy(cutscene);
        }
        cutscene = Instantiate(cutscenePrefab, Util.canvas.transform);
    }

    public void LoadTitlePage() {
        titleMenu = Instantiate(TitleMenuPrefab, Vector3.zero, Quaternion.identity, canvas.transform);
    }

    public static void SelectSlot(int i) {
        slotId = i;
        slots.slots[slotId].isEmpty = false;
        SyncSlotToGM();
        sm.SaveCurrentSlot();

        GameManager.Instance.tutorialManager.SpawnTutorialCheckers();

        SoundManager.instance.ChangeMusic(MusicType.menu);
    }


    // Transfers slot data to GM
    public static void SyncSlotToGM() {
        wormStats = slots.slots[slotId].wormStats;
        money = slots.slots[slotId].progress.money;
        level = slots.slots[slotId].progress.maxLevel;
        maxLevel = slots.slots[slotId].progress.maxLevel;

    }

    // Transfers GM to slot
    public static void SyncGMToSlot() {
        slots.slots[slotId].wormStats = wormStats;
        slots.slots[slotId].progress.money = money;
        slots.slots[slotId].progress.maxLevel = maxLevel;
    }

    public void OnSlotBackButtonPressed() {
        titleMenu.GetComponent<TitleManager>().BackButtonPress();
    }

    public float GetMoney() {
        return money;
    }

    public void AddMoney(float amount) {
        money += amount;
        //sound
        if (upgradeManager != null) upgradeManager.UpdateMoney();
    }

    public bool SpendMoney(float amount) {
        if (money - amount >= 0) {
            money -= amount;
            if (upgradeManager != null) upgradeManager.UpdateMoney(amount);
            return true;
        }
        else {
            return false;
        }
    }

    public bool SpendGems(int amount) {
        if (GameManager.slots.slots[GameManager.slotId].progress.gems - amount >= 0) {
            GameManager.slots.slots[GameManager.slotId].progress.gems -= amount;
            if (upgradeManager != null) upgradeManager.UpdateGems(amount);
            return true;
        }
        else {
            return false;
        }
    }

    void DeleteTitleMenu() {
        Destroy(titleMenu);
    }

    public void DestroyBlackBG() {
        Destroy(pregameBlackBG);
    }
    public void SpawnBlackBG() {
        pregameBlackBG = Instantiate(pregramBlackBGPrefab, Vector3.zero, Quaternion.identity, canvas.transform);
        pregameBlackBG.GetComponent<RectTransform>().SetAsFirstSibling();
    }

    public void StartGame(string name = "RANDOM") {
        Debug.Log("Starting Game");
        if (state == Gamestate.Playing) return;
        state = Gamestate.Playing;


        GameManager.slots.slots[GameManager.slotId].progress.totalPlays++;
        plays++;
        if (upgradeManager != null) {
            GoToTarget upgradeManagerGTT = upgradeManager.GetComponent<GoToTarget>();
            upgradeManagerGTT.speed = 10f;
            upgradeManagerGTT.ChangeTarget(new Vector3(1930, 0, 0));
        }
        WorldManager.stats.ResetStats(level);

        WorldManager.pg.GeneratePlanet(level, 0.4f);
        if (name != "RANDOM") WorldManager.stats.currentPlanetStats.name = name;
        Util.wm.bm.SetPlanetName();
        Util.wm.bm.SetDestructionBar(0, 1f);


        WorldManager.wormMotion.BeginGame();
        WorldManager.Worm.SetActive(false);
        WorldManager.cameraFollow.SetMode(CameraMode.hardFollow);

        WorldManager.consumableHandler.DeactivateAll();

        WorldManager.buttonManager.UpdateClosedInventory();
        WorldManager.buttonManager.UpdateExpandedInventory();

        Invoke("StartGame2", 0.1f);

        SoundManager.PlaySfx(startClip);
        SoundManager.instance.ChangeMusic(MusicType.inGame);

        //GameManager.Instance.tutorialManager.SpawnTutorialCheckers();

    }

    void StartGame2() {
        WorldManager.Worm.SetActive(true);

        Util.wm.BeginGame();

        tutorialManager.CheckSpittingWarning();

        camera.GetComponent<CameraFollow>().JumpToWorm();
    }



    public void EndGame() {
        Debug.Log("Ending Game");
        //state = Gamestate.Stats;
        DisplayStatsScreen();
    }

    public void CloseStatsScreen() {
        DisplayUpgradeMenu();
        WorldManager.pg.ErasePlanet();
    }

    void DisplayStatsScreen() {
        if (statsScreen != null) Destroy(statsScreen);
        statsScreen = Instantiate(StatsScreenPrefab, canvas.transform);
    }

    public void DisplayTitleMenu() {
        titleMenu = Instantiate(TitleMenuPrefab, Vector3.zero, Quaternion.identity, canvas.transform);
        splashScreen.transform.SetAsLastSibling();
    }

    public void DisplayUpgradeMenu() {
        state = Gamestate.UpgradeMenu;
        if (upgradeMenu != null) Destroy(upgradeMenu);
        upgradeMenu = Instantiate(UpgradeMenuPrefab, Vector3.zero, Quaternion.identity, canvas.transform);
        upgradeManager = upgradeMenu.GetComponent<UpgradeManager>();

        GameManager.Instance.tutorialManager.SpawnTutorialCheckers();
    }
}
