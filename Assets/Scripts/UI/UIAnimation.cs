using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAnimation : MonoBehaviour {

    public Sprite[] frames;
    public float delay = 0.2f;
    public int startFrame = 0;
    public bool loop = true;
    Image image;
    int currentFrame;

    // Use this for initialization
    void Start () {
        image = GetComponent<Image>();
        currentFrame = startFrame;
        InvokeRepeating("NextFrame", 0, delay);
    }
	
	void NextFrame() {
        image.sprite = frames[currentFrame];
        currentFrame++;
        if (currentFrame >= frames.Length) {
            if (loop)
                currentFrame = 0;
            else
                currentFrame--;
        }
        
    }
}
