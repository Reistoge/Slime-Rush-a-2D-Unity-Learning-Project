using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class BoxStickySpikes : MonoBehaviour, IStickable
{

    GameObject sticked;
    [SerializeField] GameObject baseEnemy;
    private float timeStick;
    Animator animator;
    [SerializeField] SpriteRenderer baseSr;
    //ideas.
    // enemy1. enemy always with spikes up, downs when sticks with player.
    // enemy2. what if the enemy detects when the object is near ?? ---> spikes up !!.
    // cannons with spikes !!!.

    // things to have in mind:
    // add core retro anim ??.


    private void Start()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        stickListener(collision);


    }
    void stickListener(Collider2D collision)
    {
        // if the object that enters is the 
        if (collision.gameObject.tag == "Player")
        {
            GameObject player = collision.gameObject;
            //if the player collides with the object execute the 
            //here is the damages that makes a regular enemy


            if (collision.gameObject.GetComponent<PlayerScript>().IsDashing == true)
            {

                // this functions stops the last cannon
                GameManager.instance.stopCannonDash();


            }


        }
        if (sticked == null)
        {
            print(collision.gameObject.name + " is touching the collider of " + this.name);
            stickObject(collision.gameObject);
            Invoke("deStickObject", 3);




        }
    }

    public void stickObject(GameObject collision)
    {

        if (baseEnemy != null)
        {
            baseEnemy.GetComponent<IEnemyBehaviour>().dealDamage(collision);
            baseSr.color = Color.red; // ----> enemy anim
        }



        sticked = collision;

        sticked.gameObject.transform.parent = transform;// we assign new parent for the position.
        if (sticked.GetComponent<Rigidbody2D>() != null)
        {
            sticked.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;// ---->  no physics

        }
    }
    public void deStickObject()
    {
        hideSpike();
        if (sticked != null)
        {
            // we freeze the constrainst 
            if (sticked.GetComponent<Rigidbody2D>() != null)
            {
                sticked.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None; // -----> physics back to normal
                sticked.GetComponent<Rigidbody2D>().AddForce(transform.up * 300, ForceMode2D.Impulse);// ----> player is impulsed (add animation)
            }
            sticked.gameObject.transform.parent = null;

            if (baseEnemy != null)
            {
                baseSr.color = Color.white; // ----> enemy anim

            }

            sticked = null;
            Invoke("unHideSpike", 1);//-> we need no enable the sprite renderer (part of the animation)

        }
    }
    // enable spriterenderer can be part of an animation
    public void hideSpike()
    {

        // this.GetComponent<SpriteRenderer>().enabled = false;
        this.animator.SetTrigger("hide");
        this.GetComponent<PolygonCollider2D>().enabled = false;
    }
    public void unHideSpike()
    {
        //enableSpriteRenderer();
        this.animator.SetTrigger("up");
        this.GetComponent<PolygonCollider2D>().enabled = true;

    }
    public void enableSpriteRenderer()
    {
        this.GetComponent<SpriteRenderer>().enabled = true;
    }


    public float TimeStick { get => timeStick; set => timeStick = value; }

    public GameObject Sticked { get => sticked; set => sticked = value; }

}
