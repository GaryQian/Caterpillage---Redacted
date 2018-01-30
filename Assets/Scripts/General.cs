using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class General : MonoBehaviour {
    public Animator anim;

    public Collider2D c1;
    public Collider2D c2;
    public Collider2D trigger;

    public float hitCD;
    public float lastHitTime;

    public GameObject AuraPrefab;

    public GameObject ShockwavePrefab;
    public GameObject DeathPrefab;

    bool isQuitting;
    // Use this for initialization
    void Start () {
        lastHitTime = Time.time;
        Physics2D.IgnoreCollision(c1, WorldManager.wormMotion.HitCollider);
        Physics2D.IgnoreCollision(c2, WorldManager.wormMotion.HitCollider);

        GameObject aura = Instantiate(AuraPrefab);
        aura.transform.localScale = transform.localScale;
        aura.GetComponent<GeneralAura>().gen = this;

        GetComponent<Character>().maxHealth = DifficultyScaler.generalHp;
        GetComponent<Character>().foodVal = DifficultyScaler.generalFood;
        //Physics2D.IgnoreCollision(trigger, WorldManager.wormMotion.HitCollider, false);

        Invoke("SayQuote", Random.Range(3f, 20f));
    }

    public void Hit() {
        lastHitTime = Time.time + hitCD;
        Debug.Log("Hitting");
        anim.SetTrigger("Idle");
        Invoke("BeginSmash", 0.5f);
        Invoke("Smash", .9f);
        Invoke("EndSmash", .833f + 0.5f);
    }

    void BeginSmash() {
        anim.SetTrigger("Smash");
    }

    void Smash() {
        GameObject shock = Instantiate(ShockwavePrefab, transform.position, Quaternion.identity);
        shock.GetComponent<Shockwave>().damage = DifficultyScaler.generalDmg;
    }

    void EndSmash() {
        anim.SetTrigger("Walk");
    }

    void OnApplicationQuit() {
        isQuitting = true;
    }


    private void OnDestroy() {
        if (isQuitting) return;
        WorldManager.stats.EatGeneral();
        GameObject dier = Instantiate(DeathPrefab, transform.position, transform.rotation);
        dier.transform.localScale = transform.localScale;


        
    }

    void SayQuote() {
        if (gameObject == null) return;
        Util.wm.SpawnMessage(gameObject, enemy.general);
        Invoke("SayQuote", Random.Range(10f, 26f));
    }

    //private void OnCollisionEnter2D(Collision2D collision) {
    //    //Layer11 = WormHead
    //    Debug.Log("Entered" + collision.gameObject.name);
    //    if (lastHitTime < Time.time && collision.gameObject.layer == 11) {
    //        Hit();
    //    }
    //}
    //
    //private void OnCollisionExit2D(Collision2D collision) {
    //    
    //}
}
