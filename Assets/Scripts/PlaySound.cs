using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour {

    public AudioClip[] clips;
    public float delay = 0;
    public float volume= 1f;
    public bool playOnDie = false;
    public bool onlyOnPlanet = true;
	// Use this for initialization
	void Start () {
        if (!playOnDie) Invoke("Play", delay);
	}
	
	public void Play() {
        if (onlyOnPlanet) {
            if (WorldManager.wormMotion.wormState != WormState.playing) return;
            else SoundManager.PlaySfx(volume, clips);
        }
        else {
            SoundManager.PlaySfx(volume, clips);
        }
    }

    void OnDestroy() {
        if (playOnDie) Play();
    }
}
