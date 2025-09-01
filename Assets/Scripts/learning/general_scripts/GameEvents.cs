using System;
using UnityEditor.PackageManager;
using UnityEngine;

public class GameEvents
{

    public static Action onMainGameSceneLoaded;
    public static Action onInGameShopSceneLoaded;
    public static Action onMainMenuSceneLoaded;
    public static Action onGameIsRestarted;
    public static Action onMainGameStart;
    public static Action onSceneChanged;

    public static void triggerOnMainGameSceneLoaded()
    {
        onMainGameSceneLoaded?.Invoke();
    }
    public static void triggerOnSceneChanged()
    {
        onSceneChanged?.Invoke();
    }

    public static void triggerOnInGameShopSceneLoaded()
    {
        onInGameShopSceneLoaded?.Invoke();
    }
    public static void triggerOnMainMenuSceneLoaded()
    {
        onMainMenuSceneLoaded?.Invoke();
    }
    public static void triggerGameIsRestarted()
    {
        onGameIsRestarted?.Invoke();
    }
}