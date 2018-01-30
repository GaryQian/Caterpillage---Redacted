using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleSpawner : MonoBehaviour {

    public GameObject TentaclePrefab;
    public GameObject tentacle;
    public float minInterval;
    public float maxInterval;

    public float distThresh;

    public ArrayList Near;
	// Use this for initialization
	void Start () {
        if (!GameManager.wormStats.tentacles) {
            this.enabled = false;
        }
        else {
            Near = new ArrayList();
            Invoke("SpawnTentacle", Random.Range(minInterval, maxInterval));
        }

    }
	
    void SpawnTentacle() {
        if (!GameManager.wormStats.tentacles) return;
        //GameObject other = null;
        if (WorldManager.wormMotion.wormState != WormState.playing) {
            Near = new ArrayList();
        }
        if (!WorldManager.wormMotion.underground) {
            foreach (GameObject obj in Near) {
                if (obj != null && obj.layer == 12 && obj.tag == "Character" && obj.GetComponent<TentacleMarker>() == null) {
                    if (Vector3.Distance(transform.position, obj.transform.position) < distThresh) {
                        Vector2 offset = new Vector2(obj.transform.position.x - transform.position.x, obj.transform.position.y - transform.position.y);
                        RaycastHit2D hit = Physics2D.Raycast(transform.position, offset, 4f, 1 << 10);
                        if (hit.collider == null) {
                            //Debug.Log(Vector3.Distance(transform.position, obj.transform.position));
                            tentacle = Instantiate(TentaclePrefab);
                            tentacle.GetComponent<Tentacle>().parent = gameObject;
                            tentacle.GetComponent<Tentacle>().targetObj = obj;
                            obj.AddComponent<TentacleMarker>();
                            break;
                        }
                    }
                }
            }
        }
        Invoke("SpawnTentacle", Random.Range(minInterval, maxInterval));
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!GameManager.wormStats.tentacles) return;
        if (collision.gameObject.layer == 12) {
            Near.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (!GameManager.wormStats.tentacles) return;
        if (collision.gameObject.layer == 12) {
            Near.Remove(collision.gameObject);
        }
    }
}
