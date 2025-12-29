using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Controls platform dissolve/undissolve animations and collider state.
/// Platforms can start in either dissolved or solid state and transition between them.
/// </summary>
public class DissolvePlatform : MonoBehaviour
{
    #region Serialized Fields

    [Tooltip("Handles the visual dissolve animations")]
    [SerializeField] private DissolvePlatformAnimHandler anim;

    [Tooltip("The collider to enable/disable with dissolve state")]
    [SerializeField] private Collider2D col;

    [Tooltip("True: platform starts invisible\nFalse: platform starts solid")]
    [SerializeField] private bool startDissolved = false;

    #endregion

    #region Properties

    /// <summary>Gets or sets whether the platform starts in dissolved state</summary>
    public bool StartDissolved { get => startDissolved; set => startDissolved = value; }

    #endregion

    #region Unity Lifecycle

    /// <summary>
    /// Initializes the platform state based on StartDissolved setting.
    /// </summary>
    void OnEnable()
    {
        if (startDissolved)
        {
            playDissolved();
        }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Plays the dissolve animation, transitioning the platform to invisible state.
    /// </summary>
    public void dissolve()
    {
        anim.playDissolve();
    }

    /// <summary>
    /// Plays the undissolve animation, transitioning the platform to solid state.
    /// </summary>
    public void unDissolve()
    {
        anim.playUnDissolve();
    }

    /// <summary>
    /// Immediately sets the platform to dissolved state without animation.
    /// </summary>
    public void playDissolved()
    {
        anim.playDissolved();
    }

    /// <summary>
    /// Enables the platform's collider, making it solid.
    /// </summary>
    public void enableCollider()
    {
        col.enabled = true;
    }

    /// <summary>
    /// Disables the platform's collider, making it intangible.
    /// </summary>
    public void disableCollider()
    {
        col.enabled = false;
    }

    /// <summary>
    /// Gets the duration of the dissolve animation.
    /// </summary>
    /// <returns>Animation duration in seconds</returns>
    public float getDissolveAnimTime()
    {
        return anim.getDissolveAnimTime();
    }

    /// <summary>
    /// Gets the duration of the undissolve animation.
    /// </summary>
    /// <returns>Animation duration in seconds</returns>
    public float getUnDissolveAnimTime()
    {
        return anim.getUnDissolveAnimTime();
    }

    #endregion
}
