using System;
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
    GameObject moveTutorial;
    [SerializeField]
    Transform levelContainer;
    [SerializeField]
    List<GameObject> levels = new List<GameObject>();

    [SerializeField]
    GameObject pauseMenu;
    [SerializeField]
    GameObject finishMenu;
    [SerializeField]
    Text finishMenuTime;
    [SerializeField]
    Text finishMenuRecord;
    [SerializeField]
    AudioSource audioWin;
    [SerializeField]
    Image[] stars = new Image[3];
    [SerializeField]
    Text[] starTexts = new Text[3];

    [SerializeField]
    Button NextBtn;

    Text mainText;
    int currentLevel;
    int secondsToStart = 3;
    float initialTime;
    float bestTime;
    float finalTime;
    

    List<RatingsManager.LevelTimmings> levelTimmings;
    RatingsManager.LevelTimmings currentLevelTimmings;

    private void Awake()
    {
        //Disable music
        if (Sound.instance != null)
        {
            Sound.instance.GetComponent<AudioSource>().enabled = false;
        }

        currentLevel = PlayerPrefsPersister.GetCurrentLevel();

        levelTimmings = RatingsManager.levelTimmings;
        if (levelTimmings == null || levelTimmings.Count == 0)
        {
            RatingsManager.FillLevels();
        }
        currentLevelTimmings = levelTimmings.Where(l => l.LevelId == currentLevel).FirstOrDefault();

        //add level prefab
        GameObject levelPrefab = Instantiate(levels[currentLevel]);
        levelPrefab.transform.SetParent(levelContainer);
        timerText.enabled = false;

        //if this is the last level -> hide "Next" button;
        if (currentLevel == levels.Count - 1)
        {
            NextBtn.GetComponent<Button>().interactable = false;
        }
    }

    // Use this for initialization
    void Start()
    {
        player.eliminated += Restart;
        player.levelEnd += End;
        player.enabled = false;
        mainText = startButton.GetComponentInChildren<Text>();
        bestTime = PlayerPrefsPersister.GetLevelBestTime(currentLevel);
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
            moveTutorial.SetActive(false);
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
        if (finalTime < bestTime || bestTime == 0)
        {
            PlayerPrefsPersister.SetRecord(currentLevel, finalTime);
            bestTime = finalTime;
        }
        
        //set level stars
        RatingsManager.SetLevelStars(currentLevelTimmings.Timmings, stars, finalTime, starTexts);
        finishMenuTime.text = finalTime.ToString("##.##") + "s";
        finishMenuRecord.text = bestTime.ToString("##.##") + "s";

        CheckUnblockLevel();

        audioWin.enabled = true;
        audioWin.Play();
        finishMenu.SetActive(true);
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
        PlayerPrefsPersister.SetCurrentLevel(currentLevel + 1);
        SceneManager.LoadScene("LevelScene");
    }
    #endregion
    #endregion

    //Checks if we need to unblock the next level(only if there ara more levels available && if we are playing the last level unblocked)
    void CheckUnblockLevel()
    {
        if (currentLevel <= PlayerPrefsPersister.GetLevelReached() && currentLevel < levels.Count - 1)
        {
            PlayerPrefsPersister.SetLevelReached(currentLevel + 1);
        }
    }

    void SetTimeScale(int timeScale)
    {
        Time.timeScale = timeScale;
    }
}
