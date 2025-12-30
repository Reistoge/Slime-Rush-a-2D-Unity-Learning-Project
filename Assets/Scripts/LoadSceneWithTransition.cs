using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Provides a convenient way to trigger scene transitions with custom configurations.
/// Can be attached to UI buttons or triggered by other game events.
/// </summary>
public class LoadSceneWithTransition : MonoBehaviour
{
    #region Serialized Fields

    [Tooltip("Default configuration for scene transition")]
    [SerializeField] private LoadSceneWithTransitionSO loadSceneConfig;

    #endregion

    #region Public Methods

    /// <summary>
    /// Loads a scene using the specified transition configuration.
    /// </summary>
    /// <param name="config">The transition configuration to use</param>
    public void LoadSceneTransition(LoadSceneWithTransitionSO config)
    {
        GameManager.Instance.loadSceneWithTransition(config);
    }

    /// <summary>
    /// Loads a scene using the default transition configuration set in the Inspector.
    /// Logs a warning if no configuration is assigned.
    /// </summary>
    public void LoadSceneTransition()
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

    #endregion
}
