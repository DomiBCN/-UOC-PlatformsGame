using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    GameObject startButton;
    [SerializeField]
    Text timerText;
    [SerializeField]
    Transform levelContainer;
    [SerializeField]
    List<GameObject> levels = new List<GameObject>();

    [SerializeField]
    GameObject pauseMenu;
    [SerializeField]
    GameObject finishMenu;

    Text mainText;
    int currentLevel;
    int secondsToStart = 3;
    float initialTime;
    float bestTime;
    float finalTime;
    float[] ratings = new float[3];
    Image[] stars = new Image[3];

    private void Awake()
    {
        currentLevel = PlayerPrefs.GetInt("CurrentLevel", 0);
        GameObject levelPrefab = Instantiate(levels[currentLevel]);
        levelPrefab.transform.SetParent(levelContainer);
        timerText.enabled = false;
    }

    // Use this for initialization
    void Start()
    {
        player.eliminated += Restart;
        player.levelEnd += End;
        player.enabled = false;
        mainText = startButton.GetComponentInChildren<Text>();
        bestTime = GetBestTime(currentLevel);
        if (bestTime > 0) { recordText.text = "Best time: " + bestTime.ToString("##.##") + " s"; } else { recordText.enabled = false; }
        //DEBUGGING
        //StartGame();
    }

    void Update()
    {
        if (player.enabled)
        {
            timerText.text = "Time: " + Math.Round((Time.time - initialTime), 2).ToString("##.##");
        }
    }

    public void StartGame()
    {
        startButton.GetComponent<Button>().enabled = false;
        mainText.text = "" + secondsToStart;
        //DEBUGGING
        //GameStarted();
        InvokeRepeating("CountDown", 1, 1);

    }

    void CountDown()
    {
        secondsToStart--;
        if (secondsToStart <= 0)
        {
            CancelInvoke();
            GameStarted();
            startButton.SetActive(false);
            recordText.enabled = false;
            timerText.enabled = true;
        }
        else
        {
            mainText.text = secondsToStart.ToString();
        }
    }

    void GameStarted()
    {
        player.enabled = true;
        initialTime = Time.time;
        if (bestTime > 0) recordText.enabled = true;
    }

    #region PopUp menus and actions

    #region PopUps
    public void Pause()
    {
        if (pauseMenu.activeInHierarchy)
        {
            pauseMenu.SetActive(false);
            SetTimeScale(1);
        }
        else
        {
            SetTimeScale(0);
            pauseMenu.SetActive(true);
        }
    }

    void End()
    {
        SetTimeScale(0);
        finalTime = (Time.time - initialTime);
        if (finalTime < bestTime || bestTime == 0) SetRecord(currentLevel, finalTime);

        stars = finishMenu.GetComponentsInChildren<Image>().Where(s => s.tag == "Star").ToArray();
        ratings = GetLevelRatings(currentLevel);

        //set level stars
        SetLevelStars(ratings, stars, finalTime);

        finishMenu.SetActive(true);
    }

    void SetLevelStars(float[] ratings, Image[] stars, float levelBestTime)
    {
        for (var j = 0; j < ratings.Length; j++)
        {
            if (levelBestTime <= ratings[j])
            {
                stars[j].enabled = true;
            }
        }
    }

    #endregion

    #region PopUp Actions
    public void Restart()
    {
        if (pauseMenu.activeInHierarchy || finishMenu.activeInHierarchy)
        {
            //If Restart from pause menu, we need to set timeScale to 1. It's value doesn't reset on loadScene
            SetTimeScale(1);
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToMenu()
    {
        //If we Exit from pause menu, we need to set timeScale to 1. It's value doesn't reset on loadScene
        SetTimeScale(1);
        SceneManager.LoadScene("LevelMenu");
    }

    public void Exit()
    {
        //If we Exit from pause menu, we need to set timeScale to 1. It's value doesn't reset on loadScene
        SetTimeScale(1);
        SceneManager.LoadScene("InitialScene");
    }

    public void Next()
    {
        //If we load the next level, we need to set timeScale to 1. It's value doesn't reset on loadScene
        SetTimeScale(1);
        PlayerPrefs.SetInt("CurrentLevel", currentLevel + 1);
        SceneManager.LoadScene("LevelScene");
    }
    #endregion
    #endregion

    //void End()
    //{
    //    SetTimeScale(0);
    //    finalTime = (Time.time - initialTime);
    //    mainText.text = "Final! " + Math.Round(finalTime, 2);
    //    if (finalTime < bestTime || bestTime == 0) SetRecord(currentLevel, finalTime);
    //}

    void SetTimeScale(int timeScale)
    {
        Time.timeScale = timeScale;
    }

    public float GetBestTime(int level)
    {
        return PlayerPrefs.GetFloat(level + "_best", 0);
    }

    public void SetRecord(int level, float record)
    {
        PlayerPrefs.SetFloat(level + "_best", record);
    }

    //ratings needed to get each star
    float[] GetLevelRatings(int level)
    {
        ratings = new float[3];
        for (int i = 0; i < 3; i++)
        {
            ratings[i] = PlayerPrefs.GetFloat(level + "_star" + i, 0);
        }
        return ratings;
    }
}
