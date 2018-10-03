using System.Collections;
using System.Collections.Generic;
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

    Text[] txtComponents;
    float offsetXincrement = 0.32157525f;
    float offsetYmin = 0.225726f;
    float offsetYmax = 0.7635161f;

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
            txtComponents = levelButton.GetComponentsInChildren<Text>();
            foreach (var component in txtComponents)
            {
                if (component.name == "Level")
                {
                    component.text = (i+1).ToString();
                }
                else
                {
                    component.text += GetLevelBestTime(i).ToString();
                }
            }
        }
    }

    string GetLevelBestTime(int level)
    {
        float score = PlayerPrefs.GetFloat(level + "_best", 0);
        return score == 0 ? "-" : score.ToString("##.##");
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
