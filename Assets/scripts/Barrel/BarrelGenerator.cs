using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BarrelGenerator : MonoBehaviour
{
    [SerializeField]
    GameObject[] BarrelPrefabs;
    [SerializeField]
    GameObject FinalBarrel;
    GameObject spawn_barrel;
    [SerializeField]
    private int amount;
    [SerializeField]
    float min_force, max_force;





    [SerializeField]
    Transform _BarrelSpawnPoint;
    //positiony is the position of the generator
    [SerializeField]
    float positionY;
    [SerializeField]
    float separation;


    // Update is called once per frame
    private void Start()
    {
        positionY = _BarrelSpawnPoint.position.y;
        amount = GameManager.instance.Waves*10;

        barrel_spawner(positionY);
    }
     
    void barrel_spawner(float positionY)
    {


        //element 0= default
        // element 1= horizontal
        // element 2= diagonal 
        // element 3= rotateAround
        //if (GameManager.instance.Waves==1)
        //{
            //float finalBarrelpos=GameManager.instance.EnemyAmount* GameManager.instance.EnemySeparation;
            ////spawn the last barrel
            //spawn_barrel = Instantiate(FinalBarrel, new Vector3
            //(0, finalBarrelpos, FinalBarrel.transform.position.z), Quaternion.identity);
            //spawn_barrel.GetComponent<BarrelScript>().Force_barrel =  max_force;
            //spawn_barrel.gameObject.transform.SetParent(transform, false);
             
        //}
        //else
        //{
            for (int i = 0; i <= amount; i++)
            {
                int random_index = Random.Range(1, BarrelPrefabs.Length);

                if (i % 2 == 0 && i != 0)
                {
                    if (random_index == 1)
                    {
                        spawn_barrel = Instantiate(BarrelPrefabs[random_index], new Vector3
                            (Random.Range(-2.45f, 2.45f), positionY, BarrelPrefabs[random_index].transform.position.z), Quaternion.identity);

                        //spawn_barrel.GetComponent<BarrelScript>().ForceBarrel = Random.Range(min_force, max_force);
                        //spawn_barrel.GetComponent<BarrelScript>().CoolDown = 1.5f;
                        spawn_barrel.gameObject.transform.SetParent(transform, false);
                        positionY += separation;
                    }
                    if (random_index == 2)
                    {
                        // i is equal to 2 so then i want to spawn a diagonal barrel, but also i want the direction to be random, so first i random the direction.
                        // i cant random the direction in the barrel diagonal becaause first i have to instatiate the object 
                        int _randomDirection = Random.Range(-1, 1);
                        while (_randomDirection == 0)
                        {
                            _randomDirection = Random.Range(-1, 1);
                        }
                        // cases of the direction

                        if (_randomDirection < 0)
                        {
                            spawn_barrel = Instantiate(BarrelPrefabs[random_index], new Vector3
                                (Random.Range(-1.74f, 0), positionY, BarrelPrefabs[random_index].transform.position.z), Quaternion.identity);
                            spawn_barrel.GetComponent<BarrelDiagonal>().Direction = _randomDirection;
                        }
                        if (_randomDirection > 0)
                        {
                            spawn_barrel = Instantiate(BarrelPrefabs[random_index], new Vector3
                                (Random.Range(0, 1.74f), positionY, BarrelPrefabs[random_index].transform.position.z), Quaternion.identity);
                            spawn_barrel.GetComponent<BarrelDiagonal>().Direction = _randomDirection;

                        }


                        //spawn_barrel.GetComponent<BarrelScript>().ForceBarrel = Random.Range(min_force, max_force);
                        //spawn_barrel.GetComponent<BarrelScript>().CoolDown = 1.5f;
                         spawn_barrel.gameObject.transform.SetParent(transform, false);
                        positionY += separation;



                    }

                    if (random_index == 3)
                    {
                        float RandomSpeed = Random.Range(-2.5f, 2.5f);
                        while (RandomSpeed > -1 && RandomSpeed < 1)
                        {
                            RandomSpeed = Random.Range(-2f, 2f);
                        }
                        spawn_barrel = Instantiate(BarrelPrefabs[random_index], new Vector3
                            (Random.Range(-2.20f, 2.20f), positionY, BarrelPrefabs[random_index].transform.position.z), Quaternion.identity);
                        spawn_barrel.GetComponent<RotateAroundBarrel>().random_speed = RandomSpeed;

                        //spawn_barrel.GetComponent<BarrelScript>().ForceBarrel = Random.Range(min_force, max_force);
                        // spawn_barrel.GetComponent<BarrelScript>().CoolDown = 1.5f;
                        spawn_barrel.gameObject.transform.SetParent(transform, false);
                        positionY += separation;

                    }
                }

                else
                {
                    spawn_barrel = Instantiate(BarrelPrefabs[0], new Vector3
                    (Random.Range(-2.5f, 2.5f), positionY, BarrelPrefabs[0].transform.position.z), Quaternion.identity);
                    
                    //spawn_barrel.GetComponent<BarrelScript>().ForceBarrel = Random.Range(min_force  , max_force  );
                    // spawn_barrel.GetComponent<BarrelScript>().CoolDown = 1.5f;
                    spawn_barrel.gameObject.transform.SetParent(transform, false);
                    positionY += separation;
                }
                if (i == amount)
                {
                    //spawn the last barrel
                    spawn_barrel = Instantiate(FinalBarrel, new Vector3
                    (0, positionY, FinalBarrel.transform.position.z), Quaternion.identity);
                    //spawn_barrel.GetComponent<BarrelScript>().ForceBarrel = Random.Range(min_force   , max_force  );
                    //spawn_barrel.GetComponent<BarrelScript>().CoolDown = 1.5f;
                    spawn_barrel.gameObject.transform.SetParent(transform, false);
                    positionY += separation;

                }

            //}
        






        }

    }

}
