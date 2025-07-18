using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Dasher : MonoBehaviour
{
    [SerializeField] float dissapearTime = 5f;
    [SerializeField] Transform center;
    [SerializeField] bool isCentering;

    Coroutine dasherCoroutine;
    Coroutine dasherCounterCoroutine;
    Coroutine lerpPositionCoroutine;



    void OnEnable()
    {
        PlayerScript.onPlayerDash += dasherIn;
    }
    void OnDisable()
    {
        PlayerScript.onPlayerDash -= dasherIn;
    }
    Animator anim;
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }
    IEnumerator lerpPositionAndRotation(Transform target, Transform center, float duration)
    {
        
        float timeElapsed = 0;
        Vector3 startPosition = target.position;
        Vector3 endPosition = center.position;
        Quaternion startRotation = target.rotation;
        Quaternion endRotation = center.rotation;

        while (timeElapsed < duration)
        {
            float t = timeElapsed / duration;
            t = t * t * (3f - 2f * t); // Smoothstep
            target.position = Vector3.Lerp(startPosition, endPosition, t);
            target.rotation = Quaternion.Slerp(startRotation, endRotation, t);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        target.position = endPosition;
        target.rotation = endRotation;
        lerpPositionCoroutine = null;
    }
    IEnumerator dasherMechanic(Transform target, float duration)
    {

        GameManager.Instance.CanMove = false;
        yield return new WaitForEndOfFrame();
        if (target.gameObject.GetComponent<PlayerScript>() != null)
        {
            PlayerScript playerScript = target.gameObject.GetComponent<PlayerScript>();
            if (playerScript.IsDashing)
            {
                playerScript.stopDash();
                anim.Play("dasherOnUse");
                lerpPositionCoroutine = StartCoroutine(lerpPositionAndRotation(playerScript.transform, center, duration));
                yield return new WaitUntil(() => lerpPositionCoroutine == null);

                playerScript.dash(playerScript.DashCoroutineTime, playerScript.DashCoroutineSpeed, transform.up);


            }
        }
        dasherCoroutine = null;
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        print("enter on dasher");
        if ( lerpPositionCoroutine == null && dasherCoroutine == null)
        {
            dasherCoroutine = StartCoroutine(dasherMechanic(col.transform, Utils.getAnimationClipDuration(anim, "dasherOnUse")));
            
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        
        GameManager.Instance.CanMove = true;
    }
    public void dasherIn()
    {


        if (dasherCounterCoroutine == null)
        {
            anim.Play("dasherIn");
            dasherCounterCoroutine = StartCoroutine(dasherCounter());
        }
        else
        {
            
            StopCoroutine(dasherCounterCoroutine);
            dasherCounterCoroutine = StartCoroutine(dasherCounter());
        }






    }
    IEnumerator dasherCounter()
    {
        dissapearTime = Mathf.Clamp(dissapearTime, Utils.getAnimationClipDuration(anim, "dasherOnUse"), Mathf.Infinity);
        yield return new WaitForSeconds(dissapearTime);
        if (dasherCoroutine != null)
        {
            yield return new WaitUntil(() => dasherCoroutine == null);

        }

        dasherOut();
        dasherCounterCoroutine = null;
    }
    public void dasherOut()
    {

        anim.Play("dasherOut");

    }


}
