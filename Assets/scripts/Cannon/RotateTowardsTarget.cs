using System.Collections;
using System.Timers;
using UnityEngine;

public class RotateAndFollowTarget : MonoBehaviour
{
    public Transform target; // The target object to face and follow
    public float rotationSpeed = 5f; // Speed of rotation
     
    // float offset;
   // bool isRotating;

    

    void Start()
    {

         
    }
    // IEnumerator seekTarget()
    // {
    //     if (target != null)
    //     {
    //         // Rotate towards the target

    //         Vector3 direction = target.position - transform.position;
    //         float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) - 90;
    //         Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
    //         float elapsed = 0;

            
             
    //         while (elapsed < 2)
    //         {
    //             transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    //             elapsed += Time.deltaTime * rotationSpeed;
    //             yield return new WaitForEndOfFrame();
    //         }
    //         transform.rotation = targetRotation;
    //         isRotating=false;
    //         // Continuously follow the target's rotation
    //     }
    // }
 
}
