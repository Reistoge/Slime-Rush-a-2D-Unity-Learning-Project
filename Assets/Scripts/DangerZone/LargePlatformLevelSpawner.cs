using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class LargePlatformLevelSpawner : ILevelSpawner
{
    private int level;
    public int Level { get => level; set => level = value; }



    public void instantiateEntities(GameObject bound)
    {
        GameObject platforms = new GameObject("Platforms");
        platforms.transform.SetParent(bound.transform);

        Vector2 pos = bound.transform.position;

        float minRandomHorizontal = 50f;
        float maxRandomHorizontal = 100f;

        float minVerticalValue = 90;
        float maxVerticalValue = 100f;

        float verticalOffset = 200f;


        float minHorizontalValueLarge = -(DangerZoneLevelManager.instance.Config.HORIZONTAL_EDGE_LIMIT - DangerZoneLevelManager.instance.Config.platformLarge.width);
        float maxHorizontalValueLarge = DangerZoneLevelManager.instance.Config.HORIZONTAL_EDGE_LIMIT - DangerZoneLevelManager.instance.Config.platformLarge.width;


        // Generate positions
        float randomX = Random.Range(minHorizontalValueLarge, maxHorizontalValueLarge);
        Vector2 randomPos1 = new Vector2(randomX, pos.y - verticalOffset);

        Vector2 randomPos2 = new Vector2(
            Random.Range(0, randomPos1.x + Random.Range(minRandomHorizontal, maxRandomHorizontal)) * (Random.Range(0, 2) == 0 ? -1 : 1),
            randomPos1.y + Random.Range(minVerticalValue, maxVerticalValue));

        Vector2 randomPos3 = new Vector2(
            Random.Range(0, randomPos2.x + Random.Range(minRandomHorizontal, maxRandomHorizontal)) * (Random.Range(0, 2) == 0 ? -1 : 1),
            randomPos2.y + Random.Range(minVerticalValue, maxVerticalValue));

        Vector2 randomPos4 = new Vector2(
            Random.Range(0, randomPos3.x + Random.Range(minRandomHorizontal, maxRandomHorizontal)) * (Random.Range(0, 2) == 0 ? -1 : 1),
            randomPos3.y + Random.Range(minVerticalValue, maxVerticalValue));

        Vector2 randomPos5 = new Vector2(
            Random.Range(0, randomPos4.x + Random.Range(minRandomHorizontal, maxRandomHorizontal)) * (Random.Range(0, 2) == 0 ? -1 : 1),
            randomPos4.y + Random.Range(minVerticalValue, maxVerticalValue));


        Vector2 randomPos6 = new Vector2(Random.Range(0, minRandomHorizontal) * (Random.Range(0, 2) == 0 ? -1 : 1), randomPos5.y + Random.Range(minVerticalValue, maxVerticalValue) ); // the last pos is fixed to make sure the player can pass through or connect to the next boundarie correctly.

        List<Vector2> vectors = new List<Vector2> { randomPos1, randomPos2, randomPos3, randomPos4, randomPos5, randomPos6 };
        System.Random rand = new System.Random();
        List<Vector2> shuffled = vectors.OrderBy(_ => rand.Next()).ToList();

        // Calculate amounts properly
        int maxPlatforms = Mathf.Min(DangerZoneLevelManager.instance.Config.maxPlatformsInBound, shuffled.Count);

        int largeAmount = maxPlatforms;

        List<GameObject> largeList = new List<GameObject>();


        // Create large platforms
        for (int i = 0; i < largeAmount; i++)
        {
            int posIndex = i;
            Vector2 clampedPos = new Vector2(
                Mathf.Clamp(shuffled[posIndex].x, minHorizontalValueLarge, maxHorizontalValueLarge),
                shuffled[posIndex].y);

            GameObject p = GameObject.Instantiate(
                DangerZoneLevelManager.instance.Config.platformLarge.prefab,
                clampedPos,
                Quaternion.identity);

            // Add multiple coins on large platforms
            if (Random.Range(0, 2) == 1)
            {
                float coinSpacing = DangerZoneLevelManager.instance.Config.platformClassic.width * 0.9f;
                Vector2 basePos = new Vector2(p.transform.position.x, p.transform.position.y + 30);

                GameObject coin1 = GameObject.Instantiate(DangerZoneLevelManager.instance.Config.coinsPrefabA.prefab, basePos + Vector2.right * coinSpacing, Quaternion.identity);
                GameObject coin2 = GameObject.Instantiate(DangerZoneLevelManager.instance.Config.coinsPrefabA.prefab, basePos, Quaternion.identity);
                GameObject coin3 = GameObject.Instantiate(DangerZoneLevelManager.instance.Config.coinsPrefabA.prefab, basePos + Vector2.left * coinSpacing, Quaternion.identity);

                coin1.transform.SetParent(platforms.transform);
                coin2.transform.SetParent(platforms.transform);
                coin3.transform.SetParent(platforms.transform);
            }

            largeList.Add(p); // Fixed: Add to correct list
        }

        // Set parents
        largeList.ForEach(p => p.transform.SetParent(platforms.transform));
    }
}

