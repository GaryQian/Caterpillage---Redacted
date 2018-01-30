using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemCore : MonoBehaviour {

    public float maxHealth = 2f;
    public bool damaging = false;
    public float enterTime;

    public GameObject GemShatterPrefab;
    public GameObject KillGemPrefab;

    bool isQuitting;

    // Use this for initialization
    void Start () {
        enterTime = 99999999f;
        damaging = false;
	}

    private void Update() {
        if (damaging) {
            if (Time.time - enterTime > maxHealth) {
                if (WorldManager.wormMotion.wormState == WormState.playing) {
                    GameManager.slots.slots[GameManager.slotId].progress.gems += 1;
                    WorldManager.wormHealth.Feed(WorldManager.wormHealth.maxHealth * 0.25f);
                    WorldManager.stats.EatGem();
                }
                Destroy(gameObject);
                
            }
            transform.position = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), -.01f) * (Time.time - enterTime) / maxHealth * 0.05f;
        }
    }

    void Undamage() {
        damaging = false;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (WorldManager.wormMotion.wormState != WormState.playing || collision.gameObject.layer != 11 || collision.gameObject.name != "Worm") return;
        enterTime = Time.time;
        damaging = true;
    }
    //private void OnTriggerStay2D(Collider2D collision) {
    //    if (WorldManager.wormMotion.wormState != WormState.playing || collision.gameObject.layer != 11 || collision.gameObject.name != "Worm") return;
    //    damaging = true;
    //    CancelInvoke("Undamage");
    //    Invoke("Undamage", 0.2f);
    //}

    private void OnTriggerExit2D(Collider2D collision) {
        if (WorldManager.wormMotion.wormState != WormState.playing || collision.gameObject.layer != 11 || collision.gameObject.name != "Worm") return;
        transform.position = new Vector3(0, 0, -.001f);
        damaging = false;
    }

    void OnApplicationQuit() {
        isQuitting = true;
    }

    private void OnDestroy() {
        if (isQuitting) return;
        Instantiate(GemShatterPrefab, transform.position, Quaternion.identity);
        Instantiate(KillGemPrefab, transform.position, Quaternion.identity);
    }
}
