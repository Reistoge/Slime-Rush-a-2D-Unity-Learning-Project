using JetBrains.Annotations;
using System;
using System.Collections;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Unity.VisualScripting.Member;
using static UnityEditor.FilePathAttribute;


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
