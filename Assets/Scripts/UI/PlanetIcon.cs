using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetIcon : MonoBehaviour {

    public Text NumberText;
    public Image PlanetImage;
    public Image StarsImage;

    public Sprite locked;
    public Sprite noStars;
    public Sprite oneStar;
    public Sprite twoStars;
    public Sprite threeStars;

    public Sprite p1;
    public Sprite p2;

    public int level;
    public int stars;

	// Use this for initialization
	void Start () {
        NumberText.text = "" + level;
        switch (level) {
            case 99: PlanetImage.color = new Color(1f, 0.2f, 0.05f, 1f); break;
            case 100: PlanetImage.color = new Color(0, 1f, 0.3f, 1f); break;
            default: PlanetImage.color = new Color(Random.Range(0.2f, 1f), Random.Range(0.2f, 1f), Random.Range(0.2f, 1f), 1f); break;
        }
        switch (stars) {
            case -1: StarsImage.sprite = locked; break;
            case 0: StarsImage.sprite = noStars; break;
            case 1: StarsImage.sprite = oneStar; break;
            case 2: StarsImage.sprite = twoStars; break;
            case 3: StarsImage.sprite = threeStars; break;

        }
        if (Random.Range(0, 1f) < 0.5f) {
            PlanetImage.sprite = p2;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
