using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarCalculator : MonoBehaviour {

    public static float GetThreeThresh(int level, bool gotGem = false) {
        float[] threeStarScale = { 12, 10, 10, 12, 16, 25, 35, 55 };

        float scaledT;
        float prevF;
        float nextF;
        float newt;

        float t = level / 100f;
        scaledT = Mathf.Max(-0.999f, t * (threeStarScale.Length - 1));
        prevF = t < 1f ? threeStarScale[(int)scaledT] : threeStarScale[threeStarScale.Length - 2];
        nextF = t < 1f ? threeStarScale[(int)(scaledT + 1f)] : threeStarScale[threeStarScale.Length - 1];
        newt = t < 1f ? scaledT - ((int)scaledT) : scaledT - (threeStarScale.Length - 2);
        return Mathf.LerpUnclamped(prevF, nextF, newt) + (gotGem ? 1f : 0);
    }

    public static float GetTwoThresh(int level, bool gotGem = false) {
        float[] twoStarScale = { 25, 20, 20, 22, 25, 36, 50, 80 };

        float scaledT;
        float prevF;
        float nextF;
        float newt;

        float t = level / 100f;
        scaledT = Mathf.Max(-0.999f, t * (twoStarScale.Length - 1));
        prevF = t < 1f ? twoStarScale[(int)scaledT] : twoStarScale[twoStarScale.Length - 2];
        nextF = t < 1f ? twoStarScale[(int)(scaledT + 1f)] : twoStarScale[twoStarScale.Length - 1];
        newt = t < 1f ? scaledT - ((int)scaledT) : scaledT - (twoStarScale.Length - 2);
        return Mathf.LerpUnclamped(prevF, nextF, newt) + (gotGem ? 1f : 0);
    }


    public static int CalculateStars(int level, float time, bool gotGem = false) {
        if (time < GetThreeThresh(level, gotGem)) return 3;
        if (time < GetTwoThresh(level, gotGem)) return 2;
        return 1;
    }
}
