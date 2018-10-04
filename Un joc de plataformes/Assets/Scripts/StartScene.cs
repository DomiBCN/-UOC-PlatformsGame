using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour {

    private void Awake()
    {
        //PlayerPrefs.DeleteAll();

        
        PlayerPrefs.SetInt("NumOfLevels", 3);

        //TIMMINGS NEEDED TO GET EACH STAR
        //Level1
        PlayerPrefs.SetFloat("0_star0", 20);
        PlayerPrefs.SetFloat("0_star1", 10);
        PlayerPrefs.SetFloat("0_star2", 5);

        //Level2
        PlayerPrefs.SetFloat("1_star0", 20);
        PlayerPrefs.SetFloat("1_star1", 10);
        PlayerPrefs.SetFloat("1_star2", 5);

        //Level3
        PlayerPrefs.SetFloat("2_star0", 20);
        PlayerPrefs.SetFloat("2_star1", 10);
        PlayerPrefs.SetFloat("2_star2", 5);
    }
    public void LoadLevelsMenu()
    {
        SceneManager.LoadScene("LevelMenu");
    }
}
