using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour, IEnemyBehaviour
{
    [SerializeField] KnockbackFeedBack feedBack;
    [SerializeField] int lavaDamage = 1;

    public int Damage { get => lavaDamage; set => lavaDamage = value; }


    void OnTriggerEnter2D(Collider2D collision)
    {
        // separate the logic between push and damage because the lava always pushes
        pushObject(collision.gameObject); 
        dealDamage(collision.gameObject); 
    }
    public void dealDamage(GameObject o)
    {
         // do this 
        if (o.TryGetComponent<IDamageable>(out IDamageable damageable) && damageable.CanTakeDamage)
        {
            // lava deals damage only when tha player is damaged
            damageable.takeDamage(lavaDamage);
        }

    }
    public void pushObject(GameObject o)
    {
        if (o.CompareTag("Player"))
        {
            pushPlayer(o);
        }
    } 
    public void pushPlayer(GameObject o)
    {
        if (o.TryGetComponent<KnockbackFeedBack>(out KnockbackFeedBack knocback))
        {
            // Calculate knockback direction based on spike orientation
            Vector2 shootDirection = transform.up;

            // Knockback source is the spike's position
            Vector2 knockbackSource = transform.position;

            // default object push feedback
            knocback.triggerFeedbackWithReference(knockbackSource, shootDirection, feedBack);

            print($"Knockback applied: Direction = {shootDirection}, Source = {knockbackSource}, Object = {knocback.name}");
        }
    }



}
