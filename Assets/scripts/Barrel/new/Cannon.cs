
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Cannon : MonoBehaviour
{
    public static Action OnEnterFirstBarrel;
    public static Action OnEnterFinalBarrel;

    IEnumerator resetGravity;
    IEnumerator ConstantVel;
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
    protected void createQueueRotateAngle(Rotations[] values)
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
            if (coroutineQueue.Count == 1 && AlwaysRotate)
            {
                createQueueRotateAngle(values);
            }
        }
    }








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
     float initG;
    float initD;
    [Header("Cannon")]
    [Range(0f, 1000f)]
    [Tooltip("Pixels per second")]
    [SerializeField] public float dashSpeed;
    [Tooltip("fixed final speed when the cannon shoot time ends")]
    [SerializeField] public float downSpeed;
    [Range(0f, 20f)]
    [Tooltip("Time with that velocity")]
    [SerializeField] public float dashTime;
    [Range(0f, 1000)]
    [Tooltip("Rotation with the dash")]
    [SerializeField] public float dashAngularSpeed;
    [Min(0), Tooltip("time that take to the object to start rotating again (in the opposite direction) ")]
    [SerializeField] public float rotDelay;











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

        GameManager.instance.LastUsedBarrel = this.gameObject;
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
        Arrow.ResetTrigger("in");
        if (finalRotation == 180 || finalRotation == -180)
        {
            finalRotation *= -1;
        }
        //yield return new WaitForSeconds(1f); // Optional delay before starting rotation

        // Calculate the initial rotation adjustment
        float currentRotation = transform.rotation.eulerAngles.z;
        float rot = finalRotation - currentRotation;



        if (inBarrel)
        {
            gameObject.GetComponent<CannonSoundSystem>().playRotateSfx();

        }
        while (transform.rotation != Quaternion.Euler(0f, 0f, finalRotation))
        {
            canShoot = false;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0f, 0f, finalRotation), rotSpeed * Time.deltaTime);




            gameObject.GetComponent<CannonSoundSystem>().downVolume(0.005f);

            yield return new WaitForEndOfFrame(); // Yield to the next frame


        }

        if (inBarrel)
        {
            gameObject.GetComponent<CannonSoundSystem>().playReadySfx();

        }
        transform.rotation = Quaternion.Euler(0f, 0f, finalRotation);
        Arrow.SetTrigger("in");
        canShoot = true;



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
    public IEnumerator RotateAngles2(GameObject Object, float angle, float RotVel, float rotDelay)
    {
        float waitTime = Math.Abs(angle / RotVel);
        StartCoroutine(RotateFor(gameObject, waitTime, RotVel));
        yield return new WaitForSeconds(waitTime + rotDelay);
    }
    public IEnumerator applyConstantVelocityToRigidbody(GameObject Player, float time, float velocity, Vector3 direction)
    {
        // time: time with that rotation.

        // velocity: pixels per second.
        // direction: in which direction do you want to translate the object. 
        // endpos: final pos of the object.
        // initG: initial gravitation.
        // dashDirection: the direction and speed, it will go in that direction with that speed.
        //precise

        Vector3 endPos = new Vector3(transform.position.x + (velocity * time * direction.x), transform.position.y + (velocity * time * direction.y), transform.position.z);
        initG = Player.GetComponent<Rigidbody2D>().gravityScale;
        initD = Player.GetComponent<Rigidbody2D>().drag;

        Vector3 dashDirection = direction * velocity;
        print("Constant vel Start");

        // moving

        Player.GetComponent<Rigidbody2D>().gravityScale = 0;
        Player.GetComponent<Rigidbody2D>().drag = 0;
        Player.GetComponent<Rigidbody2D>().velocity = dashDirection;

        yield return new WaitForSeconds(time);


        Player.GetComponent<Rigidbody2D>().velocity *= 1;

        Player.GetComponent<Rigidbody2D>().drag = initD;
        Player.GetComponent<Rigidbody2D>().gravityScale = initG;
        Player.GetComponent<PlayerScript>().GetComponent<Animator>().SetBool("dash", false);
        Player.GetComponent<PlayerScript>().GetComponent<PlayerScript>().Dash = false;
        //Player.GetComponent<Rigidbody2D>().velocity = dashDirection/2;


    }
     
    public void StopCannonDash()
    {
        StopCoroutine(this.ConstantVel);
        Player.GetComponent<Rigidbody2D>().velocity *= 1;
        Player.GetComponent<Rigidbody2D>().drag = initD;
        Player.GetComponent<Rigidbody2D>().gravityScale = initG;
        Player.GetComponent<PlayerScript>().GetComponent<Animator>().SetBool("dash", false);
        Player.GetComponent<PlayerScript>().GetComponent<PlayerScript>().Dash = false;
    }
    public void PerformCannonDash()
    {
        //2
        if (Player != null)
        {

            Player.GetComponent<PlayerScript>().GetComponent<Animator>().SetTrigger("dash");
            Player.GetComponent<PlayerScript>().GetComponent<PlayerScript>().Dash = true;

            

        }
        
        //StartCoroutine(RotateFor(Player,dashTime *0.8f,dashAngularSpeed));


        ConstantVel = applyConstantVelocityToRigidbody(Player, dashTime, dashSpeed, transform.up);
        StartCoroutine(ConstantVel);




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
        GameManager.instance.shakeCamera(shakeType.lite);
        PerformCannonDash();
        // event.


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
    public float DashSpeed { get => dashSpeed; set => dashSpeed = value; }
    public float RotDelay { get => rotDelay; set => rotDelay = value; }

}
