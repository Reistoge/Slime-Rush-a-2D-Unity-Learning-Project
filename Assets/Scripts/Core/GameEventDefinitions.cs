using UnityEngine;

/// <summary>
/// Definitions for all game events. Using structs for type-safe event handling.
/// </summary>
namespace GameEvents
{
    // Player Events
    /// <summary>Event raised when player collects a coin.</summary>
    public struct PlayerCoinCollected
    {
        public int CoinValue;
    }

    /// <summary>Event raised when player takes damage.</summary>
    public struct PlayerDamaged
    {
        public int Damage;
        public int RemainingHealth;
    }

    /// <summary>Event raised when player is healed.</summary>
    public struct PlayerHealed
    {
        public int HealAmount;
        public int CurrentHealth;
    }

    /// <summary>Event raised when player's max health increases.</summary>
    public struct PlayerHeartAdded
    {
        public int HeartsAdded;
        public int NewMaxHealth;
    }

    /// <summary>Event raised when player dies.</summary>
    public struct PlayerDied { }

    /// <summary>Event raised when player performs a dash.</summary>
    public struct PlayerDashed { }

    /// <summary>Event raised when player is instantiated.</summary>
    public struct PlayerInstantiated
    {
        public PlayerScript Player;
    }

    // Enemy Events
    /// <summary>Event raised when an enemy takes damage.</summary>
    public struct EnemyDamaged
    {
        public int Damage;
        public GameObject Enemy;
    }

    // Game State Events
    /// <summary>Event raised when the game is restarted.</summary>
    public struct GameRestarted { }

    /// <summary>Event raised when main game scene is loaded.</summary>
    public struct MainGameSceneLoaded { }

    /// <summary>Event raised when shop scene is loaded.</summary>
    public struct ShopSceneLoaded { }

    /// <summary>Event raised when main menu scene is loaded.</summary>
    public struct MainMenuSceneLoaded { }

    /// <summary>Event raised when any scene is changed.</summary>
    public struct SceneChanged
    {
        public string PreviousScene;
        public string NewScene;
    }

    // Shop Events
    /// <summary>Event raised when player purchases an item in the shop.</summary>
    public struct ItemPurchased
    {
        public string ItemName;
        public int Price;
    }

    /// <summary>Event raised when player equips an item.</summary>
    public struct ItemEquipped
    {
        public string ItemName;
    }
}
