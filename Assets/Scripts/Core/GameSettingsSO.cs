using UnityEngine;

/// <summary>
/// Central configuration for game-wide settings.
/// Contains all adjustable parameters for game balance and difficulty.
/// </summary>
[CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObjects/Game Settings", order = 0)]
public class GameSettingsSO : ScriptableObject
{
    [Header("Game Balance")]
    [Tooltip("Starting coins for new players")]
    public int startingCoins = 0;

    [Tooltip("Starting health for new players")]
    [Range(1, 10)]
    public int startingHealth = 3;

    [Tooltip("Cost multiplier for shop items")]
    [Range(0.1f, 5f)]
    public float shopPriceMultiplier = 1f;

    [Header("Physics")]
    [Tooltip("Global gravity multiplier")]
    [Range(0.1f, 5f)]
    public float gravityMultiplier = 1f;

    [Tooltip("Time scale for slow motion effects")]
    [Range(0.1f, 1f)]
    public float slowMotionTimeScale = 0.5f;

    [Header("Camera")]
    [Tooltip("Camera follow smoothness")]
    [Range(0f, 1f)]
    public float cameraSmoothing = 0.1f;

    [Tooltip("Camera shake duration multiplier")]
    [Range(0f, 2f)]
    public float cameraShakeDurationMultiplier = 1f;

    [Tooltip("Camera shake intensity multiplier")]
    [Range(0f, 2f)]
    public float cameraShakeIntensityMultiplier = 1f;

    [Header("Difficulty Scaling")]
    [Tooltip("Enable dynamic difficulty adjustment")]
    public bool dynamicDifficulty = false;

    [Tooltip("Damage multiplier for enemies")]
    [Range(0.1f, 5f)]
    public float enemyDamageMultiplier = 1f;

    [Tooltip("Speed multiplier for enemies")]
    [Range(0.1f, 5f)]
    public float enemySpeedMultiplier = 1f;

    [Header("Visual Effects")]
    [Tooltip("Enable particle effects")]
    public bool enableParticles = true;

    [Tooltip("Enable screen shake")]
    public bool enableScreenShake = true;

    [Tooltip("Enable ghost trail effect")]
    public bool enableGhostTrail = true;

    [Tooltip("Ghost trail spawn rate")]
    [Range(0.01f, 0.5f)]
    public float ghostTrailSpawnRate = 0.1f;

    [Header("Audio")]
    [Tooltip("Master volume (0-1)")]
    [Range(0f, 1f)]
    public float masterVolume = 1f;

    [Tooltip("Music volume (0-1)")]
    [Range(0f, 1f)]
    public float musicVolume = 0.7f;

    [Tooltip("SFX volume (0-1)")]
    [Range(0f, 1f)]
    public float sfxVolume = 1f;

    [Header("Debug")]
    [Tooltip("Enable debug mode with additional logging")]
    public bool debugMode = false;

    [Tooltip("Show collision boundaries")]
    public bool showCollisionBounds = false;

    [Tooltip("Enable invincibility for testing")]
    public bool godMode = false;
}
