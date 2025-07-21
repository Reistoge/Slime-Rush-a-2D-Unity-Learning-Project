using System;
using System.Collections;
using System.Drawing;
using System.Globalization;
using System.Numerics;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Analytics;
using Color = UnityEngine.Color;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
public class PlayerScript : MonoBehaviour, IDamageable
{
    // how can i check if the player is touched ?

    #region Events
    public static Action<int> OnPlayerGetCoin;
    public static Action<int> OnPlayerIsDamaged;
    public static Action<int> OnPlayerIsHealed;
    public static Action<int> OnPlayerAddHeart;

    public static Action<int> OnEnemyIsDamaged;
    public static Action<PlayerScript> OnPlayerInstantiated;
    public static Action OnPlayerDied;
    public static Action onPlayerDash;
    #endregion

    #region Components
    [SerializeField] Rigidbody2D rb;
    [SerializeField] SpriteRenderer sr;
    [SerializeField] PlayerAnimationHandler animatorHandler;
    PlayerSO playerConfig;
    [SerializeField] KnockbackFeedBack knockbackFeedBack;

    #endregion

    #region Player State
    [SerializeField] private bool isGrounded; // runtime
    [SerializeField] private bool isDashing; // runtime
    [SerializeField] private bool isFalling; // runtime
    [SerializeField] public bool onJumpCut; // runtime
    bool playerIsDead; // runtime


    #endregion

    #region Player Movement 

    [Header("Player Movement ")]
    // Movement Settings
    [SerializeField] float groundspeed;
    [SerializeField] float inAirSpeed;

    [SerializeField] float horizontalThreshold; // runtime
    [SerializeField] private float speed; // runtime

    [Range(0, 1000f), SerializeField] private float maxHorizontalVelocity;
    [Range(0, 10000f), SerializeField] private float maxUpVelocity;
    [Range(0, 10000f), SerializeField] private float maxDownVelocity;
    [SerializeField] PhysicsMaterial2D bounceMat;
    [SerializeField] Collider2D currentGround;
    [SerializeField] Collision2D currentGroundCollision;

    float pressTime;
    #endregion

    #region Player stats
    [SerializeField] int maxHp;
    [SerializeField] int hp; // runtime
    [SerializeField] int playerDamage;
    [SerializeField] bool canTakeDamage; // runtime
    [SerializeField] int coins;
    [SerializeField] float playerScore;
    [SerializeField] float canTakeDamageResetTime = 2f;

    #endregion

    #region Physics Settings
    [SerializeField] float angularVelocity; // runtime, this variable is used to assigned the value to the actual rigidbody.
    [SerializeField] float tempAng; // runtime, used to modify and not lose the values of angFalling and angJumpVelocity
    [SerializeField] float angularFallingVelocity;
    [SerializeField] float angularJumpVelocity;
    [SerializeField] float initialGravity;
    [SerializeField] float initialLinearDrag;
    [SerializeField] bool handleVelocities = true;
    [SerializeField] bool detectFall = true;

    [Header("Dash Settings")]
    [SerializeField] float maxDashCounter;
    [SerializeField] float dashCounter = 0; // runtime
    [SerializeField] float dashEndVel = .3f;
    [SerializeField] float dashForce;
    [SerializeField] float dashTime;
    private Vector2 dashDirection; // runtime

    [Header("Jump Settings")]
    const float PC_JUMP_SCALE = 0.4f;
    [SerializeField] float jumpForce = 7; // for android = 7, for pc is 4;
    [SerializeField] float minJumpForce = 10f;
    [SerializeField] float maxJumpForce = 30f;
    [SerializeField] Vector2 jumpThreshold; // for android screen
    [SerializeField] float horizontalJumpDirectionThreshold; // for pc
    [SerializeField] float rawJumpForce; // runtime;





    #endregion

    #region Current State
    [SerializeField]
    private Vector2 currentVel;
    [SerializeField]

    #endregion

    #region Coroutine
    private IEnumerator dashCoroutine;
    private IEnumerator lerpRotation;
    #endregion

