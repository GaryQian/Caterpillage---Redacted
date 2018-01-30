using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryBGStretcher : MonoBehaviour {

    RectTransform rect;
    Vector2 target;
    float rate = 0.8f;

	// Use this for initialization
	void Start () {
        rect = GetComponent<RectTransform>();
	}

    public void ToggleTarget(int size) {
        switch (size) {
            case 0:
                rect.anchorMin = new Vector2(rect.anchorMin.x, 0.8f);
                break;
            case 1:
                rect.anchorMin = new Vector2(rect.anchorMin.x, 0.65f);
                break;
            case 2:
                rect.anchorMin = new Vector2(rect.anchorMin.x, 0.5f);
                break;
            case 3:
                rect.anchorMin = new Vector2(rect.anchorMin.x, 0.35f);
                break;
        }
    }
}
