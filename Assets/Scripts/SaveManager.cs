using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class SaveManager : MonoBehaviour {


    void Awake() {
        
    }

    // Use this for initialization
    void Start() {

    }

    public SlotData LoadSlot(int slotNumber) {
        string path = "/slot" + slotNumber + ".dat";
        if (File.Exists(Application.persistentDataPath + path)) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + path, FileMode.Open);
            SlotData data = null;
            try {
                data = (SlotData)bf.Deserialize(file);
            }
            catch (Exception e) {
                Debug.LogError("Failed to load SlotData: " + e.Message);
            }
            file.Close();
            
            return data;
        }
        return new SlotData(slotNumber);
    }

    public void SaveCurrentSlot() {
        GameManager.SyncGMToSlot();
        int slotNumber = GameManager.slotId;
        string path = "/slot" + slotNumber + ".dat";
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + path);

        bf.Serialize(file, GameManager.slots.slots[slotNumber]);
        file.Close();

        Debug.Log("Saved Slot successfully");
    }

    public void SaveSlot(int s) {
        int slotNumber = s;
        string path = "/slot" + slotNumber + ".dat";
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + path);

        bf.Serialize(file, GameManager.slots.slots[slotNumber]);
        file.Close();

        Debug.Log("Saved Slot successfully");
    }

    public void SaveVersion() {
        
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/version.dat");

        bf.Serialize(file, new Version());
        file.Close();
    }

    public int GetVersion() {
        if (File.Exists(Application.persistentDataPath + "/version.dat")) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/version.dat", FileMode.Open);
            Version data = null;
            try {
                data = (Version)bf.Deserialize(file);
            }
            catch (Exception e) {
                Debug.LogError("Failed to load Version: " + e.Message);
            }
            file.Close();

            return data.version;
        }
        else {
            SaveVersion();
            return new Version().version;
        }
    }

    public void SaveSettings() {

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/settings.dat");

        bf.Serialize(file, GameManager.settings);
        file.Close();
    }

    public SettingsData GetSettings() {
        if (File.Exists(Application.persistentDataPath + "/settings.dat")) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/settings.dat", FileMode.Open);
            SettingsData data = null;
            try {
                data = (SettingsData)bf.Deserialize(file);
            }
            catch (Exception e) {
                Debug.LogError("Failed to load Settings: " + e.Message);
                return new SettingsData();
            }
            file.Close();

            return data;
        }
        else {
            return new SettingsData();
        }
    }
}


[Serializable]
public class Slots {
    public SlotData[] slots;

    public Slots() {
        slots = new SlotData[4];
        slots[1] = new SlotData(1);
        slots[2] = new SlotData(2);
        slots[3] = new SlotData(3);
    }
}

[Serializable]
public class SlotData {
    public bool isEmpty;
    public WormStats wormStats;
    public Progress progress;
    public TotalStats totalStats;
    public int slotID;

    public DateTime lastVideoWatchTime;
    public int videosRemaining;

    public DateTime lastRewardClaimedTime;
    public int streak;

    public DateTime rateTime;
    public int rateCount;

    public object obj1;
    public object obj2;
    public object obj3;
    public object obj4;
    public object obj5;
    public object obj6;

    public SlotData(int id) {
        isEmpty = true;
        wormStats = new WormStats();
        progress = new Progress();
        totalStats = new TotalStats();

        slotID = id;

        lastVideoWatchTime = DateTime.MinValue;
        videosRemaining = 3;

        lastRewardClaimedTime = DateTime.MinValue;
        streak = 0;

        rateTime = DateTime.MinValue;
        rateCount = 0;

        obj1 = null;
        obj2 = null;
        obj3 = null;
        obj4 = null;
        obj5 = null;
        obj6 = null;

    }
}

//DUPLICATE OF SETTINGSDATA. DO NOT USE THIS.
[Serializable]
public class Settings {
    public bool musicMuted;
    public bool soundMuted;
    public bool rightHand;
    public bool hasCheated;

    public Settings() {
        musicMuted = false;
        soundMuted = false;
        rightHand = true;
        hasCheated = false;
    }

}

[Serializable]
public class Purchases {
    public bool[] purchased;
}

[Serializable]
public class Version {
    public int version;

    public Version() {
        version = 1;
    }
}