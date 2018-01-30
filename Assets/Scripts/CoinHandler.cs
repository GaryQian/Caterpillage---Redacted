using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinHandler : MonoBehaviour {

    public float coins;
    float diffMult;
    float streakMult;
    public static float stagedCoins;
	
	public void ResetCoinCounter() {
        coins = 0;
        Util.wm.bm.SetCoins(coins);

    }

    public float DifficultyMultiplier(int level) {
        float val = 1f + 0.1f * level;
        int falloffLevels = 5;
        val -= 0.05f * ((GameManager.maxLevel - falloffLevels) - Mathf.Min(level, GameManager.maxLevel - falloffLevels));
        val = Mathf.Max(val, 1f);
        diffMult = 1f + 0.1f * level;
        return diffMult;
    }

    public float StreakMultiplier(int streak) {
        streakMult = 1f + 0.2f * Mathf.Min(streak, 5);
        return streakMult;
    }

    public void CommitCoins(int level, int streak) {
        float val = coins * DifficultyMultiplier(level) * StreakMultiplier(streak);
        GameManager.money += val;
        WorldManager.stats.SetCoins(coins, val);
        ResetCoinCounter();
    }

    public void EarlyCommit(int level) {
        float val = coins * DifficultyMultiplier(level);
        GameManager.money += val;
        WorldManager.stats.SetCoins(coins, val);
        ResetCoinCounter();
    }

    public void AddCoins(float c) {
        coins += c;
        stagedCoins = coins * streakMult * diffMult;
        Util.wm.bm.SetCoins(stagedCoins);

    } 

    public void EatHarmless() {
        AddCoins(1);
    }

    public void EatSoldier() {
        AddCoins(2);
    }
    public void EatCar() {
        AddCoins(2);
    }

    public void EatHelicopter() {
        AddCoins(5);
    }

    public void EatSatellite() {
        AddCoins(10);
    }

    public void EatTank() {
        AddCoins(20);
    }

    public void EatGeneral() {
        AddCoins(40);
    }
}
