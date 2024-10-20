using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GamesEventsAudio))]
public class GameEventsAnimHandler : MonoBehaviour
{
    GamesEventsAudio gameAudio;
    void Start()
    {
        gameAudio = GetComponent<GamesEventsAudio>();
    }
    void OnEnable()
    {
        coinsLevelHandler.onCoinsCollectedInScreen += playCelebrationPlayer;
    }

    void OnDisable()
    {
        coinsLevelHandler.onCoinsCollectedInScreen -= playCelebrationPlayer;
    }
    private void playCelebrationPlayer()
    {
        // this the celebration1
        gameAudio.playDefaultClip();
        // playAnimation;
    }
}
