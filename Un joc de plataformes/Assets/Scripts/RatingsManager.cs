using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RatingsManager {

    static Image star;

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
        levelTimmings.Add(new RatingsManager.LevelTimmings()
        {
            LevelId = 0,
            Timmings = new List<StarTimming>()
            {
                new StarTimming(){ StarNumber = 1, Timming = 20},
                new StarTimming(){ StarNumber = 2, Timming = 10},
                new StarTimming(){ StarNumber = 3, Timming = 5}
            }
        });

        levelTimmings.Add(new RatingsManager.LevelTimmings()
        {
            LevelId = 1,
            Timmings = new List<StarTimming>()
            {
                new StarTimming(){ StarNumber = 1, Timming = 20},
                new StarTimming(){ StarNumber = 2, Timming = 10},
                new StarTimming(){ StarNumber = 3, Timming = 5}
            }
        });

        levelTimmings.Add(new RatingsManager.LevelTimmings()
        {
            LevelId = 2,
            Timmings = new List<StarTimming>()
            {
                new StarTimming(){ StarNumber = 1, Timming = 20},
                new StarTimming(){ StarNumber = 2, Timming = 10},
                new StarTimming(){ StarNumber = 3, Timming = 5}
            }
        });
    }

    public static void SetLevelStars(List<RatingsManager.StarTimming> timmings, Image[] stars, float levelTime)
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
}
