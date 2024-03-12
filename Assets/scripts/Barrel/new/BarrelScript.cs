
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static UnityEditor.FilePathAttribute;




public class BarrelScript : MonoBehaviour
{

    //refactor.
    [Serializable]
    public class Rotations
    {
        [SerializeField] public float Angles;
        [SerializeField] public float Velocity;
    }

     
    
    
    [SerializeField] protected Rotations[] Oscilate_and_vel;
  
     
    [SerializeField] protected bool AlwaysRotate;
    // this array is the queue in array form you have to convert it in start with the createQueue method.
    // basically createqueue convert the class values to a Ienumerator Queue and DequeueCoroutines starts all the coroutines in order 

    protected Queue<IEnumerator> coroutineQueue = new Queue<IEnumerator>();
    protected void createQueue(Rotations[] values)
    {

        IEnumerator coroutine = null;
        foreach (Rotations v in values)
        {
            coroutine = RotateToAngle(v.Angles, v.Velocity);
            coroutineQueue.Enqueue(coroutine);
        }
    }
    protected IEnumerator DequeueCoroutines(Queue<IEnumerator> CoroutineQueue, float delay, Rotations[] values)
    {

        while (coroutineQueue.Count > 0)
        {
            yield return new WaitForSeconds(0.1f + delay);
            yield return coroutineQueue.Dequeue();
            if (coroutineQueue.Count == 1 && AlwaysRotate )
            {
                createQueue(values);
            }
        }
    }


    public static Action OnEnterFirstBarrel;
    public static Action OnEnterFinalBarrel;
    IEnumerator resetGravity;




    /// <summary>
    /// 
    /// i want to make a final barrel event so to this will make a final barrel prefab and in the barrel script i will make a 
    /// is_final bool and i will serialized, to change in the unity editor if the barrel is the final the is_final bool is true
    /// when the player enters to this object then he will activate the next scene.
    /// this next scene is the player flying due to the giant force of this barrel this scene is supossed to be a give reward, think the part in the kirby
    /// nightmare in dreamland, then the player start over a new scene
    /// 
    /// </summary>



    protected GameObject Player;
    protected private Rigidbody2D PlayerBody;

    protected bool shoot = false;





    bool isFirst;
    bool isFinal;
    protected bool IsMainMenu;
    private float mainMenuBarrelTime = 4;
    private Vector3 rotateComponents = new Vector3(0, 0, 1);
    Vector2 barrelDashVector;
    protected float initRot;
    float time = 0;
    [Header("States")]
    [SerializeField] protected Animator Arrow;
    [SerializeField] protected bool canShoot = false;
    [SerializeField] protected bool rotatingTo;
    [SerializeField] public bool inBarrel;

    [Header("Cannon")]
    [Range(0f, 1000f)]
    [Tooltip("The dash Speed of the barrel")]
    [SerializeField] public float dashSpeed;
    [Range(0f, 20f)]
    [Tooltip("Time with that velocity")]
    [SerializeField] public float dashTime;
    [Range(0f, 1000)]
    [Tooltip("Rotation with the dash")]
    [SerializeField] public float dashAngularSpeed;
    [Min(0.3f), Tooltip("time that take to the object to start rotating again (in the opposite direction) ")]
    [SerializeField] public float rotDelay;


    //[Header("Rotate Variables")]
    //[Range(-360f, 360f)]
    //[Tooltip("This object will rotate X degrees and then -X degrees, independent of the actual rotation")]
    //[SerializeField] protected float rotAngle;

    //[Range(0, 360f)]
    //[Tooltip("Degrees per second")]
    //[SerializeField] protected float rotSpeed;

    //[Min(0.3f), Tooltip("time that take to the object to start rotating again (in the opposite direction) ")]
    //[SerializeField] protected float rotDelay;



    //[Header("Dash Variables")]
    //[Range(0f, 1000f)]
    //[Tooltip("The dash Speed of the barrel")]
    //[SerializeField] protected float dashSpeed;
    //[Range(0f, 20f)]
    //[Tooltip("Time with that velocity")]
    //[SerializeField] protected float dashTime;
    //[Range(0f, 1000)]
    //[Tooltip("Rotation with the dash")]
    //[SerializeField] protected float dashAngularSpeed;




