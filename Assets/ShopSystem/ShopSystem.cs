using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[DefaultExecutionOrder(-1)]
public class ShopSystem : MonoBehaviour
{
    public static ShopSystem instance;
    [SerializeField] HatsRepository hatsRepository;

    public UnityEvent onRightSelected;
    public UnityEvent onLeftSelected;
    public UnityEvent onEnterShop;

    public UnityEvent onEquipItem;

    public UnityEvent onItemPurchased;
    public void triggerOnRightSelected()
    {
        if (hatsRepository.loadedHatIndex < hatsRepository.hats.Length - 1)
        {
            hatsRepository.loadedHatIndex++;
        }
        onRightSelected?.Invoke();
    }
    public void triggerOnLeftSelected()
    {
        if (hatsRepository.loadedHatIndex > 0)
        {
            hatsRepository.loadedHatIndex--;
        }
        onLeftSelected?.Invoke();
    }
    void Start()
    {
        // in first place if there is no loaded hat, assign to the first hat (nothing)
        if (hatsRepository != null && hatsRepository.hats != null)
        {
            hatsRepository.loadedHatIndex = hatsRepository.selectedHatIndex;
            onEnterShop?.Invoke();
        }

    }

    public HatConfig getLoadedItem()
    {
        return hatsRepository.hats[hatsRepository.loadedHatIndex];
    }
    public HatConfig getSelectedItem()
    {
        return hatsRepository.hats[hatsRepository.selectedHatIndex];
    }
    void OnEnable()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

    }

    public void triggerPurchasedLoadedItem()
    {
        HatConfig hat = getLoadedItem();
        GameManager.Instance.PlayerConfig.totalCoins -= hat.price;
        hat.purchased = true;
        onItemPurchased?.Invoke();
    }

    public void triggerOnItemEquip()
    {
        GameManager.Instance.SelectedPlayer = getLoadedItem().associatedPrefab;
        hatsRepository.selectedHatIndex = hatsRepository.loadedHatIndex;
        onEquipItem?.Invoke();

    }
}
