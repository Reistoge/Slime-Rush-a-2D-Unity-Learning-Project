
using System;
using UnityEngine;


public class PlayerScript : MonoBehaviour 
{

    public static Action<int> OnPlayerGetCoin;
    public static Action<int> OnPlayerIsDamaged;
    public static Action<int> OnEnemyIsDamaged;
    public static Action OnPlayerDied;
    int playerDamage;
    int hp;
    int maxHp;
    [SerializeField]bool dash;

 


    //object references:
    [SerializeField] GameObject Player;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] private Animator anim;
    [SerializeField] AudioSource bounceSfx;
    private SpriteRenderer sr;


    //int & floats:


    // changes the speed in reward and barrel scene
    [Header("Player Settings")]
    const float HORIZONTAL_LIMIT_VALUE = 180f;
    [Range(0, 1000f)]
    [SerializeField] private float movementSpeed = 300;
    [Range(0, 1000f)]
    [SerializeField] private float maxRotationVelocity = 500f;
    [Range(0, 1000f)]
    [SerializeField] private float maxHorizontalVelocity = 1000f;
    [Range(0, 1000f)]
    [SerializeField] private float maxUpVelocity = 300f;
    [Range(0, -1000f)]
    [SerializeField] private float maxDownVelocity = 300f;
    [SerializeField] private float upForce;

     
    [SerializeField] bool usingGyro;
    [SerializeField] bool usingButtons;
    [SerializeField] string moveInput;
    //private Vector2 currentPos;
    [SerializeField]
    private Vector2 currentVel;
    //[SerializeField] private Vector3 acelerometer;


    private float movX;
    private float movY;
    private float movXGyro;
    private bool inRewardScene1;

 
    // i create a vectorX object of the vector2 class and then i assign the properties of the 
 

    void Start()
    {

        maxHp = GameManager.instance.PlayerPurchasedHearts;
        hp = maxHp;
        GameManager.instance.PlayerLife = hp;
        Player = GameObject.FindWithTag("Player");
        rb = gameObject.GetComponent<Rigidbody2D>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        anim = gameObject.GetComponent<Animator>();

        movementSpeed = GameManager.instance.PlayerSpeed;
       



    }


    void Update()

    {
        // checkear rotacion para sombreado
        GameManager.instance.PlayerScore = (transform.position.y);
        currentVel = rb.velocity;
            

             moveLogic();

            checkPlayerOutOfBounds();
            checkMaxVelocityOnHorizontalAxis();
            checkMaxVelocityOnVerticalAxis();

            //if the player press the down arrows.
           


        



        ////max velocities.

    
         
            // x axis


           
  

    }
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("coin"))
        {

            OnPlayerGetCoin?.Invoke(1);
            collision.GetComponent<Coin>().playerGetCoin();




        }
         

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "bounceWall")
        {
            collision.gameObject.GetComponent<Animator>().SetTrigger("bounce");
            bounceSfx.Play();
        }
    }
    private void checkPlayerOutOfBounds()
    {
        if (gameObject.transform.position.x >= HORIZONTAL_LIMIT_VALUE)
        {
            gameObject.transform.position = new Vector3(-HORIZONTAL_LIMIT_VALUE, gameObject.transform.position.y, gameObject.transform.position.z);
        }
        else if (gameObject.transform.position.x <= -HORIZONTAL_LIMIT_VALUE)
        {
            gameObject.transform.position = new Vector3(HORIZONTAL_LIMIT_VALUE, gameObject.transform.position.y, gameObject.transform.position.z);
        }

    }
    private void checkMaxVelocityOnHorizontalAxis()
    {
        if (rb.velocity.x >= maxHorizontalVelocity)
        {
            //print("reach max velocity in X axis"%Colorize.Red);
            rb.velocity = new Vector2(maxHorizontalVelocity, rb.velocity.y);

        }
        if (rb.velocity.x <= -maxHorizontalVelocity)
        {
            //print("reach max velocity in -X axis" % Colorize.Blue);

            rb.velocity = new Vector2(-maxHorizontalVelocity, rb.velocity.y);

        }


    }
    private void checkMaxVelocityOnVerticalAxis()
    {
        // y axis
        if (rb.velocity.y >= maxUpVelocity)
        {
            //print("reach max velocity in Y axis" % Colorize.Purple);
            rb.velocity = new Vector2(rb.velocity.x, maxUpVelocity);

        }
        if (rb.velocity.y <= maxDownVelocity)
        {
            //print("reach max velocity in -Y axis" % Colorize.Magenta);
            rb.velocity = new Vector2(rb.velocity.x, maxDownVelocity);

        }
     
    }
    private void moveLogic()
    {
        int MovxButtons = GameManager.instance.MovXButtons;
        Vector2 newVel = new Vector2((MovxButtons * movementSpeed * Time.deltaTime) + rb.velocity.x, rb.velocity.y);
        rb.velocity = newVel;

        if (transform.up.y > 0)
        {
            rb.angularVelocity = -MovxButtons * 50;

        }
        else if (transform.up.y < 0)
        {
            rb.angularVelocity = MovxButtons * 50 ;

        }
    }
    public void takeDamage(int damage) // this function damages the player 
    {
        
        int HP = GameManager.instance.PlayerLife;
        
        rb.velocity = Vector2.zero;
        HP -= damage;
        GameManager.instance.PlayerLife = HP;
        //this parameter is in the enemy_damagezone.cs
        OnPlayerIsDamaged(damage);
        if (HP <= 0)
        {
            die();
        }
        
       
         



    }
    public void impulsePlayer(float intensity,Vector2 direction)
    {
        rb.AddForce(transform.up * intensity,ForceMode2D.Impulse);
    }
    public void die()
    {
        OnPlayerDied?.Invoke();
    }

    



     

     



    //constructors:CHANGE THE PLAYER SPEED WHEN IT CHANGES THE SCENE ALSO EXPLAIN WHAT YOU DO.
    
    public float UpForce { get => upForce; set => upForce = value; }
   
    public int PlayerDamage { get => playerDamage; set => playerDamage = value; }
    public int Hp { get => hp; set => hp = value; }
    public int MaxHp { get => maxHp; set => maxHp = value; }
    public bool Dash { get => dash; set => dash = value; }







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
