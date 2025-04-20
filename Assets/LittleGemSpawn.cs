using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Rendering.Universal;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class LittleGemSpawn : MonoBehaviour
{

    [SerializeField] GameObject littleGemPrefab;
    // [SerializeField] Collider2D spawnArea;
    // [SerializeField] int numberOfGems;
    [SerializeField] bool randomizeColors = true;
    [SerializeField] Color[] gemColors;

    [SerializeField] gemSpawnPoint[] spawnPoints;
    [System.Serializable]
    class gemSpawnPoint{
        public Vector2 pos;
        public float radius;

    }


    // Start is called before the first frame update
    void Start()
    {
         gemsGenerate();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void randomizeColor(){
        // for(int i = 0; i < numberOfGems; i++)
        // {
        //     Color color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
            
        //     gemColors[i] = color;
        // }
         
    }
    void gemsGenerate()
    {


            gemColors = new Color[spawnPoints.Length];
            for (int i = 0; i < spawnPoints.Length; i++)
            {
                 
                 
                GameObject gem = Instantiate(littleGemPrefab, Random.insideUnitSphere * spawnPoints[i].radius + (Vector3)spawnPoints[i].pos + transform.position, Quaternion.Euler(0,0,Random.Range(-120,120)), transform);
                var gemLight = gem.GetComponent<Light2D>();
                var gemSprite = gem.GetComponent<SpriteRenderer>();
                if(gemSprite && gemLight){
                    // gemColors[Random.Range(0, gemColors.Length)];
                    gemLight.color =  Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
                    //gemSprite.color = gemLight.color;
                    gemColors[i] = gemLight.color;
                }
                 
            }

    }

}
