using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMusic : SoundSystem
{
    [SerializeField] AudioClip[] mainGameClips;
    [SerializeField] AudioClip[] mainMenuClips;
    [SerializeField] AudioClip[] inGameShopCLips;
    new void OnEnable()
    {
        base.OnEnable();
        LegacyEvents.GameEvents.onMainGameSceneLoaded += playMainGameOST;
        LegacyEvents.GameEvents.onInGameShopSceneLoaded += playInGameShopOST;
        LegacyEvents.GameEvents.onMainMenuSceneLoaded += playMainMenuOST;
    }
    void OnDisable()
    {
        LegacyEvents.GameEvents.onMainGameSceneLoaded -= playMainGameOST;
        LegacyEvents.GameEvents.onInGameShopSceneLoaded -= playInGameShopOST;
        LegacyEvents.GameEvents.onMainMenuSceneLoaded -= playMainMenuOST;
    }

    void playMainMenuOST()
    {
        int index = Random.Range(0, mainMenuClips.Length);
        setClip(mainMenuClips[index]);
        AS.Play();
        // AS.pitch = 1f;
    }

    void playInGameShopOST()
    {
        int index = Random.Range(0, inGameShopCLips.Length);
        setClip(inGameShopCLips[index]);
        AS.Play();
        // AS.pitch = 1f;
    }


    void playMainGameOST()
    {
        int index = Random.Range(0, mainGameClips.Length);
        setClip(mainGameClips[index]);
        AS.Play();
        // AS.pitch = 1f;
    }
}
