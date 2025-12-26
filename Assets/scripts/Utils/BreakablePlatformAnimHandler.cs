using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakablePlatformAnimHandler : MonoBehaviour
{
    // Start is called before the first frame update
    BreakablePlatform platform;
    Animator anim;
    void Start()
    {
        platform = GetComponentInParent<BreakablePlatform>();
        anim= GetComponent<Animator>();

    }
    public void isBroken()
    {
        
        platform.setIsBroken(true);
    }
    public void isRepaired(){
        platform.setIsBroken(false);
    }
     public void deactivatePlatformComponents(){

        platform.deactivatePlatformComponents();
     }
     public void activatePlatformComponents(){
        platform.activatePlatformComponents();
     }
     public void playIdle(){
        GetComponent<Animator>().Play("Idle",-1,0);
     }
    public void playEntityAbove(){
        anim.Play("OnEntityAbove", -1, 0);
    }
    public void playRepair (){
        anim.Play("RepairPlatformBreakeable", -1, 0f);
    }
    public void playDestroyPlatform(){
        anim.Play("DestroyPlatform", -1, 0);
    }




}
