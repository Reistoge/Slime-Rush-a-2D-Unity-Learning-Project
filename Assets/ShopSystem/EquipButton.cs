using System;
using UnityEngine;
using UnityEngine.UI;
[DefaultExecutionOrder(1)]
public class EquipButton : MonoBehaviour
{
    [SerializeField] Button button;
    void OnEnable()
    {
        ShopSystem.instance.onLeftSelected.AddListener(checkEquippable);
        ShopSystem.instance.onRightSelected.AddListener(checkEquippable);
        ShopSystem.instance.onEnterShop.AddListener(checkEquippable);
        ShopSystem.instance.onItemPurchased.AddListener(checkEquippable);
    }



    void OnDisable()
    {
        ShopSystem.instance.onLeftSelected.RemoveListener(checkEquippable);
        ShopSystem.instance.onRightSelected.RemoveListener(checkEquippable);
        ShopSystem.instance.onEnterShop.RemoveListener(checkEquippable);
        ShopSystem.instance.onItemPurchased.RemoveListener(checkEquippable);
    }
    public void checkEquippable()
    {
        HatConfig hat = ShopSystem.instance.getLoadedItem();
        if (hat.purchased)
        {
            button.interactable = true;
            if (ShopSystem.instance.getLoadedItem() == ShopSystem.instance.getSelectedItem())
            {
                button.interactable = false;
            }
        }
        else
        {
            button.interactable = false;
        }
    }
    public void equipItem() // assigned in the editor
    {
        ShopSystem.instance.triggerOnItemEquip();
        button.interactable = false;
    }
}