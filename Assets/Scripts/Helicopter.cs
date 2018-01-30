using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helicopter : MonoBehaviour, IListener {

    public float targetAngle;
    float currAngle;
    public float juice = 2f;
    public float interval = 1f;

    public GameObject parent;
    public CharacterMotion cm;

    bool isQuitting;
    public GameObject ExplosionPrefab;
    public GameObject MissilePrefab;

    Animator animator;

    public AudioClip[] oinkClips;
    
	// Use this for initialization
	void Start () {
        WorldManager.wormMotion.AirToGroundListeners.Add(this);
        WorldManager.wormMotion.GroundToAirListeners.Add(this);
        FindTargetAngle();
        currAngle = targetAngle;
        InvokeRepeating("FindTargetAngle", Random.Range(0, interval), interval);

        animator = GetComponent<Animator>();

        GetComponent<Character>().maxHealth = DifficultyScaler.helicopterHp;
        GetComponent<Character>().foodVal = DifficultyScaler.helicopterFood;


        Invoke("SayQuote", Random.Range(5f, 30f));
    }
	
	// Update is called once per frame
	void Update () {
        if (transform.localScale.x < 0) {
            //barrel.transform.localScale = new Vector3(-Mathf.Abs(barrel.transform.localScale.x), barrel.transform.localScale.y, 1);
            transform.eulerAngles = new Vector3(0, 0,
                targetAngle + 90f);
        }
        else {
            transform.eulerAngles = new Vector3(0, 0,
                targetAngle - 90f);
        }
        //currAngle += (targetAngle - currAngle) * Time.deltaTime;
        //float angle = -parent.transform.eulerAngles.z + currAngle;
        //transform.localRotation = Quaternion.Euler(new Vector3(0, 0, angle));
	}

    void FindTargetAngle() {
        targetAngle = Util.AngleFromOffset(transform.position);
    }

    public void Call(int id) {
        if (id == 0) {
            // Jump Out of ground
            if (WorldManager.wormMotion.NearbySet.Contains(gameObject)) {
                if (!IsInvoking("Shoot")) Invoke("Shoot", Random.Range(0.7f, 1f));
                //GetComponent<CharacterMotion>().Stop(transform.position);
            }
        }
        else {
            // return to ground
            CancelInvoke("Shoot");
            //GetComponent<CharacterMotion>().Move();
        }
    }

    void Shoot() {
        Invoke("SpawnMissile", 0.15f);

        animator.SetTrigger("Shoot");

        Invoke("Shoot", Random.Range(1.2f, 1.7f));

    }

    void SpawnMissile() {
        GameObject missile = Instantiate(MissilePrefab, transform.position, Quaternion.identity);
        missile.GetComponent<HomingMissile>().target = WorldManager.Worm;
        missile.GetComponent<HomingMissile>().damage = DifficultyScaler.helicopterDmg;

        SoundManager.PlaySfx(0.6f, oinkClips);
    }

    void SayQuote() {
        Util.wm.SpawnMessage(gameObject, enemy.helicopter);
        Invoke("SayQuote", Random.Range(15f, 30f));
    }


    private void OnDestroy() {
        WorldManager.wormMotion.AirToGroundListeners.Remove(this);
        WorldManager.wormMotion.GroundToAirListeners.Remove(this);
        WorldManager.stats.EatHelicopter();
        if (isQuitting) return;
        if (WorldManager.pg.safeToSpawn) {
            Instantiate(ExplosionPrefab, transform.position, transform.localRotation);
            //exp.transform.localScale = new Vector3(0.6f, 0.6f, 1);
            for (int i = 0; i < 3; ++i) {
                GameObject character = WorldManager.pg.SpawnCharacter(transform.position);
                character.GetComponent<Rigidbody2D>().AddForce(new Vector2(transform.position.x * Random.Range(0.5f, 1f) + Random.Range(-1f, 1f), transform.position.y * Random.Range(0.5f, 1f) + Random.Range(-1f, 1f)) * 50f);
            }
        }
    }

    void OnApplicationQuit() {
        isQuitting = true;
    }
}
