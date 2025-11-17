using UnityEngine;

/// <summary>
/// Interface for objects that can take damage and die.
/// Implement this for players, enemies, and destructible objects.
/// </summary>
public interface IDamageable
{
    /// <summary>Current health points.</summary>
    int Hp { get; set; }

    /// <summary>Maximum health points.</summary>
    int MaxHp { get; set; }

    /// <summary>Whether this object can currently take damage (e.g., not invincible).</summary>
    bool CanTakeDamage { get; set; }

    /// <summary>
    /// Apply damage to this object.
    /// </summary>
    /// <param name="damage">Amount of damage to apply</param>
    void takeDamage(int damage);

    /// <summary>
    /// Called when this object's health reaches zero.
    /// </summary>
    void die();
}

/// <summary>
/// Interface for enemy behavior patterns.
/// Implement this for different enemy types.
/// </summary>
public interface IEnemyBehaviour
{
    /// <summary>Damage this enemy deals on contact.</summary>
    int Damage { get; set; }

    /// <summary>
    /// Deal damage to a game object.
    /// </summary>
    /// <param name="target">The object to damage</param>
    void dealDamage(GameObject target);
}

/// <summary>
/// Interface for objects that can stick to other objects.
/// Used for sticky surfaces or traps.
/// </summary>
public interface IStickable
{
    /// <summary>The object that is currently stuck.</summary>
    GameObject Sticked { get; set; }

    /// <summary>Duration to keep the object stuck.</summary>
    float TimeStick { get; set; }

    /// <summary>
    /// Stick an object to this surface.
    /// </summary>
    /// <param name="obj">The object to stick</param>
    void stickObject(GameObject obj);

    /// <summary>
    /// Release the stuck object.
    /// </summary>
    void deStickObject();
}

/// <summary>
/// Interface for objects that can rotate.
/// Marker interface for rotatable objects.
/// </summary>
public interface IRotable
{
}

/// <summary>
/// Interface for objects that can break and be repaired.
/// Used for breakable platforms and destructible objects.
/// </summary>
public interface IBreakable
{
    /// <summary>
    /// Break this object, disabling it or changing its state.
    /// </summary>
    void breakObject();

    /// <summary>
    /// Repair this object, restoring it to its original state.
    /// </summary>
    void repairObject();
}

/// <summary>
/// Interface for objects that drop loot when destroyed.
/// Used for enemies and treasure chests.
/// </summary>
public interface ILootable
{
    /// <summary>Amount of coins to drop as loot.</summary>
    int LootCoins { get; set; }

    /// <summary>
    /// Spawn loot items when this object is destroyed.
    /// </summary>
    void throwLoot();
}

/// <summary>
/// Interface for items that can be purchased.
/// Used in shop systems.
/// </summary>
public interface IPurchasable
{
    /// <summary>Cost of this item in coins.</summary>
    int Price { get; }

    /// <summary>
    /// Execute the purchase of this item.
    /// </summary>
    void purchase();
}

