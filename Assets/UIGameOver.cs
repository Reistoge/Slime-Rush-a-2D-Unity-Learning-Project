using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGameOver : MonoBehaviour
{


    [SerializeField] GameObject gameOver;


    void OnEnable()
    {
        PlayerScript.OnPlayerDied += enableGameOverUI;

    }
    void OnDisable()
    {
        PlayerScript.OnPlayerDied -= enableGameOverUI;
    }
    void enableGameOverUI() {
        GameManager.Instance.destroyRuntimeData();
        gameOver.SetActive(true);

    }


}
