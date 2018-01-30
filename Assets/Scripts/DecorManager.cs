using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorManager : MonoBehaviour {
    public Sprite[] sprites;

    public static ArrayList selectedDecor;

    public void SelectDecor(int i = 0) {
        selectedDecor = new ArrayList();
        int elementsPerColor = 10;
        int color = Random.Range(0, sprites.Length / elementsPerColor);

        for (int j = color * elementsPerColor; j < (color + 1) * elementsPerColor; j++) {
            selectedDecor.Add(sprites[j]);
        }
    }
}
