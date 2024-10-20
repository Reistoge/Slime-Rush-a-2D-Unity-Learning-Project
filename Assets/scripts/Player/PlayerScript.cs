using System;
using System.Collections;
using UnityEngine;

public class PlayerScript : MonoBehaviour, IDamageable
{
    // how can i check if the player is touched ?
    public static Action<int> OnPlayerGetCoin;
    public static Action<int> OnPlayerIsDamaged;
    public static Action<int> OnEnemyIsDamaged;
    public static Action OnPlayerDied;


    //object references:
    Rigidbody2D rb;
    AudioSource bounceSfx;
    SpriteRenderer sr;
    Animator animator;
    PlayerAnimationHandler animatorHandler;

    [SerializeField]
    bool isGrounded;

    //int & floats:
    // changes the speed in reward and barrel scene
    [Header("Player Settings")]
    const float HORIZONTAL_LIMIT_VALUE = 180f;
    [SerializeField] float coyoteTime;
    [SerializeField] float coyoteTimeCounter;

    [Range(0, 1000f), SerializeField]
    private float movementSpeed;

    [Range(0, 1000f), SerializeField]
    private float maxHorizontalVelocity;

    [Range(0, 1000f), SerializeField]
    private float maxUpVelocity;

    [Range(0, 1000f), SerializeField]
    private float maxDownVelocity;

    [SerializeField]
    float dashForce;

    [SerializeField]
    float dashTime;

    [SerializeField]
    int playerDamage;

    [SerializeField]
    bool isDashing;

    [SerializeField]
    float angularVelocity;
    int hp;
    int maxHp;

    [SerializeField]
    private Vector2 currentVel;

    [SerializeField]
    PlayerState state;
    bool isFalling;
    float angularDir;
    float initialDrag;
    float initialGravity;
    Vector3 dashDirection;


    IEnumerator dashCoroutine;

    void OnEnable()
    {
        sr = transform.GetChild(0).GetComponent<SpriteRenderer>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        AnimatorHandler = transform.GetChild(0).GetComponent<PlayerAnimationHandler>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        // initiañize the playerConfig
    }

    void Start()
    {
        maxHp = GameManager.instance.PlayerPurchasedHearts;
        hp = maxHp;
        GameManager.instance.PlayerLife = hp;
        movementSpeed = GameManager.instance.PlayerSpeed;
        GameManager.instance.CanMove = true;
        initialGravity = GetComponent<Rigidbody2D>().gravityScale;
        initialDrag = GetComponent<Rigidbody2D>().drag;
    }

    void Update()
    {
        // checkear rotacion para sombreado
        GameManager.instance.PlayerScore = (transform.position.y);
        currentVel = Rb.velocity;
        moveLogic();
        // checkPlayerOutOfBounds();
        checkMaxVelocityOnHorizontalAxis();
        checkMaxVelocityOnVerticalAxis();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("coin"))
        {
            int value = collision.gameObject.GetComponent<Coin>().Value; // // we get the value of the coin

            OnPlayerGetCoin?.Invoke(value); // event that holds the value of the coin it triggers a function in some classes that holds the value of the coin.

            collision.GetComponent<Coin>().getCoin(); // we call the coin
        }
        if (collision.gameObject.GetComponent<IDamageable>() != null) // enemyBehaviour or damageAble objects
        {
            IDamageable obj = collision.gameObject.GetComponent<IDamageable>();
            if (isDashing == true)
            {
                GameManager.instance.stopCannonDash();
                obj.takeDamage(playerDamage);

                if (collision.gameObject.GetComponent<StickySpikeBox>() != null) // if is spikeBox.
                {
                    dash(
                        transform.up * -1,
                        GameManager.instance.LastUsedBarrel.GetComponent<Cannon>().DashSpeed
                    );
                }
            }
            else if (isDashing == false)
            {
                if (collision.gameObject.GetComponent<StickySpikeBox>() != null) // if is spikeBox.
                {
                    collision.gameObject.GetComponent<StickySpikeBox>().boxTouched(this.gameObject);
                }
            }
        }

