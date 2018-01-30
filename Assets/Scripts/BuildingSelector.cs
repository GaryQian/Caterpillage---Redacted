using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSelector : MonoBehaviour {

    public Texture2D[] tallTexLibrary;
    public Texture2D[] wideTexLibrary;

    public ArrayList selectedTallTextures;
    public ArrayList selectedWideTextures;

	
	public void SelectBuildings(int i = 0) {
        selectedTallTextures = new ArrayList();
        selectedWideTextures = new ArrayList();

        int radius = 4;
        int center = Random.Range(radius, tallTexLibrary.Length - radius);

        for (int j = center - radius; j <= center + radius; j++) {
            selectedTallTextures.Add(tallTexLibrary[j]);
            selectedWideTextures.Add(wideTexLibrary[j]);
        }

        //selectedTallTextures = new ArrayList(tallTexLibrary);
        //selectedWideTextures = new ArrayList(wideTexLibrary);


    }


    public void SetBuildingTex(GameObject obj, bool tall = true) {
        if (tall) {
            obj.GetComponent<Destructible2D.D2dDestructible>().MainTex = (Texture2D)selectedTallTextures[Random.Range(0, selectedTallTextures.Count)];
            //obj.GetComponent<Destructible2D.D2dDestructible>().ReplaceAlphaWith((Texture2D)obj.GetComponent<Destructible2D.D2dDestructible>().MainTex);
        }
        else {
            obj.GetComponent<Destructible2D.D2dDestructible>().MainTex = (Texture2D)selectedWideTextures[Random.Range(0, selectedWideTextures.Count)];
            //obj.GetComponent<Destructible2D.D2dDestructible>().ReplaceAlphaWith((Texture2D)obj.GetComponent<Destructible2D.D2dDestructible>().MainTex);
        }
    }

}
