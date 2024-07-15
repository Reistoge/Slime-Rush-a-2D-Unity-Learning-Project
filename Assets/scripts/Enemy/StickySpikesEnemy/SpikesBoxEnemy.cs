using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpikesBoxEnemy : MonoBehaviour,ILootable
{
    // Start is called before the first frame update
    GameObject core;
    GameObject box;
    int loot;


    void Start()
    {
        Core= transform.GetChild(0).gameObject;
        Box = transform.GetChild(1).gameObject;
        loot = core.GetComponent<Coin>().Value;
    }

    // Update is called once per frame
    

    public void throwLoot()
    {
        core.GetComponent<Coin>().Anim.SetTrigger("onBreakBox"); 
        

    }
    

    public GameObject Core { get => core; set => core = value; }
    public GameObject Box { get => box; set => box = value; }
    public int LootCoins { get => loot; set => loot=value; }
}
