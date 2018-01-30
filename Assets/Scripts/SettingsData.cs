using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

[Serializable]
public class SettingsData {

    public bool soundOn;
    public bool musicOn;
    public bool leftHandedMode;
    public bool hasCheated;

    public bool rated;

    public bool adsRemoved;

    public SettingsData() {
        soundOn = true;
        musicOn = true;
        leftHandedMode = false;
        hasCheated = false;

        rated = false;

        adsRemoved = false;
    }



}
