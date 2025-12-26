using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BrokeRockSFX))]
public class BrokeRockFX : MonoBehaviour
{

    
    [SerializeField] BrokeRockScript rockScript;
    Animator anim;
    void Start(){
        anim = GetComponent<Animator>();
    }

    public void playDestroyAnim(){
        
        anim.Play("break2",-1,0f);
    }
    public void destroyRock(){
       Time.timeScale=1f;
        Destroy(rockScript.gameObject);


    }

 
}

