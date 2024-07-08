using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class physicsBehaviour : MonoBehaviour
{
    // ienumerator queue basically it waits to the coroutine to finsih and then start the another corutine sequentially.

    // crear un array de rotaciones que los tome los convierta en un array de Ienumarator para despues añadirlos a la queue
    // documentar y arreglar esta wea, funciona magicamente.

    [Serializable]
    public class Values
    {
        [SerializeField] public float Angles;
        [SerializeField] public float Velocity;
    }
    [SerializeField] Values[] Oscilate_and_vel;


    Queue<IEnumerator> coroutineQueue = new Queue<IEnumerator>();

    void createQueue(Values[] values)
    {

        IEnumerator coroutine = null;
        foreach (Values v in values)
        {
            coroutine = RotateToAngle(v.Angles, v.Velocity);
            coroutineQueue.Enqueue(coroutine);
        }
    }
    IEnumerator DequeueCoroutines(Queue<IEnumerator> CoroutineQueue, float delay, Values[] values)
    {

        while (coroutineQueue.Count > 0)
        {
            yield return new WaitForSeconds(0.1f + delay);
            yield return coroutineQueue.Dequeue();
            if (coroutineQueue.Count == 1)
            {
                createQueue(values);
            }
        }
    }















    [Header("Debug RotateAngles")]
    [SerializeField] float angles;
    [SerializeField] float rotateAngleSpeed;
    protected IEnumerator RotateToAngle(float finalRotation, float rotSpeed)
    {
        if (finalRotation == 180 || finalRotation == -180)
        {
            finalRotation *= -1;
        }
        yield return new WaitForSeconds(1f); // Optional delay before starting rotation

        // Calculate the initial rotation adjustment
        float currentRotation = transform.rotation.eulerAngles.z;
        float rot = finalRotation - currentRotation;





        while (transform.rotation != Quaternion.Euler(0f, 0f, finalRotation))
        {



            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0f, 0f, finalRotation), rotSpeed * Time.deltaTime);


            yield return new WaitForEndOfFrame(); // Yield to the next frame


        }
        transform.rotation = Quaternion.Euler(0f, 0f, finalRotation);

        // Print the final rotation angle (optional)


    }

    protected IEnumerator RotateAngles(float rotation, float speed)
    {

        float rotateTime = 0;

        // wrong
        Quaternion endRot = Quaternion.Euler(0f, 0f, rotation + transform.rotation.eulerAngles.z);
        print("parameter: " + rotation + "  current Rotation: " + transform.rotation.eulerAngles.z + " final Rotation:" + endRot.eulerAngles.z);
        // print(endRot.eulerAngles);
        // Rotate clockwise 

        while (transform.rotation != endRot)
        {



            rotateTime += Time.deltaTime;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, endRot, speed * Time.deltaTime);
            //TIME.DELTATIME = DEGREES PERSECOND.
            // speed*Time.deltaTime== speed degrees per second
            // if you rotate 90 degrees and the speed is 90 it will take 1 second.  
            // if speed is 45 it will take 2 seconds
            // SPEED=ROTATION/TIME
            // TIME=ROTATION/SPEED
            yield return new WaitForEndOfFrame(); // Yield to the next frame


        }
        transform.rotation = endRot;
        //print((speed * Time.deltaTime));


    }
    public IEnumerator RotateBetweenAngle(float angle, float rotSpeed)
    {
        // when the cannon is in stop time the player has the chance to shoot.
        // for some reason when the cannon shoots the player it rotates again.
        if (transform.rotation != Quaternion.Euler(transform.rotation.x, transform.rotation.y, angle)) { StartCoroutine(RotateToAngle(angle, rotSpeed)); }
        yield return new WaitForSeconds(1);

        while (true)
        {
            // its needs to be more precise...
            float waitTime = (Math.Abs(angle) / rotSpeed) + delay;
            print(waitTime);



            StartCoroutine(RotateAngles(-angle, rotSpeed));
            //arrow


            yield return new WaitForSeconds(waitTime);


            StartCoroutine(RotateAngles(angle, rotSpeed));



            yield return new WaitForSeconds(waitTime);



        }
    }




    [Header("Debug dash/constant velocity")]

    [Tooltip("direction of the velocity, in what direction points the velocity.")]
    [SerializeField] Vector2 direction;

    [Tooltip("Pixels per second.")]
    [SerializeField] float velocity;

    [Tooltip("Time with that velocity.")]
    [SerializeField] float velTime;
    protected IEnumerator constantVelocityWithRigidbody(GameObject Player, float time, float velocity, Vector3 direction)
    {
        // time: time with that rotation.

        // velocity: pixels per second.
        // direction: in which direction do you want to translate the object. 
        // endpos: final pos of the object.
        // initG: initial gravitation.
        // dashDirection: the direction and speed, it will go in that direction with that speed.
        //precise

        Vector3 endPos = new Vector3(transform.position.x + (velocity * time * direction.x), transform.position.y + (velocity * time * direction.y), transform.position.z);
        float initG = Player.GetComponent<Rigidbody2D>().gravityScale;
        Player.GetComponent<Rigidbody2D>().gravityScale = 0;
        Vector3 dashDirection = direction * velocity;
        print("Constant vel Start");

        // moving

        while (time > 0)
        {

            Player.GetComponent<Rigidbody2D>().velocity = dashDirection;
            time -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        // end moving
        transform.position = endPos;

        Player.GetComponent<Rigidbody2D>().gravityScale = initG;
        Player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;


    }


    //[SerializeField] float dashTime;
    [Header("Debug RotateFor ")]

    [Tooltip("Time with that rotation")]
    [SerializeField] float rotTime;

    [Tooltip("Degrees per second")]
    [SerializeField] float dragVel;
    protected IEnumerator RotateFor(GameObject Player, float time, float RotVel)
    {
        // RotVel: degrees per second.
        // time: time with that velocity.
        //precise
        Quaternion initRot = Player.transform.rotation;
        Quaternion endRot = Quaternion.Euler(initRot.eulerAngles.x, initRot.eulerAngles.y, initRot.eulerAngles.z + (RotVel * time));

        print(initRot);
        print("ConstantDrag Start");
        while (time > 0)
        {
            // for n seconds the angular velocity will be RotVel.
            Player.GetComponent<Rigidbody2D>().angularVelocity = RotVel;
            time -= Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
        // this is when the player rotates 360 degrees.
        Player.transform.rotation = endRot;
        Player.GetComponent<Rigidbody2D>().angularVelocity = 0;
    }


    [Header("Debug RotateBetween ")]
    [Tooltip("Degrees that will oscilate this object")]
    [SerializeField] float OscilateDegrees;

    [Tooltip("Degrees per second")]
    [SerializeField] float OscilateVel;

    [Min(0.05f), Tooltip("time that take to the object to start rotating again  (in the opposite direction) ")]
    [SerializeField] public float delay;
    protected IEnumerator RotateToAngleBetween(GameObject Object, float OscilateDegrees1, float OscilateDegrees2, float Velocity)
    {
        //precise.

        // ex: this is the question, if you want to rotate a object "X" degrees with a "Z" velocity how much time it will take to rotate it ?
        // my problem here was calculating the time that takes the rotation because RotateFor rotate the object by X time.
        float waitTime1 = Math.Abs(OscilateDegrees1 / Velocity);
        float waitTime2 = Math.Abs(OscilateDegrees2 / Velocity);
        while (true)
        {
            // EXPLANATION: lets say OscilateDegrees is 180 so:

            // first rotate 180 degrees, this is because "180 degrees persecond * seconds = ".
            StartCoroutine(RotateFor(Object, waitTime1, Velocity));
            // can shoot  
            // number has to be always a little bit higher than 1 in this case the time.
            yield return new WaitForSeconds(waitTime1 + delay);

            StartCoroutine(RotateFor(Object, waitTime1, -Velocity));

            yield return new WaitForSeconds(waitTime2 + delay);
        }

    }


    private void Start()
    {

        createQueue(Oscilate_and_vel);
        StartCoroutine(DequeueCoroutines(coroutineQueue, 1, Oscilate_and_vel));




    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {


            StopAllCoroutines();

        }




        if (Input.GetMouseButtonDown(1))
        {

            StopAllCoroutines();
            constantVelocityWithRigidbody(gameObject, rotTime, velocity, transform.up);
            //StartCoroutine(RotateTo(rotation, speed));

            //StartCoroutine(Rotate(-rotation * 2, 100));
        }
        if (Input.GetKey(KeyCode.R))
        {
            gameObject.GetComponent<Rigidbody2D>().angularVelocity = 30f; ;

        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            gameObject.GetComponent<Rigidbody2D>().angularVelocity = 0;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            gameObject.GetComponent<Rigidbody2D>().angularVelocity = -30f; ;

        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            gameObject.GetComponent<Rigidbody2D>().angularVelocity = 0;
        }


    }









}

