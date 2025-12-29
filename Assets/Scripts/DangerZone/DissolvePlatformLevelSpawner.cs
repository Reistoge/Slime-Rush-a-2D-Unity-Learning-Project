using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Strategy for instantiating dissolve platforms in danger zone boundaries.
/// Creates platforms that can appear and disappear, adding dynamic difficulty.
/// Includes coin spawning between platforms for rewards.
/// </summary>
[Serializable]
public class DissolvePlatformLevelSpawner : ILevelSpawner
{
    #region Properties

    /// <summary>Gets or sets the difficulty level (currently unused)</summary>
    public int Level { get; set; }

    #endregion

    #region Public Methods

    /// <summary>
    /// Instantiates a sequence of dissolve platforms and coins within the specified boundary.
    /// Creates both solid and dissolving platforms with randomized positions.
    /// </summary>
    /// <param name="bound">The boundary GameObject to populate with platforms</param>
    public void instantiateEntities(GameObject bound)
    {
        GameObject platforms = new GameObject("Platforms");
        GameObject coins = new GameObject("coins");
        platforms.transform.SetParent(bound.transform);
        coins.transform.SetParent(bound.transform);
        // 2 large platforms, 3 normal size platforms
        Vector2 pos = bound.transform.position;

        float minHorizontalValueClassic = -(DangerZoneLevelManager.instance.Config.HORIZONTAL_EDGE_LIMIT - DangerZoneLevelManager.instance.Config.platformClassic.width);
        float maxHorizontalValueClassic = DangerZoneLevelManager.instance.Config.HORIZONTAL_EDGE_LIMIT - DangerZoneLevelManager.instance.Config.platformClassic.width;

        float minHorizontalValueLarge = -(DangerZoneLevelManager.instance.Config.HORIZONTAL_EDGE_LIMIT - DangerZoneLevelManager.instance.Config.platformLarge.width);
        float maxHorizontalValueLarge = DangerZoneLevelManager.instance.Config.HORIZONTAL_EDGE_LIMIT - DangerZoneLevelManager.instance.Config.platformLarge.width;

        float minRandomHorizontal = 50f;
        float maxRandomHorizontal = 100f;

        float minVerticalValue = 90;
        float maxVerticalValue = 100f;

        float verticalOffset = 200f;

        float randomX = Random.Range(minHorizontalValueClassic, maxHorizontalValueClassic);

        Vector2 randomPos1 = new Vector2(randomX, pos.y - verticalOffset);



        Vector2 randomPos2 = new Vector2(Random.Range(0, randomPos1.x + Random.Range(minRandomHorizontal, maxRandomHorizontal)) * (Random.Range(0, 2) == 0 ? -1 : 1), randomPos1.y + Random.Range(minVerticalValue, maxVerticalValue));

        Vector2 randomPos3 = new Vector2(Random.Range(0, randomPos2.x + Random.Range(minRandomHorizontal, maxRandomHorizontal)) * (Random.Range(0, 2) == 0 ? -1 : 1), randomPos2.y + Random.Range(minVerticalValue, maxVerticalValue));

        Vector2 randomPos4 = new Vector2(Random.Range(0, randomPos3.x + Random.Range(minRandomHorizontal, maxRandomHorizontal)) * (Random.Range(0, 2) == 0 ? -1 : 1), randomPos3.y + Random.Range(minVerticalValue, maxVerticalValue));

        Vector2 randomPos5 = new Vector2(Random.Range(0, randomPos4.x + Random.Range(minRandomHorizontal, maxRandomHorizontal)) * (Random.Range(0, 2) == 0 ? -1 : 1), randomPos4.y + Random.Range(minVerticalValue, maxVerticalValue));

        Vector2 randomPos6 = new Vector2(Random.Range(0, minRandomHorizontal) * (Random.Range(0, 2) == 0 ? -1 : 1), randomPos5.y + Random.Range(minVerticalValue, maxVerticalValue) + verticalOffset/5); // the last pos is fixed to make sure the player can pass through or connect to the next boundarie correctly.


        List<Vector2> vectors = new List<Vector2> { randomPos1, randomPos2, randomPos3, randomPos4, randomPos5, randomPos6 };

        System.Random rand = new System.Random();

        List<Vector2> shuffled = vectors.OrderBy(_ => rand.Next()).ToList();


        int classicAmount = Random.Range(1, DangerZoneLevelManager.instance.Config.maxPlatformsInBound + 1);
        int largeAmount = Random.Range(1, DangerZoneLevelManager.instance.Config.maxPlatformsInBound + 1 - classicAmount);
        int rIndex = Random.Range(0, classicAmount);
        List<GameObject> classicList = new List<GameObject>();
        for (int i = 0; i < classicAmount; i++)
        {
            GameObject p = GameObject.Instantiate(DangerZoneLevelManager.instance.Config.platformClassic.prefab,
                                 new Vector2(Mathf.Clamp(shuffled[i].x, minHorizontalValueClassic, maxHorizontalValueClassic), shuffled[i].y),
                                 quaternion.identity);

            // put coins on platform
            if ((Random.Range(0, 2) == 0 ? -1 : 1) == 1)
            {
                GameObject coin = GameObject.Instantiate(DangerZoneLevelManager.instance.Config.coinsPrefabA.prefab, new Vector2(p.transform.position.x, p.transform.position.y + 30), quaternion.identity);
                coin.transform.SetParent(coins.transform);
            }
            if (i == rIndex)
            {
                float minDistance = 60f;
                float dissolveX;
                int attempts = 0;
                do
                {
                    dissolveX = Random.Range(minHorizontalValueClassic, maxHorizontalValueClassic);
                    attempts++;
                } while (Mathf.Abs(dissolveX - p.transform.position.x) < minDistance && attempts < 10);

                GameObject dissolve = GameObject.Instantiate(
                    DangerZoneLevelManager.instance.Config.platformDissolve.prefab,
                    new Vector2(dissolveX, Random.Range(p.transform.position.y - 20, p.transform.position.y + 20)),
                    quaternion.identity
                );
                dissolve.transform.SetParent(platforms.transform);
                GameObject coinBonus = GameObject.Instantiate(DangerZoneLevelManager.instance.Config.coinsPrefabB.prefab, new Vector2(dissolve.transform.position.x, dissolve.transform.position.y + 30), quaternion.identity); // it has to be a different object

            }


            classicList.Add(p);
        }

        List<GameObject> largeList = new List<GameObject>();
        for (int i = classicAmount; i < DangerZoneLevelManager.instance.Config.maxPlatformsInBound; i++)
        {
            GameObject p = GameObject.Instantiate(DangerZoneLevelManager.instance.Config.platformLarge.prefab, new Vector2(Mathf.Clamp(shuffled[i].x, minHorizontalValueLarge, maxHorizontalValueLarge), shuffled[i].y), quaternion.identity);

            // put coins on platform
            if ((Random.Range(0, 2) == 0 ? -1 : 1) == 1)
            {
                GameObject coin1 = GameObject.Instantiate(DangerZoneLevelManager.instance.Config.coinsPrefabA.prefab, new Vector2(p.transform.position.x + DangerZoneLevelManager.instance.Config.platformClassic.width * .9f, p.transform.position.y + 30), quaternion.identity);
                GameObject coin2 = GameObject.Instantiate(DangerZoneLevelManager.instance.Config.coinsPrefabA.prefab, new Vector2(p.transform.position.x, p.transform.position.y + 30), quaternion.identity);
                GameObject coin3 = GameObject.Instantiate(DangerZoneLevelManager.instance.Config.coinsPrefabA.prefab, new Vector2(p.transform.position.x - DangerZoneLevelManager.instance.Config.platformClassic.width * .9f, p.transform.position.y + 30), quaternion.identity);
                coin1.transform.SetParent(coins.transform);                
                coin2.transform.SetParent(coins.transform);                
                coin3.transform.SetParent(coins.transform);                
            }
            classicList.Add(p);

        }

        largeList.ForEach(p => p.transform.SetParent(platforms.transform));
        classicList.ForEach(p => p.transform.SetParent(platforms.transform));
    }
}
