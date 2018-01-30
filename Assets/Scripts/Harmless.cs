using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harmless : MonoBehaviour, IListener {
    public SpriteRenderer headSpriteRenderer;
    public bool isSoldier = false;

    public GameObject BloodPrefab;

    bool isQuitting;
	// Use this for initialization
	void Start () {
        GetComponent<Animator>().runtimeAnimatorController = CharacterManager.harmlessBody;
        GetComponent<SpriteRenderer>().color = CharacterManager.bodyColor;

        headSpriteRenderer.sprite = CharacterManager.harmlessHead;
        headSpriteRenderer.color = CharacterManager.bodyColor;

        if (isSoldier) {
            GetComponent<Character>().maxHealth = DifficultyScaler.soldierHp;
            GetComponent<Character>().foodVal = DifficultyScaler.soldierFood;
        }
        else {
            GetComponent<Character>().maxHealth = DifficultyScaler.harmlessHp;
            GetComponent<Character>().foodVal = DifficultyScaler.harmlessFood;
        }
       // headSpriteRenderer.color = CharacterManager.headColor;

        WorldManager.wormMotion.GroundToAirListeners.Add(this);

    }

    public void Call(int id) {
        if (id == 0) {
            // Jump Out of ground
            if (!isSoldier) {
                if (WorldManager.wormMotion.NearbySet.Contains(gameObject))
                    GetComponent<CharacterMotion>().Fear(WorldManager.Worm.transform.position);
                if (Random.Range(0, 1f) < 0.05f) Util.wm.SpawnMessage(gameObject, enemy.citizen);
            }
            else {
                if (Random.Range(0, 1f) < 0.05f) Util.wm.SpawnMessage(gameObject, enemy.soldier);
            }
        }
        else {
            // return to ground
            if (!isSoldier) {

            }
        }
    }

    void OnApplicationQuit() {
        isQuitting = true;
    }

    private void OnDestroy() {
        WorldManager.wormMotion.GroundToAirListeners.Remove(this);
        if (!isQuitting && WorldManager.wormMotion.wormState == WormState.playing) Instantiate(BloodPrefab, transform.position + new Vector3(0, 0, 0), transform.localRotation);
        if (isSoldier) {
            WorldManager.stats.EatSoldier();
        }
        else {
            WorldManager.stats.EatHarmless();
        }
    }
}
