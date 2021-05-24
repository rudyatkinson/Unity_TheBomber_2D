using UnityEngine;

[System.Obsolete]
public abstract class Achievement : MonoBehaviour
{
    public Sprite unlockedPlane;

    /// <summary>
    /// Returns achievement name
    /// </summary>
    public abstract string AchievementName();
    /// <summary>
    /// Returns info about achievement
    /// </summary>
    public abstract string AchievementInfo();
    /// <summary>
    /// Using to calculate progress of achievement.
    /// </summary>
    /// <param name="e">Contains usable parameter to calculate progress</param>
    /// <returns>Returns that achievement completed or not.</returns>
    public abstract bool Progress(AchievementEventArgs<object> e);
}
