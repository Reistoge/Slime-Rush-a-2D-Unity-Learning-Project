using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class EnemyDamageZone : MonoBehaviour
{

 



     private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameObject player= collision.gameObject;
            //if the player collides with the object execute the 
            //here is the damages that makes a regular enemy


            if (collision.gameObject.GetComponent<PlayerScript>().Dash == true)
            {
                GameManager.instance.stopCannonDash();


            }
            player.GetComponent<PlayerScript>().takeDamage(1);
            player.GetComponent<Rigidbody2D>().AddForce(transform.up * 300, ForceMode2D.Impulse);
            player.GetComponent<Animator>().Play("takeDamage");
           
            

        }
    }
}
