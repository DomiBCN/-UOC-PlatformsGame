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

    public void LoadLevel(string levelName)
    {

        SceneManager.LoadScene(levelName);
    }

    private void AddLevels()
    {
        int x = -170;
        float offsetXmin = 0.08259365f;
        float offsetXmax = 0.2724749f;
        string levelName = string.Empty;

        for (int i = 1; i <= numberOfLevels; i++)
        {
            levelName = "Level" + i;

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

            FillListener(levelButton.GetComponentInChildren<Button>(), levelName);
            txtComponents = levelButton.GetComponentsInChildren<Text>();
            foreach (var component in txtComponents)
            {
                if (component.name == "Level")
                {
                    component.text = i.ToString();
                }
                else
                {
                    component.text += GetLevelBestTime(levelName).ToString();
                }
            }
        }
    }

    string GetLevelBestTime(string level)
    {
        float score = PlayerPrefs.GetFloat(level + "_best", 0);
        return score == 0 ? "-" : score.ToString("##.##");
    }

    void FillListener(Button button, string levelName)
    {
        button.onClick.AddListener(() =>
        {
            LoadLevel(levelName);
        });
    }
}
