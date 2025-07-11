using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
public class Cannon : MonoBehaviour
{

    protected void OnEnable()
    {

        restartButton.StopCoroutines += StopAllCoroutines;
         

        // GameManager.Instance.instantiateAppearEffect(transform, 0);


    }
    protected void OnDisable()
    {
        restartButton.StopCoroutines -= StopAllCoroutines;
       
    }
    protected void Start()
    {


        arrowAnim = transform.GetChild(0).GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        soundSystem = gameObject.GetComponent<CannonSoundSystem>();
        initRot = transform.rotation.eulerAngles.z;
        anim = GetComponent<Animator>();




    }

    //refactor.

    // this array is the queue in array form you have to convert it in start with the createQueue method.
    // basically createqueue convert the class values to a Ienumerator Queue and DequeueCoroutines starts all the coroutines in order 




    protected Animator anim;
    protected Animator arrowAnim;
    [Header("Cannon State")]
    [SerializeField] protected bool canShoot = false;
    [SerializeField] protected bool isRotating;
    [SerializeField] public bool inBarrel;

    [Header("Cannon")]

    [SerializeField][Range(0f, 1000f)][Tooltip("Pixels per second")] public float dashSpeed;


    [SerializeField][Range(0f, 1000)][Tooltip("applyConstantVelocityToRigidbody() time duration")] public float dashTime;

    [SerializeField][Min(0)][Tooltip("Time that take to the object to start rotating again (in the opposite direction) ")] public float rotDelay;
    private float mainMenuBarrelTime = 4;
    protected float initRot;
    float initG;
    float initD;
    protected GameObject insideObject;
    protected private Rigidbody2D insideRb;
    IEnumerator ConstantVel;
    protected CannonSoundSystem soundSystem;
    protected SpriteRenderer sprite;
    [SerializeField] protected bool isFinal;
    [SerializeField] protected bool isFirst;
    [SerializeField] protected bool isAutoShoot;
    [SerializeField] protected bool canMoveInShoot = true;
    [SerializeField] protected bool isCharging;
    [SerializeField] float horizontalThreshold;
    [SerializeField] protected UnityEvent onEnterCannon;
    [SerializeField] protected UnityEvent onExitCannon;


    public void resetIsCharging() {
        isCharging = false;
    }
    protected void enterInsideCannon(Collider2D collision)
    {

        // have an animation to the slime that says that is inside of the cannon.
        stopCannonDash();
        if (collision.CompareTag("Player"))
        {

            arrowAnim.SetBool("canShoot", true);
            anim.Play("onEnterEntity", -1, 0f);
            GameManager.Instance.LastUsedBarrel = this.gameObject;
            inBarrel = true;

            GameManager.Instance.InBarrel = inBarrel;
            GameManager.Instance.CanMove = false;

            // make a call from player.
            insideObject = collision.gameObject;
            insideObject.transform.position = transform.position;
            insideObject.transform.rotation = transform.rotation;
            insideObject.transform.SetParent(transform);
            transform.GetChild(0).gameObject.SetActive(true);
            insideObject.transform.SetParent(transform);
            //collision.gameObject.GetComponent<PlayerScript>().Sr.enabled = false;
            onEnterCannon?.Invoke();
            if (collision.GetComponent<Rigidbody2D>() != null)
            {
                insideRb = collision.gameObject.GetComponent<Rigidbody2D>();
                insideRb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
                insideRb.angularVelocity = 0;

            }
            if (insideObject.GetComponent<PlayerScript>() != null)
            {
                // POR HACER: 
                // esperar hasta que el barril termine de hacer la animacion de "apretarse".
                StartCoroutine(waitForAnimation());


            }
            if (isAutoShoot)
            {
                insideCannonAction();

            }
        }

    }

