using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Manages the shop system for purchasing and equipping items.
/// Singleton that handles item selection, purchase, and equipment logic.
/// </summary>
[DefaultExecutionOrder(-1)]
public class ShopSystem : MonoBehaviour
{
    public static ShopSystem instance;

    [SerializeField] private HatsRepository hatsRepository;

    /// <summary>Event triggered when navigating right in shop.</summary>
    public UnityEvent onRightSelected;

    /// <summary>Event triggered when navigating left in shop.</summary>
    public UnityEvent onLeftSelected;

    /// <summary>Event triggered when entering the shop.</summary>
    public UnityEvent onEnterShop;

    /// <summary>Event triggered when equipping an item.</summary>
    public UnityEvent onEquipItem;

    /// <summary>Event triggered when purchasing an item.</summary>
    public UnityEvent onItemPurchased;

    /// <summary>
    /// Moves selection to the right in the shop.
    /// </summary>
    public void triggerOnRightSelected()
    {
        if (hatsRepository.loadedHatIndex < hatsRepository.hats.Length - 1)
        {
            hatsRepository.loadedHatIndex++;
        }
        onRightSelected?.Invoke();
    }

    /// <summary>
    /// Moves selection to the left in the shop.
    /// </summary>
    public void triggerOnLeftSelected()
    {
        if (hatsRepository.loadedHatIndex > 0)
        {
            hatsRepository.loadedHatIndex--;
        }
        onLeftSelected?.Invoke();
    }

    private void Start()
    {
        // Initialize shop with currently selected item
        if (hatsRepository != null && hatsRepository.hats != null)
        {
            hatsRepository.loadedHatIndex = hatsRepository.selectedHatIndex;
            onEnterShop?.Invoke();
        }
    }

    /// <summary>
    /// Gets the currently loaded (previewed) item in the shop.
    /// </summary>
    /// <returns>The hat configuration for the loaded item</returns>
    public HatConfig getLoadedItem()
    {
        return hatsRepository.hats[hatsRepository.loadedHatIndex];
    }

    /// <summary>
    /// Gets the currently selected (equipped) item.
    /// </summary>
    /// <returns>The hat configuration for the selected item</returns>
    public HatConfig getSelectedItem()
    {
        return hatsRepository.hats[hatsRepository.selectedHatIndex];
    }

    private void OnEnable()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    /// <summary>
    /// Purchases the currently loaded item.
    /// Deducts coins and marks item as purchased.
    /// </summary>
    public void triggerPurchasedLoadedItem()
    {
        HatConfig hat = getLoadedItem();
        GameManager.Instance.PlayerConfig.totalCoins -= hat.price;
        hat.purchased = true;
        onItemPurchased?.Invoke();
    }

    /// <summary>
    /// Equips the currently loaded item.
    /// Updates the player's appearance and saves the selection.
    /// </summary>
    public void triggerOnItemEquip()
    {
        GameManager.Instance.SelectedPlayer = getLoadedItem().associatedPrefab;
        hatsRepository.selectedHatIndex = hatsRepository.loadedHatIndex;
        onEquipItem?.Invoke();
    }
}