    void OnEnable()
    {
        //sr = transform.GetChild(0).GetComponent<SpriteRenderer>();
        //animator = transform.GetChild(0).GetComponent<Animator>();
        //AnimatorHandler = transform.GetChild(0).GetComponent<PlayerAnimationHandler>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        SwipeDetection.OnSwipeLine += swipeJump;
        SwipeDetection.OnSwipeLine += swipeDash;
        SwipeDetection.OnSwipeStart += chargeJump;
        SwipeDetection.OnSwipeEnd += releaseJumpAnimHandler;

        GameManager.Instance.instantiateAppearEffect(this.transform, 0);


        // initialize playerConfig
    }
    void OnDisable()
    {
        SwipeDetection.OnSwipeLine -= swipeJump;
        SwipeDetection.OnSwipeLine -= swipeDash;
        SwipeDetection.OnSwipeStart -= chargeJump;
        SwipeDetection.OnSwipeEnd -= releaseJumpAnimHandler;

    }
    void Start()
    {


        // maxHp = GameManager.instance.PlayerPurchasedHearts;

        //dashCounter = 1;
        //maxDashCounter = 1;
        // dashCounter = 0;
        //movementSpeed = GameManager.instance.PlayerSpeed;
        if (Camera.main && Camera.main.GetComponent<FollowCamera>())
        {
            Camera.main.GetComponent<FollowCamera>().PlayerReference = this.gameObject;
        }
        GameManager.Instance.CanMove = true;
        initialLinearDrag = GetComponent<Rigidbody2D>().drag;
        initializePlayerConfig();
       
        OnPlayerInstantiated?.Invoke(this);



    }
    void Update()
    {
        // checkear rotacion para sombreado
        // GameManager.instance.PlayerScore = (transform.position.y);



        // checkPlayerOutOfBounds();



    }
    void FixedUpdate()
    {
        currentVel = Rb.velocity;
        if (canMove())
        {
            moveLogic();
        }
        if (handleVelocities)
        {
            checkMaxVelocityOnHorizontalAxis();
            if (isDashing == false && isFalling)
            {
                checkMaxVelocityOnVerticalAxis();

            }
        }
    }

    #region CollisionDetection
    private void OnTriggerEnter2D(Collider2D collision)
    {

        HandleCoinCollision(collision);
        HandleDamageableCollision(collision);
        //HandleGroundCollision(collision);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

        HandleDamageableCollision(collision);
        HandleBounceWallCollision(collision);
        HandleGroundCollision(collision);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {

        //HandleGroundExitCollision(collision);

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        HandleGroundExitCollision(collision);
        //HandleStickySpikeBoxExitCollision(collision);
    }


    private void HandleCoinCollision(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("coin") && !collision.gameObject.CompareTag("playerDetector"))
        {
            Coin coin = collision.gameObject.GetComponent<Coin>();
            int value = coin.Value;
            coin.getCoin(); // Call the coin's getCoin method
            playerGetCoin(value); // Call the playerGetCoin method to update the player's coins
        }
    }
    public void playerGetCoin(int value)
    {


        OnPlayerGetCoin?.Invoke(value); // Trigger event with coin value
    }

    private void HandleDamageableCollision(Collider2D collision)
    {
        //stopDash();
        IDamageable damageableObject = collision.gameObject.GetComponent<IDamageable>();
        if (damageableObject != null)
        {

            if (isDashing)
            {
                HandleDashingCollision(damageableObject, collision);
            }
            else
            {
                HandleNonDashingCollision(collision);
            }
        }
    }


    private void HandleDashingCollision(IDamageable damageableObject, Collider2D collision)
    {
        GameManager.Instance.stopCannonDash();
        damageableObject.takeDamage(playerDamage);

    }
    void HandleSquishCollision(Collider2D col)
    {
        Vector2 collisionPoint = col.ClosestPoint(transform.position);
        Vector2 dif = (collisionPoint - (Vector2)transform.position).normalized;


        if (dif.y > 0)
        {
            animatorHandler.squish(PlayerAnimationHandler.SquishType.up);
        }
        else if (dif.y < 0)
        {
            animatorHandler.squish(PlayerAnimationHandler.SquishType.down);


        }



    }
    void HandleSquishCollision(Collision2D col)
    {
        Vector2 collisionPoint = col.contacts[0].point;
        Vector2 dif = (collisionPoint - (Vector2)transform.position).normalized;

        if (dif.y > 0)
        {
            animatorHandler.squish(PlayerAnimationHandler.SquishType.up);
        }
        else if (dif.y < 0)
        {
            animatorHandler.squish(PlayerAnimationHandler.SquishType.down);


        }



    }

