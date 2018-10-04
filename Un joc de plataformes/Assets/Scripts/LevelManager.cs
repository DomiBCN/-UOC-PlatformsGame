using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{


    public int numberOfLevels = 3;


    [SerializeField]
    GameObject levelButtonPrefab;
    [SerializeField]
    Transform levelsPanel;

    Image[] stars = new Image[3];
    float levelBestTime;
    float offsetXincrement = 0.32157525f;
    float offsetYmin = 0.225726f;
    float offsetYmax = 0.7635161f;
    float[] ratings = new float[3];

    private void Start()
    {
        AddLevels();
    }

    public void LoadLevel(int level)
    {
        PlayerPrefs.SetInt("CurrentLevel", level);
        SceneManager.LoadScene("LevelScene");
    }

    void AddLevels()
    {
        int x = -170;
        float offsetXmin = 0.08259365f;
        float offsetXmax = 0.2724749f;

        for (int i = 0; i < numberOfLevels; i++)
        {
            GameObject levelButton = Instantiate(levelButtonPrefab);
            levelButton.transform.SetParent(levelsPanel);

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

            FillListener(levelButton.GetComponentInChildren<Button>(), i);
            levelButton.GetComponentInChildren<Text>().text = (i + 1).ToString();

            levelBestTime = GetLevelBestTime(i);
            stars = levelButton.GetComponentsInChildren<Image>().Where(s=> s.tag == "Star").ToArray();
            ratings = GetLevelRatings(i);

            //set level stars
            SetLevelStars(ratings, stars, levelBestTime);
            
        }
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

    float GetLevelBestTime(int level)
    {
        return PlayerPrefs.GetFloat(level + "_best", 0);
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
