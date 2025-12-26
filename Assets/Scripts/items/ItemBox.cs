using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class ItemBox : MonoBehaviour
{
     [SerializeField] KnockbackFeedBack feedBack;
    void OnCollisionEnter2D(Collision2D collision)
    {
        // separate the logic between push and damage because the lava always pushes
        pushObject(collision.gameObject);

    }
    public void pushObject(GameObject o)
    {
        if (o.CompareTag("Player"))
        {
            pushPlayer(o);
        }
    }

    private void pushPlayer(GameObject o)
    {

        if (o.TryGetComponent<KnockbackFeedBack>(out KnockbackFeedBack knocback) &&
            o.TryGetComponent<PlayerScript>(out PlayerScript player) &&
            player.IsDashing)
        {
            player.transform.position = new Vector2(transform.position.x, player.transform.position.y + 1.5f);
            player.stopDash();
            
            Vector2 shootDirection = transform.up; // ignore this vector, just look the SO horizontal and vertical directions

            // Knockback source is the spike's position
            Vector2 knockbackSource = this.transform.position;
            knocback.triggerFeedbackWithReference(knockbackSource, shootDirection, feedBack);

            print($"Knockback applied: Direction = {shootDirection}, Source = {knockbackSource}, Object = {gameObject.name}");
        }

    }
}
