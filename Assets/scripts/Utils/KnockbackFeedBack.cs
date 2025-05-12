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
    // [SerializeField] float horizontalStrength = 10f;
    // [SerializeField] float verticalStrength = 10f;
    // [SerializeField] float gravityMultiplier = 0.5f;

    [Header("Clamps")]
    // [SerializeField] float minHorizontalForce = 5f;
    // [SerializeField] float maxVerticalForce = 15f;

    KnockbackFeedBack thisFeedback;
   
    float initGravity;
    
    Coroutine resetCoroutine;

    public UnityEvent OnBegin, OnDone;
    [SerializeField] KnockbackFeedBackVariables knockbackConfig;

    public float Delay { get => knockbackConfig.delay; set => knockbackConfig.delay = value; }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if(rb){
            initGravity = rb.gravityScale;

        }
        thisFeedback = this;
        
    }
   

    public void triggerFeedback(Vector2 sender, Vector2 feedBackDirection)
    {
        if(rb==null) return; 
        StopAllCoroutines();
        OnBegin?.Invoke();

        // Calculate direction
        Vector2 direction = ((Vector2)transform.position - sender).normalized;

        // Adjust horizontal and vertical forces
        float horizontalForce = feedBackDirection.x * knockbackConfig.horizontalStrength;

        rb.gravityScale = knockbackConfig.gravityMultiplier;
        // Ensure a minimum horizontal force when spike is mostly vertical
        if (Mathf.Abs(feedBackDirection.y) > 0.8f && Mathf.Abs(horizontalForce) < knockbackConfig.minHorizontalForce)
        {
            horizontalForce = knockbackConfig.minHorizontalForce * Mathf.Sign(direction.x); // Add bias based on player position
        }

        float verticalForce = Mathf.Clamp(feedBackDirection.y * knockbackConfig.verticalStrength, -knockbackConfig.maxVerticalForce, knockbackConfig.maxVerticalForce);

        Vector2 forceVector = new Vector2(horizontalForce, verticalForce);

        rb.velocity = Vector2.zero; // Reset velocity
        rb.AddForce(forceVector, ForceMode2D.Impulse);

        resetCoroutine = StartCoroutine(ResetBody());
    }
    public void triggerFeedbackWithReference(Vector2 sender, Vector2 feedBackDirection, KnockbackFeedBack feedBack)
    {
        StopAllCoroutines();
        OnBegin?.Invoke();
        feedBack.OnBegin?.Invoke();
        
         
        // Adjust horizontal and vertical forces
        float horizontalForce = feedBack.knockbackConfig.horizontalStrength;

        // Ensure a minimum horizontal force when spike is mostly vertical

        float verticalForce = Mathf.Clamp(feedBackDirection.y * feedBack.knockbackConfig.verticalStrength, -feedBack.knockbackConfig.maxVerticalForce, feedBack.knockbackConfig.maxVerticalForce);

        Vector2 forceVector = new Vector2(horizontalForce, verticalForce);

        rb.velocity = Vector2.zero; // Reset velocity
        rb.gravityScale = feedBack.knockbackConfig.gravityMultiplier;
        rb.AddForce(forceVector, ForceMode2D.Impulse);
        print("force vector: " +forceVector );
        resetCoroutine = StartCoroutine(ResetBody(feedBack));
    }
    
 


    IEnumerator ResetBody()
    {
        yield return new WaitForSeconds(knockbackConfig.delay);


        rb.gravityScale = initGravity;
        rb.velocity = Vector2.zero;
        OnDone?.Invoke();
    }


    IEnumerator ResetBody(KnockbackFeedBack feedBack)
    {
        yield return new WaitForSeconds(feedBack.knockbackConfig.delay);

        rb.gravityScale = initGravity;
        rb.velocity = Vector2.zero;
         
        OnDone?.Invoke();
        feedBack.OnDone?.Invoke();
    }
    public void stopFeedBack()
    { 
        if(resetCoroutine!=null){
            StopAllCoroutines();
            rb.gravityScale = initGravity;
            OnDone?.Invoke();

        }
         

    }
}
