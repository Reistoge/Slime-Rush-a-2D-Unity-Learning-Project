using System;
using TMPro;
using UnityEngine;
[DefaultExecutionOrder(1)]
public class TotalCoins : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;

    void OnEnable()
    {
        ShopSystem.instance.onItemPurchased.AddListener(loadTotalCoins);
        ShopSystem.instance.onEnterShop.AddListener(loadTotalCoins);
    }
    void OnDisable()
    {
        ShopSystem.instance.onItemPurchased.RemoveListener(loadTotalCoins);
        ShopSystem.instance.onEnterShop.RemoveListener(loadTotalCoins);
    }

    private void loadTotalCoins()
    {
        if (GameManager.Instance.PlayerConfig)
        {
            text.text = GameManager.Instance.PlayerConfig.totalCoins.ToString();
        }
    }
}