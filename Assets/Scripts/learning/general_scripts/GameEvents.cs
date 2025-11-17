using System;

/// <summary>
/// Legacy static event system for game-wide events.
/// NOTE: Consider migrating to the new EventSystem class for better type safety and organization.
/// </summary>
public static class GameEvents
{
    /// <summary>Event triggered when the main game scene loads.</summary>
    public static Action onMainGameSceneLoaded;

    /// <summary>Event triggered when the in-game shop scene loads.</summary>
    public static Action onInGameShopSceneLoaded;

    /// <summary>Event triggered when the main menu scene loads.</summary>
    public static Action onMainMenuSceneLoaded;

    /// <summary>Event triggered when the game is restarted.</summary>
    public static Action onGameIsRestarted;

    /// <summary>Event triggered when the main game starts.</summary>
    public static Action onMainGameStart;

    /// <summary>Event triggered when any scene changes.</summary>
    public static Action onSceneChanged;

    /// <summary>Event triggered when the resume button is clicked.</summary>
    public static Action onClickResume;

    /// <summary>Event triggered when game resumes from pause.</summary>
    public static Action onResume;

    /// <summary>Triggers the resume button click event.</summary>
    public static void triggerOnClickResume()
    {
        onClickResume?.Invoke();
    }

    /// <summary>Triggers the game resume event.</summary>
    public static void triggerOnResume()
    {
        onResume?.Invoke();
    }

    /// <summary>Triggers the main game scene loaded event.</summary>
    public static void triggerOnMainGameSceneLoaded()
    {
        onMainGameSceneLoaded?.Invoke();
    }

    /// <summary>Triggers the scene changed event.</summary>
    public static void triggerOnSceneChanged()
    {
        onSceneChanged?.Invoke();
    }

    /// <summary>Triggers the in-game shop scene loaded event.</summary>
    public static void triggerOnInGameShopSceneLoaded()
    {
        onInGameShopSceneLoaded?.Invoke();
    }

    /// <summary>Triggers the main menu scene loaded event.</summary>
    public static void triggerOnMainMenuSceneLoaded()
    {
        onMainMenuSceneLoaded?.Invoke();
    }

    /// <summary>Triggers the game restart event.</summary>
    public static void triggerGameIsRestarted()
    {
        onGameIsRestarted?.Invoke();
    }
}