using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSetter : MonoBehaviour {

    public int id;

	// Use this for initialization
	void Start () {
        switch (id) {
            case 1:
                GetComponent<Button>().onClick.AddListener(WorldManager.buttonManager.UseHealthPotion);
                break;
            case 2:
                GetComponent<Button>().onClick.AddListener(WorldManager.buttonManager.UseSpeedPotion);
                break;
            case 3:
                GetComponent<Button>().onClick.AddListener(WorldManager.buttonManager.UseBerserkPotion);
                break;
        }
        
	}
}