    IEnumerator waitForAnimation()
    {
        if (insideObject.CompareTag("Player"))
        {
            insideObject.GetComponent<PlayerScript>().AnimatorHandler.playInvisible();
            yield return new WaitForSeconds(anim.GetCurrentAnimatorClipInfo(0).Length);
            if (GameManager.Instance.InBarrel && inBarrel)
            {
                insideObject.GetComponent<PlayerScript>().AnimatorHandler.enterBarrel();

            }
        }




    }
    protected void playPlayerEnterBarrel()
    {
        if (insideObject.CompareTag("Player") && GameManager.Instance.InBarrel && inBarrel)
        {
            insideObject.GetComponent<PlayerScript>().AnimatorHandler.enterBarrel();
        }
    }
    protected void insideCannonAction()
    {

        Vector3 currentPos = transform.position;


        StopAllCoroutines();
        transform.position = currentPos;
        playShootAnimation();
        gameObject.GetComponent<Animator>().SetFloat("chargeSpeed", 1);
        onExitCannon?.Invoke();
        canShoot = false;
        if (IsFinal && transform.up.y > 0 && (SceneManager.GetActiveScene().name != "Tutorial" || SceneManager.GetActiveScene().name == "Main Game"))
        {
            // LevelObjectManager.Instance.nextMiniLevel();
            // GameManager.instance.nextMiniLevel();

        }

    }


    protected void insideCannonAction(float speed)
    {
        Vector3 currentPos = transform.position;

        StopAllCoroutines();
        transform.position = currentPos;
        playShootAnimation();
        gameObject.GetComponent<Animator>().SetFloat("chargeSpeed", speed);

        onExitCannon?.Invoke();
        canShoot = false;
        if (IsFinal && transform.up.y > 0)
        {
            //GameManager.instance.nextMiniLevel();
            // Camera.main.GetComponent<FollowCamera>().startZoom();
        }

    }
    public void playShootAnimation()
    {
        
        gameObject.GetComponent<Animator>().Play("shoot");
    }


