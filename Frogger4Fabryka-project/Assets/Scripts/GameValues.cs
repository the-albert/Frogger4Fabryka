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
    public static float vehicleSpawnDelayModifier = 1f;
    public static float platformSpawnDelayModifier = 1f;
    public static float vehicleSpeedModifier = 1f;
    public static float platformSpeedModifier = 1f;

    public static bool restarted = false;

    public static void SetValuesForLvl(int lvl)
    {
        if (lvl == 1)
        {
            currentLVL = 1;
            wholeScore = 0;
            playerHealth = 3;

            scoreModifier = 1;
            vehicleSpawnDelayModifier = 1f;
            platformSpawnDelayModifier = 1f;
            vehicleSpeedModifier = 1f;
            platformSpeedModifier = 1f;
        }
        else
        {
            currentLVL = lvl;
            scoreModifier = lvl;
            
            vehicleSpawnDelayModifier = 1 + currentLVL / 2f;
            platformSpawnDelayModifier = 1 + currentLVL / 10f;
            
            vehicleSpeedModifier = 1 + currentLVL / 3f;
            platformSpeedModifier = 1 + currentLVL / 10f;
        }
    }
}