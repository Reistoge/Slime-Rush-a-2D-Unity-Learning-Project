using System;
using UnityEditor.PackageManager;
using UnityEngine;

public class GameEvents
{

    public static Action onMainGameSceneLoaded;
    public static Action onInGameShopSceneLoaded;
    public static Action onMainMenuSceneLoaded;
    public static void triggerOnMainGameSceneLoaded()
    {
        onMainGameSceneLoaded?.Invoke();
    }

    public static void triggerOnInGameShopSceneLoaded()
    {
        onInGameShopSceneLoaded?.Invoke();
    }
    public static void triggerOnMainMenuSceneLoaded()
    {
        onMainMenuSceneLoaded?.Invoke();
    }
}