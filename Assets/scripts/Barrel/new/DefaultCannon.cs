using System.Collections;
using UnityEngine;

public class DefaultCannon : Cannon
{
    IEnumerator RotateCannon;

    
    CannonSoundSystem soundSystem;
    const float chargeMultiplier=1.3f;

    private void Start()
    {
         
        initRot = transform.rotation.eulerAngles.z;
        createQueueRotateAngle(Oscilate_and_vel);
        soundSystem = gameObject.GetComponent<CannonSoundSystem>();



    }

    void Update()
    {



        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && inBarrel )
        {
            if (canShoot)
            {
                //shoot = true;
                Vector3 currentPos = transform.position;
                StopAllCoroutines();
                transform.position = currentPos;
                gameObject.GetComponent<Animator>().Play("shoot");


                // wef wait until the shoot anim stops
                //Invoke("BarrelShootPlayer",gameObject.GetComponent<Animator>());
                // anim

                rotatingTo = true;
                canShoot = false;
            }
            gameObject.GetComponent<Animator>().SetFloat("chargeSpeed", gameObject.GetComponent<Animator>().GetFloat("chargeSpeed") * chargeMultiplier);

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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        canShoot = true;

        StopAllCoroutines();
        PlayerEnterBarrel(collision);

        // RotateCannon = RotateBetweenAngle(rotAngle,rotSpeed);
        RotateCannon = DequeueCoroutines(coroutineQueue, rotDelay, Oscilate_and_vel);

        StartCoroutine(RotateCannon);


    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        gameObject.GetComponent<Animator>().SetFloat("chargeSpeed", 1);
        StopCoroutine(RotateCannon);
        Invoke("rotateToInitialRotation", 1.5f);
    }
    public void rotateToInitialRotation()
    {
        StartCoroutine(RotateToAngle(initRot, 100));

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
