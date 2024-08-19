using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class LevelObjectManager : MonoBehaviour
{
    [SerializeField] GameObject[] startsLevels;
    [SerializeField] GameObject[] normalLevels;
    [SerializeField] GameObject[] bonusLevels;
    [SerializeField] GameObject[] finalLevels;
    [SerializeField] GameObject[] selectedLevels;
    [SerializeField] GameObject[] backgrounds;
    [SerializeField] GameObject currentLevel;
    [SerializeField] GameObject currentBg;
    [SerializeField] GameObject autoCannonPrefab;
    GameObject startPos;
    [SerializeField] int level;

    public int Level { get => level; set => level = value; }
    public GameObject[] SelectedLevels { get => selectedLevels; set => selectedLevels = value; }
    public GameObject CurrentLevel { get => currentLevel; set => currentLevel = value; }

    public void takeRandomNormalLevels(int numberTotake)
    {
        System.Random random = new System.Random();
        SelectedLevels = normalLevels.OrderBy(x => random.Next()).Take(numberTotake).ToArray();


    }
    public void takeRandomStartlLevels()
    {
        System.Random random = new System.Random();
        CurrentLevel = startsLevels.OrderBy(x => random.Next()).Take(1).ToArray()[0];


    }
    public GameObject takeRandomBackground()
    {
        System.Random random = new System.Random();
        return backgrounds.OrderBy(x => random.Next()).Take(1).ToArray()[0];

    }
    void OnEnable()
    {
        takeRandomStartlLevels();
        takeRandomNormalLevels(10);
        if (CurrentLevel != null)
        {
            instantiateLevel(CurrentLevel);
        }
        else
        {

            instantiateLevel(CurrentLevel);
        }

    }
    void Start()
    {
        Level = 0;

        startPos = transform.GetChild(0).GetChild(0).gameObject;
        GameManager.instance.instantiatePlayer(startPos.transform);




    }

    public void instantiateLevel(GameObject levelPrefab)
    {


        Transform parent = GameObject.Find("LevelObjectsManager").transform;
        if (parent != null)
        {

            CurrentLevel = Instantiate(levelPrefab, parent);
            currentBg = Instantiate(takeRandomBackground(), CurrentLevel.transform);
        }

    }

    public void instantiateNextLevel()
    {

        if (transform.childCount >= 2)
        {
            transform.GetChild(level - 2).gameObject.SetActive(false);
        }
        Transform parent = this.transform;
        float pastLevelPos=currentLevel.transform.position.y+320; 
        CurrentLevel = Instantiate(selectedLevels[level + 1], parent);
        currentBg = Instantiate(takeRandomBackground(), CurrentLevel.transform);
        Vector2 newPos = new Vector2(0, level * 900);
        CurrentLevel.transform.position = newPos;
        float medianPosBetweenLevels=(pastLevelPos+CurrentLevel.transform.position.y-320)/2;
        GameObject autoCannon = Instantiate(autoCannonPrefab, new Vector2(GameManager.instance.LastUsedBarrel.transform.position.x, medianPosBetweenLevels), Quaternion.identity);
        autoCannon.GetComponent<AutomaticCannon>().target = findFirst().transform;

    }
    public GameObject findFirst()
    {
        foreach (Transform c in currentLevel.transform.GetChild(1))
        {
            if (c.gameObject.GetComponent<Cannon>().IsFirst)
            {
                print(c.name);
                return c.gameObject;
            }
        }
        return null;
    }
    public void instantiateLevel(GameObject levelPrefab, Vector3 position)
    {
        if (CurrentLevel == null)
        {

            CurrentLevel = Instantiate(levelPrefab, position, Quaternion.identity);
            currentBg = Instantiate(takeRandomBackground(), CurrentLevel.transform);
            CurrentLevel.transform.parent = GameObject.Find("LevelObjectsManager").transform;
        }
        else
        {

            Instantiate(levelPrefab, position, Quaternion.identity);

            CurrentLevel.transform.parent = GameObject.Find("LevelObjectsManager").transform;
        }
    }
    public void instantiateLevel(GameObject levelPrefab, Transform position)
    {
        if (CurrentLevel == null)
        {

            CurrentLevel = Instantiate(levelPrefab, position);
            currentBg = Instantiate(takeRandomBackground(), CurrentLevel.transform);
            CurrentLevel.transform.parent = GameObject.Find("LevelObjectsManager").transform;
        }
        else
        {

            Instantiate(levelPrefab, position);
            CurrentLevel.transform.parent = GameObject.Find("LevelObjectsManager").transform;
        }
    }

    public void clearObjectsInChild(GameObject parent, int indexOfChild)
    {
        GameObject objecstContainer = parent.transform.GetChild(indexOfChild).gameObject;
        foreach (GameObject c in objecstContainer.transform)
        {
            Destroy(c);
        }

    }




}
