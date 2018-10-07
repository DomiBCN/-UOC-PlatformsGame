using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RatingsManager {

    static Image star;
    static Text starText;

	public class LevelTimmings
    {
        public int LevelId;
        public List<StarTimming> Timmings;
    }

    public class StarTimming
    {
        public int StarNumber;
        public float Timming;
    }

    public static List<LevelTimmings> levelTimmings = new List<LevelTimmings>();

    public static void FillLevels()
    {
        levelTimmings.Add(new LevelTimmings()
        {
            LevelId = 0,
            Timmings = new List<StarTimming>()
            {
                new StarTimming(){ StarNumber = 1, Timming = 25},
                new StarTimming(){ StarNumber = 2, Timming = 18},
                new StarTimming(){ StarNumber = 3, Timming = 13}
            }
        });

        levelTimmings.Add(new LevelTimmings()
        {
            LevelId = 1,
            Timmings = new List<StarTimming>()
            {
                new StarTimming(){ StarNumber = 1, Timming = 20},
                new StarTimming(){ StarNumber = 2, Timming = 15},
                new StarTimming(){ StarNumber = 3, Timming = 10}
            }
        });

        levelTimmings.Add(new LevelTimmings()
        {
            LevelId = 2,
            Timmings = new List<StarTimming>()
            {
                new StarTimming(){ StarNumber = 1, Timming = 22},
                new StarTimming(){ StarNumber = 2, Timming = 14},
                new StarTimming(){ StarNumber = 3, Timming = 10}
            }
        });
    }

    public static void SetLevelStars(List<StarTimming> timmings, Image[] stars, float levelTime)
    {
        foreach (var timming in timmings)
        {
            if (levelTime <= timming.Timming)
            {
                star = stars.Where(s => s.name == "Star" + timming.StarNumber).FirstOrDefault();
                if (star != null)
                {
                    star.enabled = true;
                }
            }
        }
    }

    public static void SetLevelStars(List<StarTimming> timmings, Image[] stars, float levelTime, Text[] starsText)
    {
        foreach (var timming in timmings)
        {
            if (levelTime <= timming.Timming)
            {
                star = stars.Where(s => s.name == "Star" + timming.StarNumber).FirstOrDefault();
                if (star != null)
                {
                    star.enabled = true;
                }
            }
            else
            {
                starText = starsText.Where(t => t.name == "StarText" + timming.StarNumber).FirstOrDefault();

                if(starText != null)
                {
                    starText.text = timming.Timming.ToString("##.##") + "s";
                    starText.enabled = true;
                }
            }
        }
    }
}
