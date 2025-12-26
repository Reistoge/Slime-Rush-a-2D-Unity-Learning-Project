using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class PlatformTimerDissolver : MonoBehaviour
{
    [SerializeField] DissolvePlatform dissolvePlatform;
    [SerializeField] PlatformDissolverConfig config;

    Coroutine coroutine;
    [SerializeField] float timeInvisible = 1;
    [SerializeField] float timeVisible = 1;
 
    [Tooltip("true: the platform starts the dissolve behaviour when is instantiated \nfalse: the platform does not dissolves when is instantiated")] [SerializeField] bool onStart = true;

   
    

    void OnEnable()
    {
        // timeDissolved = config.timeDissolved;
        // timeUnDissolved = config.timeUnDissolved;
        // onStart = config.onStart;
        // dissolvePlatform.dissolve();



    }
    IEnumerator Start()
    {

        if (onStart)
        {
            if (dissolvePlatform.StartDissolved)
            {
                yield return new WaitForSeconds(timeInvisible);
                dissolvePlatform.unDissolve();
                yield return new WaitForSeconds(dissolvePlatform.getUnDissolveAnimTime());
            }

            startDissolvingBehaviour();
        }
    }
    IEnumerator dissolveCoroutine()
    {

        while (true)
        {
            if (config == null || dissolvePlatform == null)
            {
                yield break;
            }

            yield return new WaitForSeconds(timeVisible);
            dissolvePlatform.dissolve();

            yield return new WaitForSeconds(dissolvePlatform.getDissolveAnimTime());

            yield return new WaitForSeconds(timeInvisible);

            dissolvePlatform.unDissolve();
            yield return new WaitForSeconds(dissolvePlatform.getUnDissolveAnimTime());
        }


    }
    public void startDissolvingBehaviour()
    {
        coroutine = StartCoroutine(dissolveCoroutine());



    }
    public void stopDissolve()
    {
        StopCoroutine(coroutine);
        dissolvePlatform.unDissolve();


    }





}
