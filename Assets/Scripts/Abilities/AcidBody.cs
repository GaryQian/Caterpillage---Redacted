using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidBody : MonoBehaviour {

    private void Start() {
        if (!GameManager.wormStats.poison) {
            Destroy(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.GetComponent<Character>() != null) {
            //Debug.Log("Acid!");
            collision.GetComponent<Character>().ApplyAcid();
            //if (collision.GetComponent<AcidTicker>() != null) {
            //    Destroy(collision.GetComponent<AcidTicker>());
            //}
            //AcidTicker ticker = collision.gameObject.AddComponent<AcidTicker>();
            //ticker.duration = 3f;
            //ticker.damage = WorldManager.wormMotion.damage;
        }
    }
}
