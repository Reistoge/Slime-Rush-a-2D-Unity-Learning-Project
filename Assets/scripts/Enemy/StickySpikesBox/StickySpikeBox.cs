using System;
using System.Collections;
using System.Data.Common;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class StickySpikeBox : MonoBehaviour, IDamageable, IEnemyBehaviour, ILootable
{
    [SerializeField]
    int hp;

    [SerializeField]
    int maxHp;

    [SerializeField]
    int damage;
    int closestSpikeIndex;

    [SerializeField]
    GameObject spikes;

    [SerializeField]
    GameObject animatorHandler;
    int lootCoins; // ----> unused

    void Start()
    {
        if (coreExist())
        {
            // we want to have the same damage and hp than the parent
            // because its the same body.
            transform.parent.GetComponent<IDamageable>().MaxHp = this.maxHp;
            transform.GetComponent<IEnemyBehaviour>().Damage = this.damage;
        }
        animatorHandler = transform.GetChild(0).gameObject;
        hp = maxHp;
    }

    public void takeDamage(int d)
    {
        hp -= d;
        // if the core exist
        if (coreExist())
        {
            // take the damage from the parent enemy (in this case he holds the life points)
            transform.parent.gameObject.GetComponent<IDamageable>().takeDamage(d);
        }

        if (hp <= 0)
        {
            die();
        }
        else
        {
            animatorHandler.GetComponent<SpikesBoxAnimHandler>().playDamageAnim();
            foreach (Transform spike in spikes.transform)
            {
                spike.gameObject.SetActive(false);
            }
        }
    }

    public void die()
    {
        // dont necessary has to destroy it (object pool)
        throwLoot();
        animatorHandler.GetComponent<SpikesBoxAnimHandler>().playDieAnimation();
    }
 

    void OnCollisionExit2D(Collision2D col) { }

    public void boxTouched(GameObject g)
    {
        GetComponent<Collider2D>().isTrigger = false;
        float distance = Mathf.Infinity;
        closestSpikeIndex = -1;

        for (int i = 0; i < spikes.transform.childCount; i++)
        {
            // we found the closest spike position

            if (
                distance > Vector2.Distance(spikes.transform.GetChild(i).position, g.transform.position)
            )
            {
                distance = Vector2.Distance(
                    spikes.transform.GetChild(i).position,
                    g.transform.position
                );
                print(distance);
                closestSpikeIndex = i;
            }
        }
        if (closestSpikeIndex >= 0)
        {
            activateSpike(0.5f);
        }
    }

    private void activateSpike()
    {
        if (spikes.transform.GetChild(closestSpikeIndex).gameObject.activeInHierarchy == false)
        {
            spikes.transform.GetChild(closestSpikeIndex).gameObject.SetActive(true);
        }
    }

    private void activateSpike(float time)
    {
        Invoke("activateSpike", time);
    }

    // used in the animation handler
    public void desactivateObject()
    {
        gameObject.SetActive(false);
    }

    // used in animation handler
    public void desactivateCollider()
    {
        GetComponent<Collider2D>().enabled = false;
    }

    public void dealDamage(GameObject g)
    {
        // if the object implements the IDamageable interface.

        if (g.GetComponent<IDamageable>() != null)
        {
            // the box holds the damage

            g.GetComponent<IDamageable>().takeDamage(damage);

            print(g.name + " receive : " + damage);
        }
        // if doesnt implements it it means that doesnt have health points.
        else
        {
            print(g.name + " cant take damage !");
        }
    }

    public void throwLoot()
    {
        // now this object has a core with the item.
        // extra loots ???.
        //// multiple coins


        if (coreExist())
        {
            // we throw the loot of the entire enemyParent (activating the core).
            transform.parent.GetComponent<ILootable>().throwLoot();
        }
        print("Throwing loot");
    }

    bool coreExist()
    {
        return transform.parent != null && transform.parent.GetComponent<ILootable>() != null;
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
    public int Damage
    {
        get => damage;
        set => damage = value;
    }
    public int LootCoins
    {
        get => lootCoins;
        set => lootCoins = value;
    }
    public GameObject Spikes
    {
        get => spikes;
        set => spikes = value;
    }
}
