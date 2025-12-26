using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class LevelObjectManager : MonoBehaviour
{
    [Header("Primary Levels")]
    [SerializeField] GameObject[] startLevels;  // Niveles de inicio
    [SerializeField] GameObject[] normalLevels; // Niveles normales
    [SerializeField] GameObject[] hardLevels;
    [SerializeField] GameObject[] backgrounds;  // Fondos aleatorios

    [Header("Danger Zone")]
    [SerializeField] GameObject dangerZoneBoundaries;
    [SerializeField] GameObject dangerZonePrefab;
    [SerializeField] GameObject Lava;
    [SerializeField] GameObject platform;
    [SerializeField] GameObject cannon;





    private GameObject currentLevel;
    private GameObject currentBg;

    void Start()
    {

        generateInitialLevels();

    }
    public void generateInitialLevels()
    {
        Transform parent = transform; // Se asegura de que los niveles estén en este GameObject

        // **1. Instanciar un nivel de inicio aleatorio**
        GameObject startLevelPrefab = startLevels[Random.Range(0, startLevels.Length)];
        currentLevel = Instantiate(startLevelPrefab, Vector3.zero, Quaternion.identity, parent);
        currentBg = Instantiate(GetRandomBackground(), currentLevel.transform);

        // **2. Instanciar un nivel normal aleatorio a 640 unidades de altura**
        GameObject normalLevelPrefab = normalLevels[Random.Range(0, normalLevels.Length)];
        Vector3 normalLevelPos = new Vector3(0, 640, 0);
        Instantiate(normalLevelPrefab, normalLevelPos, Quaternion.identity, parent);

        GameObject hardLevelPrefab = hardLevels[Random.Range(0, normalLevels.Length)];
        Vector3 hardLevelPos = new Vector3(0, 1920, 0);
        Instantiate(hardLevelPrefab, hardLevelPos, Quaternion.identity, parent);
    }

    public void generateRandomDangerZoneLevel()
    {
        Instantiate(dangerZonePrefab);
    }
    public void generateRandomDangerZoneLevel(Transform transform, float offset)
    {

        Instantiate(dangerZonePrefab, new Vector2(transform.position.x, transform.position.y + offset), quaternion.identity);
    }
    public enum DangerZoneType
    {
        platforms,
        cannons,


    }


    private GameObject GetRandomBackground()
    {
        return backgrounds[Random.Range(0, backgrounds.Length)];
    }




}




// using System.Collections;
// using System.Collections.Generic;
// using System.Drawing;
// using System.Linq;
// using Unity.VisualScripting;
// using UnityEngine;

// public class LevelObjectManager : GenericSingleton<LevelObjectManager>
// {
//     [SerializeField] GameObject[] startsLevels;
//     [SerializeField] GameObject[] normalLevels;
//     [SerializeField] GameObject[] bonusLevels;
//     [SerializeField] GameObject[] finalLevels;
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
