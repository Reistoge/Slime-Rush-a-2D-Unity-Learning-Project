using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Dasher : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.GetComponent<PlayerScript>() != null)
        {
            
            PlayerScript playerScript = col.gameObject.GetComponent<PlayerScript>();
            if (playerScript.IsDashing)
            {
                col.transform.position= transform.position;

                playerScript.dash(playerScript.DashCoroutineTime,playerScript.DashCoroutineSpeed,transform.up);

            }
        }
    }
  
}
