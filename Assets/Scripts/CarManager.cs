using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarManager : MonoBehaviour {

    public Sprite[] cars;

    public static ArrayList SelectedCars;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SelectCar() {
        SelectedCars = new ArrayList();
        //string carName = (Random.Range(1, 11)) + "_" + (Random.Range(1, 6));
        for (int i = 0; i < 10; i++) {
            SelectedCars.Add(cars[Random.Range(0, cars.Length)]);
        }
    }
}
