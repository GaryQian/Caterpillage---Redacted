using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankManager : MonoBehaviour {

    public Sprite[] tanks;
    public Sprite[] barrels;

    public static Sprite[] SelectedTanks;
    public static Sprite[] SelectedBarrels;

    public void SelectTanks() {
        SelectedTanks = tanks;
        SelectedBarrels = barrels;
        //string carName = (Random.Range(1, 11)) + "_" + (Random.Range(1, 6));
        //for (int i = 0; i < tanks.Length; i++) {
        //    SelectedTanks.Add(tanks[i]);
        //}
    }
}
