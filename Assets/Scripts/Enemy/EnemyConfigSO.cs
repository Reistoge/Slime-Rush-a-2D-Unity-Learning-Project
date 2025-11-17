using UnityEngine;

/// <summary>
/// ScriptableObject for configuring enemy properties.
/// Allows designers to create different enemy variants without code changes.
/// </summary>
[CreateAssetMenu(fileName = "EnemyConfig", menuName = "ScriptableObjects/Enemy Config", order = 2)]
public class EnemyConfigSO : ScriptableObject
{
    [Header("Basic Stats")]
    [Tooltip("Display name for this enemy type")]
    public string enemyName = "Enemy";

    [Tooltip("Amount of damage this enemy deals")]
    [Range(1, 10)]
    public int damage = 1;

    [Tooltip("Maximum health points")]
    [Range(1, 100)]
    public int maxHealth = 1;

    [Header("Movement")]
    [Tooltip("Base movement speed")]
    public float moveSpeed = 2f;

    [Tooltip("Enemy movement pattern type")]
    public EnemyMovementPattern movementPattern = EnemyMovementPattern.Stationary;

    [Header("Combat")]
    [Tooltip("How the enemy detects players")]
    public EnemyDetectionType detectionType = EnemyDetectionType.Trigger;

    [Tooltip("Detection range for player")]
    public float detectionRange = 5f;

    [Tooltip("Attack cooldown in seconds")]
    public float attackCooldown = 1f;

    [Header("Knockback")]
    [Tooltip("Knockback force applied to player")]
    public float knockbackForce = 10f;

    [Tooltip("Direction of knockback (if fixed)")]
    public Vector2 knockbackDirection = Vector2.up;

    [Tooltip("Whether to use enemy orientation for knockback direction")]
    public bool useOrientationForKnockback = true;

    [Header("Rewards")]
    [Tooltip("Coins dropped when defeated")]
    [Range(0, 100)]
    public int coinsDropped = 5;

    [Tooltip("Whether this enemy drops loot")]
    public bool dropsLoot = true;
}

/// <summary>
/// Defines different enemy movement patterns.
/// </summary>
public enum EnemyMovementPattern
{
    Stationary,
    Patrol,
    Chase,
    Circular,
    Flying
}

/// <summary>
/// Defines how enemies detect the player.
/// </summary>
public enum EnemyDetectionType
{
    Trigger,
    Collision,
    Raycast,
    Distance
}
