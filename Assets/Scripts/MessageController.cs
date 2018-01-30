using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageController : MonoBehaviour {

    public TextMesh text;
    public GameObject parent;

    public float citizenScale;
    public float carScale;
    public float soldierScale;
    public float satelliteScale;
    public float helicopterScale;
    public float tankScale;
    public float generalScale;

    public Vector3 citizenOffset;
    public Vector3 carOffset;
    public Vector3 soldierOffset;
    public Vector3 satelliteOffset;
    public Vector3 helicopterOffset;
    public Vector3 tankOffset;
    public Vector3 generalOffset;

    public string[] citizenQuotes;
    public string[] carQuotes;
    public string[] soldierQuotes;
    public string[] satelliteQuotes;
    public string[] helicopterQuotes;
    public string[] tankQuotes;
    public string[] generalQuotes;

	
	// Update is called once per frame
	void Update () {
        if (parent == null) {
            Destroy(gameObject);
            return;
        }
        transform.position = parent.transform.position + new Vector3(0, 0, -0.2f);
	}

    public void SetText(enemy e) {
        switch (e) {
            case enemy.citizen:
                text.text = citizenQuotes[Random.Range(0, citizenQuotes.Length)];
                transform.localScale = Vector3.one * .2f;
                break;
            case enemy.car:
                text.text = carQuotes[Random.Range(0, carQuotes.Length)];
                transform.localScale = Vector3.one * .2f;
                break;
            case enemy.soldier:
                text.text = soldierQuotes[Random.Range(0, soldierQuotes.Length)];
                transform.localScale = Vector3.one * .2f;
                break;
            case enemy.satellite:
                text.text = satelliteQuotes[Random.Range(0, satelliteQuotes.Length)];
                transform.localScale = Vector3.one * .2f;
                break;
            case enemy.helicopter:
                text.text = helicopterQuotes[Random.Range(0, helicopterQuotes.Length)];
                transform.localScale = Vector3.one * .2f;
                break;
            case enemy.tank:
                text.text = tankQuotes[Random.Range(0, tankQuotes.Length)];
                transform.localScale = Vector3.one * .2f;
                break;
            case enemy.general:
                text.text = generalQuotes[Random.Range(0, generalQuotes.Length)];
                transform.localScale = Vector3.one * .2f;
                break;
        }
    }
}
