using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameHealthBar : MonoBehaviour, IListener {

    public Character c;
    public GameObject green;
    public GameObject red;

    public GameObject AcidSkullPrefab;
    GameObject skull;
    public float yOffset;
    public float xOffset;
    public float scale;
	// Use this for initialization
	void Start () {
        c = GetComponentInParent<Character>();
        c.healthbar = this;
        c.healthbarObj = gameObject;
        UpdateHealthBar();
        
	}

    public void Call(int n) {
        UpdateHealthBar();
    }

    public GameObject SpawnAcidSkull() {
        if (skull == null) {
            skull = Instantiate(AcidSkullPrefab, transform);
            skull.transform.localRotation = Quaternion.identity;
            skull.transform.localPosition = new Vector3(-transform.localPosition.x / transform.localScale.x + xOffset, yOffset);
            skull.transform.localScale = new Vector3(1f / transform.localScale.x * scale, 1f / transform.localScale.y * scale, 1);
        }
        return skull;
    }
	
	public void UpdateHealthBar() {
        green.transform.localScale = new Vector3(c.health / c.maxHealth, green.transform.localScale.y);
        if (c.health == c.maxHealth) {
            green.SetActive(false);
            red.SetActive(false);
        }
        else {
            green.SetActive(true);
            red.SetActive(true);
        }
    }
}
