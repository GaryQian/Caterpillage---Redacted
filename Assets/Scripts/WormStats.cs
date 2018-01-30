using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public enum SpitterFeatures { explosiveSpit, tripleSpit, acidSpit, quintupleSpit, none };
public enum WingFeatures { flap, dive, doubleFlap, butterfly, none };
public enum ShellFeatures { thorns, poison, reflective, tentacles, none };

[Serializable]
public class WormStats {

    public int  size;
    public readonly int maxSize = 10;
    public readonly float maxHealth = 1300;
    public readonly float startHealth = 300;
    public readonly float healthIncrement = 100;

    public int  speed;
    public readonly int maxSpeed = 20;
    public readonly float maxDisplaySpeed = 450;
    public readonly float startDisplaySpeed = 250;
    public readonly float displaySpeedIncrement = 10;

    public float prestigeMultiplier;

    public bool hasWings;
    public bool hasSpit;
    public bool hasArmor;

    public int  wingsLevel;
    public int  spitLevel;
    public int  armorLevel;

    public readonly int maxSpitLevel = 20;
    public readonly float maxSpitDamage = 50;
    public readonly float startSpitDamage = 10;
    public readonly float spitDamageIncrement = 2;

    public readonly int maxWingsLevel = 20;
    public readonly float maxWingStrength = 80; 
    public readonly float startWingStrength = 30;
    public readonly float wingStrengthIncrement = 2.5f;
    public readonly float butterflyStrength = 30f;

    public readonly int maxArmorLevel = 20;
    public readonly float maxDamageReduction = 60;
    public readonly float startDamageReduction = 10;
    public readonly float damageReductionIncrement = 2.5f;

    public bool explosiveSpit;
    public bool tripleSpit;
    public bool acidSpit;
    public bool quintupleSpit;

    public bool dive;
    public bool flap;
    public bool doubleFlap;
    public bool butterfly;

    public bool thorns;
    public bool poison;
    public bool reflective;
    public bool tentacles;

    
    public float GetSpitterPercent() {
        int count = 0;
        if (explosiveSpit) count++;
        if (tripleSpit) count++;
        if (acidSpit) count++;
        if (quintupleSpit) count++;
        return count/4f;
    }

    public float GetWingsPercent() {
        int count = 0;
        if (dive) count++;
        if (flap) count++;
        if (doubleFlap) count++;
        if (butterfly) count++;
        return count / 4f;
    }

    public float GetShellPercent() {
        int count = 0;
        if (thorns) count++;
        if (poison) count++;
        if (reflective) count++;
        if (tentacles) count++;
        return count / 4f;
    }

    public SpitterFeatures NextSpitterFeature() {
        if (!explosiveSpit) return SpitterFeatures.explosiveSpit;
        if (!tripleSpit) return SpitterFeatures.tripleSpit;
        if (!acidSpit) return SpitterFeatures.acidSpit;
        if (!quintupleSpit) return SpitterFeatures.quintupleSpit;
        return SpitterFeatures.none;
    }

    public WingFeatures NextWingFeature() {
        if (!dive) return WingFeatures.dive;
        if (!flap) return WingFeatures.flap;
        if (!doubleFlap) return WingFeatures.doubleFlap;
        if (!butterfly) return WingFeatures.butterfly;
        return WingFeatures.none;
    }

    public ShellFeatures NextShellFeature() {
        if (!thorns) return ShellFeatures.thorns;
        if (!poison) return ShellFeatures.poison;
        if (!reflective) return ShellFeatures.reflective;
        if (!tentacles) return ShellFeatures.tentacles;
        return ShellFeatures.none;
    }

    public int NextSpitterID() {
        switch (NextSpitterFeature()) {
            case SpitterFeatures.explosiveSpit:
                return 0;
            case SpitterFeatures.tripleSpit:
                return 1;
            case SpitterFeatures.acidSpit:
                return 2;
            case SpitterFeatures.quintupleSpit:
                return 3;
            case SpitterFeatures.none:
                return -1;
        }
        return -1;
    }

    public int NextWingID() {
        switch (NextWingFeature()) {
            case WingFeatures.flap:
                return 0;
            case WingFeatures.dive:
                return 1;
            case WingFeatures.doubleFlap:
                return 2;
            case WingFeatures.butterfly:
                return 3;
            case WingFeatures.none:
                return -1;
        }
        return -1;
    }

    public int NextShellID() {
        switch (NextShellFeature()) {
            case ShellFeatures.thorns:
                return 0;
            case ShellFeatures.poison:
                return 1;
            case ShellFeatures.reflective:
                return 2;
            case ShellFeatures.tentacles:
                return 3;
            case ShellFeatures.none:
                return -1;
        }
        return -1;
    }

