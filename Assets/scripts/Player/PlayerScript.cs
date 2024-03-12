
using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.SceneManagement;
using Unity.Mathematics;
using Unity.Android.Types;
using UnityEngine.EventSystems;

 
public class PlayerScript : MonoBehaviour
{

    public static Action<int> OnPlayerGetCoin;
    public static Action<int> OnPlayerIsDamaged;
    public static Action<int> OnEnemyIsDamaged;
    public static Action OnPlayerDied;
    int playerDamage;


    



    void ExecuteOnEnemyIsDamaged(int damage)
    {
        //the parameter is given by the player damaged function
        // while someone is subscribed to this event
        if (OnEnemyIsDamaged != null)
        {
            OnEnemyIsDamaged(damage);
        }

    }
    void ExecuteOnPlayerIsDamaged(int damage)
    {
        if (OnPlayerIsDamaged != null)
        {
            OnPlayerIsDamaged(damage);
        }
    }
    void ExecuteOnPlayerDied()
    {
        if (OnPlayerDied != null)
        {
            OnPlayerDied();
        }
    }







    //object references:
    [SerializeField] GameObject Player;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] private Animator anim;
    [SerializeField] AudioSource bounceSfx;
    private SpriteRenderer sr;


    //int & floats:


    // changes the speed in reward and barrel scene
    [Header("Player Settings")]
    [Range(0, 1000f)]
    [SerializeField] float playerMass=300;
    [Range(0, 1000f)]
    [SerializeField] float playerDrag=0;
    [Range(0, 1000f)]
    [SerializeField] float playerGravity=100;
    [Range(0, 1000f)]
    [SerializeField] private float SpeedValue=300;

    [Range(0, 1000f)]
    [SerializeField] private float maxAngularVelocity = 500f;
    [Range(0, 1000f)]
    [SerializeField] private float xAxisMaxVelocity = 1000f;

    const float HORIZONTAL_LIMIT_VALUE = 180f;
    [SerializeField] private bool dashing;

    [SerializeField] private float upForce;

    [Range(0, 1000f)]
    [SerializeField] private float yAxisMaxVelocity = 300f;
    private float movX;
    private float movY;
    [SerializeField] 
    private float movXGyro;
    
    private bool inRewardScene1;

    




    // i create a vectorX object of the vector2 class and then i assign the properties of the 
    private Vector2 currentPos;
    [SerializeField]
    private Vector2 currentVel;
    [SerializeField] private Vector3 acelerometer;


    

   [SerializeField] 
    bool gyro;

    [SerializeField]
    bool Buttons;


   [SerializeField] 
    string moveInput;

    
    
    private string animatorParameterMove = "move";
     

    public float UpForce1 { get => UpForce; set => UpForce = value; }
    public float UpForce { get => upForce; set => upForce = value; }
    public bool Dashing { get => dashing; set => dashing = value; }

    private void OnEnable()
    {
        // event subscribers
        // EnemyHitBox.hit_box_event1 += JumpOverEnemy;

      
        
    }
    private void OnDisable()
    {
        // event subscribers
        //EnemyHitBox.hit_box_event1 -= JumpOverEnemy;
  
    }
    
    void Start()
    {

        int HP = GameManager.instance.PlayerPurchasedHearts;
        GameManager.instance.PlayerLife = HP;
        Player = GameObject.FindWithTag("Player");
        rb = gameObject.GetComponent<Rigidbody2D>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        anim = gameObject.GetComponent<Animator>();   

        SpeedValue = GameManager.instance.PlayerSpeed;
        



    }
    

    void Update()

    {
        //rb.gravityScale = playerGravity;
        //rb.drag = playerDrag;
        //rb.mass = playerMass;
        
        



        // checkear rotacion para sombreado


        GameManager.instance.PlayerScore = (transform.position.y);
        currentVel = rb.velocity;
        movX=Input.GetAxis("Horizontal");
        movY=Input.GetAxis("Vertical");


        acelerometer = Input.acceleration;
        movXGyro = Input.acceleration.x;
        movXGyro = calibrateGyro(movXGyro);


        if (GameManager.instance.IsInRewardScene == false)
        {
            if (!dashing)
            {
                if (gyro && !Buttons)
                {

                    //rb.AddForce(Vector2.right * movXGyro * Time.deltaTime * SpeedValue, ForceMode2D.Force);
                    //rb.AddTorque(-movXGyro * 5f * Time.deltaTime);
                    Vector2 newVel= new Vector2(10f,rb.velocity.y);

                    rb.velocity = newVel;
                }
                if (Buttons && !gyro)
                {
                    int MovxButtons = GameManager.instance.MovXButtons;
                    //rb.AddForce(Vector2.right * MovxButtons * Time.deltaTime * SpeedValue, ForceMode2D.Force);
                    //rb.AddTorque(-MovxButtons * 5f * Time.deltaTime);
                    Vector2 newVel = new Vector2((MovxButtons * SpeedValue*Time.deltaTime) + rb.velocity.x, rb.velocity.y);
                    
                    rb.velocity = newVel;
                }
                else if (!gyro && !Buttons)
                {
                    //rb.AddForce(Vector2.right * movX * Time.deltaTime * SpeedValue, ForceMode2D.Force);
                    //rb.AddTorque(-movX * 5f * Time.deltaTime);

                    int MovxButtons = GameManager.instance.MovXButtons;
                    Vector2 newVel = new Vector2((MovxButtons * SpeedValue * Time.deltaTime) + rb.velocity.x, rb.velocity.y);
                    rb.velocity=newVel;
                }
            }
            


            //if the player press the down arrows.
            if (movY == -1)
            {

                rb.AddForce(Vector2.down * Time.deltaTime * SpeedValue * 0.3f, ForceMode2D.Impulse);

            }


        }




        //max velocities.

        if (GameManager.instance.IsInRewardScene==false)
        {
            // x axis
            if (gameObject.transform.position.x >= HORIZONTAL_LIMIT_VALUE)
            {
                gameObject.transform.position = new Vector3(-HORIZONTAL_LIMIT_VALUE, gameObject.transform.position.y, gameObject.transform.position.z);
            }
            else if (gameObject.transform.position.x <= -HORIZONTAL_LIMIT_VALUE)
            {
                gameObject.transform.position = new Vector3(HORIZONTAL_LIMIT_VALUE, gameObject.transform.position.y, gameObject.transform.position.z);
            }
            if (rb.velocity.x >= xAxisMaxVelocity)
            {
                //print("reach max velocity in X axis"%Colorize.Red);
                rb.velocity = new Vector2(xAxisMaxVelocity, rb.velocity.y);

            }
            if (rb.velocity.x <= -xAxisMaxVelocity)
            {
                //print("reach max velocity in -X axis" % Colorize.Blue);

                rb.velocity = new Vector2(-xAxisMaxVelocity, rb.velocity.y);

            }

            // y axis
            if (rb.velocity.y >= yAxisMaxVelocity)
            {
                //print("reach max velocity in Y axis" % Colorize.Purple);
                rb.velocity = new Vector2(rb.velocity.x, yAxisMaxVelocity);

            }
            if (rb.velocity.y <= -yAxisMaxVelocity)
            {
                //print("reach max velocity in -Y axis" % Colorize.Magenta);
                rb.velocity = new Vector2(rb.velocity.x, -yAxisMaxVelocity);

            }
            //  angular velocity
            if (rb.angularVelocity >= maxAngularVelocity)
            {
                //rb.AddTorque(-30f);
            }
            if (rb.angularVelocity <= -maxAngularVelocity)
            {
                //rb.AddTorque(30f);
            }


            if (Input.GetKeyDown(KeyCode.Space) && movX != 0)
            {

                //if (movX >= 1)
                //{
                //    rb.AddForce(Vector2.right * Time.deltaTime * SpeedValue, ForceMode2D.Impulse);

                //}
                //if (movX <= -1)
                //{
                //    rb.AddForce(Vector2.left * Time.deltaTime * SpeedValue, ForceMode2D.Impulse);
                //}


            }
            if (Input.GetKeyDown("space") && !GameManager.instance.InBarrel)
            {

                //ImpulsePlayer("down", 500);
                //// we set the angular to 0 to stop rotating the object
                //rb.angularVelocity = 0;
                


            }
            if((Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0)) && !GameManager.instance.InBarrel)
            {
                 
                //rb.AddForce(Vector3.up * 1000 * Time.deltaTime);
                //rb.AddTorque(movX*5 * Time.deltaTime);
            }
             
        }
        else if(GameManager.instance.IsInRewardScene)   
        {
            // is in reward zone 

            if (gyro)
            {
                rb.AddForce(Vector2.right * movXGyro * Time.deltaTime * 100 * SpeedValue, ForceMode2D.Force);

            }
            else if (gyro == false)
            {

                rb.AddForce(Vector2.up * movY * Time.deltaTime * 100 * SpeedValue, ForceMode2D.Force);
                rb.AddForce(Vector2.right * movX * Time.deltaTime * 100 * SpeedValue, ForceMode2D.Force);
            }



            if (rb.velocity.x >= xAxisMaxVelocity)
            {
                rb.velocity = new Vector2(xAxisMaxVelocity, rb.velocity.y);

            }
            if (rb.velocity.x <= -xAxisMaxVelocity)
            {
                rb.velocity = new Vector2(-xAxisMaxVelocity, rb.velocity.y);

            }

            // y axis
            if (rb.velocity.y >= yAxisMaxVelocity)
            {
                rb.velocity = new Vector2(rb.velocity.x, yAxisMaxVelocity);

            }
            if (rb.velocity.y <= -yAxisMaxVelocity)
            {
                rb.velocity = new Vector2(rb.velocity.x, -yAxisMaxVelocity);

            }
            //  angular velocity
            if (rb.angularVelocity >= maxAngularVelocity)
            {
                //rb.AddTorque(-30f);
            }
            if (rb.angularVelocity <= -maxAngularVelocity)
            {
                //rb.AddTorque(30f);
            }


        }









    }
    //private void FixedUpdate()
    //{

       


    //    // Horizontal move
        

        
        
        

    //    //if the player moves to the left.
    //    if (movX<=-1)
    //    {
                
    //            // sr.flipX = true;
    //            //anim.SetBool(animatorParameterMove, true) ;
        

    //    }

    //    //if the player moves to the right
    //    if (movX>=1)
    //        {
    //            // sr.flipX = false;
    //            //anim.SetBool(animatorParameterMove, true);
                
              

    //            // you want to verify when the player press the buttons when is also moving.
    //            // by this way when you press the space or w it always be driven.
                
                 

    //    }

    //    //if the player is not moving
    //    if (movX ==0)
    //        {
    //            //anim.SetBool(animatorParameterMove, false);
    //        }     

        
    //        // añadir una funcion que cuando tu apretes una tecla este aumente su escala, para asi pasar ciertos obstaculos.
    //        //añadir una funcion que cuando tu apretes espacio este vaya como acelerando como si fuese un hamster en una pelota
 

       
  
    //}
    private void OnTriggerEnter2D(Collider2D collision)
    {
         if (collision.gameObject.CompareTag("coin"))
        {
             
            OnPlayerGetCoin?.Invoke(1);
             
            Destroy(collision.gameObject);
                                    
                      
        }
        else if (collision.gameObject.name=="enemyHitBox"){
            ExecuteOnEnemyIsDamaged(playerDamage);
            GameManager.instance.EnemiesKilled++;
            collision.gameObject.GetComponentInParent<Enemy>().die();
            JumpOverEnemy(upForce);
             

    

        }
        else if (collision.gameObject.name == "enemyDamageZone"){
            ExecuteOnPlayerIsDamaged(1);
            collision.gameObject.GetComponentInParent<Enemy>().die();
            DamagePlayer(1);
            
            
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

    public void JumpOverEnemy(float UpForce) // impulse the player up when jumps over an enemy
    {
            
            rb.velocity= Vector3.zero;
            // rb.AddForce(-currentVel, ForceMode2D.Impulse);
            ImpulsePlayer("up", UpForce);
            print("Player, kills enemy");
         
    }

    public void ImpulsePlayer(string direction,float amount) //general function to impulse the player to any direction 
    {


        if (direction == "up" && !GameManager.instance.InBarrel) {
            rb.AddForce(Vector2.up * amount*Time.deltaTime, ForceMode2D.Impulse);

        }
        else if(direction == "down"&& !GameManager.instance.InBarrel)
        {   
            rb.AddForce(Vector2.up * -amount*Time.deltaTime, ForceMode2D.Impulse);


        }
        else if(direction == "left" && !GameManager.instance.InBarrel)
        {
            rb.AddForce(Vector2.left * amount * Time.deltaTime, ForceMode2D.Impulse);

        }
        else if (direction == "right" && !GameManager.instance.InBarrel)
        {
            rb.AddForce(Vector2.right * amount * Time.deltaTime, ForceMode2D.Impulse);

        }





    }
    public void DamagePlayer(int damage) // this function damages the player 
    {
        int HP = GameManager.instance.PlayerLife;
        rb.velocity = Vector2.zero;
        HP-=damage;
        GameManager.instance.PlayerLife = HP;
        //this parameter is in the enemy_damagezone.cs
        if(HP <= 0) {
            ExecuteOnPlayerDied();
        }
        
         

         
    }
    
    void Reached_level()
    {

        print("reached level 2");

    }
   
  
    
    private void RotateObject(float rotationSpeed)
    {
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }

    private void ResetAngularVelocityRot()
    {
        rb.angularVelocity = 0;

    }



    //constructors:CHANGE THE PLAYER SPEED WHEN IT CHANGES THE SCENE ALSO EXPLAIN WHAT YOU DO.
    public float getSpeed()
    {
        return SpeedValue;
    }

    public void setSpeed(float new_speed)
    {
        SpeedValue= new_speed;
    }
    float calibrateGyro(float gyro)
    {
        if (gyro > 0)
        {
            if (gyro > (0.2f))
            {
                gyro = 1f;
            }
            if (gyro <= (0.2f))
            {
                gyro = 0f;

            }

        }
        else if (gyro < 0)
        {
            if (gyro < -(0.2f))
            {
                gyro = -1f;
            }
            if (gyro >= -(0.2f))
            {
                gyro = 0f;

            }
        }
        return gyro;
    }




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
