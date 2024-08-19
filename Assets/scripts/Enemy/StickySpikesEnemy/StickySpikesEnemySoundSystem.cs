using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickySpikesEnemySoundSystem : SoundSystem
{
    public void playDamageSfx(){
        setClip(clips[0]);
        AS.Play();

    }
    public void playDieSfx(){
        setClip(clips[1]);
        AS.Play();

    }
    


}
