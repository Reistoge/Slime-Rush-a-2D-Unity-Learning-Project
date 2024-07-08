using System;
using UnityEngine;
using UnityEngine.SceneManagement;


public class restartButton : MonoBehaviour
{
    // Start is called before the first frame update

    public static Action StopCoroutines;
    public void reloadscene()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        PauseButton.game_paused = false;
        StopCoroutines?.Invoke();
        Time.timeScale = 1;
    }
}