    private void OnEnable()
    {
        restartButton.StopCoroutines += StopAllCoroutines;
    }
    private void OnDisable()
    {
        restartButton.StopCoroutines -= StopAllCoroutines;
    }

    protected void PlayerEnterBarrel(Collider2D collision)
    {
        Player = collision.gameObject;
        inBarrel = true;

        GameManager.instance.InBarrel = inBarrel;
        Player.transform.position = transform.position;



        transform.GetChild(0).gameObject.SetActive(true);
        collision.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        collision.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;








        collision.gameObject.GetComponent<Rigidbody2D>().angularVelocity = 0;
        PlayerBody = collision.gameObject.GetComponent<Rigidbody2D>();

    }
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

    //protected IEnumerator RotateToAngle(float finalRotation, float rotSpeed)
    //{

    //    yield return new WaitForSeconds(1f); // Optional delay before starting rotation

    //    // Calculate the initial rotation adjustment
    //    float currentRotation = transform.rotation.eulerAngles.z;




    //    while (transform.rotation != Quaternion.Euler(0f, 0f, finalRotation))
    //    {



    //        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0f, 0f, finalRotation), rotSpeed * Time.deltaTime);


    //        yield return new WaitForEndOfFrame(); // Yield to the next frame


    //    }
    //    transform.rotation = Quaternion.Euler(0f, 0f, finalRotation);

    //    // Print the final rotation angle (optional)


    //}




    //public IEnumerator RotateBetween(float OscilateDegrees1, float OscilateDegrees2, float Velocity)
    //{
    //    //precise.

    //    // ex: this is the question, if you want to rotate a object "X" degrees with a "Z" velocity how much time it will take to rotate it ?
    //    // my problem here was calculating the time that takes the rotation because RotateFor rotate the object by X time.
    //    float waitTime1 = Math.Abs(OscilateDegrees1 / Velocity);
    //    float waitTime2 = Math.Abs(OscilateDegrees2 / Velocity);
    //    while (true)
    //    {
    //        // EXPLANATION: lets say OscilateDegrees is 180 so:

    //        // first rotate 180 degrees, this is because "180 degrees persecond * seconds = 180 ".
    //        StartCoroutine(RotateFor(gameObject, waitTime1, Velocity));
    //        // can shoot  
    //        // number has to be always a little bit higher than 1 in this case the time.
    //        yield return new WaitForSeconds(waitTime1 + rotDelay);

    //        StartCoroutine(RotateFor(gameObject, waitTime2, -Velocity));

    //        yield return new WaitForSeconds(waitTime2 + rotDelay);
    //    }

    //}

    public IEnumerator RotateFor(GameObject Player, float time, float RotVel)
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
    public IEnumerator RotateAngles2(GameObject Object, float angle, float RotVel,float rotDelay)
    {
        float waitTime = Math.Abs(angle / RotVel);
        StartCoroutine(RotateFor(gameObject, waitTime, RotVel));
        yield return new WaitForSeconds(waitTime + rotDelay);
    }


    public IEnumerator ConstantVelocityWithRigidbody(GameObject Player, float time, float velocity, Vector3 direction)
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
        // Player.transform.position = endPos; ??