    protected IEnumerator rotateToAngle(float finalRotation, float rotSpeed)
    {

        isRotating = true;

        if (finalRotation == 180 || finalRotation == -180)
        {
            finalRotation *= -1;
        }
        float currentRotation = transform.rotation.eulerAngles.z;
        float rot = finalRotation - currentRotation;
        if (inBarrel)
        {

            hideArrow();

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
            showArrow();
            gameObject.GetComponent<CannonSoundSystem>().playReadySfx();

        }
        transform.rotation = Quaternion.Euler(0f, 0f, finalRotation);
        canShoot = true;
        isRotating = false;




    }
    protected IEnumerator rotateToAngle(float finalRotation, float rotSpeed, float dashSpeed)
    {

        isRotating = true;
        this.dashSpeed = dashSpeed;
        if (finalRotation == 180 || finalRotation == -180)
        {
            finalRotation *= -1;
        }
        float currentRotation = transform.rotation.eulerAngles.z;
        float rot = finalRotation - currentRotation;
        if (inBarrel)
        {

            hideArrow();

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
            showArrow();
            gameObject.GetComponent<CannonSoundSystem>().playReadySfx();

        }
        transform.rotation = Quaternion.Euler(0f, 0f, finalRotation);
        canShoot = true;
        isRotating = false;




    }
    protected IEnumerator rotateAngles(float rotation, float speed)
    {

        float rotateTime = 0;

        // wrong
        Quaternion endRot = Quaternion.Euler(0f, 0f, rotation + transform.rotation.eulerAngles.z);
        // print("parameter: " + rotation + "  current Rotation: " + transform.rotation.eulerAngles.z + " final Rotation:" + endRot.eulerAngles.z);
        // print(endRot.eulerAngles);
        // Rotate clockwise 

        while (transform.rotation != endRot)
        {



            rotateTime += Time.deltaTime;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, endRot, speed * Time.deltaTime);

            yield return new WaitForEndOfFrame(); // Yield to the next frame


        }
        transform.rotation = endRot;
        //print((speed * Time.deltaTime));


    }
    public IEnumerator rotateFor(GameObject Player, float time, float RotVel)
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
    public IEnumerator rotateAngles2(GameObject Object, float angle, float RotVel, float rotDelay)
    {
        float waitTime = Math.Abs(angle / RotVel);
        StartCoroutine(rotateFor(gameObject, waitTime, RotVel));
        yield return new WaitForSeconds(waitTime + rotDelay);
    }
    public IEnumerator applyConstantVelocityToRigidbody(GameObject insideObject, float time, float velocity, Vector3 direction)
    {
        // time: time with that rotation.

        // velocity: pixels per second.
        // direction: in which direction do you want to translate the object. 
        // endpos: final pos of the object.
        // initG: initial gravitation.
        // dashDirection: the direction and speed, it will go in that direction with that speed.
        //precise
        insideObject.transform.SetParent(null);
        Vector3 endPos = new Vector3(transform.position.x + (velocity * time * direction.x), transform.position.y + (velocity * time * direction.y), transform.position.z);
        initG = insideObject.GetComponent<Rigidbody2D>().gravityScale;
        initD = insideObject.GetComponent<Rigidbody2D>().drag;
        if (insideObject.GetComponent<PlayerScript>() != null)
        {
            //
            //insideObject.GetComponent<PlayerScript>().Anim.Play("dash",-1,0);
            insideObject.GetComponent<PlayerScript>().AnimatorHandler.playDash();
            //insideObject.GetComponent<PlayerScript>().Anim.SetBool("dash", true);
            //insideObject.GetComponent<PlayerScript>().Anim.Play("dash");
            insideObject.GetComponent<PlayerScript>().IsDashing = true;






        }
        Vector3 dashDirection = direction * velocity;
        // print("Constant vel start "+gameObject.name);

        // moving

        insideObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        insideObject.GetComponent<Rigidbody2D>().drag = 0;
        insideObject.GetComponent<Rigidbody2D>().velocity = dashDirection;

        yield return new WaitForSeconds(time);

        // print("Constant vel ends "+gameObject.name);
        insideObject.GetComponent<Rigidbody2D>().velocity *= 1;

        insideObject.GetComponent<Rigidbody2D>().drag = initD;
        insideObject.GetComponent<Rigidbody2D>().gravityScale = initG;
        if (insideObject.GetComponent<PlayerScript>() != null)
        {

            //insideObject.GetComponent<PlayerScript>().Anim.SetBool("dash", false);
            insideObject.GetComponent<PlayerScript>().AnimatorHandler.stopDash();
            insideObject.GetComponent<PlayerScript>().IsDashing = false;
            insideObject.GetComponent<PlayerScript>().HorizontalThreshold = 1f;
        }


        //Player.GetComponent<Rigidbody2D>().velocity = dashDirection/2;


    }
    public void stopCannonDash()
    {
        if (ConstantVel != null)
        {
            StopCoroutine(this.ConstantVel);
            insideObject.GetComponent<Rigidbody2D>().velocity *= 1;
            insideObject.GetComponent<Rigidbody2D>().drag = initD;
            insideObject.GetComponent<Rigidbody2D>().gravityScale = initG;
            if (insideObject.GetComponent<PlayerScript>() != null)
            {
                insideObject.GetComponent<PlayerScript>().stopDash();
                insideObject.GetComponent<PlayerScript>().IsDashing = false;
            }
        }

    }


    protected void shootObject()
    {
        //1
        isCharging = false;
        insideObject.transform.SetParent(null);
        insideObject.transform.position = transform.position;
        insideRb.constraints = RigidbodyConstraints2D.None;
        inBarrel = false;

        GameManager.Instance.InBarrel = false;
        GameManager.Instance.CanMove = true;

        hideArrow();
        GameManager.Instance.shakeCamera(shakeType.lite);
        if (insideObject.GetComponent<PlayerScript>() != null)
        {
            PlayerScript playerScript = insideObject.GetComponent<PlayerScript>();

            if (canMoveInShoot == false)
            {
                playerScript.dashWithThreshold(dashTime, dashSpeed, transform.up, 0);
            }
            else
            {
                playerScript.dashWithThreshold(dashTime, dashSpeed, transform.up, horizontalThreshold);
            }
        }
        else
        {
            ConstantVel = applyConstantVelocityToRigidbody(insideObject, dashTime, dashSpeed, transform.up);
            StartCoroutine(ConstantVel);

        }



    }
    public void hideObject()
    {
        isCharging = true;
        if (insideObject != null)
        {
            insideRb.GetComponent<PlayerScript>().AnimatorHandler.onPlayerOut();
        }

    }
    public void showArrow()
    {
        arrowAnim.SetBool("canShoot", true);


    }
    public void changeScene(string args)
    {
        GameManager.Instance.loadSceneWithTransition(args);
    }

    public void hideArrow()
    {
        arrowAnim.SetBool("canShoot", false);
    }

    public float MainMenuBarrelTime
    {
        get { return mainMenuBarrelTime; }
        set { mainMenuBarrelTime = value; }
    }



    public float DashSpeed { get => dashSpeed; set => dashSpeed = value; }
    public float RotDelay { get => rotDelay; set => rotDelay = value; }

    public bool IsFinal { get => isFinal; set => isFinal = value; }
    public bool IsFirst { get => isFirst; set => isFirst = value; }
}
