using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickySpikeBoxSoundSystem : SoundSystem 
{
    public void playDamageSfx(){
        
        AS.pitch=Random.Range(1f,2f);
        setClip(clips[0]);
        AS.Play();

    }
    public void dieSfx(){
        AS.pitch=1;
        setClip(clips[1]);
        AS.Play();
    }


}