        Player.GetComponent<Rigidbody2D>().gravityScale = initG;
        Player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;


    }


    IEnumerator BarrelDashMainMenu()
    {
        if (Player != null)
        {
            Player.GetComponent<PlayerScript>().Dashing = true;
        }
        float originalGravity = GameManager.instance.PlayerMainMenuGravityScale;
        float originalDrag = GameManager.instance.PlayerMainMenuDrag;
        // a function or a thing to get the speed in the scene that is the player 

        PlayerBody.gravityScale = 0f;
        PlayerBody.drag = 0;
        // forceBarrel
        float directionX = transform.up.x;
        float directionY = transform.up.y;
        PlayerBody.velocity = new Vector2(directionX, 1) * GameManager.instance.MainMenuBarrelForce;

        yield return new WaitForSeconds(GameManager.instance.MainMenuBarrelCooldown);
        PlayerBody.gravityScale = originalGravity;
        PlayerBody.drag = originalDrag;
        if (Player != null)
        {
            Player.GetComponent<PlayerScript>().Dashing = false;
        }



    }
    IEnumerator resetAngularSpeed()
    {
        yield return new WaitForSeconds(dashTime / 2);
        PlayerBody.angularVelocity = 0;


    }
    public void PerformCannonDash()
    {
        //2
        if (Player != null)
        {
            Player.GetComponent<PlayerScript>().GetComponent<Animator>().SetBool("dash", true);

        }
        StartCoroutine(RotateFor(Player,dashTime *0.8f,dashAngularSpeed));
        StartCoroutine(ConstantVelocityWithRigidbody(Player, dashTime, dashSpeed, transform.up));


        //float originalGravity = GameManager.instance.PlayerMainGameGravityScale;
        //float originalDrag = GameManager.instance.PlayerMainGameDrag;
        //resetGravity = (ResetGravityAndDrag(originalGravity, originalDrag));


        ////// Disable gravity and drag during the dash
        //PlayerBody.gravityScale = 0f;
        //PlayerBody.drag = 0;

        //// Calculate the dash direction (you can customize this based on your game)
        //Vector2 dashDirection = new Vector2(transform.up.normalized.x * dashSpeed, transform.up.normalized.y * dashSpeed);
        //PlayerBody.angularVelocity = dashAngularSpeed;

        ////PlayerBody.AddForce(dashDirection * dashForce,ForceMode2D.Impulse);

        //// Apply the dash force
        //PlayerBody.velocity = dashDirection;
        //StartCoroutine(resetGravity);
        //StartCoroutine(resetAngularSpeed());
        //print(dashDirection);

    }


    private IEnumerator ResetGravityAndDrag(float gravity, float drag)
    {
        yield return new WaitForSeconds( dashTime);
        print("dash time finished");
        //// Restore original gravity and drag
        PlayerBody.gravityScale = gravity;
        PlayerBody.drag = 0;


        //PlayerBody.velocity = Vector2.zero;

        // this isnt the error
        //yield return new WaitForSeconds(4);
        if (Player != null)
        {
            Player.GetComponent<PlayerScript>().GetComponent<Animator>().SetBool("dash", false); ;
        }
    }



    IEnumerator FinalBarrelShoot(float Time_animation, float Barrel_time_anim)
    {
        yield return new WaitForSeconds(Barrel_time_anim);

        //BarrelShootPlayer(GameManager.instance.FinalBarrelCooldown, GameManager.instance.FinalBarrelForce);
        yield return new WaitForSeconds(Time_animation);

        SceneManager.LoadScene("REWARD_SCENE_1");


    }
    IEnumerator ShootPlayerIn(float Time, string barrel)
    {
        if (barrel == "mainMenu")
        {
            yield return new WaitForSeconds(Time);
            BarrelShootPlayerMainMenu();
        }
        else
        {
            yield return new WaitForSeconds(Time);
            BarrelShootPlayer();

        }



    }
    public void BarrelShootPlayerMainMenu()
    {



        transform.GetChild(0).gameObject.SetActive(false);
        PlayerBody.constraints = RigidbodyConstraints2D.None;
        inBarrel = false;
        GameManager.instance.InBarrel = false;

        StartCoroutine(BarrelDashMainMenu());

        PlayerBody.GetComponent<SpriteRenderer>().enabled = true;



        print("barrel Shoot first" % Colorize.Magenta);







    }

    protected void BarrelShootPlayer()
    {
        //1

        // barrel anim

        //transform.GetChild(0).gameObject.SetActive(false);
        // player 
        PlayerBody.constraints = RigidbodyConstraints2D.None;
        inBarrel = false;
        GameManager.instance.InBarrel = false;

        //StartCoroutine(BarrelDash(coolDown,forceBarrel));
        PerformCannonDash();

        PlayerBody.GetComponent<SpriteRenderer>().enabled = true;








    }
    public float MainMenuBarrelTime
    {
        get { return mainMenuBarrelTime; }
        set { mainMenuBarrelTime = value; }
    }
    //public float ForceBarrel
    //{
    //    get { return forceBarrel; }
    //    set { forceBarrel = value; }
    //}


    // i use this variable to check if in the inspector the barrel is the first(used for the start of the game)
    public bool Is_First
    {
        get { return isFirst; }
        set { isFirst = value; }
    }

    // public float CoolDown { get => coolDown; set => coolDown = value; }
    public float DashSpeed { get =>  dashSpeed; set =>  dashSpeed = value; }
    public float RotDelay { get =>  rotDelay; set =>  rotDelay = value; }

}
