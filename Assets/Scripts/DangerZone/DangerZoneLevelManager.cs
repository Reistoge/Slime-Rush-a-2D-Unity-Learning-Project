using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;

/// <summary>
/// Manages the dynamic "danger zone" level that spawns platforms, coins, and portals as the player ascends.
/// This manager handles the procedural generation of level segments and coordinates camera movement with lava rising.
/// </summary>
public class DangerZoneLevelManager : MonoBehaviour
{
    #region Serialized Fields

    [Header("Danger Zone References")]
    [Tooltip("Container for all boundary GameObjects")]
    [SerializeField] private GameObject dangerZoneBoundaries;
    [SerializeField] private GameObject dangerZonePrefab;
    [Tooltip("Rising lava that forces the player upward")]
    [SerializeField] private GameObject lava;
    [Tooltip("Camera used during the danger zone sequence")]
    [SerializeField] private GameObject dangerZoneCamera;
    [Tooltip("Reference point for the floor level")]
    [SerializeField] private GameObject floorHeight;
    [Tooltip("Reference point for the ceiling level")]
    [SerializeField] private GameObject topHeight;

    [Header("Level Configuration")]
    [SerializeField] private DangerZoneConfig config;
    [SerializeField] private GameObject platformPrefab;
    [SerializeField] private GameObject coinsPrefab;
    [SerializeField] private GameObject cannonsPrefab;

    [Header("Timing & Camera")]
    [Tooltip("Delay before starting the danger zone sequence")]
    [SerializeField] private float waitTimeToStartLevel = 2f;
    [SerializeField] private FollowCamera camera;

    [Header("Level Generation")]
    [Tooltip("Portal prefab for shop transitions")]
    [SerializeField] private GameObject shopPortalPrefab;
    [SerializeField] private List<LevelEntitiesStrategySO> easyLevelStrategiesSO;

    #endregion

    #region Private Fields

    /// <summary>List of currently active boundary GameObjects</summary>
    private List<GameObject> boundaries = new List<GameObject>();

    /// <summary>Strategies for instantiating easy level entities</summary>
    private List<ILevelSpawner> easyLevelsStrategies = new List<ILevelSpawner>();

    /// <summary>Strategies for instantiating normal level entities (currently unused)</summary>
    private List<ILevelSpawner> normalLevelsStrategies = new List<ILevelSpawner>();

    /// <summary>Counter for tracking level progression</summary>
    [SerializeField] private int levelCount = 1;

    #endregion

    #region Singleton

    /// <summary>Singleton instance for global access</summary>
    public static DangerZoneLevelManager instance { get; private set; }

    #endregion

    #region Properties

    /// <summary>Gets or sets the danger zone configuration</summary>
    public DangerZoneConfig Config { get => config; set => config = value; }



    List<ILevelSpawner> easylList = new List<ILevelSpawner> { new LargePlatformLevelSpawner() };
    List<ILevelSpawner> normalList = new List<ILevelSpawner> { new DefaultPlatformLevelSpawner(), new DissolvePlatformLevelSpawner() };

    #region Unity Lifecycle

    /// <summary>
    /// Initializes the singleton instance and subscribes to game events.
    /// Also sets up level instantiation strategies.
    /// </summary>
    void OnEnable()
    {
        easyLevelsStrategies.AddRange(easylList);

        normalLevelsStrategies.AddRange(normalList);

        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Subscribe to game restart events
        LegacyEvents.GameEvents.Portals.onPlayerEnterInGameShopPortal += () =>
        {
            camera.stopRise();
        };


        LegacyEvents.GameEvents.onGameIsRestarted += StopAllCoroutines;
        LegacyEvents.GameEvents.onGameIsRestarted += StopCameraFollow;
    }

    /// <summary>
    /// Unsubscribes from all events to prevent memory leaks.
    /// </summary>
    void OnDisable()
    {

        LegacyEvents.GameEvents.Portals.onPlayerEnterInGameShopPortal -= () =>
        {

            camera.stopRise();

        };
        LegacyEvents.GameEvents.onGameIsRestarted -= StopAllCoroutines;
        LegacyEvents.GameEvents.onGameIsRestarted -= StopCameraFollow;
    }

    /// <summary>
    /// Initializes the danger zone level on start.
    /// </summary>
    void Start()
    {
        generateDangerZoneLevel(DangerZoneType.newPlatforms);
    }

    #endregion

    #region Camera Control

    /// <summary>
    /// Disables the camera following behavior.
    /// </summary>
    private void StopCameraFollow()
    {
        camera.GetComponent<FollowCamera>().enabled = false;
    }

