using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPersistence : MonoBehaviour {

	public float GetBestTime(string level)
    {
        return PlayerPrefs.GetFloat(level + "_best", 0);
    }

    public void SetRecord(string level, float record)
    {
        PlayerPrefs.SetFloat(level + "_best", record);
    }
}
