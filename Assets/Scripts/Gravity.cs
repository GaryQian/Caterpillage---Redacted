using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour {
    //static int[] skip = { 32, 16, 10, 7, 3, 1 };
    static int[] mult = { 1, 3, 6, 7, 3, 1 };
    static float[] velocityThreshold = { 0.00005f, 0.0001f, 0.0004f, 0.001f, 0.002f, 0.003f };

    public float GravityStrength = 200f;
    public float Interval = 0.5f;

    public Rigidbody2D body;
    Vector2 centerOfMass;
    ushort activityLevel = 2;
    int counter;
    // Use this for initialization
    private void Start() {
        StartManual();
    }

    public void StartManual () {
        body = GetComponent<Rigidbody2D>();
        UpdateCenterOfMass();
        InvokeRepeating("UpdateCenterOfMass", Random.Range(0, Interval), Interval);

        GravityStrength = PhysicsManager.Gravity;

        counter = Random.Range(0, 100);
    }

    private void FixedUpdate() {
        if (counter % 1 == 0) {
            ApplyForce(1.0f);
        }
        /*if (counter % skip[activityLevel] == 0) {
            ApplyForce(skip[activityLevel]);
            CheckActivityLevel();
            Debug.Log(activityLevel);
        }*/

        counter++;
    }

    private void CheckActivityLevel() {
        float vel = body.velocity.sqrMagnitude;
        //Debug.Log(vel);
        for (ushort a = 5; a > 0; --a) {
            if (vel > velocityThreshold[a]) {
                activityLevel = a;
                body.freezeRotation = false;
                return;
            }
        }
        activityLevel = 0;
        //body.freezeRotation = true;
    }

    private void ApplyForce(float scale) {
        body.AddForce(centerOfMass * GravityStrength * body.mass / -50 * scale);
    }

    private void ApplyForce() {
        ApplyForce(1);
    }

    public void UpdateCenterOfMass() {
        centerOfMass = body.worldCenterOfMass;
    }
}
