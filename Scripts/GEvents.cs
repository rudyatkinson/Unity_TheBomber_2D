using UnityEngine.Events;
using UnityEngine;

public static class GEvents
{
    // public static UnityEvent CloseCompletedAchievementsEvent = new UnityEvent();
    /// <summary>
    /// This event used to build next generation of level. <see cref="LevelDesigner.SpawnNext"/>
    /// </summary>
    public static UnityEvent BuildNextGeneration = new UnityEvent();
    /// <summary>
    /// This event notifies the score counter when an enemy destroyed.
    /// <see cref="StatisticsHolder"/>
    /// </summary>
    public static UnityEvent<string> EnemyDestroyed = new UnityEvent<string>();
    /// <summary>
    /// This event notifies <see cref="StatisticsHolder"/> when game ended to show statistics of destroyed enemies and total score.
    /// </summary>
    public static UnityEvent GameOver = new UnityEvent();
    /// <summary>
    /// This event notifies when player want to back main menu.
    /// </summary>
    public static UnityEvent BackToMainMenu = new UnityEvent();
    /// <summary>
    /// This event calls methods that whatever has to do when player received damage from projectiles.
    /// <para>Give 1 as parameter if player need to be untouchable, otherwise set the parameter to 0.</para>
    /// </summary>
    public static UnityEvent<int> PlayerDamageReceived = new UnityEvent<int>();
    /// <summary>
    /// This event notifies active sources in the scene to decrease or increase volume.
    /// </summary>
    public static UnityEvent<float> AudioVolumeChanged = new UnityEvent<float>();
    /// <summary>
    /// This event notifies active sources in the scene to mute or unmute sources.
    /// </summary>
    public static UnityEvent<bool> AudioMuteSettingChanged = new UnityEvent<bool>();
    /// <summary>
    /// This event notifies PlayerPrefs when player wanted to change skin.
    /// </summary>
    public static UnityEvent<string> PlayerSkinChanged = new UnityEvent<string>();
    /// <summary>
    /// This event notifies UI Element when bomb refilled.
    /// </summary>
    public static UnityEvent<int, float> BombRefilled = new UnityEvent<int, float>();
    /// <summary>
    /// This event notifies UI Element when player drop bomb.
    /// </summary>
    public static UnityEvent<int> BombDropped = new UnityEvent<int>();
    /// <summary>
    /// Notifies player with a text and sprite but sprite is optional.
    /// </summary>
    public static UnityEvent<string, Sprite> NotifyPlayer = new UnityEvent<string, Sprite>();
}
