using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollManager : MonoBehaviour {

    public UpgradeManager um;
    public GameObject PlanetPrefab;
    public Text PlanetName;
    public Text PlanetLabel;

    public static string currName;

    public string[] rings;

    public float separation = 0.3f;
    public float juice = 3;
    public float scrollSpeed = 2f;
    public Vector2 center;
    public float radius;
    public Vector2 secondarySlotPos;
    public float secondaryScale;

    public float pos = 0;
    public float targetPos = 0;
    float anchorPos = 0;
    public int index = 1;
    public bool dragging = false;
    bool updatePos = true;

    Hashtable planets;
    Hashtable names;
    GameObject planetMiddle;

    public AudioClip clickClip;

    public ScrollRect sr;
	// Use this for initialization
	void Start () {
        planets = new Hashtable();
        names = new Hashtable();
        //names.Add(100, "Earth");
        //names.Add(99, "Mars");
        index = GameManager.maxLevel;
        pos = index * separation - separation * 0.3f;
        targetPos = index * separation;


        float screenRatio = (float)Screen.height / Screen.width;
        secondarySlotPos = new Vector2(secondarySlotPos.x, secondarySlotPos.y / .56f * screenRatio);
        //planetMiddle = Instantiate(PlanetPrefab, Vector3.zero, Quaternion.identity, transform);

        InvokeRepeating("ClearMemory", 2f, 2f);
    }
	
	// Update is called once per frame
	void Update () {
		if (!dragging) {
            pos += (targetPos - pos) * juice * Time.deltaTime;
            if(updatePos) index = PosToIndex();
        }
        for (int i = index - 4; i <= index + 4; ++i) {
            if (i > 0) {
                if (!planets.ContainsKey(i)) {
                    SpawnPlanet(i);
                }
                MovePlanet((GameObject)planets[i], pos - (i - index) * separation - index * separation);
            }
        }
        PlanetName.text = (string)names[index];
        PlanetLabel.text = index / 10 < rings.Length ? rings[(int)(index / 10)] : "Capital System";
    }

    public void OnBeginDrag() {
        anchorPos = pos;
        dragging = true;
        OnDrag();
    }

    public void OnDrag() {
        pos = anchorPos - (sr.verticalNormalizedPosition - 0.5f) * scrollSpeed;
        index = PosToIndex();
    }

    public void OnEndDrag() {
        OnDrag();
        sr.verticalNormalizedPosition = 0.5f;
        targetPos = index * separation;
        dragging = false;
    }

    void MovePlanet(GameObject planet, float p) {
        float normP = p / separation;
        if (Mathf.Abs(normP) < 1f) {
            planet.transform.localPosition = new Vector3(secondarySlotPos.x * Mathf.Abs(normP), secondarySlotPos.y * normP, 0) * Util.xScale;
        }
        else {
            planet.transform.localPosition = new Vector3(secondarySlotPos.x * (Mathf.Abs(normP) - 1f) * 2f + secondarySlotPos.x, secondarySlotPos.y * normP, 0) * Util.xScale;
        }
        planet.transform.localPosition = planet.transform.localPosition + new Vector3(60f, -50f, 0) * Util.xScale;
        
        planet.transform.localScale = new Vector3(1f - (1f - secondaryScale) * Mathf.Abs(normP), 1f - (1f - secondaryScale) * Mathf.Abs(normP), 1);
    }

    void SpawnPlanet(int i) {
        if (i <= 0) return;
        //Debug.Log("Spawning " + i);
        GameObject p = Instantiate(PlanetPrefab, Vector3.zero, Quaternion.identity, transform);
        p.GetComponent<PlanetIcon>().level = i;
        //Debug.Log(GameManager.slotId);
        p.GetComponent<PlanetIcon>().stars = i > GameManager.maxLevel ? -1 : GameManager.slots.slots[GameManager.slotId].progress.stars[i];
        planets.Add(i, p);
        if (names.ContainsKey(i)) {
            names.Remove(i);
        }
        names.Add(i, PlanetNameGenerator.NewName(i));
    }

    public void OnValueChanged(Vector2 val) {
        //Debug.Log("ValueChanged: " + val.y * 1f);
    }

    int PosToIndex() {
        int ind = Mathf.Max(1, (int)((pos + separation / 2f) / separation));
        if (ind != index) {
            SoundManager.PlaySingleSfx(clickClip);
        }
        return ind;
    }

    void ClearCurrentPlanets() {
        for (int i = index - 4; i <= index + 4; ++i) {
            if (i > 0) {
                if (planets.ContainsKey(i)) MovePlanet((GameObject)planets[i], -3f);
            }
        }
    }

    void ClearMemory() {
        ICollection l = planets.Keys;
        List<int> ids = new List<int>();
        foreach (int i in l) {
            ids.Add(i);
        }
        foreach (int i in ids) {
            if (Mathf.Abs(i - index) > 30) {
                Destroy((GameObject)planets[i]);
                planets.Remove(i);
                names.Remove(i);
            }
        }
    }

    public void OnClick() {
        ClearCurrentPlanets();
        if (InputManager.holdScreenPos2.y < 350 * Util.yScale) {
            index = Mathf.Max(1, index + 1);
            
        }
        else if (InputManager.holdScreenPos2.y > 650 * Util.yScale)  {
            index = Mathf.Max(1, index - 1);
        }
        targetPos = index * separation;
        updatePos = false;
        CancelInvoke("ResetUpdatePos");
        Invoke("ResetUpdatePos", 0.2f);
    }

    void ResetUpdatePos() {
        updatePos = true;
    }

    public void Skip10Up() {
        ClearCurrentPlanets();
        int prevIndex = index;
        index = Mathf.Max(1, index - 10);
        targetPos = index * separation;
        updatePos = false;
        if (prevIndex > 3) {
            pos = (index + 3) * separation;

        }
        Update();
        CancelInvoke("ResetUpdatePos");
        Invoke("ResetUpdatePos", 0.2f);
    }

    public void Skip10Down() {
        ClearCurrentPlanets();
        index = index + 10;
        targetPos = index * separation;
        updatePos = false;
        pos = Mathf.Max(1, (index - 3)) * separation;
        Update();
        CancelInvoke("ResetUpdatePos");
        Invoke("ResetUpdatePos", 0.2f);
    }


//#[t/w]#_#.png
//TechLevel t/w Exoticness


    public void OnStartGameButtonPress() {
        if (GameManager.state == Gamestate.Playing) return;
        Debug.Log("Pressed Next Level");
        if (index > GameManager.maxLevel) {
            //Prevent starting a game past maxLevel
            GameManager.level = GameManager.maxLevel;
            targetPos = GameManager.maxLevel * separation;
            pos = Mathf.Min(GameManager.maxLevel + 2, index) * separation;
            juice *= 3f;
            ClearCurrentPlanets();
            Invoke("StartGame", .55f);
        }
        else {
            GameManager.level = index;
            StartGame();
        }
        currName = (string)names[GameManager.level];
    }

    void StartGame() {
        GameObject.Find("GameManager").GetComponent<GameManager>().StartGame((string)names[index]);

        um.Exit();
    }
}
