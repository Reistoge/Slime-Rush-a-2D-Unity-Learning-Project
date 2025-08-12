using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Video;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class DangerZoneLevelManager : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Danger Zone child objects")]

    [SerializeField] GameObject dangerZoneBoundaries;
    [SerializeField] GameObject dangerZonePrefab;
    [SerializeField] GameObject lava;
    [SerializeField] GameObject dangerZoneCamera;
    [SerializeField] GameObject floorHeight;
    [SerializeField] GameObject topHeight;


    [SerializeField] DangerZoneConfig config;
    [SerializeField] GameObject platformPrefab;

    [SerializeField] GameObject coinsPrefab;
    [SerializeField] GameObject cannonsPrefab;
    [SerializeField] float waitTimeToStartLevel = 2f;
    [SerializeField] FollowCamera camera;

    [SerializeField] int levelCount = 1;

    List<GameObject> boundaries;



    IEnumerator dangerZoneCoroutine()
    {
        // generateDangerZoneLevel(DangerZoneType.newPlatforms);
        yield return new WaitForSeconds(waitTimeToStartLevel);
        startLevel();
        // startLevel();
        // add offset;

    }

    public void startDangerZone()
    {
        StartCoroutine(dangerZoneCoroutine());
    }
    void Start()
    {
        generateDangerZoneLevel(DangerZoneType.newPlatforms);
    }

    public void startLevel()
    {
        GameObject mainCamera = GameObject.Find("Main Camera");
        if (mainCamera != null)
        {

            dangerZoneCamera.transform.position = mainCamera.transform.position;
            Destroy(mainCamera.gameObject);
        }
        dangerZoneCamera.SetActive(true);

    }

    public void generateDangerZoneLevel(DangerZoneType type)
    {

        List<GameObject> boundaries = new List<GameObject>();
        //GameObject dangerZone = Instantiate(dangerZonePrefab, this.transform);

        switch (type)
        {
            case DangerZoneType.platforms:

                GameObject platforms = new GameObject("Platforms");
                GameObject coins = new GameObject("Coins");

                platforms.transform.SetParent(this.transform);
                coins.transform.SetParent(this.transform);
                platforms.transform.position = floorHeight.transform.position;
                coins.transform.position = floorHeight.transform.position;
                float x = Random.Range(-160f, 160f);

                float xNext = 0;
                float height = floorHeight.transform.position.y + 100f;
                Vector2 nextPos = new Vector2(xNext, height);
                for (int i = 0; height <= topHeight.transform.position.y; i++)
                {


                    GameObject currentPlatform = Instantiate(platformPrefab, new Vector2(xNext, height), Quaternion.identity);

                    xNext = Random.Range(-80, 80);
                    height += Random.Range(70f, 130f);
                    currentPlatform.transform.SetParent(platforms.transform);
                    x = xNext;
                    Vector2 coinPos = new Vector2(currentPlatform.transform.position.x + (xNext - currentPlatform.transform.position.x) / 2, currentPlatform.transform.position.y + ((height - currentPlatform.transform.position.y) / 2));
                    GameObject coin = Instantiate(coinsPrefab, coinPos, Quaternion.identity);
                    coin.transform.SetParent(coins.transform);

                }
                break;

            case DangerZoneType.cannons:
                break;
            case DangerZoneType.newPlatforms:



                xNext = 0;
                height = floorHeight.transform.position.y + 320f;
                nextPos = new Vector2(xNext, height - 50f);


                GameObject bound1 = Instantiate(config.boundarie.prefab, new Vector2(xNext, height), Quaternion.identity);
                bound1.transform.SetParent(dangerZoneBoundaries.transform);
                instantiatePlatforms(bound1);
                //instantiateFloors(bound1);

                GameObject bound2 = Instantiate(config.boundarie.prefab, new Vector2(xNext, height + 640), Quaternion.identity);
                bound2.transform.SetParent(dangerZoneBoundaries.transform);
                instantiatePlatforms(bound2);

                GameObject bound3 = Instantiate(config.boundarie.prefab, new Vector2(xNext, height + 640 + 640), Quaternion.identity);
                bound3.transform.SetParent(dangerZoneBoundaries.transform);
                instantiatePlatforms(bound3);



                var boundariesComponent = bound2.GetComponent<Boundaries>();
                boundariesComponent.OnPassThroughMiddle.RemoveAllListeners();
                boundariesComponent.OnExitThroughMiddle.RemoveAllListeners();

                UnityEngine.Events.UnityAction passThroughAction = null;
                UnityEngine.Events.UnityAction removeInstantiateBoundariesListener = null;



                passThroughAction = () =>
                {
                    // Remove listeners before instantiating to avoid multiple calls
                    boundariesComponent.OnPassThroughMiddle.RemoveListener(passThroughAction);
                    boundariesComponent.OnExitThroughMiddle.RemoveListener(removeInstantiateBoundariesListener);
                    InstantiateBoundaries(bound3.transform.position.y + 320);
                };
                removeInstantiateBoundariesListener = () =>
                {
                    boundariesComponent.OnPassThroughMiddle.RemoveListener(passThroughAction);
                    boundariesComponent.OnExitThroughMiddle.RemoveListener(removeInstantiateBoundariesListener);
                };

                boundariesComponent.OnPassThroughMiddle.AddListener(passThroughAction);
                boundariesComponent.OnExitThroughMiddle.AddListener(removeInstantiateBoundariesListener);



                break;
        }
    }
    void InstantiateBoundaries(float boundHeight)
    {
        levelCount++;
        camera.Rise.increaseSpeed(1.2f);
        float x = Random.Range(-160f, 160f);

        float xNext = 0;
        float height = boundHeight + 320f;
        var nextPos = new Vector2(xNext, height);

        GameObject bound1 = Instantiate(config.boundarie.prefab, new Vector2(xNext, height), Quaternion.identity);
        bound1.transform.SetParent(dangerZoneBoundaries.transform);
        instantiatePlatforms(bound1);
        // instantiateFloors(bound1);


        GameObject bound2 = Instantiate(config.boundarie.prefab, new Vector2(xNext, height + 640), Quaternion.identity);
        bound2.transform.SetParent(dangerZoneBoundaries.transform);
        instantiatePlatforms(bound2);

        GameObject bound3 = Instantiate(config.boundarie.prefab, new Vector2(xNext, height + 640 + 640), Quaternion.identity);
        bound3.transform.SetParent(dangerZoneBoundaries.transform);
        instantiatePlatforms(bound3);


        var boundariesComponent = bound2.GetComponent<Boundaries>();
        boundariesComponent.OnPassThroughMiddle.RemoveAllListeners();
        boundariesComponent.OnExitThroughMiddle.RemoveAllListeners();

        UnityEngine.Events.UnityAction passThroughAction = null;
        UnityEngine.Events.UnityAction removeInstantiateBoundariesListener = null;

        passThroughAction = () =>
        {
            boundariesComponent.OnPassThroughMiddle.RemoveListener(passThroughAction);
            boundariesComponent.OnExitThroughMiddle.RemoveListener(removeInstantiateBoundariesListener);
            InstantiateBoundaries(bound3.transform.position.y + 320);
        };
        removeInstantiateBoundariesListener = () =>
        {
            boundariesComponent.OnPassThroughMiddle.RemoveListener(passThroughAction);
            boundariesComponent.OnExitThroughMiddle.RemoveListener(removeInstantiateBoundariesListener);
        };

        boundariesComponent.OnPassThroughMiddle.AddListener(passThroughAction);
        boundariesComponent.OnExitThroughMiddle.AddListener(removeInstantiateBoundariesListener);




    }


    private void instantiateFloors(GameObject bound)
    {
        GameObject platforms = new GameObject("Platforms");
        platforms.transform.SetParent(bound.transform);


        Vector2 pos = bound.transform.position;

        float minHorizontalValueClassic = -(config.HORIZONTAL_EDGE_LIMIT - config.floorClassic.width);
        float maxHorizontalValueClassic = config.HORIZONTAL_EDGE_LIMIT - config.floorClassic.width;

        float minHorizontalValueLarge = -(config.HORIZONTAL_EDGE_LIMIT - config.floorLarge.width);
        float maxHorizontalValueLarge = config.HORIZONTAL_EDGE_LIMIT - config.floorLarge.width;

        float minRandomHorizontal = config.floorLarge.width;
        float maxRandomHorizontal = config.floorLarge.width + 75;

        float minVerticalValue = 90f;
        float maxVerticalValue = 120f;

        float verticalOffset = 200f;

        // Ensure each platform has a separated x position
        int platformCount = 6;
        float minSeparation = 60f; // Minimum horizontal separation between platforms
        List<Vector2> vectors = new();
        float prevX = Random.Range(minHorizontalValueClassic, maxHorizontalValueClassic);
        float prevY = pos.y - verticalOffset;
        vectors.Add(new(prevX, prevY));

        for (int i = 1; i < platformCount; i++)
        {
            float newY = prevY + Random.Range(minVerticalValue, maxVerticalValue); // calculate the new height 
            float newX;
            int attempts = 0;
            do
            {
                newX = Random.Range(minHorizontalValueLarge, maxHorizontalValueLarge); // 
                attempts++;
            } while (Mathf.Abs(newX - prevX) < minSeparation && attempts < 10);
            vectors.Add(new(newX, newY));
            prevX = newX;
            prevY = newY;
        }

        var rand = new System.Random();


        var shuffled = vectors;


        int classicAmount = Random.Range(1, config.maxPlatformsInBound + 1);
        int largeAmount = Random.Range(1, config.maxPlatformsInBound + 1 - classicAmount);

        List<GameObject> classicList = new List<GameObject>();
        for (int i = 0; i < classicAmount; i++)
        {
            GameObject p = Instantiate(config.platformClassic.prefab,
                                 new Vector2(Mathf.Clamp(shuffled[i].x, -maxHorizontalValueClassic, maxHorizontalValueClassic), shuffled[i].y),
                                 quaternion.identity);
            classicList.Add(p);
        }

        // List<GameObject> largeList = new List<GameObject>();
        // for (int i = classicAmount; i < config.maxPlatformsInBound; i++)
        // {
        //     GameObject p = Instantiate(config.horizontalMoveFloor.prefab, new Vector2(Mathf.Clamp(shuffled[i].x, -maxHorizontalValueLarge, maxHorizontalValueLarge), shuffled[i].y), quaternion.identity);
        //     classicList.Add(p);
        // }
        List<GameObject> largeList = new List<GameObject>();
        for (int i = classicAmount; i < config.maxPlatformsInBound; i++)
        {
            GameObject p = Instantiate(config.horizontalMoveFloor.prefab, new Vector2(0, shuffled[i].y), quaternion.identity);
            classicList.Add(p);
        }

        largeList.ForEach(p => p.transform.SetParent(platforms.transform));
        classicList.ForEach(p => p.transform.SetParent(platforms.transform));







    }

    private void instantiatePlatforms(GameObject bound)
    {
        GameObject platforms = new GameObject("Platforms");
        platforms.transform.SetParent(bound.transform);

        // 2 large platforms, 3 normal size platforms
        Vector2 pos = bound.transform.position;

        float minHorizontalValueClassic = -(config.HORIZONTAL_EDGE_LIMIT - config.platformClassic.width);
        float maxHorizontalValueClassic = config.HORIZONTAL_EDGE_LIMIT - config.platformClassic.width;

        float minHorizontalValueLarge = -(config.HORIZONTAL_EDGE_LIMIT - config.platformLarge.width);
        float maxHorizontalValueLarge = config.HORIZONTAL_EDGE_LIMIT - config.platformLarge.width;

        float minRandomHorizontal = 50f;
        float maxRandomHorizontal = 100f;

        float minVerticalValue = 90f;
        float maxVerticalValue = 120f;

        float verticalOffset = 200f;

        float randomX = Random.Range(minHorizontalValueClassic, maxHorizontalValueClassic);

        Vector2 randomPos1 = new Vector2(randomX, pos.y - verticalOffset);



        Vector2 randomPos2 = new Vector2(Random.Range(0, randomPos1.x + Random.Range(minRandomHorizontal, maxRandomHorizontal)) * (Random.Range(0, 2) == 0 ? -1 : 1), randomPos1.y + Random.Range(minVerticalValue, maxVerticalValue));

        Vector2 randomPos3 = new Vector2(Random.Range(0, randomPos2.x + Random.Range(minRandomHorizontal, maxRandomHorizontal)) * (Random.Range(0, 2) == 0 ? -1 : 1), randomPos2.y + Random.Range(minVerticalValue, maxVerticalValue));

        Vector2 randomPos4 = new Vector2(Random.Range(0, randomPos3.x + Random.Range(minRandomHorizontal, maxRandomHorizontal)) * (Random.Range(0, 2) == 0 ? -1 : 1), randomPos3.y + Random.Range(minVerticalValue, maxVerticalValue));

        Vector2 randomPos5 = new Vector2(Random.Range(0, randomPos4.x + Random.Range(minRandomHorizontal, maxRandomHorizontal)) * (Random.Range(0, 2) == 0 ? -1 : 1), randomPos4.y + Random.Range(minVerticalValue, maxVerticalValue));

        Vector2 randomPos6 = new Vector2(Random.Range(0, minRandomHorizontal) * (Random.Range(0, 2) == 0 ? -1 : 1), randomPos5.y + Random.Range(minVerticalValue, maxVerticalValue)); // the last pos is fixed to make sure the player can pass through or connect to the next boundarie correctly.


        List<Vector2> vectors = new List<Vector2> { randomPos1, randomPos2, randomPos3, randomPos4, randomPos5, randomPos6 };

        System.Random rand = new System.Random();

        List<Vector2> shuffled = vectors.OrderBy(_ => rand.Next()).ToList();


        int classicAmount = Random.Range(1, config.maxPlatformsInBound + 1);
        int largeAmount = Random.Range(1, config.maxPlatformsInBound + 1 - classicAmount);
        int rIndex = Random.Range(0, classicAmount);
        List<GameObject> classicList = new List<GameObject>();
        for (int i = 0; i < classicAmount; i++)
        {
            GameObject p = Instantiate(config.platformClassic.prefab,
                                 new Vector2(Mathf.Clamp(shuffled[i].x, minHorizontalValueClassic, maxHorizontalValueClassic), shuffled[i].y),
                                 quaternion.identity);

            // put coins on platform
            if ((Random.Range(0, 2) == 0 ? -1 : 1) == 1)
            {
                GameObject coin = Instantiate(config.coinsPrefabA.prefab, new Vector2(p.transform.position.x, p.transform.position.y + 30), quaternion.identity);
            }
            // if (i == rIndex)
            // {
            //     float minDistance = 60f;
            //     float dissolveX;
            //     int attempts = 0;
            //     do
            //     {
            //         dissolveX = Random.Range(minHorizontalValueClassic, maxHorizontalValueClassic);
            //         attempts++;
            //     } while (Mathf.Abs(dissolveX - p.transform.position.x) < minDistance && attempts < 10);

            //     GameObject dissolve = Instantiate(
            //         config.platformDissolve.prefab,
            //         new Vector2(dissolveX, Random.Range(p.transform.position.y - 20, p.transform.position.y + 20)),
            //         quaternion.identity
            //     );
            //     GameObject coinBonus = Instantiate(config.coinsPrefabB.prefab, new Vector2(dissolve.transform.position.x, dissolve.transform.position.y + 30), quaternion.identity);

            // }


            classicList.Add(p);
        }

        List<GameObject> largeList = new List<GameObject>();
        for (int i = classicAmount; i < config.maxPlatformsInBound; i++)
        {
            GameObject p = Instantiate(config.platformLarge.prefab, new Vector2(Mathf.Clamp(shuffled[i].x, minHorizontalValueLarge, maxHorizontalValueLarge), shuffled[i].y), quaternion.identity);

            // put coins on platform
            if ((Random.Range(0, 2) == 0 ? -1 : 1) == 1)
            {
                GameObject coin1 = Instantiate(config.coinsPrefabA.prefab, new Vector2(p.transform.position.x + config.platformClassic.width * .9f, p.transform.position.y + 30), quaternion.identity);
                GameObject coin2 = Instantiate(config.coinsPrefabA.prefab, new Vector2(p.transform.position.x, p.transform.position.y + 30), quaternion.identity);
                GameObject coin3 = Instantiate(config.coinsPrefabA.prefab, new Vector2(p.transform.position.x - config.platformClassic.width * .9f, p.transform.position.y + 30), quaternion.identity);
            }
            classicList.Add(p);

        }

        largeList.ForEach(p => p.transform.SetParent(platforms.transform));
        classicList.ForEach(p => p.transform.SetParent(platforms.transform));



    }
    private void instantiatePlatformsVariant(GameObject bound)
    {
        GameObject platforms = new GameObject("Platforms");
        platforms.transform.SetParent(bound.transform);

        // 2 large platforms, 3 normal size platforms
        Vector2 pos = bound.transform.position;

        float minHorizontalValueClassic = -(config.HORIZONTAL_EDGE_LIMIT - config.platformClassic.width);
        float maxHorizontalValueClassic = config.HORIZONTAL_EDGE_LIMIT - config.platformClassic.width;

        float minHorizontalValueLarge = -(config.HORIZONTAL_EDGE_LIMIT - config.platformLarge.width);
        float maxHorizontalValueLarge = config.HORIZONTAL_EDGE_LIMIT - config.platformLarge.width;

        float minRandomHorizontal = 50f;
        float maxRandomHorizontal = 100f;

        float minVerticalValue = 90f;
        float maxVerticalValue = 120f;

        float verticalOffset = 200f;

        float randomX = Random.Range(minHorizontalValueClassic, maxHorizontalValueClassic);

        Vector2 randomPos1 = new Vector2(randomX, pos.y - verticalOffset);



        Vector2 randomPos2 = new Vector2(Random.Range(0, randomPos1.x + Random.Range(minRandomHorizontal, maxRandomHorizontal)) * (Random.Range(0, 2) == 0 ? -1 : 1), randomPos1.y + Random.Range(minVerticalValue, maxVerticalValue));

        Vector2 randomPos3 = new Vector2(Random.Range(0, randomPos2.x + Random.Range(minRandomHorizontal, maxRandomHorizontal)) * (Random.Range(0, 2) == 0 ? -1 : 1), randomPos2.y + Random.Range(minVerticalValue, maxVerticalValue));

        Vector2 randomPos4 = new Vector2(Random.Range(0, randomPos3.x + Random.Range(minRandomHorizontal, maxRandomHorizontal)) * (Random.Range(0, 2) == 0 ? -1 : 1), randomPos3.y + Random.Range(minVerticalValue, maxVerticalValue));

        Vector2 randomPos5 = new Vector2(Random.Range(0, randomPos4.x + Random.Range(minRandomHorizontal, maxRandomHorizontal)) * (Random.Range(0, 2) == 0 ? -1 : 1), randomPos4.y + Random.Range(minVerticalValue, maxVerticalValue));

        Vector2 randomPos6 = new Vector2(Random.Range(0, minRandomHorizontal) * (Random.Range(0, 2) == 0 ? -1 : 1), randomPos5.y + Random.Range(minVerticalValue, maxVerticalValue)); // the last pos is fixed to make sure the player can pass through or connect to the next boundarie correctly.


        List<Vector2> vectors = new List<Vector2> { randomPos1, randomPos2, randomPos3, randomPos4, randomPos5, randomPos6 };

        System.Random rand = new System.Random();

        List<Vector2> shuffled = vectors.OrderBy(_ => rand.Next()).ToList();


        int classicAmount = Random.Range(1, config.maxPlatformsInBound + 1);
        int largeAmount = Random.Range(1, config.maxPlatformsInBound + 1 - classicAmount);
        int rIndex = Random.Range(0, classicAmount);
        List<GameObject> classicList = new List<GameObject>();
        for (int i = 0; i < classicAmount; i++)
        {
            GameObject p = Instantiate(config.platformClassic.prefab,
                                 new Vector2(Mathf.Clamp(shuffled[i].x, minHorizontalValueClassic, maxHorizontalValueClassic), shuffled[i].y),
                                 quaternion.identity);

            // put coins on platform
            if ((Random.Range(0, 2) == 0 ? -1 : 1) == 1)
            {
                GameObject coin = Instantiate(config.coinsPrefabA.prefab, new Vector2(p.transform.position.x, p.transform.position.y + 30), quaternion.identity);
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

                GameObject dissolve = Instantiate(
                    config.platformDissolve.prefab,
                    new Vector2(dissolveX, Random.Range(p.transform.position.y - 20, p.transform.position.y + 20)),
                    quaternion.identity
                );
                GameObject coinBonus = Instantiate(config.coinsPrefabB.prefab, new Vector2(dissolve.transform.position.x, dissolve.transform.position.y + 30), quaternion.identity); // it has to be a different object

            }


            classicList.Add(p);
        }

        List<GameObject> largeList = new List<GameObject>();
        for (int i = classicAmount; i < config.maxPlatformsInBound; i++)
        {
            GameObject p = Instantiate(config.platformLarge.prefab, new Vector2(Mathf.Clamp(shuffled[i].x, minHorizontalValueLarge, maxHorizontalValueLarge), shuffled[i].y), quaternion.identity);

            // put coins on platform
            if ((Random.Range(0, 2) == 0 ? -1 : 1) == 1)
            {
                GameObject coin1 = Instantiate(config.coinsPrefabA.prefab, new Vector2(p.transform.position.x + config.platformClassic.width * .9f, p.transform.position.y + 30), quaternion.identity);
                GameObject coin2 = Instantiate(config.coinsPrefabA.prefab, new Vector2(p.transform.position.x, p.transform.position.y + 30), quaternion.identity);
                GameObject coin3 = Instantiate(config.coinsPrefabA.prefab, new Vector2(p.transform.position.x - config.platformClassic.width * .9f, p.transform.position.y + 30), quaternion.identity);
            }
            classicList.Add(p);

        }

        largeList.ForEach(p => p.transform.SetParent(platforms.transform));
        classicList.ForEach(p => p.transform.SetParent(platforms.transform));



    }
    
    public enum DangerZoneType
    {
        platforms,
        cannons,
        newPlatforms,



    }
}
