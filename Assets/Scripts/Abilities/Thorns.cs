using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thorns : MonoBehaviour {

    private void Start() {
        if (!GameManager.wormStats.thorns) {
            Destroy(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.GetComponent<Character>() != null) {
            collision.GetComponent<Character>().ThornDamage(WorldManager.wormMotion.damage);
            Vector3 diff = (collision.transform.position - transform.position).normalized;
            Debug.Log(collision.GetComponent<Rigidbody2D>());
            if (collision.gameObject.layer != 15) collision.GetComponent<Rigidbody2D>().AddForce(new Vector2(diff.x, diff.y) * 200f);
        }
    }
}
