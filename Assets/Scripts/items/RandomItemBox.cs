using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using Vector2 = UnityEngine.Vector2;

public class RandomItemBox : MonoBehaviour, IPurchasable, IBreakable
{
    [SerializeField] KnockbackFeedBack feedBack;
    [SerializeField] int itemBoxPrice = 25;
    [SerializeField] UnityEvent onItemPurchased;
    [SerializeField] RandomItemBoxAnimHandler animator;
    [SerializeField] SpawnRandom randomSpawner;
    [SerializeField] Dialogue textDialogue;
    [SerializeField] BoxCollider2D col;

    public int Price { get => itemBoxPrice; set => itemBoxPrice = value; }
    public UnityEvent OnItemPurchased { get => onItemPurchased; set => onItemPurchased = value; }

    void Start()
    {
        // onItemPurchased.AddListener(breakObject);
        // onItemPurchased.AddListener(() => col.enabled= false);

        if (textDialogue.isActiveAndEnabled)
        {

            textDialogue.TextMeshPro.text = itemBoxPrice.ToString();

        }
    }
    

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
            purchase();
            player.transform.position = new Vector2(transform.position.x, player.transform.position.y + 1.5f);
            player.stopDash();

            Vector2 shootDirection = transform.up; // ignore this vector, just look the SO horizontal and vertical directions

            // Knockback source is the spike's position
            Vector2 knockbackSource = this.transform.position;
            knocback.triggerFeedbackWithReference(knockbackSource, shootDirection, feedBack);

            print($"Knockback applied: Direction = {shootDirection}, Source = {knockbackSource}, Object = {gameObject.name}");
        }

    }

    public void purchase()
    {
        int currentCoins = GameManager.Instance.getPlayerCoins();
        if (currentCoins >= itemBoxPrice)
        {
            //GameManager.instance.setPlayerCoins(currentCoins - itemBoxPrice);
            GameManager.Instance.onPlayerGetCoins(-itemBoxPrice); // problem here, we dont know the current change value for the anim
            onItemPurchased?.Invoke();
        }
        else
        {
            // play the error sound
            //GameManager.instance.playErrorSound();
            animator.playCantPurchaseAnimation();
        }

    }

    public void breakObject()
    {
        // play the break animation

        animator.playBreakAnimation();

    }
    
    public void destroyRandomBox()
    {
        // destroy the object
        Destroy(gameObject);
    }


    public void repairObject()
    {

    }
    public void setPrice(int newPrice)
    {
        itemBoxPrice = newPrice;
        textDialogue.TextMeshPro.text = itemBoxPrice.ToString();
    }
    // public List<GameObject> getItems()
    // {
    //     // return randomSpawner.getItems();
    // }
}
