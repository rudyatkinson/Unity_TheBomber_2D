//#define DEBUG_ProgressEventArgs
//#define DEBUG_ProgressComplete

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Obsolete]
public class ScoreAchievement : Achievement
{
    private float requiredScore = 50;
    private bool isCompleted = false;

    public override string AchievementInfo()
    {
        return "Get more than 50 score to show your talent.";
    }

    public override string AchievementName()
    {
        return "Talented";
    }

    public override bool Progress(AchievementEventArgs<object> e)
    {
#if DEBUG_ProgressEventArgs

        Debug.Log($"Gelen parametre: {e.parameter}");

#endif

        //Checking achievement conditions
        if (!isCompleted &&
            (int) e.parameter >= requiredScore)
        {
#if DEBUG_ProgressComplete

            Debug.Log($"{AchievementName()} tamamlandý.");

#endif
            isCompleted = true;
        }

        return isCompleted;
    }
}
