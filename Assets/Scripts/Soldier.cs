using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour, IListener {

    public GameObject BulletPrefab;

    public AudioClip laserShootClip;

    public SpriteRenderer gunSr;

	// Use this for initialization
	void Start () {
        
        WorldManager.wormMotion.AirToGroundListeners.Add(this);
        WorldManager.wormMotion.GroundToAirListeners.Add(this);

        gunSr.sprite = CharacterManager.selectedGun;

    }

    public void Call(int id) {
        if (id == 0) {
            // Jump Out of ground
            if (WorldManager.wormMotion.NearbySet.Contains(gameObject)) {
                if (!IsInvoking("Shoot")) Invoke("Shoot", Random.Range(0.3f, 1f));
                GetComponent<CharacterMotion>().Stop(transform.position);
            }
        }
        else {
            // return to ground
            CancelInvoke("Shoot");
            GetComponent<CharacterMotion>().Move();
        }
    }

    void Shoot() {
        GameObject bulletTrail = Instantiate(BulletPrefab, transform.position, Quaternion.identity);
        bulletTrail.GetComponent<BulletTrail>().target = WorldManager.wormMotion.transform.position;
        bulletTrail.GetComponent<BulletTrail>().damage = DifficultyScaler.soldierDmg;

        SoundManager.PlaySingleSfx(laserShootClip, .75f);

        Invoke("Shoot", Random.Range(0.7f, 1.2f));
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnDestroy() {
        WorldManager.wormMotion.AirToGroundListeners.Remove(this);
        WorldManager.wormMotion.GroundToAirListeners.Remove(this);
    }
}
