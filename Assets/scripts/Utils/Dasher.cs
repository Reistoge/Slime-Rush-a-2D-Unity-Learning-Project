using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Dasher : MonoBehaviour
{
    float dissapearTime = 5f;
    bool activeDash;
    bool isCentering;

    Coroutine dasherCoroutine;



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
    IEnumerator LerpPositionAndRotation(Transform target, Transform center, float duration)
    {
        isCentering = true;
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
        isCentering = false;
    }
    IEnumerator dasherMechanic(Transform target, Transform center, float duration)
    {        
        if (target.gameObject.GetComponent<PlayerScript>() != null)
        {
            PlayerScript playerScript = target.gameObject.GetComponent<PlayerScript>();
            if (playerScript.IsDashing)
            {
                playerScript.stopDash();
                StartCoroutine(LerpPositionAndRotation(playerScript.transform, this.transform, 0.5f));
                yield return new WaitUntil(() => isCentering == false);
                playerScript.dash(playerScript.DashCoroutineTime, playerScript.DashCoroutineSpeed, transform.up);
                if (activeDash == true && anim)
                {

                    anim.Play("dasherOnUse");

                }

            }
        }



    }
    void OnTriggerEnter2D(Collider2D col)
    {
        StartCoroutine(dasherMechanic(col.transform,this.transform,0.25f));
    }
    public void dasherIn()
    {

        if (activeDash == false)
        {

            activeDash = true;

            anim.Play("dasherIn");
            Invoke("dasherOut", dissapearTime);

        }


    }
    public void dasherOut()
    {
        if (activeDash)
        {

            activeDash = false;

            anim.Play("dasherOut");
        }
    }


}
