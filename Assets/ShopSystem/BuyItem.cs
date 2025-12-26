using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
[DefaultExecutionOrder(1)]
public class BuyItem : MonoBehaviour
{

    [SerializeField] Image itemImage;
    [SerializeField] TextMeshProUGUI priceText;
    [SerializeField] GameObject coinImage;
    [SerializeField] Button buyButton;

    void OnEnable()
    {
        ShopSystem.instance.onLeftSelected.AddListener(loadItem);
        ShopSystem.instance.onRightSelected.AddListener(loadItem);
        ShopSystem.instance.onEnterShop.AddListener(loadItem);

    }
    void OnDisable()
    {
        ShopSystem.instance.onLeftSelected.RemoveListener(loadItem);
        ShopSystem.instance.onRightSelected.RemoveListener(loadItem);
        ShopSystem.instance.onEnterShop.RemoveListener(loadItem);
    }


    public void loadItem()
    {
        int currentCoins = GameManager.Instance.PlayerConfig.totalCoins;

        HatConfig hat = ShopSystem.instance.getLoadedItem(); // get the loaded hat

        // load the new values
        itemImage.sprite = hat.hatIcon;
        priceText.text = hat.price.ToString();

        // if the player doesnt have enough money to purchase or is already purchased
        if (hat.purchased || hat.price > currentCoins)
        {
            buyButton.interactable = false;
        }
        else
        {
             buyButton.interactable = true;
        }
        
    }
    public void buyLoadedItem() // assign in the editor
    {
        ShopSystem.instance.triggerPurchasedLoadedItem();
        loadItem();
    }






}