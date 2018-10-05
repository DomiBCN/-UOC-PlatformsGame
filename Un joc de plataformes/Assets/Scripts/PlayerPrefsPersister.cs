using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsPersister
{
    public static int GetCurrentLevel()
    {
        return PlayerPrefs.GetInt("CurrentLevel", 0);
    }

    public static float GetLevelBestTime(int level)
    {
        return PlayerPrefs.GetFloat(level + "_best", 0);
    }

    public static int GetLevelReached()
    {
        return PlayerPrefs.GetInt("LevelReached", 0);
    }

    public static void SetCurrentLevel(int level)
    {
        PlayerPrefs.SetInt("CurrentLevel", level);
    }

    public static void SetLevelReached(int level)
    {
        PlayerPrefs.SetInt("LevelReached", level);
    }

    public static void SetRecord(int level, float record)
    {
        PlayerPrefs.SetFloat(level + "_best", record);
    }
}
