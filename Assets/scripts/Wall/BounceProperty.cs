using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Numerics;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;


public class BounceProperty : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float Bounciness = 2;
    [SerializeField] Vector2 bounceVector;
    [SerializeField] bool useBounceVector;
    Vector2 collisionNormal;
    
    [Tooltip("¿Does the player needs to be above the collider to trigger bounciness?"), SerializeField] bool trampolineRestriction = true;

    void OnCollisionEnter2D(Collision2D collider)
    {
        if (trampolineRestriction)
        {
            collisionNormal = collider.contacts[0].normal;
            if (collider.gameObject.GetComponent<Rigidbody2D>() && (collider.contacts[0].point.y) <= (collider.transform.position.y) ) Bounce(collider.gameObject.GetComponent<Rigidbody2D>());
            else return;
        }
        else
        {
            Bounce(collider.gameObject.GetComponent<Rigidbody2D>());
            
        }



    }
    public void Bounce(Rigidbody2D rb)
    {
        if(trampolineRestriction){
            
        }

        if (useBounceVector)
        {
       
            rb.AddForce(bounceVector.normalized * Bounciness, ForceMode2D.Impulse);
            

         
        }
        else
        {

            rb.AddForce(transform.up * Bounciness, ForceMode2D.Impulse);

        }
        rb.transform.rotation = transform.rotation; 
    }

}
