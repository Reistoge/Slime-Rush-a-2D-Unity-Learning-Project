using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokeRockSFX : SoundSystem
{
    public void brokeRockSfx(){
        AS.pitch= Random.Range(1f,1.8f);
        playDefaultClip();
         
    }

}
