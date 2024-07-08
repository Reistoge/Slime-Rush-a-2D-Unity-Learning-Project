using System;
using System.Collections;
using UnityEngine;
public class Enemy : MonoBehaviour,IDamageable
{
    // create a difficult state.
    GameObject player;
    enum Rotations
    {
        Example1, Example2, Example3
    }
    // create a instance of the enum
    [SerializeField] Rotations EnemyRotations;

    int hp;
    int maxHp;
    float rotAngle;
    float rotSpeed;
    float rotDelay = 1;


    public void takeDamage(int d)
    {
        hp -= d;
        if(hp <= 0) {
        
            die();
        }

    }

    public void die()
    {
        Destroy(gameObject);
    }

   

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        // check which rotation is max:10
    //    switch (EnemyRotations)
    //    {
    //        case Rotations.Example1:

    //            StartCoroutine(RotateBetweenAngle(-45, 45));
    //            print("example1, -45, 45");
    //            break;
    //        case Rotations.Example2:
    //            StartCoroutine(RotateBetweenAngle(45, 45));
    //            print("example2, 45, 45");
    //            break;
    //        case Rotations.Example3:
    //            StartCoroutine(RotateBetweenAngle(135, 45));
    //            print("example3, 135, 45");
    //            break;
    //    }
    }
    //public IEnumerator RotateAngles(float angle, float speed)
    //{



    //    // NOW IS precise, THE REASON IS QUATERNION.ROTATETOWARDS, the method used before was gameobject.rotate(), less accurate.

    //    float rotateTime = 0;


    //    Quaternion endRot = Quaternion.Euler(0f, 0f, angle + transform.rotation.eulerAngles.z);
    //    // print(endRot.eulerAngles);
    //    // Rotate clockwise 

    //    while (transform.rotation != endRot)
    //    {



    //        rotateTime += Time.deltaTime;
    //        transform.rotation = Quaternion.RotateTowards(transform.rotation, endRot, speed * Time.deltaTime);
    //        //TIME.DELTATIME = DEGREES PERSECOND.
    //        // speed*Time.deltaTime== speed degrees per second
    //        // if you rotate 90 degrees and the speed is 90 it will take 1 second.  
    //        // if speed is 45 it will take 2 seconds
    //        // SPEED=ROTATION/TIME
    //        // TIME=ROTATION/SPEED
    //        yield return new WaitForEndOfFrame(); // Yield to the next frame


    //    }



    //}
    //public IEnumerator RotateBetweenAngle(float angle, float rotSpeed)
    //{
    //    // when the cannon is in stop time the player has the chance to shoot.
    //    // for some reason when the cannon shoots the player it rotates again.

    //    yield return new WaitForSeconds(1);

    //    while (true)
    //    {
    //        // its needs to be more precise...
    //        float waitTime = (Math.Abs(angle) / rotSpeed) + rotDelay;
    //        print(waitTime);



    //        StartCoroutine(RotateAngles(-angle, rotSpeed));
    //        //arrow


    //        yield return new WaitForSeconds(waitTime);


    //        StartCoroutine(RotateAngles(angle, rotSpeed));



    //        yield return new WaitForSeconds(waitTime);



    //    }
    //}


    public int Hp { get => hp; set => hp = value; }
    public int MaxHp { get => maxHp; set => maxHp = value; }
    public float RotAngle { get => rotAngle; set => rotAngle = value; }
    public float RotSpeed { get => rotSpeed; set => rotSpeed = value; }
    public float RotDelay { get => rotDelay; set => rotDelay = value; }

}
