using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class DangerZoneLevelManager : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Danger Zone")]
    [SerializeField] GameObject dangerZoneBoundaries;
    [SerializeField] GameObject dangerZonePrefab;
    [SerializeField] GameObject lava;
    [SerializeField] GameObject dangerZoneCamera;
    [SerializeField] GameObject platformPrefab;
    [SerializeField] GameObject cannonsPrefab;
    [SerializeField] GameObject coinsPrefab;
    [SerializeField] GameObject floorHeight;
    
    [SerializeField] GameObject topHeight;

 
    void Start()
    {
        generateDangerZoneLevel(DangerZoneType.platforms);
        startLevel();
        // add offset;
        
    }
    public void startLevel(){
        if (GameObject.Find("Main Camera")) Destroy(GameObject.Find("Main Camera"));
        dangerZoneCamera.SetActive(true);
        
    }
   
    public void generateDangerZoneLevel(DangerZoneType type)
    {


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
                float height = floorHeight.transform.position.y+100f;
                Vector2 nextPos = new Vector2( xNext,height);
                for (int i = 0; height<=topHeight.transform.position.y ; i++)
                {


                    GameObject currentPlatform = Instantiate(platformPrefab, new Vector2(xNext, height), Quaternion.identity);
                    
                    xNext = Random.Range(-80, 80);
                    height += Random.Range(70f, 130f);
                    currentPlatform.transform.SetParent(platforms.transform);
                    x = xNext;
                    Vector2 coinPos = new Vector2(currentPlatform.transform.position.x+(xNext - currentPlatform.transform.position.x)/2, currentPlatform.transform.position.y + ((height - currentPlatform.transform.position.y)/2) );
                    GameObject coin = Instantiate(coinsPrefab, coinPos,Quaternion.identity);
                    coin.transform.SetParent(coins.transform);

                }
                break;

            case DangerZoneType.cannons:
                break;
        }
    }
    public enum DangerZoneType
    {
        platforms,
        cannons,


    }
}
