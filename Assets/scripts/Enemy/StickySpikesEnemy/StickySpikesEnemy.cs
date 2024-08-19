using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class StickySpikesEnemy : MonoBehaviour,ILootable,IDamageable,IEnemyBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject core;
    [SerializeField] GameObject box;
    [SerializeField] GameObject soundSystem;
    [SerializeField] int loot;
    int hp;
    [SerializeField] int maxHp;
    [SerializeField] int damage;
    void Start()
    {
        core= transform.GetChild(0).gameObject;
        box = transform.GetChild(1).gameObject;
        loot = core.GetComponent<Coin>().Value;
    }

    // Update is called once per frame
    

    public void throwLoot()
    {
        soundSystem.GetComponent<StickySpikesEnemySoundSystem>().playDieSfx();
        core.GetComponent<Coin>().Anim.SetTrigger("onBreakBox"); 
        

    }

    public void takeDamage(int d)
    {
        // his bodySpikes handles the life points so we just make a sound
        soundSystem.GetComponent<StickySpikesEnemySoundSystem>().playDamageSfx();
    }
    public void die()
    { 
         // when the coin is collected and the box is broken
    }

    public void dealDamage(GameObject o)
    {
        // the body is responsible for rest the life points
    }

    public GameObject Core { get => core; set => core = value; }
    public GameObject Box { get => box; set => box = value; }
    public int LootCoins { get => loot; set => loot=value; }
    public int Hp { get => hp; set => hp=value; }
    public int MaxHp { get => maxHp; set => maxHp=value; }
    public int Damage { get =>  damage; set =>damage=value ; }
}
