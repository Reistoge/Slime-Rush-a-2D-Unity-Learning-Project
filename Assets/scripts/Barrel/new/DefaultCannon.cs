using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class DefaultCannon : BarrelScript
{
     IEnumerator RotateCannon;
         

    // Start is called before the first frame update



    private void Start()
    {
        initRot = transform.rotation.eulerAngles.z;
        createQueue(Oscilate_and_vel);
        
        
         

    }

    void Update()
    {
      


        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && inBarrel  &&canShoot)
        {
            //shoot = true;
            Vector3 currentPos = transform.position;
            StopAllCoroutines();
            transform.position = currentPos;
            gameObject.GetComponent<Animator>().SetTrigger("Shoot");
            // we wait until the shoot anim stops
            Invoke("BarrelShootPlayer",1f);
            // anim

            rotatingTo = true;
            canShoot = false;
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
    private void OnTriggerEnter2D(Collider2D collision)    {
            StopAllCoroutines();
            PlayerEnterBarrel(collision);

            // RotateCannon = RotateBetweenAngle(rotAngle,rotSpeed);
            RotateCannon=DequeueCoroutines(coroutineQueue, rotDelay, Oscilate_and_vel);

            StartCoroutine(RotateCannon);
             
         
    }
    private void OnTriggerExit2D(Collider2D collision)
    {

                StopCoroutine(RotateCannon);
                StartCoroutine(RotateToAngle(initRot , 100));

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

}
