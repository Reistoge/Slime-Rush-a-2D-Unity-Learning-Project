using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// Base class for cannon mechanics in the game.
/// Handles player entry, rotation, charging, and launching.
/// Extend this class to create specialized cannon behaviors.
/// </summary>
public class Cannon : MonoBehaviour
{
    #region Components
    protected Animator anim;
    protected Animator arrowAnim;
    protected CannonSoundSystem soundSystem;
    protected SpriteRenderer sprite;
    #endregion

    #region Cannon State
    [Header("Cannon State")]
    [SerializeField] protected bool canShoot = false;
    [SerializeField] protected bool isRotating;
    [SerializeField] public bool inBarrel;
    [SerializeField] protected bool isCharging;
    #endregion

    #region Cannon Configuration
    [Header("Cannon Configuration")]
    [SerializeField]
    [Range(0f, 1000f)]
    [Tooltip("Launch speed in units per second")]
    public float dashSpeed;

    [SerializeField]
    [Range(0f, 1000)]
    [Tooltip("Duration of the launch dash")]
    public float dashTime;

    [SerializeField]
    [Min(0)]
    [Tooltip("Delay before rotation starts")]
    public float rotDelay;

    [SerializeField]
    [Tooltip("Whether this is the first cannon in the level")]
    protected bool isFirst;

    [SerializeField]
    [Tooltip("Whether this is the final cannon that completes the level")]
    protected bool isFinal;

    [SerializeField]
    [Tooltip("Auto-shoot after this many seconds")]
    protected bool isAutoShoot;

    [SerializeField]
    private float autoShootSeconds = 0;

    [SerializeField]
    [Tooltip("Whether player can move horizontally during launch")]
    protected bool canMoveInShoot = true;

    [SerializeField]
    [Tooltip("Horizontal movement multiplier during launch")]
    private float horizontalThreshold;
    #endregion

    #region Events
    [Header("Events")]
    [SerializeField] protected UnityEvent onEnterCannon;
    [SerializeField] protected UnityEvent onExitCannon;
    #endregion

    #region Private Fields
    private float mainMenuBarrelTime = 4;
    protected float initRot;
    private float initG;
    private float initD;
    protected GameObject insideObject;
    protected Rigidbody2D insideRb;
    private IEnumerator ConstantVel;
    #endregion

    protected void OnEnable()
    {
        restartButton.StopCoroutines += StopAllCoroutines;

        TryGetComponent(out Collider2D col);
        if (Utils.isVisible(col))
        {
            GameManager.Instance.instantiateAppearEffect(transform, 0);
        }
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

    /// <summary>
    /// Resets the charging state of the cannon.
    /// </summary>
    public void resetIsCharging()
    {
        isCharging = false;
    }

    /// <summary>
    /// Handles when an object enters the cannon.
    /// Sets up the object for launching and plays entry animations.
    /// </summary>
    /// <param name="collision">The collider that entered the cannon</param>
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

                StartCoroutine(executeInSeconds(insideCannonAction,autoShootSeconds));

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
    IEnumerator executeInSeconds(Action func, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        func?.Invoke();
    }
    /// <summary>
    /// Action performed when object is ready to be shot from cannon.
    /// Stops coroutines, plays animation, and triggers exit events.
    /// </summary>
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
            // Final cannon logic - can trigger level completion
        }
    }

    /// <summary>
    /// Action performed when object is ready to be shot from cannon with custom charge speed.
    /// </summary>
    /// <param name="speed">Charge animation speed multiplier</param>
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
            // Final cannon logic
        }
    }

    /// <summary>
    /// Plays the cannon shoot animation.
    /// </summary>
    public void playShootAnimation()
    {
        gameObject.GetComponent<Animator>().Play("shoot");
    }

    /// <summary>
    /// Rotates the cannon to a target angle over time.
    /// </summary>
    /// <param name="finalRotation">Target rotation in degrees</param>
    /// <param name="rotSpeed">Rotation speed in degrees per second</param>
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
    protected IEnumerator rotateToAngleInterruptible(float finalRotation, float rotSpeed, float dashSpeed)
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
            // canShoot = false;
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
        print("Constant vel start " + gameObject.name + " vel: " + dashDirection);

        // moving

        insideObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        insideObject.GetComponent<Rigidbody2D>().drag = 0;
        insideObject.GetComponent<Rigidbody2D>().velocity = dashDirection;


        Vector2 beforeDash = insideObject.transform.position;
        print("Constant vel start " + insideObject.gameObject.name + " vel: " + velocity + " time: " + time);

        float elapsed = 0f;
        while (elapsed < time)
        {
            float step = Mathf.Min(Time.fixedDeltaTime, time - elapsed); // run the function in fixed update for physics behaviour
            yield return new WaitForFixedUpdate();
            elapsed += step;
        }

        float delta = Vector2.Distance(beforeDash, transform.position);
        print("delta position " + delta + " (should be: " + (velocity * time) + ")");

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


    /// <summary>
    /// Shoots the object currently inside the cannon.
    /// Applies launch force and triggers camera shake.
    /// </summary>
    protected void shootObject()
    {
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

    /// <summary>
    /// Hides the object inside the cannon (for charging animation).
    /// </summary>
    public void hideObject()
    {
        isCharging = true;
        if (insideObject != null)
        {
            insideRb.GetComponent<PlayerScript>().AnimatorHandler.onPlayerOut();
        }
    }

    /// <summary>
    /// Shows the direction arrow indicating cannon is ready to fire.
    /// </summary>
    public void showArrow()
    {
        arrowAnim.SetBool("canShoot", true);
    }

    /// <summary>
    /// Triggers a scene change with transition.
    /// </summary>
    /// <param name="args">Scene change arguments</param>
    public void changeScene(string args)
    {
        GameManager.Instance.loadSceneWithTransition(args);
    }

    /// <summary>
    /// Hides the direction arrow.
    /// </summary>
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
