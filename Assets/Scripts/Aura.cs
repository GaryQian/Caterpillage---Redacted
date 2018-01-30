using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aura : MonoBehaviour {
	// Use this for initialization
	void Start () {
        //mask = 1 << 12 + 1 << 14 + 1 << 19 + 1 << 15;
	}

    private void OnTriggerEnter2D(Collider2D collision) {
        int l = collision.gameObject.layer;
        if (l == 12 || l == 14 || l == 19 || l == 15) {
            WorldManager.wormMotion.AddNear(collision.gameObject);
        }
        if (!WorldManager.wormMotion.underground) {
            IListener listener = collision.GetComponent<IListener>();
            if (listener != null) {
                listener.Call(0);
            }
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision) {
        int l = collision.gameObject.layer;
        if (l == 12 || l == 14 || l == 19 || l == 15) {
            WorldManager.wormMotion.RemoveNear(collision.gameObject);
        }
    }
}
