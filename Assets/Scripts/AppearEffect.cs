using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls visual appear effects using smoke animations.
/// Provides methods to play different smoke effect animations.
/// </summary>
public class AppearEffect : MonoBehaviour
{
    #region Serialized Fields

    [Tooltip("Animator controlling the smoke effect animations")]
    [SerializeField] private Animator animator;

    #endregion

    #region Public Methods

    /// <summary>
    /// Plays the first smoke effect animation.
    /// Typically used for standard object appearances.
    /// </summary>
    public void PlaySmoke1Effect()
    {
        animator.Play("smoke1", -1, 0f);
    }

    /// <summary>
    /// Plays the second smoke effect animation.
    /// Can be used for different visual styles or contexts.
    /// </summary>
    public void PlaySmoke2Effect()
    {
        animator.Play("smoke2", -1, 0f);
    }

    #endregion
}
