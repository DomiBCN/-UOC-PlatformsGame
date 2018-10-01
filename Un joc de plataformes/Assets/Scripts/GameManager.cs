using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    PlayerController player;
    [SerializeField]
    Text recordText;
    [SerializeField]
    Button startButton;
    [SerializeField]
    string level;

    int secondsToStart = 3;
    Text mainText;
    float initialTime;
    float bestTime;
    float finalTime;

    // Use this for initialization
    void Start()
    {
        player.eliminated += Restart;
        player.levelEnd += End;
        player.enabled = false;
        mainText = startButton.GetComponentInChildren<Text>();
        bestTime = GetBestTime(level);
        if (bestTime > 0) { recordText.text = "Record: " + bestTime.ToString("##.##") + " s"; } else { recordText.enabled = false; }
    }

    void Restart()
    {
        SceneManager.LoadScene(level);
    }

    public void StartGame()
    {
        startButton.enabled = false;
        mainText.text = "" + secondsToStart;
        InvokeRepeating("CountDown", 1, 1);
    }

    void CountDown()
    {
        secondsToStart--;
        if (secondsToStart <= 0) { CancelInvoke(); GameStarted(); } else { mainText.text = secondsToStart.ToString(); }
    }

    void GameStarted()
    {
        player.enabled = true;
        initialTime = Time.time;
        if (bestTime > 0) recordText.enabled = true;
    }

    private void FixedUpdate()
    {
        if (player.enabled)
        {
            mainText.text = "Tiempo: " + (Time.time - initialTime).ToString("##.##");
        }
    }

    void End()
    {
        player.enabled = false;
        finalTime = (Time.time - initialTime);
        mainText.text = "Final! " + finalTime;
        if (finalTime < bestTime || bestTime == 0) SetRecord(level, finalTime);
    }

    public float GetBestTime(string level)
    {
        return PlayerPrefs.GetFloat(level + "_best", 0);
    }

    public void SetRecord(string level, float record)
    {
        PlayerPrefs.SetFloat(level + "_best", record);
    }
}
