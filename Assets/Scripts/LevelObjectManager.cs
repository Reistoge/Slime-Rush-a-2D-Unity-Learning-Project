using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

/// <summary>
/// Manages the generation and placement of level segments in the game.
/// Responsible for spawning start levels, normal levels, hard levels, and backgrounds.
/// </summary>
public class LevelObjectManager : MonoBehaviour
{
    #region Serialized Fields

    [Header("Primary Levels")]
    [Tooltip("Levels used at the start of the game")]
    [SerializeField] private GameObject[] startLevels;
    [Tooltip("Standard difficulty levels")]
    [SerializeField] private GameObject[] normalLevels;
    [Tooltip("High difficulty levels")]
    [SerializeField] private GameObject[] hardLevels;
    [Tooltip("Random backgrounds for visual variety")]
    [SerializeField] private GameObject[] backgrounds;

    [Header("Danger Zone (Legacy)")]
    [Tooltip("Container for danger zone boundaries")]
    [SerializeField] private GameObject dangerZoneBoundaries;
    [SerializeField] private GameObject dangerZonePrefab;
    [SerializeField] private GameObject Lava;
    [SerializeField] private GameObject platform;
    [SerializeField] private GameObject cannon;

    #endregion

    #region Private Fields

    /// <summary>Currently active level GameObject</summary>
    private GameObject currentLevel;
    
    /// <summary>Currently active background GameObject</summary>
    private GameObject currentBg;

    #endregion

    #region Unity Lifecycle

