using UnityEngine;

/// <summary>
/// ScriptableObject for configuring cannon launch properties.
/// Allows level designers to create different cannon behaviors without code.
/// </summary>
[CreateAssetMenu(fileName = "CannonConfig", menuName = "ScriptableObjects/Cannon Config", order = 3)]
public class CannonConfigSO : ScriptableObject
{
    [Header("Launch Settings")]
    [Tooltip("Launch speed in units per second")]
    [Range(0f, 1000f)]
    public float dashSpeed = 50f;

    [Tooltip("Duration of the dash in seconds")]
    [Range(0f, 10f)]
    public float dashTime = 1f;

    [Tooltip("Delay before rotating to target angle")]
    [Range(0f, 5f)]
    public float rotationDelay = 0.5f;

    [Header("Rotation")]
    [Tooltip("Speed of rotation in degrees per second")]
    [Range(0f, 360f)]
    public float rotationSpeed = 90f;

    [Tooltip("Target angle for cannon rotation")]
    [Range(-180f, 180f)]
    public float targetAngle = 0f;

    [Tooltip("Whether the cannon rotates automatically")]
    public bool autoRotate = false;

    [Header("Player Control")]
    [Tooltip("Whether player can move during launch")]
    public bool canMoveInShoot = true;

    [Tooltip("Horizontal movement threshold (0 = no horizontal movement)")]
    [Range(0f, 1f)]
    public float horizontalThreshold = 0.5f;

    [Header("Auto-Fire")]
    [Tooltip("Whether cannon fires automatically")]
    public bool isAutoShoot = false;

    [Tooltip("Delay before auto-firing in seconds")]
    [Range(0f, 10f)]
    public float autoShootDelay = 2f;

    [Header("Cannon Type")]
    [Tooltip("Is this the first cannon in the level?")]
    public bool isFirst = false;

    [Tooltip("Is this the final cannon that completes the level?")]
    public bool isFinal = false;

    [Header("Visual Effects")]
    [Tooltip("Particle effect prefab to spawn on launch")]
    public GameObject launchEffectPrefab;

    [Tooltip("Sound to play on launch")]
    public AudioClip launchSound;

    [Tooltip("Camera shake intensity on launch")]
    [Range(0f, 1f)]
    public float cameraShakeIntensity = 0.5f;
}
