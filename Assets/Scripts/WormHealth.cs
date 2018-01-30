using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormHealth : MonoBehaviour {

    class DamageInd {
        public float deathTime;
        public float damage;

        public DamageInd(float d, float t) {
            damage = d;
            deathTime = t;
        }
    }

    public float health;
    public float maxHealth;
    public float damageCD = 0.0f;
    public bool invincible = true;
    public float hunger = 10;

    public bool dying;

    public WormMotion wm;
    public BarManager bm;

    public float armor;

    List<DamageInd> damageQueue;
    List<DamageInd> feedQueue;
    public float indDelay = 0.5f;
    // Use this for initialization
    void Start() {
        WorldManager.wormHealth = this;
        ResetHealth();
    }

    private void Update() {
        switch (wm.wormState) {
            case WormState.playing: {
                    if (health > maxHealth * 0.1f) Damage(hunger * Time.deltaTime, false, true);
                    else Damage(hunger * Time.deltaTime * 0.5f, false, true);
                    break;
                }
            case WormState.dead: break;
            default: Feed(hunger * 16f * Time.deltaTime); break;
        }


        if (wm.wormState == WormState.dead) return;
        // Damage Indicator
        while (true) {
            if (damageQueue.Count == 0) break;
            if (damageQueue[0].deathTime < Time.time) {
                damageQueue.Remove(damageQueue[0]);
            }
            else {
                break;
            }
        }
        while (true) {
            if (feedQueue.Count == 0) break;
            if (feedQueue[0].deathTime < Time.time) {
                feedQueue.Remove(feedQueue[0]);
            }
            else {
                break;
            }
        }
        float dsum = 0;
        int border = (int)(damageQueue.Count * (.75f));
        int i = 0;
        float shakeSum = 0;
        foreach (DamageInd d in damageQueue) {
            dsum += d.damage;
            if (i >= border) shakeSum += d.damage;
            i++;
        }
        float fsum = 0;
        foreach (DamageInd d in feedQueue) {
            fsum += d.damage;
        }
        bm.SetHealthInd(health + Mathf.Max(dsum - fsum), maxHealth);

        //apply shake
        if (shakeSum / maxHealth > 0.01f) {
            float shakeAmount = Mathf.Min((shakeSum / maxHealth) / 0.05f * 2f, 1.5f);
            WorldManager.cameraFollow.Shake(shakeAmount, 0.1f);
        }

    }

    public void Damage(float d, bool blood = true, bool ignoreArmor=false) {
        if (invincible || WorldManager.wormMotion.wormState != WormState.playing) return;
        if (!ignoreArmor) d *= 1f - armor;
        health -= d;
        //float shakeAmount = Mathf.Min((d / maxHealth) / 0.05f * 5f, 25f);
        //WorldManager.cameraFollow.Shake(shakeAmount, 0.2f);
        
        damageQueue.Add(new DamageInd(d, Time.time + indDelay));
        WorldManager.stats.TakeDamage(d);
        if (blood) BloodOverlay.AddBlood(d);
        if (health < 0) {
            health = 0;
        }
        //Invoke("SetVulnerable", damageCD);
        if (health <= 0) {
            Die();
        }
        bm.SetHealthBar(health, maxHealth);
    }

    public void Feed(float d) {
        if (WorldManager.wormMotion.wormState == WormState.dying || WorldManager.wormMotion.wormState == WormState.dead) return;
        health += d;
        feedQueue.Add(new DamageInd(d, Time.time + indDelay));
        if (health > maxHealth) {
            health = maxHealth;
        }
        //Invoke("SetVulnerable", damageCD);
        bm.SetHealthBar(health, maxHealth);
    }

    public void ResetHealth() {
        maxHealth = GameManager.wormStats.size * GameManager.wormStats.healthIncrement + GameManager.wormStats.startHealth;
        health = maxHealth;
        invincible = true;
        Invoke("SetVulnerable", 3f);
        bm.SetHealthBar(health, maxHealth);
        bm.SetHealthTicks(maxHealth);
        dying = false;

        damageQueue = new List<DamageInd>();
        feedQueue = new List<DamageInd>();

        armor = GameManager.wormStats.armorLevel * GameManager.wormStats.damageReductionIncrement / 100f + GameManager.wormStats.startDamageReduction / 100f;
        hunger = DifficultyScaler.hunger;
        if (GameManager.level == 1) hunger *= 0.4f;
        if (GameManager.level == 2) hunger *= 0.7f;
    }

    public void FillHealth() {
        Feed(maxHealth - health);
    }

    public void Die(bool force=false) {
        if (!dying && (wm.wormState == WormState.playing || force)) {
            dying = true;
            Util.gm.EndGame();
            WorldManager.wormMotion.Die();
        }
    }

    public void SetVulnerable() {
        invincible = false;
    }
}
