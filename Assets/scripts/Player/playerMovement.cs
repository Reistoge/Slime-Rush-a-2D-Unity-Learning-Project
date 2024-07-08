using UnityEngine;

public class playerMovement : MonoBehaviour
{
    // barrel





    //

    //all the things that use gravity, physics and other things, you use in fixed update.

    public float SPEED_AMOUNT = 300f;
    public float speed8 = 0f;
    public float jumpSpeed;
    public float jumpspeed8 = 0f;
    private float MAX_ANGULAR_VELOCITY = 10f;
    private float MAX_X_AXIS_VELOCITY = 20f;
    private float MAX_Y_AXIS_VELOCITY = 10f;
    Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;


    // i create a vectorX object of the vector2 class and then i assign the properties of the 

    public Vector2 currentPos;
    public Vector2 currentVel;
    public Vector2 speed2;

    //create a vector and a quaternion null to the set the initial position and initialrotation.


    public int movement;
    float movX;


    [SerializeField] string moveInput;

    // variables for jumping

    private bool canJump = true;

    // Start is called before the first frame update
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private SpriteRenderer sr;
    private string MOVE_STRING = "move";


    void Start()
    {




        rb = gameObject.GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        //the vectors will store the initial position of the object
        //initialPosition = transform.position;
        //initialRotation = transform.rotation;
    }

    // Update is ++called once per frame
    void Update()

    {
        currentVel = rb.velocity;
        movX = Input.GetAxisRaw("Horizontal");

        if (Input.GetKey("1"))
        {
            jumpSpeed = 0;
            SPEED_AMOUNT = 1;

            print("movement set to 1, change position, physics independent");
            movement = 1;

        }
        if (Input.GetKey("2"))
        {
            print("movement set to 2, constant velocity, set velocity");
            movement = 2;
        }
        if (Input.GetKey("3"))
        {
            print("movement set to 3, adds velocity every seconds, accelerates.");
            movement = 3;

        }
        if (Input.GetKey("4"))
        {
            print("movement set to 4 add force");
            movement = 4;
        }
        if (Input.GetKey("5"))
        {
            print("movement set to 5, adds impulse");

            movement = 5;
            jumpSpeed = 30f;
            SPEED_AMOUNT = 100f;
        }
        if (Input.GetKey("6"))
        {
            print("movement set to 6");
            movement = 6;
        }
        if (Input.GetKey("7"))
        {
            print("movement set to 7");
            movement = 7;
        }
        if (Input.GetKey("8"))
        {
            print("movement set to 8");
            movement = 8;
        }

        if (movement == 1)
        {
            // with this type of movement is better to use when
            // you dont wanna use physics in your game also the rotations 
            // and other things to consider, this is the current position,
            // if the object is rotated the object will fly if the vector 
            //is forward because is the current position of the object
            // this type of movement only works or is well used when your 
            // object dont have a rigidbody, and dont rotatate, 
            if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(Vector2.right * Time.deltaTime * SPEED_AMOUNT);

            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(Vector2.left * Time.deltaTime * SPEED_AMOUNT);

            }
            if (Input.GetKey(KeyCode.W))
            {
                transform.Translate(Vector2.up * Time.deltaTime * SPEED_AMOUNT);

            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(Vector2.down * Time.deltaTime * jumpSpeed);
            }
        }
        if (movement == 6)
        {

            rb.AddForce(new Vector3(Input.GetAxis("Horizontal") * speed2.x * Time.deltaTime, 0, Input.GetAxis("Vertical") * speed2.y * Time.deltaTime));

        }
        if (movement == 8)
        {
            transform.Translate(new Vector3(movX, 0, 0) * Time.deltaTime * speed8);

        }


        //max velocities.


