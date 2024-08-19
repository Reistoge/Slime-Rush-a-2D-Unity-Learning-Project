using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeButton : MonoBehaviour
{
    // Start is called before the first frame update
    public void Home()
    {
        SceneManager.LoadScene(GameManager.instance.MainMenuSceneName);
        if (PauseController.isGamePaused)
        {
            Time.timeScale = 1.0f;
            PauseController.isGamePaused = !PauseController.isGamePaused;
        }

    }
}