    /// <summary>
    /// Initializes the level by generating the initial set of levels.
    /// </summary>
    void Start()
    {
        GenerateInitialLevels();
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Generates the initial set of levels with increasing difficulty.
    /// Creates a start level at Y=0, a normal level at Y=640, and a hard level at Y=1920.
    /// </summary>
    public void GenerateInitialLevels()
    {
        Transform parent = transform;

        // Instantiate a random start level at origin
        GameObject startLevelPrefab = startLevels[Random.Range(0, startLevels.Length)];
        currentLevel = Instantiate(startLevelPrefab, Vector3.zero, Quaternion.identity, parent);
        currentBg = Instantiate(GetRandomBackground(), currentLevel.transform);

        // Instantiate a random normal level at 640 units height
        GameObject normalLevelPrefab = normalLevels[Random.Range(0, normalLevels.Length)];
        Vector3 normalLevelPos = new Vector3(0, 640, 0);
        Instantiate(normalLevelPrefab, normalLevelPos, Quaternion.identity, parent);

        // Instantiate a random hard level at 1920 units height
        GameObject hardLevelPrefab = hardLevels[Random.Range(0, hardLevels.Length)];
        Vector3 hardLevelPos = new Vector3(0, 1920, 0);
        Instantiate(hardLevelPrefab, hardLevelPos, Quaternion.identity, parent);
    }

    /// <summary>
    /// Generates a danger zone level at the default position.
    /// Note: This is a legacy method that may be deprecated.
    /// </summary>
    public void GenerateRandomDangerZoneLevel()
    {
        Instantiate(dangerZonePrefab);
    }

    /// <summary>
    /// Generates a danger zone level at a specified transform with an offset.
    /// </summary>
    /// <param name="transform">The transform to use as the base position</param>
    /// <param name="offset">The Y offset to apply to the position</param>
    public void GenerateRandomDangerZoneLevel(Transform transform, float offset)
    {
        Vector2 position = new Vector2(transform.position.x, transform.position.y + offset);
        Instantiate(dangerZonePrefab, position, Quaternion.identity);
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Gets a random background GameObject from the backgrounds array.
    /// </summary>
    /// <returns>A random background GameObject</returns>
    private GameObject GetRandomBackground()
    {
        return backgrounds[Random.Range(0, backgrounds.Length)];
    }

    #endregion

    #region Enums

    /// <summary>
    /// Defines the types of danger zones (legacy enum, may be moved to DangerZoneLevelManager).
    /// </summary>
    public enum DangerZoneType
    {
        /// <summary>Platform-based danger zone</summary>
        platforms,
        
        /// <summary>Cannon-based danger zone</summary>
        cannons
    }

    #endregion
}
//     [SerializeField] GameObject[] selectedLevels;
//     [SerializeField] GameObject[] backgrounds;
//     [SerializeField] GameObject currentLevel;
//     [SerializeField] GameObject currentBg;
//     [SerializeField] GameObject autoCannonPrefab;
//     GameObject startPos;
//     [SerializeField] int level;

//     public int Level { get => level; set => level = value; }
//     public GameObject[] SelectedLevels { get => selectedLevels; set => selectedLevels = value; }
//     public GameObject CurrentLevel { get => currentLevel; set => currentLevel = value; }

//     public void takeRandomNormalLevels(int numberTotake)
//     {
//         System.Random random = new System.Random();
//         SelectedLevels = normalLevels.OrderBy(x => random.Next()).Take(numberTotake).ToArray();


//     }
//     public void takeRandomStartlLevels()
//     {
//         System.Random random = new System.Random();
//         CurrentLevel = startsLevels.OrderBy(x => random.Next()).Take(1).ToArray()[0];


//     }
//     public GameObject takeRandomBackground()
//     {
//         System.Random random = new System.Random();
//         return backgrounds.OrderBy(x => random.Next()).Take(1).ToArray()[0];

//     }
//     void OnEnable()
//     {
//         takeRandomStartlLevels();
//         takeRandomNormalLevels(10);
//         if (CurrentLevel != null)
//         {
//             instantiateLevel(CurrentLevel);
//         }
//         else
//         {

//             instantiateLevel(CurrentLevel);
//         }

//     }
//     void Start()
//     {
//         Level = 0;

//         // startPos = transform.GetChild(0).GetChild(0).gameObject;
//         // GameManager.instance.instantiatePlayer(startPos.transform);




//     }
//     public void instantiateLevels()
//     {
//         CurrentLevel = Instantiate(selectedLevels[0], this.transform);
//         CurrentLevel.name = level + ". " + currentLevel.name;
//         CurrentLevel = Instantiate(selectedLevels[0], this.transform);


//     }

//     public void instantiateLevel(GameObject levelPrefab)
//     {


//         Transform parent = GameObject.Find("LevelObjectsManager").transform;
//         if (parent != null)
//         {

//             CurrentLevel = Instantiate(levelPrefab, parent);
//             currentBg = Instantiate(takeRandomBackground(), CurrentLevel.transform);
//         }

//     }


//     public void instantiateNextLevel()
//     {

//         if (transform.childCount >= 2)
//         {
//             // levels from above are deleted.
//             transform.GetChild(level - 2).gameObject.SetActive(false);
//         }
//         Transform parent = this.transform;
//         float pastLevelPos = currentLevel.transform.position.y + 320;
//         CurrentLevel = Instantiate(selectedLevels[level + 1], parent);
//         CurrentLevel.name = level + ". " + currentLevel.name;
//         currentBg = Instantiate(takeRandomBackground(), CurrentLevel.transform);
//         Vector2 newPos = new Vector2(0, level * 900);
//         CurrentLevel.transform.position = newPos;

//         //float medianPosBetweenLevels = (pastLevelPos + CurrentLevel.transform.position.y - 320) / 2;

//         //GameObject autoCannon = Instantiate(autoCannonPrefab, new Vector2(GameManager.instance.LastUsedBarrel.transform.position.x, medianPosBetweenLevels), Quaternion.identity);

//         //autoCannon.GetComponent<AutomaticCannon>().target = findFirst().transform;

//     }
//     public GameObject findFirst()
//     {
//         foreach (Transform c in currentLevel.transform.GetChild(1))
//         {
//             if (c.gameObject.GetComponent<Cannon>().IsFirst)
//             {
//                 print(c.name);
//                 return c.gameObject;
//             }
//         }
//         return null;
//     }
//     public void instantiateLevel(GameObject levelPrefab, Vector3 position)
//     {
//         if (CurrentLevel == null)
//         {

//             CurrentLevel = Instantiate(levelPrefab, position, Quaternion.identity);
//             currentBg = Instantiate(takeRandomBackground(), CurrentLevel.transform);
//             CurrentLevel.transform.parent = GameObject.Find("LevelObjectsManager").transform;
//         }
//         else
//         {

//             Instantiate(levelPrefab, position, Quaternion.identity);

//             CurrentLevel.transform.parent = GameObject.Find("LevelObjectsManager").transform;
//         }
//     }
//     public void instantiateLevel(GameObject levelPrefab, Transform position)
//     {
//         if (CurrentLevel == null)
//         {

//             CurrentLevel = Instantiate(levelPrefab, position);
//             currentBg = Instantiate(takeRandomBackground(), CurrentLevel.transform);
//             CurrentLevel.transform.parent = GameObject.Find("LevelObjectsManager").transform;
//         }
//         else
//         {

//             Instantiate(levelPrefab, position);
//             CurrentLevel.transform.parent = GameObject.Find("LevelObjectsManager").transform;
//         }
//     }

//     public void clearObjectsInChild(GameObject parent, int indexOfChild)
//     {
//         GameObject objecstContainer = parent.transform.GetChild(indexOfChild).gameObject;
//         foreach (GameObject c in objecstContainer.transform)
//         {
//             Destroy(c);
//         }

//     }
//     public void nextMiniLevel()
//     {

//         Camera.main.GetComponent<FollowCamera>().startZoom();
//         Level += 1;
//         //instantiateNextLevel();

//     }




// }
