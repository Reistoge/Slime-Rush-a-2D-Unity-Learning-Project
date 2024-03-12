using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FirstCannon : BarrelScript
{
     IEnumerator firstBarrelRotate;
    void Update()
    {



        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && inBarrel  )
        {


            StopCoroutine(firstBarrelRotate);
            BarrelShootPlayer();




        }


        if (inBarrel)
        {
            //    Vector3 barrel_pos = new Vector3(transform.position.x,transform.position.y+1,transform.position.z);
            Player.transform.position = transform.position;
            ////

            //Vector3 barrel_pos = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
            //Player.transform.position = barrel_pos;
        }




    }
    private void Start()
    {
        createQueue(Oscilate_and_vel);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
             

            PlayerEnterBarrel(collision);

             
            OnEnterFirstBarrel?.Invoke();
             

            firstBarrelRotate = DequeueCoroutines(coroutineQueue,rotDelay,Oscilate_and_vel);

            StartCoroutine(firstBarrelRotate);
        }

        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {



        StartCoroutine(RotateToAngle(0, 5));



    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            //here could be something // make the barrel stops and then shoots

            string components = transform.up.ToString();
            GameManager.instance.TransformUpVectorChars = components;
            collision.gameObject.GetComponent<Transform>().rotation = transform.rotation;




        }


        // pensar en un sistema automatizado de niveles.


    }
    public void FirstShoot()
    {

         
        StopCoroutine(firstBarrelRotate);
        PlayerBody.constraints = RigidbodyConstraints2D.None;
        inBarrel = false;
        GameManager.instance.InBarrel = false;

        //StartCoroutine(BarrelDash(CoolDown, forceBarrel));
        PerformCannonDash();

        PlayerBody.GetComponent<SpriteRenderer>().enabled = true;



        print("barrel Shoot first" % Colorize.Magenta);
        if (Player != null)
        {
            Player.GetComponent<PlayerScript>().Dashing = false;
        }







    }

}
