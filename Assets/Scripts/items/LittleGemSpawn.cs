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
    // [SerializeField] bool randomizeColors = true;
    [SerializeField] Color[] gemColors;
    [SerializeField] BoxCollider2D spawnArea;
    [SerializeField] gemSpawnPoint[] spawnPoints;
    [SerializeField] GameObject gems;




    [System.Serializable]
    class gemSpawnPoint
    {
        public Vector2 pos;
        public float radius;

    }
    void OnEnable()
    {
        gems = new GameObject("gems");
        gems.transform.SetParent(transform);
        gemsGenerate();

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public void desactivateGems()
    {
        foreach (Transform child in gems.transform)
        {

            child.gameObject.GetComponent<Animator>().Play("disappear");

        }
        // gems.SetActive(false);
    }
    public void desactivateGemsNoAnimation()
    {
        foreach (Transform g in gems.transform)
        {
            g.gameObject.GetComponent<Animator>().Play("disappear", 0, 99f);
        }
    }
    public void activateGems()
    {
        foreach (Transform child in gems.transform)
        {

            child.gameObject.GetComponent<Animator>().Play("appear");

        }
    }

    void gemsGenerate()
    {


        gemColors = new Color[spawnPoints.Length];
        for (int i = 0; i < spawnPoints.Length; i++)
        {

            Vector2 spawnPos = Random.insideUnitSphere * spawnPoints[i].radius + (Vector3)spawnPoints[i].pos + transform.position;
            if (spawnArea)
            {

                spawnPos = new Vector2(
                    Mathf.Clamp(spawnPos.x, spawnArea.bounds.min.x, spawnArea.bounds.max.x),
                    Mathf.Clamp(spawnPos.y, transform.position.y, spawnArea.bounds.max.y)
                    );
            }
            GameObject gem = Instantiate(littleGemPrefab,
             spawnPos,
              Quaternion.Euler(0, 0, Random.Range(-120, 120)), transform);
            gem.transform.SetParent(gems.transform);
            var gemLight = gem.GetComponent<Light2D>();
            var gemSprite = gem.GetComponent<SpriteRenderer>();
            if (gemSprite && gemLight)
            {
                // gemColors[Random.Range(0, gemColors.Length)];
                gemLight.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
                //gemSprite.color = gemLight.color;
                gemColors[i] = gemLight.color;
            }

        }

    }

}
