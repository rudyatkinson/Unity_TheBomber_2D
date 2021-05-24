using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script attached to achievement panel that gets visible when game over.
/// </summary>
[System.Obsolete]
public class GameOverAchievementsPanel : MonoBehaviour
{
    public GameObject achievementPrefab;
    public Transform achievementTransform;

    private void OnEnable()
    {
        var completedAchievements = AchievementManager._Instance.completedAchievements;

        if (completedAchievements.Count > 0)
        {
            foreach (Achievement achievement in completedAchievements.Values)
            {
                GameObject go = Instantiate(achievementPrefab, achievementTransform);
                go.transform.GetChild(0).GetComponent<Image>().sprite = achievement.unlockedPlane;
                go.transform.GetChild(1).GetComponent<Text>().text = achievement.AchievementName();
                go.transform.GetChild(2).GetComponent<Text>().text = achievement.AchievementInfo();
            }
        }
        else
            transform.gameObject.SetActive(false);

    }
}
