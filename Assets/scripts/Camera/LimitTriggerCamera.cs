using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitTriggerCamera : MonoBehaviour
{
    // use this if 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")){
            this.transform.parent.GetComponent<FollowCamera>().lerp();
        }
    }
}
