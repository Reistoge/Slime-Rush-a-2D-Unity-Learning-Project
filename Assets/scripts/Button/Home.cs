using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeButton : MonoBehaviour
{
    // Start is called before the first frame update
    public void Home()
    {
        SceneManager.LoadScene(GameManager.instance.Main_menu_name);
        if(PauseButton.game_paused)
        {
            Time.timeScale = 1.0f;
            PauseButton.game_paused=!PauseButton.game_paused;
        }

    }
}
