using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour {

    public GameObject holder;
    public SpriteRenderer sr;
    public CircleCollider2D col;

    float initRad;

    public float fadeSpeed;

    public float targHeight;
    public float prevHeight;
    public float currHeight;

    public float hoverHeight;

    public float moveDuration;
    float startTime = 0;

    public float routineDelay;

    Coroutine routine;

    public GameObject PoofPrefab;
    public AudioClip poofClip;

    public Sprite[] sprites;
    public Sprite[] poofSprites;


	// Use this for initialization
	void Start() {
        //initRad = col.radius;

        GetComponent<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Length)];

        Invoke("Init", 0.01f);
        ApplyScaling(WorldManager.stats.currentPlanetStats.radius * 5f);

        InvokeRepeating("BeginNewPosition", Random.Range(.01f, routineDelay), routineDelay);
    }

    void Init() {
        //CalcTargetHeight();
        targHeight = WorldManager.stats.currentPlanetStats.radius * 2f - .3f;

        prevHeight = targHeight;

        currHeight = targHeight;

        startTime = Time.time;
        ApplyScaling();
    }
	
    void BeginNewPosition() {
        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(NewPosition());
    }


	IEnumerator NewPosition() {
        CalcTargetHeight();

        startTime = Time.time;

        while (Time.time - startTime < moveDuration) {
            currHeight = Mathf.SmoothStep(prevHeight, targHeight, (Time.time - startTime) / moveDuration);
            ApplyScaling();
            yield return null;
        }

        yield return null;

    }

    void ApplyScaling(float f = -1f) {
        if (f < 0) f = currHeight;
        holder.transform.localScale = new Vector3(holder.transform.localScale.x, currHeight + hoverHeight, 1);
        //col.radius = initRad / ((currHeight + hoverHeight) / 4f);
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.x / (currHeight + hoverHeight), 1);
    }

    void CalcTargetHeight() {
        prevHeight = currHeight;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.position * -1f, WorldManager.stats.currentPlanetStats.radius * 2f, 1 << 10);

        targHeight = hit.collider == null ? targHeight :  hit.point.magnitude;
    }

    IEnumerator Die() {
        while (sr.color.a >= 0.07f) {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, Mathf.Max(0, sr.color.a - fadeSpeed * Time.deltaTime));
            yield return null;
        }
        Delete();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.layer == 11 && collision.gameObject.name != "Aura") {
            StartCoroutine(Die());
            int poofs = Random.Range(3, 5);
            for (int i = 0; i < poofs; i++) {
                GameObject poof = Instantiate(PoofPrefab, collision.gameObject.transform.position + new Vector3(Random.Range(-.1f, 0.1f), Random.Range(-.1f, 0.1f), -1f), Quaternion.Euler(0, 0, Random.Range(0, 360)));
                poof.GetComponent<Rigidbody2D>().velocity = (Vector2)transform.position.normalized * 5f + new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * 4f;//collision.gameObject.GetComponent<Rigidbody2D>().velocity * 3f + new Vector2(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f)) * 3f;
                poof.GetComponent<SpriteRenderer>().sprite = poofSprites[Random.Range(0, poofSprites.Length)];
            }
            SoundManager.PlaySfx(poofClip);
            Invoke("Delete", 1f);
        }
    }

    void Delete() {
        Destroy(holder);
        Destroy(gameObject);
    }
}
