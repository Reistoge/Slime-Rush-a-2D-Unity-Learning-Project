using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class randomBarrelGenerator2 : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject barrel_prefab;
    GameObject spawn_barrel;
    [SerializeField]
    private int amount;
    [SerializeField]
    int min_force,max_force=50;

    [SerializeField]
    Transform spawn_point;

    [SerializeField]
    private int distance;

    // Update is called once per frame
    private void Start()
    {


        barrel_spawner();
        
    }
    void barrel_spawner()
    {


        for (int i = 0; i < amount; i++)
        {
            spawn_point.Translate(Vector3.up * 10);
            spawn_point.position = new Vector3(Random.Range(0, 3), spawn_point.position.y, spawn_point.position.z);
            Debug.Log(spawn_point.position.y);
            spawn_barrel = Instantiate(barrel_prefab, spawn_point, true);



            //spawn_barrel.GetComponent<BarrelScript>().ForceBarrel = Random.Range(min_force, max_force);





        }

    }

}