    public void EvolveFeature(categories category, bool isBought = true) {
        switch (category) {
            case categories.spitter:
                hasSpit = isBought;
                break;
            case categories.wings:
                hasWings = isBought;
                break;
            case categories.shell:
                hasArmor = isBought;
                break;
        }
    }

    public void SpitterFeatureModify(SpitterFeatures feature, bool isBought = true) {
        switch (feature) {
            case SpitterFeatures.explosiveSpit: explosiveSpit = isBought; break;
            case SpitterFeatures.tripleSpit: tripleSpit = isBought; break;
            case SpitterFeatures.acidSpit: acidSpit = isBought; break;
            case SpitterFeatures.quintupleSpit: quintupleSpit = isBought; break;
        }
    }

    public void WingFeatureModify(WingFeatures feature, bool isBought) {
        switch (feature) {
            case WingFeatures.dive: dive = isBought; break;
            case WingFeatures.flap: flap = isBought; break;
            case WingFeatures.doubleFlap: doubleFlap = isBought; break;
            case WingFeatures.butterfly: butterfly = isBought; break;
        }
    }

    public void ShellFeatureModify(ShellFeatures feature, bool isBought) {
        switch (feature) {
            case ShellFeatures.thorns: thorns = isBought; break;
            case ShellFeatures.poison: poison = isBought; break;
            case ShellFeatures.reflective: reflective = isBought; break;
            case ShellFeatures.tentacles: tentacles = isBought; break;
        }
    }

    public WormStats() {
        this.size = 0;
        this.speed = 0;

        this.hasWings = false;
        this.hasSpit = false;
        this.hasArmor = false;

        this.wingsLevel = 0;
        this.spitLevel = 0;
        this.armorLevel = 0;

        this.prestigeMultiplier = 1f;

        this.explosiveSpit = false;
        this.tripleSpit = false;
        this.acidSpit = false;
        this.quintupleSpit = false;

        this.flap = false;
        this.dive = false;
        this.doubleFlap = false;
        this.butterfly = false;

        this.thorns = false;
        this.poison = false;
        this.reflective = false;
        this.tentacles = false;
    }

    public WormStats(
        int size,
        int speed,

        bool hasWings,
        bool hasSpit,
        bool hasArmor,

        int wingsLevel,
        int spitLevel,
        int armorLevel,

        bool explosiveSpit,
        bool tripleSpit,
        bool acidSpit,
        bool quintupleSpit,

        bool flap,
        bool dive,
        bool doubleFlap,
        bool butterfly,

        bool thorns,
        bool poison,
        bool reflective,
        bool tentacles
        ) {

        this.size =          size;
        this.speed =         speed;
 
        this.hasWings =      hasWings;
        this.hasSpit =       hasSpit;
        this.hasArmor =      hasArmor;

        this.wingsLevel =    wingsLevel;
        this.spitLevel =     spitLevel;
        this.armorLevel =    armorLevel;

        this.prestigeMultiplier = prestigeMultiplier;

        this.explosiveSpit = explosiveSpit;
        this.tripleSpit =    tripleSpit;
        this.acidSpit =      acidSpit;
        this.quintupleSpit = quintupleSpit;

        this.flap =          flap;
        this.dive =          dive;
        this.doubleFlap =    doubleFlap;
        this.butterfly =     butterfly;

        this.thorns =        thorns;
        this.poison =        poison;
        this.reflective =    reflective;
        this.tentacles =     tentacles;

    }

    public WormStats(WormStats other) {

        this.size =          other.size;
        this.speed =         other.speed;

        this.hasWings =      other.hasWings;
        this.hasSpit =       other.hasSpit;
        this.hasArmor =      other.hasArmor;

        this.wingsLevel =    other.wingsLevel;
        this.spitLevel =     other.spitLevel;
        this.armorLevel =    other.armorLevel;

        this.prestigeMultiplier = prestigeMultiplier;

        this.explosiveSpit = other.explosiveSpit;
        this.tripleSpit =    other.tripleSpit;
        this.acidSpit =      other.acidSpit;
        this.quintupleSpit = other.quintupleSpit;

        this.flap =          other.flap;
        this.dive =          other.dive;
        this.doubleFlap =    other.doubleFlap;
        this.butterfly =     other.butterfly;

        this.thorns =        other.thorns;
        this.poison =        other.poison;
        this.reflective =    other.reflective;
        this.tentacles =     other.tentacles;

    }

}
