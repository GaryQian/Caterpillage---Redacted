using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour {

    public static WorldManager Instance;

    public static GameObject Worm;
    public static bool paused = false;
    public static WormMotion wormMotion;
    public static WormHealth wormHealth;
    public static PlanetGenerator pg;
    public static StatsManager stats;
    public static CoinHandler ch;
    public static ConsumableHandler consumableHandler;
    public static ButtonManager buttonManager;
    public ButtonManager buttonManagerMember;

    public static GameObject cocoon;
    public static Camera camera;
    public static CameraFollow cameraFollow;


    public GameObject killCoinPrefab;

    public GameObject levelOneOverlayPrefab;
    GameObject levelOneOverlay;
    public GameObject messagePrefab;

    static Gamestate tempstate;

    public BarManager bm;

    public GameObject newEnemyMenuPrefab;


    // Use this for initialization
    void Start () {
        Instance = this;
        //Application.targetFrameRate = 40;
        pg = GameObject.Find("PlanetManager").GetComponent<PlanetGenerator>();
        stats = GetComponent<StatsManager>();
        Util.wm = this;
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        cameraFollow = camera.GetComponent<CameraFollow>();
        ch = GetComponent<CoinHandler>();
        consumableHandler = GetComponent<ConsumableHandler>();
        buttonManager = buttonManagerMember;

        ExplosionTracker.Reset();
    }

    public static void DamagedAlpha() {
        Debug.Log("Damaged Alpha");
    }

    public void BeginGame() {
        CheckDestruction();
        ExplosionTracker.Reset();
    }
	
	// Update is called once per frame

    public static void togglePause() {
        if (paused) { //unpause
            paused = false;
            GameManager.state = tempstate;
            Time.timeScale = 1;
        }        
        else { //pause
            paused = true;
            tempstate = GameManager.state;
            SoundManager.InstantSetLaserVol(0);
            Time.timeScale = 0;
        }
    }

    public static void ForcePause(bool s) {
        if (s) {
            paused = true;
            tempstate = GameManager.state;
            SoundManager.InstantSetLaserVol(0);
            Time.timeScale = 0;
        }
        else {
            paused = false;
            GameManager.state = tempstate;
            Time.timeScale = 1;

        }
    }

    public void CheckDestruction() {
        if (wormMotion.wormState == WormState.dying || wormMotion.wormState == WormState.dead) return;
        if (wormMotion.wormState == WormState.playing) {
            float des = stats.currentPlanetStats.Destruction();
            //Debug.Log(des);
            float maxDestruction = DifficultyScaler.destructionReq;
            bm.SetDestructionBar(des, maxDestruction);
            if (des > maxDestruction) {
                wormMotion.ExitPlanet();
                GameManager.slots.slots[GameManager.slotId].progress.totalPlanetsEaten++;
            }
            if (stats.currentPlanetStats.canSpawnGem && !stats.currentPlanetStats.gemSpawned && des > maxDestruction * 0.5f) {
                stats.currentPlanetStats.gemSpawned = true;
                pg.SpawnGem();
            }
        }
        CancelInvoke("CheckDestruction");
        Invoke("CheckDestruction", 0.25f);
    }

    public void CancelCheckDestruction() {
        CancelInvoke("CheckDestruction");
    }

    public void SpawnMessage(GameObject parent, enemy e) {
        GameObject message = Instantiate(messagePrefab);
        message.GetComponent<MessageController>().parent = parent;
        message.GetComponent<MessageController>().SetText(e);
    }

    public void SpawnNewEnemyMenu() {
        //buttonManager.pauseButtonEnabled = false; //turns off pause button
        int level = WorldManager.stats.currentPlanetStats.level;
        if (level != GameManager.maxLevel) return;
        if (level == 2) {
            Destroy(levelOneOverlay); 
        }
        if (level == DifficultyScaler.harmlessFirstLevel) {
            levelOneOverlay = Instantiate(levelOneOverlayPrefab, Util.gm.canvas.transform);
            GameObject newEnemyMenu = Instantiate(newEnemyMenuPrefab, Vector3.zero, Quaternion.identity, Util.gm.canvas.transform);
            newEnemyMenu.GetComponent<NewEnemyNotification>().SetEnemy(enemy.citizen);
            togglePause();
        }
        else if (level == DifficultyScaler.carFirstLevel) {
            GameObject newEnemyMenu = Instantiate(newEnemyMenuPrefab, Vector3.zero, Quaternion.identity, Util.gm.canvas.transform);
            newEnemyMenu.GetComponent<NewEnemyNotification>().SetEnemy(enemy.car);
            togglePause();
        }
        else if (level == DifficultyScaler.soldierFirstLevel) {
            GameObject newEnemyMenu = Instantiate(newEnemyMenuPrefab, Vector3.zero, Quaternion.identity, Util.gm.canvas.transform);
            newEnemyMenu.GetComponent<NewEnemyNotification>().SetEnemy(enemy.soldier);
            togglePause();
        }
        else if (level == DifficultyScaler.satelliteFirstLevel) {
            GameObject newEnemyMenu = Instantiate(newEnemyMenuPrefab, Vector3.zero, Quaternion.identity, Util.gm.canvas.transform);
            newEnemyMenu.GetComponent<NewEnemyNotification>().SetEnemy(enemy.satellite);
            togglePause();
        }
        else if (level == DifficultyScaler.helicopterFirstLevel) {
            GameObject newEnemyMenu = Instantiate(newEnemyMenuPrefab, Vector3.zero, Quaternion.identity, Util.gm.canvas.transform);
            newEnemyMenu.GetComponent<NewEnemyNotification>().SetEnemy(enemy.helicopter);
            togglePause();
        }
        else if (level == DifficultyScaler.tankFirstLevel) {
            GameObject newEnemyMenu = Instantiate(newEnemyMenuPrefab, Vector3.zero, Quaternion.identity, Util.gm.canvas.transform);
            newEnemyMenu.GetComponent<NewEnemyNotification>().SetEnemy(enemy.tank);
            togglePause();
        }
        else if (level == DifficultyScaler.generalFirstLevel) {
            GameObject newEnemyMenu = Instantiate(newEnemyMenuPrefab, Vector3.zero, Quaternion.identity, Util.gm.canvas.transform);
            newEnemyMenu.GetComponent<NewEnemyNotification>().SetEnemy(enemy.general);
            togglePause();
        }
    }
}
