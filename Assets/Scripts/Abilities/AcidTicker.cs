using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidTicker : MonoBehaviour {

    public float damage;
    public float duration;

    GameObject acidSkull;

    Character c;
	// Use this for initialization
	void Start () {
        c = GetComponent<Character>();
        if (c == null) {
            Die();
        }
        if (c.healthbarObj != null) {
            acidSkull = c.healthbarObj.GetComponent<InGameHealthBar>().SpawnAcidSkull();
        }
	}

    public void Reset() {
        CancelInvoke("Die");
        Invoke("Die", duration);
    }

    // Update is called once per frame
    void Update () {
        c.Damage(damage / duration * Time.deltaTime, true, false);
	}

    void Die() {
        Destroy(acidSkull);
        Destroy(this);

    }
}
