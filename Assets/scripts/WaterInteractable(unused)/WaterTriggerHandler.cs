using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class WaterTriggerHandler : MonoBehaviour
{
    // [SerializeField] LayerMask waterMask;
    // [SerializeField] GameObject splashParticles;
    // EdgeCollider2D edgeColl;
    // InteractableWater water;
    // void Awake()
    // {
    //     edgeColl = GetComponent<EdgeCollider2D>();
    //     water = GetComponent<InteractableWater>();
    // }
    
    // void OnTriggerEnter2D(Collider2D collision)
    // {
    //     if ((waterMask.value & (1 << collision.gameObject.layer)) > 0  )
    //     {
    //         Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
    //         if (rb != null)
    //         {
    //             print(rb.gameObject.name);
    //             // spawn particles
    //             Vector2 localPos = gameObject.transform.localPosition;
    //             Vector2 hitObjectPos = collision.transform.position;
    //             Bounds hitObjectBounds = collision.bounds;

    //             Vector3 spawnPos = Vector3.zero;
         
                



    //             if (collision.transform.position.y >= edgeColl.points[1].y + edgeColl.offset.y + localPos.y)
    //             {
    //                 //hit from above
    //                 spawnPos = hitObjectPos - new Vector2(0, hitObjectBounds.extents.y);
    //             }
    //             else
    //             {
    //                 //hit from below
    //                 spawnPos = hitObjectPos + new Vector2(0, hitObjectBounds.extents.y);
    //             }
    //             if(splashParticles){
    //                 Instantiate(splashParticles, spawnPos, Quaternion.identity);

    //             }
    //             int multiplier = 1;
    //             if (rb.velocity.y < 0)
    //             {
    //                 multiplier = -1;

    //             }
    //             else { multiplier = 1; }
    //             float vel = rb.velocity.y * water.ForceMultiplier;
    //             vel = Mathf.Clamp(Mathf.Abs(vel), 0, water.MaxForce);

    //             water.Splash(collision, vel);
    //         }
    //     }
    // }


}
