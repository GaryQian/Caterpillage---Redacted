using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormGenerator : MonoBehaviour {

    public float scale;
    public int segmentCount;
    public float headSpacing;
    public float bodySpacing;
    public float tailSpacing;

    public GameObject bodyPrefab;
    public GameObject tailPrefab;

    public List<GameObject> bodySegments;

	// Use this for initialization
	void Start () {
        

        //GenerateWorm(segmentCount);

    }

    public void GenerateWorm(int segs) {
        GenerateWorm(segs, transform.position);
    }

    public void GenerateWorm() {
        GenerateWorm(GameManager.wormStats.size, transform.position);
    }

    void ClearOldSegs() {
        if (bodySegments == null) return;
        foreach (GameObject seg in bodySegments) {
            if (seg != null) {
                Destroy(seg);
            }
        }
    }
	
    public void GenerateWorm(int segs, Vector3 startPosition) {
        ClearOldSegs();
        bodySegments = new List<GameObject>();
        transform.localScale = new Vector3(scale, scale, 1);
        transform.position = startPosition;

        GameObject prevSeg = this.gameObject;
        float xoff;
        HingeJoint2D joint;
        GameObject body;
        
        for (int i = 0; i < segs; ++i) {
            
            xoff = headSpacing + i * bodySpacing;
            body = Instantiate(bodyPrefab);
            bodySegments.Add(body);

            body.transform.localScale = new Vector3(scale, scale, 1);
            body.transform.position = new Vector3(-xoff, 0, 1f / (segs - i + 1)) + startPosition;

            joint = prevSeg.GetComponent<HingeJoint2D>();

            if (i == 0) {
                joint.connectedBody = body.GetComponent<Rigidbody2D>();
                joint.anchor = new Vector2(0, 0);
                joint.connectedAnchor = new Vector2(0.2857143f / 0.35f * scale, 0);
            }
            else {
                joint.connectedBody = body.GetComponent<Rigidbody2D>();
                joint.anchor = new Vector2(-0.2857143f / 0.35f * scale, 0);
                joint.connectedAnchor = new Vector2(0.2857143f / 0.35f * scale, 0);
            }

            if (segs <= 10) {
                //body.GetComponent<Rigidbody2D>().drag = 1f + (10 - segs) * 0.2f;
            }

            prevSeg = body;
            
        }

        xoff = headSpacing + (segs - 1) * bodySpacing + tailSpacing;
        GameObject tail = Instantiate(tailPrefab);

        tail.transform.localScale = new Vector3(scale, scale, 1);
        tail.transform.position = new Vector3(-xoff, 0, 0.99f) + startPosition;
        bodySegments.Add(tail);

        joint = prevSeg.GetComponent<HingeJoint2D>();

        if (segs == 0) {
            joint.connectedBody = tail.GetComponent<Rigidbody2D>();
            joint.anchor = new Vector2(0.01f / .35f * scale, 0);
            joint.connectedAnchor = new Vector2(0.01f, 0);
        }
        else {
            joint.connectedBody = tail.GetComponent<Rigidbody2D>();
            joint.anchor = new Vector2(-0.2857143f / .35f * scale, 0);
            joint.connectedAnchor = new Vector2(0.1f / .35f * scale, 0);
        }
        

    }
}
