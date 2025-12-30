using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

/// <summary>
/// Manages the generation and placement of level segments in the game.
/// Responsible for spawning start levels, normal levels, hard levels, and backgrounds.
/// </summary>
public class LevelObjectManager : MonoBehaviour
{
    #region Serialized Fields

    [Header("Primary Levels")]
    [Tooltip("Levels used at the start of the game")]
    [SerializeField] private GameObject[] startLevels;
    [Tooltip("Standard difficulty levels")]
    [SerializeField] private GameObject[] normalLevels;
    [Tooltip("High difficulty levels")]
    [SerializeField] private GameObject[] hardLevels;
    [Tooltip("Random backgrounds for visual variety")]
    [SerializeField] private GameObject[] backgrounds;

    [Header("Danger Zone (Legacy)")]
    [Tooltip("Container for danger zone boundaries")]
    [SerializeField] private GameObject dangerZoneBoundaries;
    [SerializeField] private GameObject dangerZonePrefab;
    [SerializeField] private GameObject Lava;
    [SerializeField] private GameObject platform;
    [SerializeField] private GameObject cannon;

    #endregion

    #region Private Fields

    /// <summary>Currently active level GameObject</summary>
    private GameObject currentLevel;
    
    /// <summary>Currently active background GameObject</summary>
    private GameObject currentBg;

    #endregion

    #region Unity Lifecycle

    /// <summary>
    /// Initializes the level by generating the initial set of levels.
    /// </summary>
    void Start()
    {
        GenerateInitialLevels();
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Generates the initial set of levels with increasing difficulty.
    /// Creates a start level at Y=0, a normal level at Y=640, and a hard level at Y=1920.
    /// </summary>
    public void GenerateInitialLevels()
    {
        Transform parent = transform;

        // Instantiate a random start level at origin
        GameObject startLevelPrefab = startLevels[Random.Range(0, startLevels.Length)];
        currentLevel = Instantiate(startLevelPrefab, Vector3.zero, Quaternion.identity, parent);
        currentBg = Instantiate(GetRandomBackground(), currentLevel.transform);

        // Instantiate a random normal level at 640 units height
        GameObject normalLevelPrefab = normalLevels[Random.Range(0, normalLevels.Length)];
        Vector3 normalLevelPos = new Vector3(0, 640, 0);
        Instantiate(normalLevelPrefab, normalLevelPos, Quaternion.identity, parent);

        // Instantiate a random hard level at 1920 units height
        GameObject hardLevelPrefab = hardLevels[Random.Range(0, hardLevels.Length)];
        Vector3 hardLevelPos = new Vector3(0, 1920, 0);
        Instantiate(hardLevelPrefab, hardLevelPos, Quaternion.identity, parent);
    }

    /// <summary>
    /// Generates a danger zone level at the default position.
    /// Note: This is a legacy method that may be deprecated.
    /// </summary>
    public void GenerateRandomDangerZoneLevel()
    {
        Instantiate(dangerZonePrefab);
    }

    /// <summary>
    /// Generates a danger zone level at a specified transform with an offset.
    /// </summary>
    /// <param name="transform">The transform to use as the base position</param>
    /// <param name="offset">The Y offset to apply to the position</param>
    public void GenerateRandomDangerZoneLevel(Transform transform, float offset)
    {
        Vector2 position = new Vector2(transform.position.x, transform.position.y + offset);
        Instantiate(dangerZonePrefab, position, Quaternion.identity);
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Gets a random background GameObject from the backgrounds array.
    /// </summary>
    /// <returns>A random background GameObject</returns>
    private GameObject GetRandomBackground()
    {
        return backgrounds[Random.Range(0, backgrounds.Length)];
    }

    #endregion

    #region Enums

    /// <summary>
    /// Defines the types of danger zones (legacy enum, may be moved to DangerZoneLevelManager).
    /// </summary>
    public enum DangerZoneType
    {
        /// <summary>Platform-based danger zone</summary>
        platforms,
        
        /// <summary>Cannon-based danger zone</summary>
        cannons
    }

    #endregion
}
 