    private void HandleNonDashingCollision(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<SpikeBox>() != null)
        {

            // collision.gameObject.GetComponent<StickySpikeBox>().boxTouched2(this.gameObject);
        }
    }


    private void resetRotationAndFreeze()
    {

        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0);
        rb.freezeRotation = true;
        if (canTakeDamage == true)
        {
            knockbackFeedBack.stopFeedBack();

        }
        animatorHandler.stopHurtAnimation();
        animatorHandler.stopDash();

        if (canTakeDamage == true)
        {
            animatorHandler.playIdle();

        }

    }
    public void resetPlayerTransform(Vector3 position, Quaternion rotation)
    {
        // Stop any ongoing dash or movement coroutines
        stopDash();

        // Reset Rigidbody2D physics
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.gravityScale = initialGravity;
            rb.drag = initialLinearDrag;
            rb.sharedMaterial = null;
            rb.freezeRotation = false;
        }

        // Reset transform position and rotation
        transform.position = position;
        transform.rotation = rotation;

        // Reset animation and state
        animatorHandler.stopDash();
        animatorHandler.stopHurtAnimation();
        animatorHandler.playIdle();

        // Reset player state variables
        isDashing = false;
        isFalling = false;
        onJumpCut = false;
        playerIsDead = false;
        IsGrounded = false;
        dashCounter = maxDashCounter;
        // currentGround = null;
        // currentGroundCollision = null;
        horizontalThreshold = 1f;
    }
    public void lerpPosition(Vector3 targetPosition, Quaternion finalRot,float duration)
    {

        StartCoroutine(LerpPositionCoroutine(targetPosition,finalRot, duration));

    }
 
    protected IEnumerator LerpPositionCoroutine(Vector3 finalPos,Quaternion finalRot, float speed)
    {
        resetPlayerTransform(transform.position, transform.rotation);
        yield return new WaitForEndOfFrame();
        Vector3 startPosition = transform.position;

        // Temporarily disable physics so gravity doesn't pull the player down
        bool prevKinematic = rb.isKinematic;
        rb.isKinematic = true;
        rb.velocity = Vector2.zero;

        Quaternion startRotation = transform.rotation;
        float elapsedTime = 0f;

        // Calculate total distance to determine duration
        float distance = Vector2.Distance(transform.position, finalPos);
        float duration = distance / speed;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            // Smooth out the interpolation
            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            transform.position = Vector3.Lerp(startPosition, finalPos, smoothT);
            transform.rotation = Quaternion.Lerp(startRotation,
                Quaternion.Euler(0f, 0f, finalRot.z), smoothT);

            yield return null;
        }


        // Ensure we reach the exact final position
        rb.isKinematic = prevKinematic;
        transform.position = finalPos;
        transform.rotation = Quaternion.Euler(0f, 0f, finalRot.z);


    }


    private void HandleDamageableCollision(Collision2D collision)
    {
        IDamageable damageableObject = collision.gameObject.GetComponent<IDamageable>();
        if (damageableObject != null)
        {
            if (isDashing)
            {

                damageableObject.takeDamage(playerDamage);
            }
        }
    }

    private void HandleBounceWallCollision(Collision2D collision)
    {
        if (collision.gameObject.tag == "bounceWall")
        {
            //collision.gameObject.GetComponent<Animator>().SetTrigger("bounce");
            // bounceSfx.Play();
        }
    }

    private void HandleGroundCollision(Collision2D collision)
    {
        HandleSquishCollision(collision);
        if ((collision.gameObject.CompareTag("ground") || collision.gameObject.CompareTag("moveableGround")) && currentGround == null)
        {

            IsGrounded = true;
            if (lerpRotation != null)
            {
                StopCoroutine(lerpRotation);
                angularFallingVelocity = tempAng;
            }
            if (collision.contacts[0].point.y <= transform.position.y && canTakeDamage == true)
            {
                animatorHandler.playOnTouchGround();
            }
            // if the player is above the ground play the animation to touch the ground


            resetRotationAndFreeze();


            currentGround = collision.collider;
            DashCounter = MaxDashCounter;
            if (collision.gameObject.CompareTag("moveableGround") && transform.parent == null)
            {
                transform.SetParent(collision.transform);
            }

        }
    }



    private void HandleGroundExitCollision(Collision2D collision)
    {
        if ((collision.gameObject.CompareTag("ground") || collision.gameObject.CompareTag("moveableGround")) && currentGround == collision.collider)
        {
            //print("b");

            IsGrounded = false;
            currentGroundCollision = null;
            currentGround = null;
            rb.freezeRotation = false;
            if (collision.gameObject.CompareTag("moveableGround") && transform.parent != null)
            {
                transform.SetParent(null);
            }



        }
    }

    #endregion

    #region Movement
    public bool canMove()
    {
        if (GameManager.Instance.InBarrel == false)
        {
            return true;
        }
        return false;

    }
    private void checkMaxVelocityOnHorizontalAxis()
    {

        if (Rb.velocity.x >= maxHorizontalVelocity)
        {
            //print("reach max velocity in X axis"%Colorize.Red);
            Rb.velocity = new Vector2(maxHorizontalVelocity, Rb.velocity.y);
        }
        if (Rb.velocity.x <= -maxHorizontalVelocity)
        {
            //print("reach max velocity in -X axis" % Colorize.Blue);
            Rb.velocity = new Vector2(-maxHorizontalVelocity, Rb.velocity.y);
        }
    }

    private void checkMaxVelocityOnVerticalAxis()
    {
        // y axis
        if (Rb.velocity.y >= maxUpVelocity)
        {
            //print("reach max velocity in Y axis" % Colorize.Purple);
            Rb.velocity = new Vector2(Rb.velocity.x, maxUpVelocity);
        }
        if (Rb.velocity.y <= -maxDownVelocity)
        {
            //print("reach max velocity in -Y axis" % Colorize.Magenta);
            Rb.velocity = new Vector2(Rb.velocity.x, -maxDownVelocity);
        }
    }

    private void moveLogic()
    {
        // Get the current movement direction from the game manager
        // float horizontalAxis = GameManager.instance.MovXButtons;
        // float verticalAxis = GameManager.instance.MovYButtons;
        // Check if the player is allowed to move
        float verticalAxis = Input.GetAxis("Vertical");


        // // for pc
        // float horizontalAxis = getHorizontalInput();
        // GameManager.instance.MovXButtons = horizontalAxis;
        // // for pc

        // //mobile
        // horizontalAxis = GameManager.instance.MovXButtons;
        // // for mobile


        // if (horizontalAxis == 0)
        // {
        //     horizontalAxis = getHorizontalInput();


        // }
        // Handle jumping
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded == true)
        {
            // Record the time when the jump button was pressed
            pressTime = Time.time;
            print("1: " + pressTime);
            animatorHandler.playChargeIn();


        }
        if (Input.GetKeyUp(KeyCode.Space) && IsGrounded == true && !isDashing)
        {
            // Allow the player to move again after jumping
            // GameManager.instance.CanMove = true;

            // Calculate the jump force based on the time the jump button was held
            animatorHandler.playChargeOut();
            if (pressTime > 0)
            {
                float elapsed = (1 + Time.time - pressTime);



                jump(jumpForce * PC_JUMP_SCALE * Math.Clamp(elapsed, 1f, 1.5f), InputManager.Instance.HorizontalAxis / horizontalJumpDirectionThreshold, 1);
                pressTime = 0;

            }


        }
        else if (Input.GetKeyUp(KeyCode.Space) && IsGrounded == false && isDashing == false && DashCounter > 0)
        {



            Vector2 dir = new Vector2(InputManager.Instance.HorizontalAxisRaw, verticalAxis * 1.5f);
            dir = dir.normalized;
            if (dir.x == 0 && dir.y == 0)
            {
                dir.x = transform.up.x;
                dir.y = transform.up.y;
            }

            DashCounter--;
            dash(dir);
        }

        // Update the player's movement speed based on whether they are grounded or not
        if (IsGrounded == false)
        {
            speed = inAirSpeed;
            // Determine if the player is falling and update angular direction accordingly
            bool isFalling = rb.velocity.y < -5f;
            if (detectFall)
            {
                fallHandler(isFalling);

            }



        }
        if (IsGrounded && GameManager.Instance.CanMove == true)
        {
            animatorHandler.playWalkAnimation(InputManager.Instance.HorizontalAxisRaw);
            speed = groundspeed;
            isFalling = false;


        }
        // Update the player's velocity based on their movement direction and speed
        if (GameManager.Instance.CanMove == true)
        {
            Vector2 newVel = new Vector2(Rb.velocity.x + ((InputManager.Instance.HorizontalAxisRaw * horizontalThreshold) * speed) * Time.fixedDeltaTime, Rb.velocity.y);
            Rb.velocity = newVel;
            if (IsGrounded == false)
            {

                if (transform.up.y <= 0)
                {
                    rb.angularVelocity = angularVelocity * InputManager.Instance.HorizontalAxisRaw;
                }
                else
                {
                    rb.angularVelocity = angularVelocity * -InputManager.Instance.HorizontalAxisRaw;
                }
            }

        }

    }

    public void takeDamage(int damage) // this function damages the player
    {
        // WHEN THE PLAYER ENTERS A BARREL IT NEEDS TO STOP BEING DAMAGED.
        if (canTakeDamage == false)
        {
            return; 
        }
        if (isDashing)
        {
            stopDash();
        }
        hp -= damage;
        GameManager.Instance.getRuntimeData().playerHp = hp;
        // GameManager.Instance.CanMove = false;
        //this parameter is in the enemy_damagezone.cs
        OnPlayerIsDamaged(damage);
        animatorHandler.playHurt(canTakeDamageResetTime);
        

        if (hp <= 0)
        {

            die();
        }
        else
        {
            GameManager.Instance.shakeCamera(shakeType.lite);

        }
    }
    public void jump(float force)
    {

        if (IsGrounded && GameManager.Instance.InBarrel == false)
        {

            animatorHandler.playJump();

            rb.AddForce(new Vector2(InputManager.Instance.HorizontalAxisRaw, 1) * force * rb.mass * rb.gravityScale, ForceMode2D.Impulse);
            IsGrounded = false;

        }
    }
    public void jump(float force, float Xdirection, float Ydirection)
    {

        if (IsGrounded && GameManager.Instance.InBarrel == false)
        {
            //GetComponent<AudioSource>().Play();
            animatorHandler.playJump();

            rb.AddForce(new Vector2(Xdirection, Ydirection) * force * rb.mass * rb.gravityScale, ForceMode2D.Impulse);
            //IsGrounded = false;
        }
    }

    public void swipeJump(Vector2 startPos, Vector2 endPos)
    {

        if (IsGrounded && GameManager.Instance.InBarrel == false && GameManager.Instance.CanMove == true)
        {

            transform.SetParent(null);


            isFalling = false;
            animatorHandler.playJump();

            Vector3 originalVector = endPos - startPos; // A vector pointing along the x-axis
            // Define a quaternion (e.g., a 90-degree rotation around the y-axis)
            Quaternion rotation = Quaternion.Euler(0, 0, 180);
            // Rotate the vector using the swipe
            Vector3 rotatedVector = (rotation * originalVector).normalized;

            // Vector2 directionVector = new Vector2(rotatedVector.x * originalVector.magnitude / jumpThreshold.x, rotatedVector.y * originalVector.magnitude / jumpThreshold.y) * Camera.main.aspect;
            float clampX = Mathf.Clamp(originalVector.magnitude / JumpThreshold.x, MinJumpForce, MaxJumpForce);
            float clampY = Mathf.Clamp(originalVector.magnitude / JumpThreshold.y, MinJumpForce, MaxJumpForce);
            Vector2 directionVector = new Vector2(rotatedVector.x * clampX, rotatedVector.y * clampY) * Camera.main.aspect;
            float angle = Mathf.Atan2(directionVector.y, directionVector.x);

            // add a threshold fot the x axis

            //double angle = Math.Atan(rotatedVector.x/rotatedVector.y)*180f;
            // print(angle);
            rb.AddForce(directionVector * rb.gravityScale, ForceMode2D.Impulse);

            // print(
            // "directionVector: " + directionVector +
            // "\n" +
            // "angle: " + angle
            // + "\n" +
            // "gravity scale: " + rb.gravityScale
            // + "\n" +
            // "vel scale: " + rb.velocity + ": " + rb.velocity.magnitude

            // );
            transform.eulerAngles = new Vector3(0, 0, (angle * Mathf.Rad2Deg) - 90);
            // StartCoroutine(LerpRotation(0.3f, (angle * Mathf.Rad2Deg) - 90));
            //             print(Vector2.Angle(startPos, endPos));

        }
    }
    public void swipeDash(Vector2 startPos, Vector2 endPos)
    {

        if (IsGrounded == false && DashCounter > 0 && GameManager.Instance.InBarrel == false && GameManager.Instance.CanMove == true)
        {

            DashCounter--;
            Vector3 originalVector = endPos - startPos; // A vector pointing along the x-axis
            // Define a quaternion (e.g., a 90-degree rotation around the y-axis)
            Quaternion rotation = Quaternion.Euler(0, 0, 180);
            // Rotate the vector using the swipe
            Vector3 rotatedVector = (rotation * originalVector).normalized;

            float x = rotatedVector.x * originalVector.magnitude / JumpThreshold.x;
            float y = rotatedVector.y * originalVector.magnitude / JumpThreshold.y;
            Vector2 directionVector = new Vector2(x, y) * Camera.main.aspect;

            dash(directionVector.normalized);
        }
    }
    public void chargeJump()
    {
        if (GameManager.Instance.InBarrel == false && IsGrounded && GameManager.Instance.CanMove == true)
        {
            // Record the time when the jump button was pressed
            GameManager.Instance.CanMove = false;
            animatorHandler.playChargeIn();


        }
    }
    public void releaseJumpAnimHandler()
    {
        if (GameManager.Instance.InBarrel == false)
        {
            animatorHandler.playChargeOut();

        }
    }
    public void resetDashCounter()
    {
        dashCounter = maxDashCounter;

    }
    public void dash(float time, float velocity, Vector3 direction)
    {
        // used by the dasher
        if (isDashing == false)
        {
            stopDash();

        }

        if (isDashing == false && GameManager.Instance.InBarrel == false)
        {

            isDashing = true;
            onPlayerDash?.Invoke();
            DashCoroutine = applyConstantVelocityToRigidbody(this.gameObject, time, velocity, direction);
            StartCoroutine(DashCoroutine);
        }
    }
    public void dashWithThreshold(float time, float velocity, Vector3 direction, float threshold)
    {
        if (isDashing == true)
        {
            stopDash();

        }

        if (isDashing == false && GameManager.Instance.InBarrel == false)
        {

            // animatorHandler.playDash();
            isDashing = true;
            onPlayerDash?.Invoke();
            DashCoroutine = applyConstantVelocityToRigidbodyWithThreshold(this.gameObject, time, velocity, direction, threshold);
            StartCoroutine(DashCoroutine);
        }
    }

    public void dash(Vector2 direction)
    {

        if (isDashing == true)
        {
            stopDash();

        }
        if (isDashing == false && GameManager.Instance.InBarrel == false)
        {

            //animatorHandler.playDash();
            isDashing = true;
            onPlayerDash?.Invoke();
            DashCoroutine = applyConstantVelocityToRigidbody(this.gameObject, dashTime, dashForce, direction);
            StartCoroutine(DashCoroutine);
        }
    }
    public void stopDash()
    {
        if (DashCoroutine != null)
        {

            StopCoroutine(DashCoroutine);
            GetComponent<Rigidbody2D>().velocity *= 1;
            GetComponent<Rigidbody2D>().drag = initialLinearDrag;
            GetComponent<Rigidbody2D>().gravityScale = initialGravity;
            GetComponent<Rigidbody2D>().sharedMaterial = null;
            horizontalThreshold = 1;
            animatorHandler.stopDash();
            IsDashing = false;
        }

        // Anim.SetBool("dash", false);
        // handler.stopDash();
    }
    public void fallHandler(bool state)
    {

        if (isFalling != state)
        {

            isFalling = state;

            if (state == true)
            {
                // Handle falling state
                angularVelocity = angularFallingVelocity;
                if (dashCounter == 0 && dashCoroutine == null)
                {
                    if (!IsGrounded && !isDashing)
                    {
                        lerpRotation = LerpRotation(0.5f, 180);
                        StartCoroutine(lerpRotation);
                    }

                }
                animatorHandler.playFall();

            }
            else
            {

                // animatorHandler.playIdle();
                //StartCoroutine(LerpRotation(0.1f, 0));

                angularVelocity = angularJumpVelocity;
            }
        }
    }

    IEnumerator LerpRotation(float duration, float targetAngle)
    {

        float elapsed = 0;
        float startAngle = transform.eulerAngles.z;
        tempAng = angularFallingVelocity;
        angularFallingVelocity = 0;
        // print(tempAng);
        while (elapsed < duration)
        {

            float t = elapsed / duration;
            t = t * t * (3f - 2f * t); // smoothstep
            float newAngle = Mathf.Lerp(startAngle, targetAngle, t);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, newAngle);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, targetAngle);
        angularFallingVelocity = tempAng;

    }
    public IEnumerator applyConstantVelocityToRigidbody(
        GameObject Player,
        float time,
        float velocity,
        Vector3 direction
    )
    {
        // time: time with that rotation.
        // velocity: pixels per second.
        // direction: in which direction do you want to translate the object.
        // endpos: final pos of the object.
        // initG: initial gravitation.
        // dashDirection: the direction and speed, it will go in that direction with that speed.
        //precise

        Vector3 endPos = new Vector3(
            transform.position.x + (velocity * time * direction.x),
            transform.position.y + (velocity * time * direction.y),
            transform.position.z
        );
        float initRot = transform.rotation.z;
        // initialGravity = Player.GetComponent<Rigidbody2D>().gravityScale;
        // initialDrag = Player.GetComponent<Rigidbody2D>().drag;
        DashCoroutineSpeed = velocity;
        DashDirection = direction;
        DashCoroutineTime = time;
        Vector3 dash = DashDirection * velocity;
        // issue with this rotation
        float angle = Mathf.Atan2(direction.normalized.y, direction.normalized.x);
        transform.eulerAngles = new Vector3(0, 0, (angle * Mathf.Rad2Deg) - 90);

        // print("Constant vel Start " + gameObject.name);
        // moving
        Player.GetComponent<Rigidbody2D>().sharedMaterial = bounceMat;
        Player.GetComponent<Rigidbody2D>().gravityScale = 0;
        Player.GetComponent<Rigidbody2D>().drag = 0;
        Player.GetComponent<Rigidbody2D>().velocity = dash;

        // Player.GetComponent<PlayerScript>().Anim.SetBool("dash", true);
        // animatorHandler.playDash();
        // HANDLER.DASH();
         
        animatorHandler.playDash();
        Vector2 beforeDash = transform.position;
        print("Constant vel start " + gameObject.name + " vel: " + velocity + " time: " + time);

        float elapsed = 0f;
        while (elapsed < time)
        {
            float step = Mathf.Min(Time.fixedDeltaTime, time - elapsed);
            yield return new WaitForFixedUpdate();
            elapsed += step;
        }

        float delta = Vector2.Distance(beforeDash, transform.position);
        print("delta position " + delta + " (should be: " + (velocity * time) + ")");
        //Player.GetComponent<PlayerScript>().Anim.SetBool("dash", false);
        // HANDLER.STOPDASH();
        Player.GetComponent<Rigidbody2D>().sharedMaterial = null;
        Player.GetComponent<Rigidbody2D>().velocity *= dashEndVel;
        Player.GetComponent<Rigidbody2D>().drag = initialLinearDrag;
        Player.GetComponent<Rigidbody2D>().gravityScale = initialGravity;

        IsDashing = false;
        print("Constant vel ends " + gameObject.name);

        //Player.GetComponent<Rigidbody2D>().velocity = dashDirection/2;
    }
    public IEnumerator applyConstantVelocityToRigidbodyWithThreshold(
        GameObject Player,
        float time,
        float velocity,
        Vector3 direction, float threshold
    )
    {
        // time: time with that rotation.
        // velocity: pixels per second.
        // direction: in which direction do you want to translate the object.
        // endpos: final pos of the object.
        // initG: initial gravitation.
        // dashDirection: the direction and speed, it will go in that direction with that speed.
        //precise

        Vector3 endPos = new Vector3(
            transform.position.x + (velocity * time * direction.x),
            transform.position.y + (velocity * time * direction.y),
            transform.position.z
        );
        // initialGravity = Player.GetComponent<Rigidbody2D>().gravityScale;
        // initialDrag = Player.GetComponent<Rigidbody2D>().drag;
        DashCoroutineSpeed = velocity;
        DashDirection = direction;
        DashCoroutineTime = time;
        Vector3 dash = DashDirection * velocity;

        // issue with this rotation // correct way to rotate this object
        float angle = Mathf.Atan2(direction.normalized.y, direction.normalized.x);
        transform.eulerAngles = new Vector3(0, 0, (angle * Mathf.Rad2Deg) - 90);

        // print("Constant vel Start " + gameObject.name);
        // moving

        Player.GetComponent<Rigidbody2D>().sharedMaterial = bounceMat;
        Player.GetComponent<Rigidbody2D>().gravityScale = 0;
        Player.GetComponent<Rigidbody2D>().drag = 0;
        Player.GetComponent<Rigidbody2D>().velocity = dash;
        // Player.GetComponent<PlayerScript>().Anim.SetBool("dash", true);
        // animatorHandler.playDash();
        // HANDLER.DASH();
        horizontalThreshold = threshold;
        animatorHandler.playDash();
        Vector2 beforeDash = transform.position;
        print("Constant vel start " + gameObject.name + " vel: " + velocity + " time: " + time);

        float elapsed = 0f;
        while (elapsed < time)
        {
            float step = Mathf.Min(Time.fixedDeltaTime, time - elapsed);
            yield return new WaitForFixedUpdate();
            elapsed += step;
        }

        float delta = Vector2.Distance(beforeDash, transform.position);
        print("delta position " + delta + " (should be: " + (velocity * time) + ")");
        horizontalThreshold = 1;
        animatorHandler.stopDash();
        //Player.GetComponent<PlayerScript>().Anim.SetBool("dash", false);
        // HANDLER.STOPDASH();
        Player.GetComponent<Rigidbody2D>().velocity *= dashEndVel;
        Player.GetComponent<Rigidbody2D>().drag = initialLinearDrag;
        Player.GetComponent<Rigidbody2D>().gravityScale = initialGravity;
        Player.GetComponent<Rigidbody2D>().sharedMaterial = null;
        IsDashing = false;
        // print("Constant vel ends " + gameObject.name);

        //Player.GetComponent<Rigidbody2D>().velocity = dashDirection/2;
    }

    public void die()
    {

        OnPlayerDied?.Invoke();
        playerIsDead = true;
        storeValuesWhenDie();
        animatorHandler.playDie();


    }
    public void storeValuesWhenDie()
    {
        // store values in the player config
        if (playerConfig == null)
        {
            playerConfig = GameManager.Instance.PlayerConfig;
        }
        playerConfig.startHp = maxHp;
        playerConfig.maxHp = maxHp;
        playerConfig.startCoins = Coins;
        playerConfig.totalCoins = Coins;
        playerConfig.playerDamage = playerDamage;
        playerConfig.handleVelocities = handleVelocities;
    }
    IEnumerator dieRoutine()
    {
        rb.isKinematic = true;

        GameManager.Instance.CanMove = false;
        GameManager.Instance.shakeCamera(shakeType.strong);
        yield return new WaitUntil(() => GameManager.Instance.cameraState() == shakeType.none);
        yield return new WaitForSeconds(1f);
        animatorHandler.playDie();
        // play Die sound & animation
        yield return new WaitForSeconds(animatorHandler.getCurrentClipDuration() + 1f);

        // wait until the animation ends

        // GameManager.instance.loadDieUI();
        // load points anim and ui

        // ask for return main menu


    }

    public void switchHandleVel()
    {
        handleVelocities = !handleVelocities;
    }
    public void initializePlayerConfig()
    {

        PlayerSO scriptableObject = GameManager.Instance.PlayerConfig;

        // Player State
        JumpThreshold = scriptableObject.jumpThreshold;
        horizontalJumpDirectionThreshold = scriptableObject.horizontalJumpDirectionThreshold;
        horizontalThreshold = scriptableObject.horizontalThreshold;
        // Player Settings
        groundspeed = scriptableObject.groundspeed;
        inAirSpeed = scriptableObject.inAirSpeed;
        maxHorizontalVelocity = scriptableObject.maxHorizontalVelocity;
        maxUpVelocity = scriptableObject.maxUpVelocity;
        maxDownVelocity = scriptableObject.maxDownVelocity;

        // Player Health
        hp = GameManager.Instance.getRuntimeData().playerHp;
        maxHp = scriptableObject.maxHp;
        GameManager.Instance.getRuntimeData().playerMaxHp = maxHp; // Update runtime data
        playerDamage = scriptableObject.playerDamage;

        // Physics Settings
        handleVelocities = scriptableObject.handleVelocities;

        // Dash Settings
        maxDashCounter = scriptableObject.maxDashCounter;
        dashEndVel = scriptableObject.dashEndVel;
        dashForce = scriptableObject.dashForce;
        dashTime = scriptableObject.dashTime;

        // Jump Settings
        jumpForce = scriptableObject.jumpForce;
        MinJumpForce = scriptableObject.minJumpForce;
        MaxJumpForce = scriptableObject.maxJumpForce;

        rb.mass = scriptableObject.mass;
        rb.gravityScale = scriptableObject.gravity;
        initialGravity = rb.gravityScale;
        rb.drag = scriptableObject.linearDrag;




    }

    public void heal(int healAmount)
    {
        if (hp < maxHp)
        {
            hp += healAmount;
            if (hp > maxHp)
            {
                hp = maxHp;
            }
            OnPlayerIsHealed?.Invoke(healAmount);
        }
    }

    public void addHeart(bool isPermanent)
    {
        if (isPermanent)
        {
            playerConfig.maxHp++;
        }

        maxHp++; // Increase max HP
        GameManager.Instance.getRuntimeData().playerMaxHp = maxHp; // Update runtime data

        if (hp < maxHp - 1)
        {
            // If player has missing health, heal one heart and add a new broken one
            hp++;
            OnPlayerAddHeart?.Invoke(1);
        }
        else
        {
            // If player is at full health, just add a new filled heart
            hp++;
            OnPlayerAddHeart?.Invoke(1);
        }
    }


    #endregion

    #region Modifiers and Getters


    public int PlayerDamage
    {
        get => playerDamage;
        set => playerDamage = value;
    }
    public int Hp
    {
        get => hp;
        set => hp = value;
    }
    public int MaxHp
    {
        get => maxHp;
        set => maxHp = value;
    }
    public bool IsDashing
    {
        get => isDashing;
        set => isDashing = value;
    }
    public SpriteRenderer Sr
    {
        get => sr;
        set => sr = value;
    }

    public Rigidbody2D Rb
    {
        get => rb;
        set => rb = value;
    }


    public PlayerAnimationHandler AnimatorHandler
    {
        get => animatorHandler;
        set => animatorHandler = value;
    }

    private float dashCoroutineSpeed;
    public Vector3 DashDirection { get => dashDirection; set => dashDirection = value; }

    private float dashCoroutineTime;


    public IEnumerator DashCoroutine { get => dashCoroutine; set => dashCoroutine = value; }
    public float DashCoroutineSpeed { get => dashCoroutineSpeed; set => dashCoroutineSpeed = value; }
    public float DashCoroutineTime { get => dashCoroutineTime; set => dashCoroutineTime = value; }
    public float DashCounter { get => dashCounter; set => dashCounter = value; }
    public float MaxDashCounter { get => maxDashCounter; set => maxDashCounter = value; }
    public bool HandleVelocities { get => handleVelocities; set => handleVelocities = value; }
    public bool CanTakeDamage { get => canTakeDamage; set => canTakeDamage = value; }
    public float HorizontalThreshold { get => horizontalThreshold; set => horizontalThreshold = value; }
    public float PlayerScore { get => playerScore; set => playerScore = value; }
    public bool DetectFall { get => detectFall; set => detectFall = value; }
    public Vector2 JumpThreshold { get => jumpThreshold; set => jumpThreshold = value; }
    public float MinJumpForce { get => minJumpForce; set => minJumpForce = value; }
    public float MaxJumpForce { get => maxJumpForce; set => maxJumpForce = value; }
    public bool IsGrounded { get => isGrounded; set => isGrounded = value; }
    public PlayerSO PlayerConfig { get => playerConfig; set => playerConfig = value; }
    public int Coins { get => coins; set => coins = value; }
    #endregion
    // dos formas de verificar las colisiones entre objetos, puede ser mediante oncollisionenter que detecta cada vez que colisionan dos objetos con sus rb
    //Therefore, it is recommended to cache the
    //reference to the component in a private field
    //and use it instead of calling GetComponent<T>() repeatedly.
    /*
        The difference between OnTriggerEnter2D and OnCollisionEnter2D is that they are used for different types of collisions in Unity.
        OnCollisionEnter2D is used when two objects collide with each other, and provides more detailed information about the collision,
        such as the contact points, the relative velocity, and the impulse. You can use this method to detect or modify the physical behavior
        of the objects, such as bouncing, breaking, or applying forces. For example, you can use this method to make a ball bounce off a wall,
        or to damage an enemy when it collides with a bullet.
        OnTriggerEnter2D is used when one object passes through another object that has its Is Trigger property set to true. This means that the
        objects do not physically interact with each other, but they can still trigger a reaction through code. You can use this method to detect
        or activate events that are not related to physics, such as collecting power-ups, triggering alarms, or entering a portal. For example,
        you can use this method to make a coin disappear when the player touches it, or to start a timer when the player enters a zone.
        To summarize, you should use OnCollisionEnter2D for solid interactions that involve physics, and OnTriggerEnter2D for trigger interactions
        that involve events. You can also use collision layers and tags to fine-tune which objects can collide or trigger with each other.
        Both methods use the Collider2D as the parameter, but the difference is that OnCollisionEnter2D requires both objects to have a
        Rigidbody2D and a Collider2D with Is Trigger set to false, while OnTriggerEnter2D requires one object to have a Collider2D with Is
        Trigger set to true and the other object to have any Collider2D.
     */
}
