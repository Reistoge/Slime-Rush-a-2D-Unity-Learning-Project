using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCoin : MonoBehaviour
{


    public void getCoin()
    {
        // this function will be trigger by the parent when the coins collide.
        // this trigger will start the animation getCoin that will waits to the sounds to end and will execute disable Coin
        this.GetComponent<Animator>().SetTrigger("getCoin");
    }
    public void disableCoin()
    {
        // this function will be trigger in this object animator.
        this.transform.parent.GetComponent<Coin>().disableCoin();
        
    }
    public void enableCoin()
    {
        // this function will used in this object animator.
        this.transform.parent.GetComponent<Coin>().enableCoin();

    }
    public void resize()
    {
        // used in core;

        this.transform.localScale = Vector3.one*2;
        
    }



}
