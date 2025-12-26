using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Rendering.Universal;

public class BreakablePlatform : MonoBehaviour
{
    // BreakAndRepair breakAndRepair;
    // IEnumerator breaking;
    IEnumerator breakableCorutine;
    [SerializeField] BreakablePlatformAnimHandler anim;
    IEnumerator repairTimerCoroutine;

    [SerializeField] Light2D platformLight;
    [SerializeField] float maxLifeTime;
    [SerializeField] float lifeTime;

    [SerializeField] float vanishedTime;

    [SerializeField] Collider2D platformCollider;

    bool isBroken;
    bool isBreaking; // for the coroutines.

    public bool IsBroken { get => isBroken; set => isBroken = value; }
    public bool IsBreaking { get => isBreaking; set => isBreaking = value; }

    bool isTouching;

    void Start()
    {

        //breakAndRepair = GetComponent<BreakAndRepair>();
        ;

        lifeTime = maxLifeTime;
    }
    void Update()
    {

        if (isTouching && lifeTime > 0)
        {
            lifeTime -= Time.deltaTime;
            if (platformLight.intensity <= 5)
            {
                
                platformLight.intensity += (5 / maxLifeTime) * Time.deltaTime;

            }

        }

        if (lifeTime <= 0 && breakableCorutine == null)
        {
            lifeTime = 0;
            breakableCorutine = StartBehaviour();
            StartCoroutine(breakableCorutine);

        }
         
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.GetContact(0).normal.y < 0 && IsBroken == false)
        {
            isTouching = true;
            // play trembling animation
            anim.playEntityAbove();
            // anim.Play("OnEntityAbove", -1, 0);

            // breakAndRepair.playEntityAboveAnim();


        }

    }

    void OnCollisionExit2D(Collision2D collision)
    {

        isTouching = false;
        if (breakableCorutine != null)
        {
            StopCoroutine(breakableCorutine);
            breakableCorutine = null;
        }


    }
    public void setIsBroken(bool v)
    {
        isBroken = v;
    }
    IEnumerator ReppairProcess()
    {

        platformLight.intensity = 0;
        yield return new WaitForSeconds(vanishedTime);
        anim.playRepair();
        // anim.Play("RepairPlatformBreakeable", -1, 0f);

    }
    public void deactivatePlatformComponents()
    {

        if (GetComponent<PlatformEffector2D>() == null)
        {
            platformCollider.enabled = false;
        }
        else
        {
            GetComponent<PlatformEffector2D>().enabled = false;
            platformCollider.enabled = false;
        }

    }
    public void activatePlatformComponents()
    {
        if (GetComponent<PlatformEffector2D>() == null)
        {
            platformCollider.enabled = true;
        }
        else
        {
            GetComponent<PlatformEffector2D>().enabled = true;
            platformCollider.enabled = true;
        }

    }

    IEnumerator StartBehaviour()
    {

        if (isTouching)
        {
            //breakAndRepair.breakObject();

            // anim.Play("DestroyPlatform", -1, 0);
            anim.playDestroyPlatform();
        }

        yield return new WaitUntil(() => IsBroken == true);
        //print("Platform Broken");
        repairTimerCoroutine = ReppairProcess();
        StartCoroutine(repairTimerCoroutine);
        yield return new WaitUntil(() => IsBroken == false);
        //print("Platform Repaired");
        lifeTime = maxLifeTime;
        breakableCorutine = null;







    }



}
