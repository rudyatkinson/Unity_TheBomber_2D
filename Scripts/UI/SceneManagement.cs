using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public void LoadGameScene()
    {
        SceneManager.LoadScene(1);

        //AchievementManager._Instance.ChangeGameState(GameState.Playing);
    }

    public void LoadMainMenuScene()
    {
        // Invokes event when player wanted to go back main menu 
        GEvents.BackToMainMenu?.Invoke();
        SceneManager.LoadScene(0);

        //AchievementManager._Instance.ChangeGameState(GameState.MainMenu);
    }
}