        if (collision.gameObject.CompareTag("ground")) // ground behaviuour
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0);
            rb.freezeRotation = true;
            isGrounded = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {

            rb.freezeRotation = false;
            isGrounded = false;
        }
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<IDamageable>() != null)
        {
            if (isDashing)
            {
                IDamageable obj = collision.gameObject.GetComponent<IDamageable>();
                obj.takeDamage(playerDamage);

            }
        }
        if (collision.gameObject.tag == "bounceWall") // wall behaviour
        {
            collision.gameObject.GetComponent<Animator>().SetTrigger("bounce");
            bounceSfx.Play();
        }

        if (collision.gameObject.CompareTag("ground")) // ground behaviuour
        {

            transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0);
            rb.freezeRotation = true;
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground")) // ground behaviuour
        {

            rb.freezeRotation = false;
            isGrounded = false;
        }

        if (collision.gameObject.GetComponent<StickySpikeBox>() != null) // spikeBox behaviour
        {
            collision.gameObject.GetComponent<Collider2D>().isTrigger = true;
        }
    }

    private void checkPlayerOutOfBounds()
    {
        if (gameObject.transform.position.x >= HORIZONTAL_LIMIT_VALUE)
        {
            gameObject.transform.position = new Vector3(
                -HORIZONTAL_LIMIT_VALUE,
                gameObject.transform.position.y,
                gameObject.transform.position.z
            );
        }
        else if (gameObject.transform.position.x <= -HORIZONTAL_LIMIT_VALUE)
        {
            gameObject.transform.position = new Vector3(
                HORIZONTAL_LIMIT_VALUE,
                gameObject.transform.position.y,
                gameObject.transform.position.z
            );
        }
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
            Rb.velocity = new Vector2(Rb.velocity.x, maxDownVelocity);
        }
    }

    private void moveLogic()
    {

        if (GameManager.instance.CanMove)
        {

            float MovxButtons = GameManager.instance.MovXButtons;
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                MovxButtons = -1;
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                MovxButtons = 1;

            }
            if (  (Input.GetKeyDown(KeyCode.Space) || ActionButton.onPressActionButton == true) &&  GameManager.instance.InBarrel==false)
            {
                
                ActionButton.onPressActionButton = false;

               
                jump(1000);
               

            }
      

            if (isGrounded == false)
            {
                 
                if (rb.velocity.y < -0.5f)
                {

                    fallHandler(true);
                    angularDir = 1;

                }
                else
                {
                    fallHandler(false);
                    angularDir = -1;
                }
                Rb.angularVelocity = MovxButtons * angularDir * angularVelocity;


            }
            

            Vector2 newVel = new Vector2(
            (MovxButtons * movementSpeed * Time.deltaTime) + Rb.velocity.x,Rb.velocity.y
        );
            Rb.velocity = newVel;
        }







    }


    public void takeDamage(int damage) // this function damages the player
    {
        int HP = GameManager.instance.PlayerLife;
        Rb.velocity = Vector2.zero;
        HP -= damage;
        GameManager.instance.PlayerLife = HP;
        //this parameter is in the enemy_damagezone.cs
        OnPlayerIsDamaged(damage);
        if (HP <= 0)
        {
            die();
        }
    }
    public void jump(float force)
    {
         
        if (isGrounded)
        {
            GetComponent<AudioSource>().Play();
            rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);
            isGrounded = false;
        }
    }


    public void dash(GameObject Player, float time, float velocity, Vector3 direction)
    {
        stopDash();
        if (isDashing == false && GameManager.instance.InBarrel == false)
        {
            isDashing = true;

            DashCoroutine = applyConstantVelocityToRigidbody(Player, time, velocity, direction);
            StartCoroutine(DashCoroutine);
        }
    }
    public void dash(float time, float velocity, Vector3 direction)
    {
        stopDash();
        if (isDashing == false && GameManager.instance.InBarrel == false)
        {
            isDashing = true;

            DashCoroutine = applyConstantVelocityToRigidbody(this.gameObject, time, velocity, direction);
            StartCoroutine(DashCoroutine);
        }
    }

    public void dash()
    {
        stopDash();
        if (isDashing == false && GameManager.instance.InBarrel == false)
        {
            isDashing = true;
            DashCoroutine = applyConstantVelocityToRigidbody(
                this.gameObject,
                dashTime,
                dashForce,
                transform.up
            );
            StartCoroutine(DashCoroutine);
        }
    }

    public void dash(Vector2 direction)
    {
        stopDash();
        if (isDashing == false && GameManager.instance.InBarrel == false)
        {
            isDashing = true;
            DashCoroutine = applyConstantVelocityToRigidbody(
                this.gameObject,
                dashTime,
                dashForce,
                direction
            );
            StartCoroutine(DashCoroutine);
        }
    }

    public void dash(Vector2 direction, float speed)
    {
        stopDash();
        if (isDashing == false && GameManager.instance.InBarrel == false)
        {
            isDashing = true;
            DashCoroutine = applyConstantVelocityToRigidbody(
                this.gameObject,
                dashTime,
                speed,
                direction
            );
            StartCoroutine(DashCoroutine);
        }
    }

    public void stopDash()
    {
        if (DashCoroutine != null)
        {
            StopCoroutine(DashCoroutine);
            GetComponent<Rigidbody2D>().velocity *= 1;
            GetComponent<Rigidbody2D>().drag = initialDrag;
            GetComponent<Rigidbody2D>().gravityScale = initialGravity;
            animatorHandler.stopDash();
        }
        IsDashing = false;

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

                StartCoroutine(LerpRotation(0.1f, 180));

            }
            else
            {
                StartCoroutine(LerpRotation(0.1f, 0));
            }
        }
    }
    IEnumerator LerpRotation(float duration, float targetAngle)
    {
        float elapsed = 0;
        float startAngle = transform.eulerAngles.z;
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
        // initialGravity = Player.GetComponent<Rigidbody2D>().gravityScale;
        // initialDrag = Player.GetComponent<Rigidbody2D>().drag;
        DashCoroutineSpeed = velocity;
        DashDirection = direction;
        DashCoroutineTime = time;
        Vector3 dash = DashDirection * velocity;
        // issue with this rotation
        float angle = Mathf.Atan2(direction.normalized.y, direction.normalized.x);
        transform.eulerAngles = new Vector3(0, 0, (angle * Mathf.Rad2Deg) - 90);

        print("Constant vel Start " + gameObject.name);
        // moving
        Player.GetComponent<Rigidbody2D>().gravityScale = 0;
        Player.GetComponent<Rigidbody2D>().drag = 0;
        Player.GetComponent<Rigidbody2D>().velocity = dash;
        // Player.GetComponent<PlayerScript>().Anim.SetBool("dash", true);
        animatorHandler.playDash();
        // HANDLER.DASH();
        yield return new WaitForSeconds(time);
        animatorHandler.stopDash();
        //Player.GetComponent<PlayerScript>().Anim.SetBool("dash", false);
        // HANDLER.STOPDASH();
        Player.GetComponent<Rigidbody2D>().velocity *= 1;
        Player.GetComponent<Rigidbody2D>().drag = initialDrag;
        Player.GetComponent<Rigidbody2D>().gravityScale = initialGravity;
        IsDashing = false;
        print("Constant vel ends " + gameObject.name);
        //Player.GetComponent<Rigidbody2D>().velocity = dashDirection/2;
    }

    public void die()
    {
        OnPlayerDied?.Invoke();
    }

    //constructors: CHANGE THE PLAYER SPEED WHEN IT CHANGES THE SCENE ALSO EXPLAIN WHAT YOU DO.
        

    public enum PlayerState
    {
        inBarrel,
        damaged,
        attacking,
        happy,
        walking,
        falling,
        die,
    }

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
    public Animator Anim
    {
        get => animator;
        set => animator = value;
    }
    public Rigidbody2D Rb
    {
        get => rb;
        set => rb = value;
    }

    public PlayerState State
    {
        get => state;
        set => state = value;
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
