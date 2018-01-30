using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour, IListener {

    public GameObject ExplosionPrefab;

    bool isQuitting;

    
    // Use this for initialization
    void Start () {
		WorldManager.wormMotion.GroundToAirListeners.Add(this);

        GetComponent<SpriteRenderer>().sprite = (Sprite)CarManager.SelectedCars[Random.Range(0, CarManager.SelectedCars.Count)];

        GetComponent<Character>().maxHealth = DifficultyScaler.carHp;
        GetComponent<Character>().foodVal = DifficultyScaler.carFood;
    }

    public void Call(int id) {
        if (id == 0) {
            // Jump Out of ground
            if (WorldManager.wormMotion.NearbySet.Contains(gameObject))
                GetComponent<CharacterMotion>().Fear(WorldManager.Worm.transform.position);
        }
        else {
            // return to ground
        }
    }

    void OnApplicationQuit() {
        isQuitting = true;
    }

    private void OnDestroy() {
        WorldManager.wormMotion.GroundToAirListeners.Remove(this);
        if (isQuitting) return;
        if (WorldManager.pg.safeToSpawn) {
            if (WorldManager.wormMotion.wormState == WormState.playing) Instantiate(ExplosionPrefab, transform.position + new Vector3(0, 0, -2f), transform.localRotation);
            for (int i = 0; i < 3; ++i) {
                GameObject character = WorldManager.pg.SpawnCharacter(transform.position);
                character.GetComponent<Rigidbody2D>().AddForce(new Vector2(transform.position.x * Random.Range(0.5f, 1f) + Random.Range(-1f, 1f), transform.position.y * Random.Range(0.5f, 1f) + Random.Range(-1f, 1f)) * 50f);
            }
        }

        WorldManager.stats.EatCar();
    }
}
