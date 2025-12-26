using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
using JetBrains.Annotations;
using Unity.Mathematics;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
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
    [SerializeField] GameObject shopPortalPrefab;

    List<GameObject> boundaries = new List<GameObject>();
    public static DangerZoneLevelManager instance { get; private set; }
    public DangerZoneConfig Config { get => config; set => config = value; }

    [SerializeField]
    private List<LevelEntitiesStrategySO> easyLevelStrategiesSO;
    List<ILevelEntitiesInstantiationStrategy> easyLevelsStrategies = new List<ILevelEntitiesInstantiationStrategy>();
    List<ILevelEntitiesInstantiationStrategy> normalLevelsStrategies = new List<ILevelEntitiesInstantiationStrategy>();

    void OnEnable()
    {
        easyLevelsStrategies.Add(new BasicDissolvePlatforms());
        easyLevelsStrategies.Add(new BasicDissolvePlatforms());
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
        LegacyEvents.GameEvents.onGameIsRestarted += StopAllCoroutines;
        LegacyEvents.GameEvents.onGameIsRestarted += stopCameraFollow;
    }
    void OnDisable()
    {
        LegacyEvents.GameEvents.onGameIsRestarted -= StopAllCoroutines;
        LegacyEvents.GameEvents.onGameIsRestarted -= stopCameraFollow;
    }
    void stopCameraFollow()
    {
        camera.GetComponent<FollowCamera>().enabled = false;


    }
    void enableCameraFollow()
    {
        camera.GetComponent<FollowCamera>().enabled = true;

    }
    IEnumerator dangerZoneCoroutine()
    {
        // generateDangerZoneLevel(DangerZoneType.newPlatforms);
        yield return new WaitForSeconds(waitTimeToStartLevel);
        enableCameraFollow();
        print(camera.CameraType);
        lava.SetActive(true);
        startLevel();
        // startLevel();
        // add offset;

    }

    public void startDangerZone()
    {
        // method call when the floor is broken
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


                instantiateBoundaries(floorHeight.transform.position.y);
                // xNext = 0;
                // height = floorHeight.transform.position.y + 320f;
                // nextPos = new Vector2(xNext, height - 50f);


                // GameObject bound1 = Instantiate(Config.boundarie.prefab, new Vector2(xNext, height), Quaternion.identity);

                // bound1.transform.SetParent(dangerZoneBoundaries.transform);
                // easyLevelsStrategies[Random.Range(0, easyLevelsStrategies.Count)].instantiateEntities(bound1);
                // // instantiatePlatforms(bound1);
                // // instantiateFloors(bound1);


                // GameObject bound2 = Instantiate(Config.boundarie.prefab, new Vector2(xNext, height + 640), Quaternion.identity);
                // bound2.transform.SetParent(dangerZoneBoundaries.transform);
                // easyLevelsStrategies[Random.Range(0, easyLevelsStrategies.Count)].instantiateEntities(bound2);

                // // instantiatePlatforms(bound2);


                // if (levelCount % 3 == 0)
                // {

                //     instantiateShopPortal(bound2);
                // }


                // GameObject bound3 = Instantiate(Config.boundarie.prefab, new Vector2(xNext, height + 640 + 640), Quaternion.identity);
                // bound3.transform.SetParent(dangerZoneBoundaries.transform);
                // easyLevelsStrategies[Random.Range(0, easyLevelsStrategies.Count)].instantiateEntities(bound3);

                // // instantiatePlatforms(bound3);



                // var boundariesComponent = bound2.GetComponent<Boundaries>();
                // boundariesComponent.OnPassThroughMiddle.RemoveAllListeners();
                // boundariesComponent.OnExitThroughMiddle.RemoveAllListeners();

                // UnityEngine.Events.UnityAction passThroughAction = null;
                // UnityEngine.Events.UnityAction removeInstantiateBoundariesListener = null;



                // passThroughAction = () =>
                // {
                //     // Remove listeners before instantiating to avoid multiple calls
                //     boundariesComponent.OnPassThroughMiddle.RemoveListener(passThroughAction);
                //     boundariesComponent.OnExitThroughMiddle.RemoveListener(removeInstantiateBoundariesListener);
                //     InstantiateBoundaries(bound3.transform.position.y + 320);
                // };
                // removeInstantiateBoundariesListener = () =>
                // {
                //     boundariesComponent.OnPassThroughMiddle.RemoveListener(passThroughAction);
                //     boundariesComponent.OnExitThroughMiddle.RemoveListener(removeInstantiateBoundariesListener);
                // };

                // boundariesComponent.OnPassThroughMiddle.AddListener(passThroughAction);
                // boundariesComponent.OnExitThroughMiddle.AddListener(removeInstantiateBoundariesListener);



                break;
        }
    }
    void instantiateBoundaries(float boundHeight)
    {

        camera.Rise.increaseSpeed(1.2f);
        float x = Random.Range(-160f, 160f);

        float xNext = 0;
        float height = boundHeight + 320f;
        var nextPos = new Vector2(xNext, height);

        GameObject bound1 = Instantiate(Config.boundarie.prefab, new Vector2(xNext, height), Quaternion.identity);
        bound1.transform.name = "bound" + ((levelCount * 3) - 2);
        bound1.transform.SetParent(dangerZoneBoundaries.transform);
        easyLevelsStrategies[Random.Range(0, easyLevelsStrategies.Count)].instantiateEntities(bound1);
        boundaries.Add(bound1);
        // instantiatePlatforms(bound1);
        // instantiateFloors(bound1);


        GameObject bound2 = Instantiate(Config.boundarie.prefab, new Vector2(xNext, height + 640), Quaternion.identity);
        bound2.transform.name = "bound" + ((levelCount * 3) - 1);
        bound2.transform.SetParent(dangerZoneBoundaries.transform);
        easyLevelsStrategies[Random.Range(0, easyLevelsStrategies.Count)].instantiateEntities(bound2);
        boundaries.Add(bound2);

        // instantiatePlatforms(bound2);


        GameObject bound3 = Instantiate(Config.boundarie.prefab, new Vector2(xNext, height + 640 + 640), Quaternion.identity);
        bound3.transform.name = "bound" + ((levelCount * 3));
        bound3.transform.SetParent(dangerZoneBoundaries.transform);
        easyLevelsStrategies[Random.Range(0, easyLevelsStrategies.Count)].instantiateEntities(bound3);
        boundaries.Add(bound3);
        // instantiatePlatforms(bound3);


        var boundariesComponent = bound2.GetComponent<Boundaries>();
        boundariesComponent.OnPassThroughMiddle.RemoveAllListeners();
        boundariesComponent.OnExitThroughMiddle.RemoveAllListeners();

        UnityEngine.Events.UnityAction passThroughAction = null;
        UnityEngine.Events.UnityAction removeInstantiateBoundariesListener = null;

        passThroughAction = () =>
        {
            boundariesComponent.OnPassThroughMiddle.RemoveListener(passThroughAction);
            boundariesComponent.OnExitThroughMiddle.RemoveListener(removeInstantiateBoundariesListener);
            instantiateBoundaries(bound3.transform.position.y + 320);
        };
        removeInstantiateBoundariesListener = () =>
        {
            boundariesComponent.OnPassThroughMiddle.RemoveListener(passThroughAction);
            boundariesComponent.OnExitThroughMiddle.RemoveListener(removeInstantiateBoundariesListener);
        };

        boundariesComponent.OnPassThroughMiddle.AddListener(passThroughAction);
        boundariesComponent.OnExitThroughMiddle.AddListener(removeInstantiateBoundariesListener);
        levelCount++;
        if (levelCount > 1)
        {
            if (Random.Range(0f, 1f) < config.shopPortal.spawnRate)
            {
                // instantiate portal in one of the 3 recently spawned boundaries
                instantiateShopPortal(boundaries[(boundaries.Count - 1) - Random.Range(0, 3)]);
            }
        }
        if ((levelCount) % 3 == 0)
        {
            for (int i = 0; i < 3; i++)
            {
                boundaries[0].SetActive(false);
                boundaries.RemoveAt(0);
            }
        }






    }

    private void instantiateShopPortal(GameObject bound)
    {
        var platforms = bound.transform.Find("Platforms");
        if (platforms != null && platforms.childCount > 0)
        {
            int randomIndex = Random.Range(0, platforms.childCount);
            Transform randomPlatform = platforms.GetChild(randomIndex);

            float minX = -180f, maxX = 180f;
            float minDist = 90f, maxDist = 140f;
            float platformX = randomPlatform.position.x;

            // try random side first
            int sign = (Random.Range(0, 2) == 0) ? -1 : 1;
            float offset = Random.Range(minDist, maxDist);
            float candidate = platformX + sign * offset;

            // if candidate out of bounds try the other side
            if (candidate < minX || candidate > maxX)
            {
                int otherSign = -sign;
                float candidateOther = platformX + otherSign * offset;
                if (candidateOther >= minX && candidateOther <= maxX)
                {
                    candidate = candidateOther;
                }
                else
                {
                    // compute available space on each side and pick a valid offset within [minDist, maxDist]
                    float availablePos = maxX - platformX;   // max offset to the right
                    float availableNeg = platformX - minX;   // max offset to the left

                    if (availablePos >= minDist || availableNeg >= minDist)
                    {
                        if (availablePos >= minDist && availableNeg >= minDist)
                        {
                            // both sides valid, pick one randomly
                            if (Random.Range(0, 2) == 0)
                            {
                                offset = Random.Range(minDist, Mathf.Min(maxDist, availablePos));
                                candidate = platformX + offset;
                            }
                            else
                            {
                                offset = Random.Range(minDist, Mathf.Min(maxDist, availableNeg));
                                candidate = platformX - offset;
                            }
                        }
                        else if (availablePos >= minDist)
                        {
                            offset = Random.Range(minDist, Mathf.Min(maxDist, availablePos));
                            candidate = platformX + offset;
                        }
                        else
                        {
                            offset = Random.Range(minDist, Mathf.Min(maxDist, availableNeg));
                            candidate = platformX - offset;
                        }
                    }
                    else
                    {
                        // fallback clamp (should be rare); keeps within bounds
                        candidate = Mathf.Clamp(candidate, minX, maxX);
                    }
                }
            }

            float portalX = candidate;
            print($" RANDOM PORTAL POSITION {portalX}");
            print($" RANDOM PORTAL POSITION {portalX}");
            GameObject portal = Instantiate(config.shopPortal.prefab, new Vector2(portalX, randomPlatform.position.y + 45f), Quaternion.identity);
            portal.transform.SetParent(bound.transform);
            portal.SetActive(true);
        }
    }



    public enum DangerZoneType
    {
        platforms,
        cannons,
        newPlatforms,



    }
}
