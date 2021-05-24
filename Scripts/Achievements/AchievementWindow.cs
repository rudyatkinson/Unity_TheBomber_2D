using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Obsolete]
public class AchievementWindow : MonoBehaviour
{
    public GameObject achievementButtonPrefab;
    public Transform achievementButtonParent;

    public Color closedAchievementColor;

    private Dictionary<string, GameObject> instantiatedAchievements;

    private void Awake()
    {
        instantiatedAchievements = new Dictionary<string, GameObject>();

        Dictionary<string, Achievement> activeAchievements = AchievementManager._Instance.activeAchievements;
        Dictionary<string, Achievement> closedAchievements = AchievementManager._Instance.closedAchievements;

        foreach (KeyValuePair<string, Achievement> achievement in closedAchievements)
        {
            GameObject go = Instantiate(achievementButtonPrefab, achievementButtonParent);
            go.transform.GetChild(0).GetComponent<Text>().text = achievement.Key;
            go.transform.GetChild(1).GetComponent<Text>().text = achievement.Value.AchievementInfo();
            go.transform.GetChild(2).GetComponent<Image>().sprite = achievement.Value.unlockedPlane;
            go.GetComponent<Button>().onClick.AddListener(() => 
            {
                go.transform.GetChild(3).gameObject.SetActive(true);
                UnityEngine.PlayerPrefs.SetString("selectedSkin", achievement.Key);
                GEvents.PlayerSkinChanged?.Invoke(achievement.Key);
            });
            go.GetComponent<Image>().color = closedAchievementColor;

            instantiatedAchievements.Add(achievement.Key, go);
        }

        foreach (KeyValuePair<string, Achievement> achievement in activeAchievements)
        {
            GameObject go = Instantiate(achievementButtonPrefab, achievementButtonParent);
            go.transform.GetChild(0).GetComponent<Text>().text = achievement.Value.AchievementName();
            go.transform.GetChild(1).GetComponent<Text>().text = achievement.Value.AchievementInfo();
            go.transform.GetChild(2).GetComponent<Image>().sprite = achievement.Value.unlockedPlane;
            go.transform.GetChild(3).gameObject.SetActive(false);

            instantiatedAchievements.Add(achievement.Key, go);
        }
    }

    private void OnDisable()
    {
        UnityEngine.PlayerPrefs.Save();
    }

    private void OnDestroy()
    {
        foreach (GameObject go in instantiatedAchievements.Values) Destroy(go);
        instantiatedAchievements.Clear();
    }
}
