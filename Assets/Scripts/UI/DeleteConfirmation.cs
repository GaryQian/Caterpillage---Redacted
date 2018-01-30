using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteConfirmation : MonoBehaviour {

    public Slot slot;
	// Use this for initialization
	
	public void No() {
        Destroy(gameObject);
    }

    public void Yes() {
        slot.DeleteSlot();
        Destroy(gameObject);
    }
}