        if (rb.velocity.x >= MAX_X_AXIS_VELOCITY)
        {
            rb.velocity = new Vector2(MAX_X_AXIS_VELOCITY, rb.velocity.y);

        }
        if (rb.velocity.x <= -MAX_X_AXIS_VELOCITY)
        {
            rb.velocity = new Vector2(-MAX_X_AXIS_VELOCITY, rb.velocity.y);

        }
        if (rb.angularVelocity >= MAX_ANGULAR_VELOCITY)
        {
            rb.angularVelocity = MAX_ANGULAR_VELOCITY;
        }
        if (rb.angularVelocity <= -MAX_ANGULAR_VELOCITY)
        {
            rb.angularVelocity = -MAX_ANGULAR_VELOCITY;
        }

    }
    private void FixedUpdate()
    {

        if (movement == 8)
        {
            if (Input.GetButtonDown("Jump") && canJump)
            {

                canJump = false;
                rb.AddForce(new Vector3(0, jumpspeed8, 0), ForceMode2D.Impulse);
            }


        }

        if (movement == 2)
        {
            // if in the y axis you put 0 the object in the midair when you move it it will start to float becasue in the air
            // the the velocity is setting to 2 in the x axis and 0 in the y axis instead of being negative in the y axis due gravity.


            Vector2 right = new Vector2(SPEED_AMOUNT * Time.deltaTime, rb.velocity.y);
            Vector2 left = new Vector2(-SPEED_AMOUNT * Time.deltaTime, rb.velocity.y);
            Vector2 up = new Vector2(rb.velocity.x, SPEED_AMOUNT * Time.deltaTime);
            Vector2 down = new Vector2(rb.velocity.x, -SPEED_AMOUNT * Time.deltaTime);



            if (Input.GetKey(KeyCode.D))
            {
                rb.velocity = right;

            }
            if (Input.GetKey(KeyCode.A))
            {
                rb.velocity = left;

            }
            if (Input.GetKey(KeyCode.W))
            {
                rb.velocity = up;

            }
            if (Input.GetKey(KeyCode.S))
            {
                rb.velocity = down;

            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.velocity += Vector2.up * Time.deltaTime * jumpSpeed;

            }




        }
        if (movement == 3)
        {
            #region movement 3 code "add velocity"
            if (Input.GetKey(KeyCode.D))
            {
                rb.velocity += Vector2.right * Time.deltaTime * SPEED_AMOUNT;

            }
            if (Input.GetKey(KeyCode.A))
            {
                rb.velocity += Vector2.left * Time.deltaTime * SPEED_AMOUNT;

            }
            if (Input.GetKey(KeyCode.W))
            {
                rb.velocity += Vector2.up * Time.deltaTime * SPEED_AMOUNT;

            }
            if (Input.GetKey(KeyCode.S))
            {
                rb.velocity += Vector2.up * Time.deltaTime * SPEED_AMOUNT;

            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.velocity += Vector2.up * Time.deltaTime * jumpSpeed;

            }
            #endregion
            //here you are sum a vector to the current velocity of the object
        }
        if (movement == 4)
        {
            // add force
            #region Movement 4 code "add force"
            SPEED_AMOUNT = 500;

            if (Input.GetKey(KeyCode.D))
            {
                rb.AddForce(Vector2.right * Time.deltaTime * SPEED_AMOUNT, ForceMode2D.Force);

            }
            if (Input.GetKey(KeyCode.A))
            {

                rb.AddForce(Vector2.left * Time.deltaTime * SPEED_AMOUNT, ForceMode2D.Force);

            }
            if (Input.GetKey(KeyCode.W))
            {
                rb.AddForce(Vector2.up * Time.deltaTime * SPEED_AMOUNT, ForceMode2D.Force);

            }
            if (Input.GetKey(KeyCode.S))
            {
                rb.AddForce(Vector2.down * Time.deltaTime * SPEED_AMOUNT, ForceMode2D.Force);

            }


            #endregion
        }
        if (movement == 5)
        {
            //add impulse
            #region Movement 5 code "add impulse"
            if (Input.GetKey(KeyCode.D))
            {

                rb.AddForce(Vector2.right * Time.deltaTime * SPEED_AMOUNT, ForceMode2D.Impulse);

            }
            if (Input.GetKey(KeyCode.A))
            {
                rb.AddForce(Vector2.left * Time.deltaTime * SPEED_AMOUNT, ForceMode2D.Impulse);

            }
            if (Input.GetKey(KeyCode.W))
            {
                rb.AddForce(Vector2.up * Time.deltaTime * SPEED_AMOUNT, ForceMode2D.Impulse);

            }
            if (Input.GetKey(KeyCode.S))
            {
                rb.AddForce(Vector2.down * Time.deltaTime * SPEED_AMOUNT, ForceMode2D.Impulse);

            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.AddForce(Vector2.up * jumpSpeed * Time.deltaTime, ForceMode2D.Impulse);
            }



            #endregion 
        }

        if (movement == 7)// add force
        {
            //speed=300
            //angular=0,05
            //linear drag=2
            //jump speed=150

            //section 1: horizontal move
            // when the user hits the "A" or "B" button
            #region Horizontal move





            if (Input.GetKey(KeyCode.D))
            {
                sr.flipX = false;
                anim.SetBool(MOVE_STRING, true);
                moveInput = "Right";
                rb.AddForce(Vector2.right * Time.deltaTime * SPEED_AMOUNT, ForceMode2D.Force);

                // you want to verify when the player press the buttons when is also moving.
                // by this way when you press the space or w it always be driven.
                if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.Space))
                {

                    if (moveInput == "Right")
                    {
                        rb.AddForce(Vector2.right * Time.deltaTime * SPEED_AMOUNT, ForceMode2D.Impulse);
                    }
                    if (moveInput == "Left")
                    {
                        rb.AddForce(Vector2.left * Time.deltaTime * SPEED_AMOUNT, ForceMode2D.Impulse);
                    }


                }


            }
            if (movX == 0)
            {
                anim.SetBool(MOVE_STRING, false);
            }


            if (Input.GetKey(KeyCode.A))
            {
                moveInput = "Left";
                sr.flipX = true;
                anim.SetBool(MOVE_STRING, true);

                rb.AddForce(Vector2.left * Time.deltaTime * SPEED_AMOUNT, ForceMode2D.Force);
                if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.Space))
                {
                    if (moveInput == "Right")
                    {
                        rb.AddForce(Vector2.right * Time.deltaTime * SPEED_AMOUNT, ForceMode2D.Impulse);
                    }
                    if (moveInput == "Left")
                    {
                        rb.AddForce(Vector2.left * Time.deltaTime * SPEED_AMOUNT, ForceMode2D.Impulse);
                    }


                }



            }
            #endregion



            if (Input.GetKey(KeyCode.S))
            {
                rb.AddForce(Vector2.down * Time.deltaTime * SPEED_AMOUNT * 0.5f, ForceMode2D.Impulse);

            }




            // añadir una funcion que cuando tu apretes una tecla este aumente su escala, para asi pasar ciertos obstaculos.
            //añadir una funcion que cuando tu apretes espacio este vaya como acelerando como si fuese un hamster en una pelota

        }



        // section 3: vector that always points in head direction of the gameobject player.

        // este vector siempre estara mirando hacia arriba del objeto como la normal.
        // direccion rotacion es un vector qu siempre esta mirando encima del gameobject






    }






    private void RotateObject(float rotationSpeed)
    {
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }

    private void ResetAngularVelocityRot()
    {
        rb.angularVelocity = 0;

    }


    // dos formas de verificar las colisiones entre objetos, puede ser mediante oncollisionenter que detecta cada vez que colisionan dos objetos con sus rb
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))

        {   // si el gameobject de la collsion tiene un tag (ground)

            canJump = true;
        }

    }
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
