using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Keeps values needed when reloading scene to next level
public static class GameValues
{
    public static int currentLVL = 1;
    public static int wholeScore = 0;
    public static int playerHealth = 3;

    public static int scoreModifier = 1;
    public static float spawnDelayModifier = 1;
    public static float objSpeedModifier = 1;

    public static bool restarted = false;

    public static void SetValuesForLvl(int lvl)
    {
        if (lvl == 1)
        {
            currentLVL = 1;
            wholeScore = 0;
            playerHealth = 3;

            scoreModifier = 1;
            spawnDelayModifier = 1;
            objSpeedModifier = 1;
        }
        else
        {
            currentLVL = lvl;
            scoreModifier = lvl;
            spawnDelayModifier = (spawnDelayModifier + currentLVL) / 2.5f;
            objSpeedModifier = (objSpeedModifier + currentLVL) / 2.5f;
        }
    }
}