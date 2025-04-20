using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    [SerializeField] KnockbackFeedBack feedBack;
    void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.GetComponent<KnockbackFeedBack>() && collision.gameObject.CompareTag("Player"))
        {
            // Calculate knockback direction based on spike orientation
            
            Vector2 spikeDirection = transform.up;

            // Knockback source is the spike's position
            Vector2 knockbackSource = transform.position;

            // Trigger knockback feedback
            
            // take values from lava feedback
            collision.gameObject.GetComponent<KnockbackFeedBack>().PlayFeedBack(knockbackSource, spikeDirection,feedBack);

            print($"Knockback applied: Direction = {spikeDirection}, Source = {knockbackSource}");
        }

        if (collision.gameObject.GetComponent<IDamageable>() != null)
        {
            // Apply damage

            collision.gameObject.GetComponent<IDamageable>().takeDamage(1);
        }
    }
 

    // Start is called before the first frame update
 
}
