using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class coinsLevelHandler : MonoBehaviour
{

    // this class basically checks the coins inside.
    
    public static Action onCoinsCollectedInScreen;

    int coins;
    void OnEnable()
    {
        PlayerScript.OnPlayerGetCoin += checkAllCoinsCollected;
    }
    void OnDisable()
    {
        PlayerScript.OnPlayerGetCoin -= checkAllCoinsCollected;
    }


    public void checkAllCoinsCollected(int value)
    {
        int activeChildCount = 0;

        if (transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).gameObject.activeSelf)
                {
                    activeChildCount++;
                }
            }

            if (activeChildCount == 1)
            {
                onCoinsCollectedInScreen?.Invoke();
                // Only one child is active.
                // You can access that child using 'transform.GetChild(i)' where 'i' corresponds to the active child index.
            }
        }


    }
}
