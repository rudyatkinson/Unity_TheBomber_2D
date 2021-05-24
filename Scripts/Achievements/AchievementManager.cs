//#define DEBUG_ActiveAchievementCount
//#define DEBUG_CheckActiveAchievement_ProgressSuccess

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public enum GameState
{
    MainMenu,
    Playing
}

[System.Obsolete]
public class AchievementManager : MonoBehaviour
{
    public static AchievementManager _Instance { get; private set; }

    public Dictionary<string, Achievement> activeAchievements { get; private set; }

    /// <summary>
    /// Completed Achievement comes to this {<see cref="completedAchievements"/>}
    /// and after to show player what did he/she success, achievement goes to {<seealso cref="closedAchievements"/>}.
    /// </summary>
    public Dictionary<string, Achievement> completedAchievements { get; private set; }
    public Dictionary<string, Achievement> closedAchievements { get; private set; }

    public GameState currentGameState { get; private set; }

    private void Awake()
    {
        if (_Instance == null)
        {
            _Instance = this;
            DontDestroyOnLoad(gameObject);
            currentGameState = GameState.MainMenu;
        }
        else
            Destroy(gameObject);

        activeAchievements = new Dictionary<string, Achievement>();
        completedAchievements = new Dictionary<string, Achievement>();
        closedAchievements = new Dictionary<string, Achievement>();

        //All achievemets listing in activeAchievements for availability
        List<Achievement> achievements = GetComponents<Achievement>().ToList();
        achievements.ForEach(
            achievement => activeAchievements.Add
            (achievement.AchievementName(), achievement));

#if DEBUG_ActiveAchievementCount
        //Debugging to see how many active achievements and to list names.
        Debug.Log($"achievements activated: {activeAchievements.Count}");
        activeAchievements.Keys.ToList().ForEach
            (achievementName => Debug.Log(achievementName));

#endif

        //Event 
        GEvents.GameOver.AddListener(MoveCompleteToClosed);
    }

    /// <summary>
    /// Checks the achievement is exist, returns progress as a <see cref="bool"/> if achievement is exist. 
    /// <para>Achievement drops to <see cref="completedAchievements"/> automatically if it's progress completed.</para>
    /// </summary>
    /// <param name="achievementName">The name of achievement which will check.</param>
    /// <param name="e"><seealso cref="AchievementEventArgs{T}"/> has to contain current condition to check that achievement has been achieved.</param>
    /// <returns>Returns true if achievement exist and completed.</returns>
    public bool CheckActiveAchievement(string achievementName, AchievementEventArgs<object> e)
    {
        if (activeAchievements.ContainsKey(achievementName) &&
            currentGameState == GameState.Playing &&
            activeAchievements[achievementName].Progress(e))
        {
            MoveActiveToComplete(achievementName);

#if DEBUG_CheckActiveAchievement_ProgressSuccess
            DEBUG_WriteDictionaries();
#endif

            return true;
        }  
        return false;
    }

    /// <summary>
    /// Moves an achievement to {<see cref="completedAchievements"/>} 
    /// from {<see cref="activeAchievements"/>} when an achievement completed.
    /// </summary>
    /// <param name="achievementName">Name of completed achievement.</param>
    private void MoveActiveToComplete(string achievementName)
    {
        completedAchievements.Add(
            achievementName,
            activeAchievements[achievementName]);

        activeAchievements.Remove(achievementName);
    }

    /// <summary>
    /// After level completed and player see the completed achievemets, 
    /// all completed achievements moves to {<see cref="closedAchievements"/>} 
    /// and {<see cref="completedAchievements"/>} is cleared.
    /// </summary>
    private void MoveCompleteToClosed()
    {
        foreach(KeyValuePair<string, Achievement> achievemet in completedAchievements)
        {
            closedAchievements.Add(
                achievemet.Key, 
                achievemet.Value);
        }

        completedAchievements.Clear();
    }

    public void ChangeGameState(GameState gameState) => currentGameState = gameState;
}
