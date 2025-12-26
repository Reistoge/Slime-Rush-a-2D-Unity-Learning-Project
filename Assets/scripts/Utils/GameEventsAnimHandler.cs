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
    

    
    public void playCelebrationPlayer()
    {
        // this the celebration1
        gameAudio.playDefaultClip();
        // playAnimation;
    }
}
