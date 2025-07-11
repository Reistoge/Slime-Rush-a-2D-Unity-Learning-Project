using System;
using UnityEngine;
using UnityEngine.SceneManagement;


public class restartButton : MonoBehaviour
{
    // Start is called before the first frame update

    public static Action StopCoroutines;
    public void reloadscene()
    {

        PauseController.isGamePaused = false;
        StopCoroutines?.Invoke();
        Time.timeScale = 1;
        
        GameManager.Instance.loadSceneWithTransition(SceneManager.GetActiveScene().name);
    }
}
