using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages background music for different game scenes.
/// Automatically switches music tracks based on scene changes.
/// Extends SoundSystem to utilize audio playback functionality.
/// </summary>
public class GameMusic : SoundSystem
{
    #region Serialized Fields

    [Header("Music Tracks")]
    [Tooltip("Audio clips to play during main gameplay")]
    [SerializeField] private AudioClip[] mainGameClips;

    [Tooltip("Audio clips to play in the main menu")]
    [SerializeField] private AudioClip[] mainMenuClips;

    [Tooltip("Audio clips to play in the in-game shop")]
    [SerializeField] private AudioClip[] inGameShopClips;

    #endregion

    #region Unity Lifecycle

    /// <summary>
    /// Subscribes to scene-loaded events to automatically switch music.
    /// </summary>
    new void OnEnable()
    {
        base.OnEnable();
        LegacyEvents.GameEvents.onMainGameSceneLoaded += PlayMainGameOST;
        LegacyEvents.GameEvents.onInGameShopSceneLoaded += PlayInGameShopOST;
        LegacyEvents.GameEvents.onMainMenuSceneLoaded += PlayMainMenuOST;
    }

    /// <summary>
    /// Unsubscribes from events to prevent memory leaks.
    /// </summary>
    void OnDisable()
    {
        LegacyEvents.GameEvents.onMainGameSceneLoaded -= PlayMainGameOST;
        LegacyEvents.GameEvents.onInGameShopSceneLoaded -= PlayInGameShopOST;
        LegacyEvents.GameEvents.onMainMenuSceneLoaded -= PlayMainMenuOST;
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Plays a random track from the main menu music collection.
    /// </summary>
    private void PlayMainMenuOST()
    {
        int index = Random.Range(0, mainMenuClips.Length);
        setClip(mainMenuClips[index]);
        AS.Play();
    }

    /// <summary>
    /// Plays a random track from the in-game shop music collection.
    /// </summary>
    private void PlayInGameShopOST()
    {
        int index = Random.Range(0, inGameShopClips.Length);
        setClip(inGameShopClips[index]);
        AS.Play();
    }

    /// <summary>
    /// Plays a random track from the main game music collection.
    /// </summary>
    private void PlayMainGameOST()
    {
        int index = Random.Range(0, mainGameClips.Length);
        setClip(mainGameClips[index]);
        AS.Play();
    }

    #endregion
}
