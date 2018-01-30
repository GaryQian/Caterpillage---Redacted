using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSound : MonoBehaviour {

    public AudioClip[] hitSounds;

    public float vol = 1;

    public float playDelay = 0.5f;
    float lastPlayTime = 0;
	
	public void PlaySound() {
        if (Time.time - lastPlayTime > playDelay) {
            SoundManager.PlaySfx(vol, hitSounds);
            lastPlayTime = Time.time;
        }
    }
}
