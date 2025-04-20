using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioSystem : SoundSystem
{
    public void jumpSound()
    {
        AS.pitch = Random.Range(1f, 1.3f); 
        setClip(clips[0]);
        AS.volume=0.5f;
        AS.Play();
        // AS.pitch = 1f;
       
    }
    public void dashSound(){
        AS.pitch = Random.Range(1f, 1.3f); 
        setClip(clips[1]);
        AS.volume=0.6f;
        AS.Play();
        // AS.pitch = 1f;
    }
    public void hurtSound(){
        AS.pitch = Random.Range(1f, 1.3f); 
        setClip(clips[2]);
        AS.volume=0.6f;
        AS.Play();
        // AS.pitch = 1f;
    }
    
}
