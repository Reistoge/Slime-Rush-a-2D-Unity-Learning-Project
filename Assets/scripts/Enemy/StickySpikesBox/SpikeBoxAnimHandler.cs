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
        box = transform.gameObject;

    }
    // functions to animator
    public void desactivateObject()
    {
        if (box && box.GetComponent<SpikeBox>())
        {
            box.GetComponent<SpikeBox>().disableBox();

        }
    }
    public void desactivateCollider()
    {
        if( box && box.GetComponent<SpikeBox>()){

            box.GetComponent<SpikeBox>().desactivateCollider();
        }
    }


    // functions to use in Box
    public void playDieAnimation()
    {

        anim.Play("die", -1, 0f);
    }
    public void playDealDamageAnim()
    {
        anim.Play("dealDamage", -1, 0f);
    }
    public void playDamageAnim()
    {
        anim.Play("takeDamage", -1, 0f);
    }






}
