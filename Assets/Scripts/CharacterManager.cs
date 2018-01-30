using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour {

    public RuntimeAnimatorController thin;
    public RuntimeAnimatorController med;
    public RuntimeAnimatorController fat;

    public int headCount;

    public static RuntimeAnimatorController harmlessBody;
    public static Sprite harmlessHead;
    public static Color headColor;
    public static Color bodyColor;

    public Sprite[] guns;
    public static Sprite selectedGun;
	
    public void SelectCharacter() {
        switch ((int)Random.Range(0, 3)) {
            case 0: harmlessBody = thin; break;
            case 1: harmlessBody = med; break;
            case 2: harmlessBody = fat; break;
        }
        string headName = "harmlessHead" + (Random.Range(1, headCount + 1));
        //Debug.Log(headName);
        harmlessHead = Resources.Load<Sprite>(headName);

        bodyColor = new Color(Random.Range(.4f, 1f), Random.Range(.4f, 1f), Random.Range(.4f, 1f));
        headColor = bodyColor;

        selectedGun = guns[Random.Range(0, guns.Length)];
    }
}
