using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    
    [SerializeField]
    GameObject levelBtnPrefab;
    [SerializeField]
    GameObject levelBlockedBtnPrefab;
    [SerializeField]
    Transform levelsPanel;
    
    int levelReached;
    Image[] stars = new Image[3];
    float levelBestTime;
    int x = -170;
    float offsetXincrement = 0.32157525f;
    float offsetXmin = 0.08259365f;
    float offsetXmax = 0.2724749f;
    float offsetYmin = 0.225726f;
    float offsetYmax = 0.7635161f;

    List<RatingsManager.LevelTimmings> levelTimmings;
    
    private void Awake()
    {
        levelTimmings = RatingsManager.levelTimmings;
        if (levelTimmings == null || levelTimmings.Count == 0)
        {
            RatingsManager.FillLevels();
        }
        levelTimmings.OrderBy(l => l.LevelId);
        levelReached = PlayerPrefsPersister.GetLevelReached();

        //if(!Sound.instance.GetComponent<AudioSource>().enabled && PlayerPrefsPersister.GetAudioStatus())
        //{
        //    Sound.instance.GetComponent<AudioSource>().enabled = true;
        //}
    }

    private void Start()
    {
        AddLevels();
    }

    public void LoadLevel(int level)
    {
        PlayerPrefsPersister.SetCurrentLevel(level);
        SceneManager.LoadScene("LevelScene");
    }

    void AddLevels()
    {
        foreach (var level in levelTimmings)
        {
            GameObject levelButton = new GameObject();
            if (level.LevelId > levelReached)
            {
                levelButton = Instantiate(levelBlockedBtnPrefab);
                SetLevelButton(levelButton, false);
            }
            else
            {
                levelButton = Instantiate(levelBtnPrefab);
                SetLevelButton(levelButton, true, level);
            }
        }
    }

    void SetLevelButton(GameObject levelButton, bool isUnblocked, RatingsManager.LevelTimmings level = null)
    {
        levelButton.transform.SetParent(levelsPanel);

        //Setting position && anchors
        RectTransform levelButtonRect = levelButton.GetComponent<RectTransform>();

        levelButtonRect.localPosition = new Vector3(0, 0, 0);

        levelButtonRect.anchorMax = new Vector2(offsetXmax, offsetYmax);
        levelButtonRect.anchorMin = new Vector2(offsetXmin, offsetYmin);

        levelButtonRect.offsetMax = new Vector2(0, 0);
        levelButtonRect.offsetMin = new Vector2(0, 0);

        levelButtonRect.localScale = new Vector3(1, 1, 1);

        x += 170;
        offsetXmax += offsetXincrement;
        offsetXmin += offsetXincrement;

        if (isUnblocked)
        {
            FillListener(levelButton.GetComponentInChildren<Button>(), level.LevelId);
            levelButton.GetComponentInChildren<Text>().text = (level.LevelId + 1).ToString();

            levelBestTime = PlayerPrefsPersister.GetLevelBestTime(level.LevelId);
            stars = levelButton.GetComponentsInChildren<Image>().Where(s => s.tag == "Star").ToArray();

            //if 'levelBestTime' != 0(to check if at least has been completed 1 time) set the amount of stars the user has got
            if (levelBestTime != 0)
            {
                RatingsManager.SetLevelStars(level.Timmings, stars, levelBestTime);
            }
        }
        else
        {
            levelButton.GetComponent<Button>().interactable = false;
        }
    }
    
    void FillListener(Button button, int level)
    {
        button.onClick.AddListener(() =>
        {
            LoadLevel(level);
        });
    }

    public void BackToStart()
    {
        SceneManager.LoadScene("InitialScene");
    }
}
