using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Events;
using Vector2 = UnityEngine.Vector2;

public class KnockbackFeedBack : MonoBehaviour
{
    Rigidbody2D rb;

    [Header("Feedback Settings")]
    [SerializeField] float horizontalStrength = 10f;
    [SerializeField] float verticalStrength = 10f;
    [SerializeField] float delay = 0.5f;
    float initDelay;
    [SerializeField] float gravityMultiplier = 0.5f;

    [Header("Clamps")]
    [SerializeField] float minHorizontalForce = 5f;
    [SerializeField] float maxVerticalForce = 15f;
    KnockbackFeedBack initFeedback;
    float originalGravity;
    Coroutine resetCoroutine;

    public UnityEvent OnBegin, OnDone;

    public float Delay { get => delay; set => delay = value; }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if(rb){
            originalGravity = rb.gravityScale;

        }
        initFeedback = this;
        initDelay = delay;
    }

    public void PlayFeedBack(Vector2 sender, Vector2 spikeDirection)
    {
        if(rb==null) return; 
        StopAllCoroutines();
        OnBegin?.Invoke();

        // Calculate direction
        Vector2 direction = ((Vector2)transform.position - sender).normalized;

        // Adjust horizontal and vertical forces
        float horizontalForce = spikeDirection.x * horizontalStrength;

        rb.gravityScale = gravityMultiplier;
        // Ensure a minimum horizontal force when spike is mostly vertical
        if (Mathf.Abs(spikeDirection.y) > 0.8f && Mathf.Abs(horizontalForce) < minHorizontalForce)
        {
            horizontalForce = minHorizontalForce * Mathf.Sign(direction.x); // Add bias based on player position
        }

        float verticalForce = Mathf.Clamp(spikeDirection.y * verticalStrength, -maxVerticalForce, maxVerticalForce);

        Vector2 forceVector = new Vector2(horizontalForce, verticalForce);

        rb.velocity = Vector2.zero; // Reset velocity
        rb.AddForce(forceVector, ForceMode2D.Impulse);

        resetCoroutine = StartCoroutine(ResetBody());
    }
    public void PlayFeedBack(Vector2 sender, Vector2 spikeDirection, KnockbackFeedBack feedBack)
    {
        StopAllCoroutines();
        OnBegin?.Invoke();
        feedBack.OnBegin?.Invoke();
        delay = feedBack.delay;

        // Calculate direction
        Vector2 direction = ((Vector2)transform.position - sender).normalized;

        // Adjust horizontal and vertical forces
        float horizontalForce = feedBack.horizontalStrength;

        // Ensure a minimum horizontal force when spike is mostly vertical

        float verticalForce = Mathf.Clamp(spikeDirection.y * feedBack.verticalStrength, -feedBack.maxVerticalForce, feedBack.maxVerticalForce);

        Vector2 forceVector = new Vector2(horizontalForce, verticalForce);

        rb.velocity = Vector2.zero; // Reset velocity
        rb.gravityScale = feedBack.gravityMultiplier;
        rb.AddForce(forceVector, ForceMode2D.Impulse);

        resetCoroutine = StartCoroutine(ResetBody(feedBack));
    }


    IEnumerator ResetBody()
    {
        yield return new WaitForSeconds(delay);


        rb.gravityScale = originalGravity;
        rb.velocity = Vector2.zero;
        OnDone?.Invoke();
    }


    IEnumerator ResetBody(KnockbackFeedBack feedBack)
    {
        yield return new WaitForSeconds(feedBack.delay);


        rb.gravityScale = originalGravity;
        rb.velocity = Vector2.zero;
        delay = initDelay;

        OnDone?.Invoke();
        feedBack.OnDone?.Invoke();
    }
    public void stopFeedBack()
    { 
        if(resetCoroutine!=null){
            StopAllCoroutines();
            rb.gravityScale = originalGravity;
            OnDone?.Invoke();

        }
         

    }
}