    /// <summary>
    /// Enables the camera following behavior.
    /// </summary>
    private void EnableCameraFollow()
    {
        camera.GetComponent<FollowCamera>().enabled = true;
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Initiates the danger zone sequence after a delay.
    /// Called when the floor breaks and the danger zone begins.
    /// </summary>
    public void startDangerZone()
    {
        StartCoroutine(DangerZoneCoroutine());
    }

    /// <summary>
    /// Starts the level by switching from the main camera to the danger zone camera.
    /// </summary>
    public void startLevel()
    {
        // Find and replace the main camera with the danger zone camera
        GameObject mainCamera = GameObject.Find("Main Camera");
        if (mainCamera != null)
        {
            dangerZoneCamera.transform.position = mainCamera.transform.position;
            Destroy(mainCamera.gameObject);
        }
        dangerZoneCamera.SetActive(true);
    }

    /// <summary>
    /// Generates a danger zone level based on the specified type.
    /// </summary>
    /// <param name="type">The type of danger zone to generate (platforms, cannons, or newPlatforms)</param>
    public void generateDangerZoneLevel(DangerZoneType type)
    {
        switch (type)
        {
            case DangerZoneType.platforms:

                break;

            case DangerZoneType.cannons:
                // Not yet implemented
                break;

            case DangerZoneType.newPlatforms:
                instantiateBoundaries(floorHeight.transform.position.y);




                break;
        }
    }
    List<ILevelSpawner> ChooseSpawnerStrat(float level)
    {
        // Create container GameObjects for organization
        GameObject platforms = new GameObject("Platforms");
        GameObject coins = new GameObject("Coins");

        switch (level)
        {
            case <= 1:
                return easyLevelsStrategies;

            case <= 2:
                return normalLevelsStrategies;

            default:
                return null;

        }

    }

    void instantiateBoundaries(float height)
    {
        var stratLevelSpawner = ChooseSpawnerStrat(levelCount);

        camera.Rise.increaseSpeed(1.2f);

        // Calculate positions for three consecutive boundary segments
        float xNext = 0;
        float heightOffset = 320;

        float boundVerticalSizeOffset = -70;
        float boundVerticalSize = 640 + boundVerticalSizeOffset;
        
        float boundInitHeight = height + heightOffset;
   

        // Instantiate three boundary segments vertically
        GameObject bound1 = Instantiate(Config.boundarie.prefab, new Vector2(xNext, boundInitHeight), Quaternion.identity);
        bound1.transform.name = "bound" + ((levelCount * 3) - 2);
        bound1.transform.SetParent(dangerZoneBoundaries.transform);
        stratLevelSpawner[Random.Range(0, stratLevelSpawner.Count)].instantiateEntities(bound1);
        boundaries.Add(bound1);

        GameObject bound2 = Instantiate(Config.boundarie.prefab, new Vector2(xNext, boundInitHeight + boundVerticalSize), Quaternion.identity);
        bound2.transform.name = "bound" + ((levelCount * 3) - 1);
        bound2.transform.SetParent(dangerZoneBoundaries.transform);
        stratLevelSpawner[Random.Range(0, stratLevelSpawner.Count)].instantiateEntities(bound2);
        boundaries.Add(bound2);

        GameObject bound3 = Instantiate(Config.boundarie.prefab, new Vector2(xNext, boundInitHeight + 2*boundVerticalSize), Quaternion.identity);
        bound3.transform.name = "bound" + ((levelCount * 3));
        bound3.transform.SetParent(dangerZoneBoundaries.transform);
        stratLevelSpawner[Random.Range(0, stratLevelSpawner.Count)].instantiateEntities(bound3);
        boundaries.Add(bound3);

        // Set up event listeners on the middle boundary to trigger next segment generation
        var boundariesComponent = bound2.GetComponent<Boundaries>();
        boundariesComponent.OnPassThroughMiddle.RemoveAllListeners();
        boundariesComponent.OnExitThroughMiddle.RemoveAllListeners();

        UnityEngine.Events.UnityAction passThroughAction = null;
        UnityEngine.Events.UnityAction removeInstantiateBoundariesListener = null;

        // When player passes through the middle boundary, generate the next set
        passThroughAction = () =>
        {
            boundariesComponent.OnPassThroughMiddle.RemoveListener(passThroughAction);
            boundariesComponent.OnExitThroughMiddle.RemoveListener(removeInstantiateBoundariesListener);
            instantiateBoundaries(bound3.transform.position.y +boundVerticalSize/2 );
        };

        // Clean up listeners when player exits the boundary
        removeInstantiateBoundariesListener = () =>
        {
            boundariesComponent.OnPassThroughMiddle.RemoveListener(passThroughAction);
            boundariesComponent.OnExitThroughMiddle.RemoveListener(removeInstantiateBoundariesListener);
        };

        boundariesComponent.OnPassThroughMiddle.AddListener(passThroughAction);
        boundariesComponent.OnExitThroughMiddle.AddListener(removeInstantiateBoundariesListener);

        levelCount++;

        // Randomly spawn shop portals after the first level
        if (levelCount > 1)
        {
            if (Random.Range(0f, 1f) < config.shopPortal.spawnRate)
            {
                // Instantiate portal in one of the 3 recently spawned boundaries
                instantiateShopPortal(boundaries[(boundaries.Count - 1) - Random.Range(0, 3)]);
            }
        }

        // Clean up old boundaries every 3 levels to prevent memory buildup
        if (levelCount % 3 == 0)
        {
            for (int i = 0; i < 3; i++)
            {
                boundaries[0].SetActive(false);
                boundaries.RemoveAt(0);
            }
        }
    }

    /// <summary>
    /// Instantiates a shop portal on a random platform within the specified boundary.
    /// The portal is positioned away from the platform center to provide a navigation challenge.
    /// </summary>
    /// <param name="bound">The boundary GameObject containing platforms</param>
    private void instantiateShopPortal(GameObject bound)
    {
        var platforms = bound.transform.Find("Platforms");
        if (platforms == null || platforms.childCount == 0)
        {
            return;
        }

        // Select a random platform to place the portal
        int randomIndex = Random.Range(0, platforms.childCount);
        Transform randomPlatform = platforms.GetChild(randomIndex);

        // Configuration for portal positioning
        const float minX = -180f;
        const float maxX = 180f;
        const float minDist = 90f;  // Minimum distance from platform
        const float maxDist = 140f; // Maximum distance from platform
        const float portalYOffset = 45f; // Height above platform

        float platformX = randomPlatform.position.x;

        // Try to place portal on a random side of the platform
        int sign = (Random.Range(0, 2) == 0) ? -1 : 1;
        float offset = Random.Range(minDist, maxDist);
        float candidate = platformX + sign * offset;

        // If candidate position is out of bounds, find a valid alternative
        if (candidate < minX || candidate > maxX)
        {
            // Try the opposite side
            float candidateOther = platformX - sign * offset;
            if (candidateOther >= minX && candidateOther <= maxX)
            {
                candidate = candidateOther;
            }
            else
            {
                // Calculate available space on both sides
                float availableRight = maxX - platformX;
                float availableLeft = platformX - minX;

                // Choose the side with enough space
                if (availableRight >= minDist && availableLeft >= minDist)
                {
                    // Both sides valid, pick one randomly
                    bool useRight = Random.Range(0, 2) == 0;
                    float available = useRight ? availableRight : availableLeft;
                    offset = Random.Range(minDist, Mathf.Min(maxDist, available));
                    candidate = platformX + (useRight ? offset : -offset);
                }
                else if (availableRight >= minDist)
                {
                    offset = Random.Range(minDist, Mathf.Min(maxDist, availableRight));
                    candidate = platformX + offset;
                }
                else if (availableLeft >= minDist)
                {
                    offset = Random.Range(minDist, Mathf.Min(maxDist, availableLeft));
                    candidate = platformX - offset;
                }
                else
                {
                    // Fallback: clamp to valid bounds (rare case)
                    candidate = Mathf.Clamp(candidate, minX, maxX);
                }
            }
        }

        // Instantiate the portal at the calculated position
        Vector2 portalPosition = new Vector2(candidate, randomPlatform.position.y + portalYOffset);
        Debug.Log($"Shop portal spawned at X: {candidate}");

        GameObject portal = Instantiate(config.shopPortal.prefab, portalPosition, Quaternion.identity);
        portal.SetActive(true);
        portal.transform.SetParent(bound.transform);
    }

    #endregion

    #region Coroutines

    /// <summary>
    /// Coroutine that handles the danger zone initialization sequence.
    /// </summary>
    private IEnumerator DangerZoneCoroutine()
    {
        yield return new WaitForSeconds(waitTimeToStartLevel);
        EnableCameraFollow();
        Debug.Log($"Camera Type: {camera.CameraType}");
        initializeLavaWaypoints();

        startLevel();

        void initializeLavaWaypoints()
        {
            lava = Instantiate(lava);
            WayPointMovement.MoveBehaviour center = new WayPointMovement.MoveBehaviour();
            center.velocity = 50;
            center.wayPoint = camera.Center;

            WayPointMovement.MoveBehaviour bottom = new WayPointMovement.MoveBehaviour();
            bottom.velocity = 30;
            bottom.wayPoint = camera.Bottom;


            lava.GetComponentInChildren<WayPointMovement>().MoveVariables = new List<WayPointMovement.MoveBehaviour> {center, bottom}.ToArray();
 
        }
    }


    #endregion

    #region Enums

    /// <summary>
    /// Defines the types of danger zone levels that can be generated.
    /// </summary>
    public enum DangerZoneType
    {
        /// <summary>Legacy platform generation (deprecated)</summary>
        platforms,

        /// <summary>Cannon-based level (not yet implemented)</summary>
        cannons,

        /// <summary>New procedural platform generation with boundaries</summary>
        newPlatforms
    }

    #endregion
    #endregion
}