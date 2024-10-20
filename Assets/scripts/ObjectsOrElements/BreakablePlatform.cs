using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
[RequireComponent(typeof(BreakAndRepair))]
public class BreakablePlatform : MonoBehaviour
{
    BreakAndRepair breakAndRepair;
    IEnumerator breaking;
    [SerializeField] float maxLifeTime;
    [SerializeField] float lifeTime;

    bool isTouching;

    void Start()
    {

        breakAndRepair = GetComponent<BreakAndRepair>();
        lifeTime = maxLifeTime;
    }
    void Update()
    {
        
        if (isTouching && lifeTime>0)
        {
            lifeTime -= Time.deltaTime;
        }

        if (lifeTime <= 0 && breaking == null)
        {
            lifeTime=0;
            breaking = breakPlatform();
            StartCoroutine(breaking);
             
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.GetContact(0).normal.y < 0  && breakAndRepair.IsBroken==false )
        {
            isTouching = true;

        }

    }
    void OnCollisionExit2D(Collision2D collision)
    {

        isTouching = false;
        if (breaking != null)
        {
            StopCoroutine(breaking);
            breaking = null;
        }

    }
    IEnumerator breakPlatform()
    {

        if (isTouching)
        {
            breakAndRepair.breakObject();

        }
        yield return new WaitUntil(()=> breakAndRepair.IsBroken==true);
        print("Platform Broken");
        yield return new WaitUntil(()=> breakAndRepair.IsBroken==false);
        print("Platform Repaired");
        lifeTime=maxLifeTime;
        breaking = null;
         


    }



}
