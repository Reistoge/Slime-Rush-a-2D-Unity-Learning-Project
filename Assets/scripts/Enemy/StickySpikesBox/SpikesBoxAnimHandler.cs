using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesBoxAnimHandler : MonoBehaviour
{
    Animator anim;
    SpriteRenderer sr;
    GameObject box;
 

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        box = transform.parent.gameObject;
       
    }
    // functions to animator
    public void desactivateObject()
    {
        box.GetComponent<StickySpikeBox>().desactivateObject();
    }
    public void desactivateCollider()
    {
        box.GetComponent<StickySpikeBox>().desactivateCollider();
    }
   
 
    // functions to use in Box
    public void playDieAnimation(){
        
        anim.Play("die",-1,0f);
    }
    public void playDamageAnim(){
        anim.Play("takeDamage",-1,0f);
    }

   




}
