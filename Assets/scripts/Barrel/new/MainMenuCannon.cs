using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCannon : BarrelScript
{
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {


            //barrelShootPlayer is the main function that shoot the thing 
            PlayerEnterBarrel(collision);

            BarrelShootPlayer();
 

        }
    }
}