using UnityEngine;

/// <summary>
/// ScriptableObject that stores player configuration data.
/// Used to define different player variants with varying stats and physics.
/// </summary>
[CreateAssetMenu(fileName = "PlayerVariant", menuName = "ScriptableObjects/Player", order = 1)]
public class PlayerSO : ScriptableObject
{
    [Header("Movement Settings")]
    [Tooltip("Speed when moving on ground")]
    public float groundspeed;

    [Tooltip("Speed when moving in air")]
    public float inAirSpeed;

    [Tooltip("Multiplier for horizontal input sensitivity")]
    public float horizontalThreshold;

    [Range(0, 1000f)]
    [Tooltip("Maximum horizontal velocity the player can reach")]
    public float maxHorizontalVelocity;

    [Range(0, 10000f)]
    [Tooltip("Maximum upward velocity")]
    public float maxUpVelocity;

    [Range(0, 10000f)]
    [Tooltip("Maximum downward velocity")]
    public float maxDownVelocity;

    #region Player Stats
    [Header("Player Stats")]
    [Tooltip("Maximum health points")]
    public int maxHp;

    [Tooltip("Starting health when spawning")]
    public int startHp;

    [Tooltip("Damage dealt to enemies")]
    public int playerDamage;

    [Tooltip("Total coins collected across all sessions")]
    public int totalCoins;

    [Tooltip("Coins the player starts with in a session")]
    public int startCoins;

    [Tooltip("Player's score")]
    public float score;
    #endregion

    #region Physics Settings
    [Header("Physics Settings")]
    [Tooltip("Angular velocity when falling")]
    public float angularFallingVelocity;

    [Tooltip("Angular velocity when jumping")]
    public float angularJumpVelocity;

    [Tooltip("Initial gravity scale")]
    public float initialGravity;

    [Tooltip("Initial linear drag")]
    public float initialDrag;

    [Tooltip("Rigidbody mass")]
    public float mass;

    [Tooltip("Linear drag coefficient")]
    public float linearDrag;

    [Tooltip("Gravity scale applied to rigidbody")]
    public float gravity;

    [Tooltip("Whether to enforce max velocities")]
    public bool handleVelocities;
    #endregion

    [Header("Dash Settings")]
    [Tooltip("Maximum number of dashes available")]
    public float maxDashCounter;

    [Tooltip("Velocity multiplier at end of dash (0-1)")]
    public float dashEndVel = 0.3f;

    [Tooltip("Force applied during dash")]
    public float dashForce;

    [Tooltip("Duration of dash in seconds")]
    public float dashTime;

    [Header("Jump Settings")]
    [Tooltip("Base jump force (7 for mobile, 4 for PC)")]
    public float jumpForce = 7;

    [Tooltip("Minimum jump force")]
    public float minJumpForce = 10f;

    [Tooltip("Maximum jump force")]
    public float maxJumpForce = 30f;

    [Tooltip("Swipe threshold for mobile jump input")]
    public Vector2 jumpThreshold;

    [Tooltip("Horizontal input threshold for jump direction (PC)")]
    public float horizontalJumpDirectionThreshold;
}
