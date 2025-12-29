using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Strategy for instantiating basic solid platforms in danger zone boundaries.
/// This strategy creates a vertical sequence of platforms with randomized positions.
/// </summary>
[Serializable]
public class DefaultPlatformLevelSpawner : ILevelSpawner
{
    #region Properties

    /// <summary>Gets or sets the difficulty level (currently unused)</summary>
    public int Level { get; set; }

    #endregion

    #region Public Methods

    /// <summary>
    /// Instantiates a sequence of platforms within the specified boundary.
    /// Creates 6 platforms with randomized horizontal positions and vertical spacing.
    /// </summary>
    /// <param name="bound">The boundary GameObject to populate with platforms</param>
    public void instantiateEntities(GameObject bound)
    {
        // Create container for all platforms
        GameObject platforms = new GameObject("Platforms");
        platforms.transform.SetParent(bound.transform);

        Vector2 pos = bound.transform.position;
        var config = DangerZoneLevelManager.instance.Config;

        // Calculate horizontal boundaries for each platform type
        float minHorizontalValueClassic = -(config.HORIZONTAL_EDGE_LIMIT - config.platformClassic.width);
        float maxHorizontalValueClassic = config.HORIZONTAL_EDGE_LIMIT - config.platformClassic.width;

        float minHorizontalValueLarge = -(config.HORIZONTAL_EDGE_LIMIT - config.platformLarge.width);
        float maxHorizontalValueLarge = config.HORIZONTAL_EDGE_LIMIT - config.platformLarge.width;

        // Platform positioning parameters
        const float minRandomHorizontal = 50f;
        const float maxRandomHorizontal = 100f;
        const float minVerticalValue = 90f;
        const float maxVerticalValue = 100f;
        const float verticalOffset = 200f;

        // Generate 6 platform positions with progressive vertical spacing
        float randomX = Random.Range(minHorizontalValueClassic, maxHorizontalValueClassic);
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

        // Last position is centered to ensure smooth transition to next boundary
        Vector2 randomPos6 = new Vector2(
            Random.Range(0, minRandomHorizontal) * (Random.Range(0, 2) == 0 ? -1 : 1),
            randomPos5.y + Random.Range(minVerticalValue, maxVerticalValue) + verticalOffset / 4);

        List<Vector2> vectors = new List<Vector2> { randomPos1, randomPos2, randomPos3, randomPos4, randomPos5, randomPos6 };
        System.Random rand = new System.Random();
        List<Vector2> shuffled = vectors.OrderBy(_ => rand.Next()).ToList();

        // Calculate amounts properly
        int maxPlatforms = Mathf.Min(DangerZoneLevelManager.instance.Config.maxPlatformsInBound, shuffled.Count);
        int classicAmount = Random.Range(1, maxPlatforms);
        int largeAmount = maxPlatforms - classicAmount;

        List<GameObject> classicList = new List<GameObject>();
        List<GameObject> largeList = new List<GameObject>();

        // Create classic platforms
        for (int i = 0; i < classicAmount; i++)
        {
            Vector2 clampedPos = new Vector2(
                Mathf.Clamp(shuffled[i].x, minHorizontalValueClassic, maxHorizontalValueClassic),
                shuffled[i].y);

            GameObject p = GameObject.Instantiate(
                DangerZoneLevelManager.instance.Config.platformClassic.prefab,
                clampedPos,
                Quaternion.identity);

            // Add coins randomly
            if (Random.Range(0, 2) == 1)
            {
                GameObject coin = GameObject.Instantiate(
                    DangerZoneLevelManager.instance.Config.coinsPrefabA.prefab,
                    new Vector2(p.transform.position.x, p.transform.position.y + 30),
                    Quaternion.identity);
                coin.transform.SetParent(platforms.transform);
            }

            classicList.Add(p);
        }

        // Create large platforms
        for (int i = 0; i < largeAmount; i++)
        {
            int posIndex = classicAmount + i;
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
        classicList.ForEach(p => p.transform.SetParent(platforms.transform));
    }
}
