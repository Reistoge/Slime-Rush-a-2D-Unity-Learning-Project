using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRandom : MonoBehaviour
{
    // Start is called before the first frame update
    
    [SerializeField] private GameObject[] prefabsToSpawn;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private float spawnDelay = 0f;
    [SerializeField] private int maxSpawnCount = 5;
    private int currentSpawnCount = 0;
    private void Start()
    {
       
    }
    public void StartSpawning()
    {
        currentSpawnCount = 0; // Reset the spawn count
        StartCoroutine(SpawnRandomObjects());
    }
    private IEnumerator SpawnRandomObjects()
    {
        yield return new WaitForSeconds(spawnDelay);

        while (currentSpawnCount < maxSpawnCount)
        {
            int randomPrefabIndex = Random.Range(0, prefabsToSpawn.Length);
            int randomSpawnPointIndex = Random.Range(0, spawnPoints.Length);

            GameObject prefabToSpawn = prefabsToSpawn[randomPrefabIndex];
            Transform spawnPoint = spawnPoints[randomSpawnPointIndex];

            Instantiate(prefabToSpawn, spawnPoint.position, Quaternion.identity);
            currentSpawnCount++;

            yield return new WaitForSeconds(spawnInterval);
        }
    }
 
}
