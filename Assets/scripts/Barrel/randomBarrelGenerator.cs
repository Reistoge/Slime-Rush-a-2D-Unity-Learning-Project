using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class randomBarrelGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject barrel_prefab;
    GameObject spawn_barrel;
    [SerializeField]
    private int amount;
    [SerializeField]
    int min_force,max_force=50;
  
    
    // Update is called once per frame
    private void Start()
    {
        
        Vector3 vector3= Vector3.zero;
        for (int i = 0; i < amount; i++)
        {
            spawn_barrel=Instantiate(barrel_prefab,vector3,Quaternion.identity);
            vector3.y += 10;
            spawn_barrel.transform.position = vector3;
            
            // spawn_barrel.GetComponent<BarrelScript>().ForceBarrel = Random.Range(min_force, max_force);
            
            
            
            
            
        }
    }
  
}
