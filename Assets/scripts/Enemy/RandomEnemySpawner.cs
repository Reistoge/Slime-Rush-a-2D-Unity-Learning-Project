using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEnemySpawner : MonoBehaviour

{
    [SerializeField]
    GameObject[] enemies;
    GameObject enemySpawn;
    private int amount;
    private float separation;

    [SerializeField]
    Transform spawnPoint;
    // Start is called before the first frame update
    
    private void Start()
    {
        
        float positionY = spawnPoint.position.y;

        enemy_spawner(positionY);
    }
    // Update is called once per frame
   
    void enemy_spawner(float positionY)
    {
        separation = GameManager.instance.EnemySeparation;
        amount = GameManager.instance.EnemyAmount;
        

        for (int i = 0; i < amount; i++)
        {

            int randomIndex=Random.Range(0, enemies.Length);
            positionY += separation;
            enemySpawn = Instantiate(enemies[randomIndex], new Vector3(Random.Range(-2f, 2f), positionY, enemies[randomIndex].transform.position.z), Quaternion.identity);

            enemySpawn.gameObject.transform.SetParent(transform, false);



        }

    }
}
