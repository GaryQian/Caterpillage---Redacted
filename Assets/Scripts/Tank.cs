using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour, IListener {

    public SpriteRenderer sr;
    public SpriteRenderer barrelSr;
    public GameObject barrel;
    public GameObject ExplosionPrefab;
    public GameObject BulletPrefab;
    public GameObject TankPoofPrefab;

    public AudioClip shootClip;

    public float damage = 50f;

    bool tracking;

    bool isQuitting;


	// Use this for initialization
	void Start () {
        WorldManager.wormMotion.AirToGroundListeners.Add(this);
        WorldManager.wormMotion.GroundToAirListeners.Add(this);
        int selection = Random.Range(0, TankManager.SelectedTanks.Length);
        sr.sprite = TankManager.SelectedTanks[selection];
        barrelSr.sprite = TankManager.SelectedBarrels[selection % 5];
        GetComponent<Character>().maxHealth = DifficultyScaler.tankHp;
        GetComponent<Character>().foodVal = DifficultyScaler.tankFood;
        tracking = false;
    }

    void Update() {
        if (tracking) {
            if (transform.localScale.x < 0) {
                //barrel.transform.localScale = new Vector3(-Mathf.Abs(barrel.transform.localScale.x), barrel.transform.localScale.y, 1);
                barrel.transform.eulerAngles = new Vector3(0, 0,
                    Util.AngleFromOffset(WorldManager.Worm.transform.position - transform.position) + 180f);
            }
            else {
                barrel.transform.eulerAngles = new Vector3(0, 0,
                    Util.AngleFromOffset(WorldManager.Worm.transform.position - transform.position));
            }
        }
    }

    public void Call(int id) {
        if (id == 0) {
            // Jump Out of ground
            if (WorldManager.wormMotion.NearbySet.Contains(gameObject)) {
                if (!IsInvoking("Shoot")) Invoke("Shoot", Random.Range(0.7f, 1.5f));
                GetComponent<CharacterMotion>().Stop(transform.position);
                tracking = true;
            }
        }
        else {
            // return to ground
            CancelInvoke("Shoot");
            GetComponent<CharacterMotion>().Move();
            tracking = false;
        }
    }

    void Shoot() {
        GameObject bulletTrail = Instantiate(BulletPrefab, transform.position, Quaternion.identity);
        bulletTrail.GetComponent<BulletTrail>().target = WorldManager.wormMotion.transform.position;
        bulletTrail.GetComponent<BulletTrail>().color = Color.white * 0.7f;
        bulletTrail.GetComponent<BulletTrail>().width = 0.9f;
        bulletTrail.GetComponent<BulletTrail>().damage = DifficultyScaler.tankDmg;
        bulletTrail.GetComponent<BulletTrail>().life = 0.7f;
        bulletTrail.transform.position = bulletTrail.transform.position + new Vector3(0, 0, 0.001f);

        float ang = 0;
        GameObject poof = Instantiate(TankPoofPrefab, transform.position, Quaternion.Euler(0, 0, ang), transform);
        if (transform.localScale.x < 0) {
            //barrel.transform.localScale = new Vector3(-Mathf.Abs(barrel.transform.localScale.x), barrel.transform.localScale.y, 1);
            poof.transform.eulerAngles = new Vector3(0, 0,
                Util.AngleFromOffset(WorldManager.Worm.transform.position - transform.position) + 180f);
        }
        else {
            poof.transform.eulerAngles = new Vector3(0, 0,
                Util.AngleFromOffset(WorldManager.Worm.transform.position - transform.position));
        }

        SoundManager.PlaySingleSfx(shootClip);

        Invoke("Shoot", Random.Range(2.5f, 2.5f));
    }

    void OnApplicationQuit() {
        isQuitting = true;
    }

    private void OnDestroy() {
        WorldManager.wormMotion.AirToGroundListeners.Remove(this);
        WorldManager.wormMotion.GroundToAirListeners.Remove(this);
        if (isQuitting) return;
        if (WorldManager.pg.safeToSpawn) {
            if (WorldManager.wormMotion.wormState == WormState.playing) {
                GameObject exp = Instantiate(ExplosionPrefab, transform.position + new Vector3(0, 0, -2f), transform.localRotation);
                exp.transform.localScale = new Vector3(0.6f, 0.6f, 1);
            }
            for (int i = 0; i < 3; ++i) {
                GameObject character = WorldManager.pg.SpawnCharacter(transform.position);
                character.GetComponent<Rigidbody2D>().AddForce(new Vector2(transform.position.x * Random.Range(0.5f, 1f) + Random.Range(-1f, 1f), transform.position.y * Random.Range(0.5f, 1f) + Random.Range(-1f, 1f)) * 50f);
            }
        }

        WorldManager.stats.EatTank();
    }
}
