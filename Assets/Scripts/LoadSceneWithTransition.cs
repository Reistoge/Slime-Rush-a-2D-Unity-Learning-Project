using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSceneWithTransition : MonoBehaviour
{
    [SerializeField] LoadSceneWithTransitionSO loadSceneConfig;

    public void loadSceneWithTransition(LoadSceneWithTransitionSO config)
    {
        GameManager.Instance.loadSceneWithTransition(config);
    }
    public void loadSceneWithTransition()
    {
        if (loadSceneConfig != null)
        {
            GameManager.Instance.loadSceneWithTransition(loadSceneConfig);
        }
        else
        {
            Debug.LogWarning("LoadSceneWithTransitionSO is not assigned.");
        }
    }
 
}


