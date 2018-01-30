using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Satellite : MonoBehaviour, IListener {

    public Rigidbody2D body;
    public GameObject LaserPrefab;
    int dir;

    public float angle = 0;
    public float angularV = 1f;
    public float height;
    public float avgHeight = 1f;

    float radiusMultiplier = 1f;
    public float zeroRadiusTime = 160f;

    public GameObject laser;

    public GameObject ExplosionPrefab;

    bool isQuitting;

    // Use this for initialization
    void Start () {
        WorldManager.wormMotion.GroundToAirListeners.Add(this);

        angle = Random.Range(0, Mathf.PI * 2);
        height = Random.Range(avgHeight - 0.2f,  avgHeight + 0.2f);
        transform.position = GetTargetPos();
        GetComponent<Character>().maxHealth = DifficultyScaler.satelliteHp;
        NewDir();

        Invoke("SayQuote", Random.Range(5f, 40f));
    }

    void NewDir() {
        dir = Random.Range(0, 100f) < 50f ? -1 : 1;
        //transform.localScale = new Vector3(transform.localScale.x * dir, transform.localScale.y, transform.localScale.z);
        Invoke("NewDir", Random.Range(10f, 25f));
    }

    // Update is called once per frame
    void FixedUpdate () {
        angle += angularV * dir * Time.fixedDeltaTime;
        body.MovePosition(GetTargetPos());
        body.MoveRotation(Util.AngleFromOffset(new Vector2(WorldManager.wormMotion.transform.position.x - transform.position.x, WorldManager.wormMotion.transform.position.y - transform.position.y)) + 90f);
	}




    Vector3 GetTargetPos() {
        radiusMultiplier = Mathf.Max(0, (zeroRadiusTime - WormMotion.timeUnderground) / zeroRadiusTime * 0.5f + 0.5f);
        return new Vector3(Mathf.Cos(angle) * (height + WorldManager.pg.radius * radiusMultiplier),
            Mathf.Sin(angle) * (height + WorldManager.pg.radius * radiusMultiplier), 0);
    }

    public void Call(int id) {
        if (laser == null) {
            laser = Instantiate(LaserPrefab);
            laser.GetComponent<Laser>().dps = DifficultyScaler.satelliteDmg;
            laser.GetComponent<Laser>().parent = gameObject;
        }
    }

    void SayQuote() {
        Util.wm.SpawnMessage(gameObject, enemy.general);
        Invoke("SayQuote", Random.Range(15f, 40f));
    }

    void OnApplicationQuit() {
        isQuitting = true;
    }

    private void OnDestroy() {
        if (WorldManager.wormMotion.wormState == WormState.playing) Instantiate(ExplosionPrefab, transform.position + new Vector3(0, 0, 0), transform.localRotation);
        WorldManager.wormMotion.GroundToAirListeners.Remove(this);
        Destroy(laser);
        WorldManager.stats.EatSatellite();
    }
}
