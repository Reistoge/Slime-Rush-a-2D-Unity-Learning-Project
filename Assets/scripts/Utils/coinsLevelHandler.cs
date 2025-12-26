using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class coinsLevelHandler : MonoBehaviour
{

    // this class basically checks the coins inside.

    [SerializeField] UnityEvent onCoinsCollectedInScreen;
    [SerializeField] float spawnCoinInterval = .2f;
    [SerializeField] CoinsBehaviour behaviour;
    [SerializeField] Coroutine coroutineBehaviour;



    [SerializeField] int activeCoins;
    [SerializeField] GameObject coinPrefab;

    public GameObject CoinPrefab { get => coinPrefab; set => coinPrefab = value; }

    void Start()
    {

        setupCoins();
        processBehaviour();


    }

    public void checkAllCoinsCollected()
    {

        if (this.activeCoins == 0)
        {

            onCoinsCollectedInScreen?.Invoke();


        }

    }
    public void coinCollected()
    {
        activeCoins--;
        checkAllCoinsCollected();
    }
    public void setupCoins()
    {
        activeCoins = transform.childCount;
        foreach (Transform coin in transform)
        {

            coin.gameObject.SetActive(false);
            //yield return new WaitUntil(() => coin.gameObject.activeInHierarchy==false);
        }
    }
    public void processBehaviour()
    {
        switch (behaviour)
        {
            case CoinsBehaviour.withInterval:
                coroutineBehaviour = StartCoroutine(Behaviour1());
                break;
            case CoinsBehaviour.waitForCoinTake:
                coroutineBehaviour = StartCoroutine(Behaviour2());
                break;


        }
    }

    IEnumerator Behaviour2()
    {
        // instantiate a coin wait to the player to collect the coin and instantiate the other one.
        setupCoins();
        foreach (Transform coin in transform)
        {
            coin.gameObject.SetActive(true);
            yield return new WaitUntil(() => coin.gameObject.activeInHierarchy == false);
        }
    }
    IEnumerator Behaviour1()
    {
        setupCoins();
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
            yield return new WaitForSeconds(spawnCoinInterval);
        }
    }
    public enum CoinsBehaviour
    {
        withInterval,
        waitForCoinTake,



    }

}
#if UNITY_EDITOR
[CustomEditor(typeof(coinsLevelHandler))]
public class CoinsLevelHandlerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        coinsLevelHandler handler = (coinsLevelHandler)target;
        if (GUILayout.Button("Instantiate Coin at Origin"))
        {
            GameObject coin = Instantiate(handler.CoinPrefab, Vector3.zero, Quaternion.identity);
            coin.transform.parent = handler.transform;
            coin.name = "Coin";
        }
    }
}
#endif
