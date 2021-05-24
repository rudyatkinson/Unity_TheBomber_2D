using UnityEngine;
using UnityEngine.UI;

public class StatisticsHolder : MonoBehaviour
{
    #region Destroyed Enemy Counters

    public int destroyedPlane { get; private set; }
    public int destroyedArtillery { get; private set; }
    public int destroyedAmmunition { get; private set; }

    #endregion
    #region Scores

    private int planeScore = 50;
    private int artilleryScore = 10;
    private int ammunitionScore = 100;

    #endregion
    #region Text Components

    public Text destroyedPlaneText;
    public Text destroyedArtilleryText;
    public Text destroyedAmmunitionText;
    public Text totalScoreText;

    #endregion

    private void Start()
    {
        GEvents.EnemyDestroyed.AddListener(CountDestroyedEnemies);
        GEvents.GameOver.AddListener(WriteStatistics);
    }

    /// <summary>
    /// Increase destroyed enemies when <see cref="GEvents.EnemyDestroyed"/> event triggered
    /// </summary>
    /// <param name="enemyTag"></param>
    private void CountDestroyedEnemies(string enemyTag)
    {
        switch(enemyTag)
        {
            case "Tank":
                destroyedArtillery++;
                break;
            case "Ammunition":
                destroyedAmmunition++;
                break;
            case "Plane":
                destroyedPlane++;
                break;
        }
    }

    /// <summary>
    /// Counts score to show player when game ended.
    /// </summary>
    private void WriteStatistics()
    {
        destroyedPlaneText.text = destroyedPlane.ToString();
        destroyedArtilleryText.text = destroyedArtillery.ToString();
        destroyedAmmunitionText.text = destroyedAmmunition.ToString();

        int totalScore = (destroyedPlane * planeScore) +
            (destroyedArtillery * artilleryScore) +
            (destroyedAmmunition * ammunitionScore);

        totalScoreText.text = totalScore.ToString();
    }

    private void OnDestroy()
    {
        GEvents.EnemyDestroyed.RemoveListener(CountDestroyedEnemies);
        GEvents.GameOver.RemoveListener(WriteStatistics);
    }
}
