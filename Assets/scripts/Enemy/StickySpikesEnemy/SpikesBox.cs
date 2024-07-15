using System;
using System.Collections;
using UnityEngine;
public class SpikesBox : MonoBehaviour, IDamageable, IEnemy,ILootable
{
 
    [SerializeField] int hp;
    [SerializeField] int maxHp;
    [SerializeField] int damage;
    int lootCoins;// ----> unused
      
    

    private void Start()
    {
        hp = maxHp;
        
    }

    public void takeDamage(int d)
    {
        hp -= d;
        if(hp <= 0) {
        
            die();
        }

    }

    public void die()
    {
        // dont necessary has to destroy it (object pool)
        throwLoot();
        Destroy(gameObject);
    }
    
    public void dealDamage(GameObject g)
    {
        // if the object implements the IDamageable interface.
        if (g.GetComponent<IDamageable>()!=null)
        {
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
        //GameObject coin = GameManager.instance.CoinReference;
        //coin.GetComponent<Coin>().Value = lootCoins;
        //// coins position are not right
        //Instantiate(coin,null);
        
        if(coreExist())
        {
            // we throw the loot of the entire enemyParent (activating the core).
            transform.parent.GetComponent<ILootable>().throwLoot();


        }
        print("Thrwing loot");
        
        
    }
    bool coreExist()
    {
        return transform.parent != null && transform.parent.GetComponent<ILootable>() != null;
    }
    
    
    public int Hp { get => hp; set => hp = value; }
    public int MaxHp { get => maxHp; set => maxHp = value; }
    public int Damage { get => damage; set => damage=value; }
    public int LootCoins { get => lootCoins; set => lootCoins = value; }
}
