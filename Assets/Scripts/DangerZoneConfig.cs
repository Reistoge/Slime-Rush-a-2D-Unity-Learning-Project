using System;
using UnityEditor;
using UnityEngine;

// ScriptableObject template
[CreateAssetMenu(fileName = "DZC", menuName = "ScriptableObjects/DangerZoneConfig", order = 1)]
public class DangerZoneConfig : ScriptableObject
{


    public Platform platformClassic;
    public Platform platformLarge;
    public Platform platformDissolve;
    public Floor floorClassic;
    public Floor floorLarge;
    public Floor floorSpikes;
    public Floor horizontalMoveFloor;
    public Floor breakableFloor;

    public Coin coinsPrefabA;
    public Coin coinsPrefabB;
    public Cannon cannonPrefab;
    public Boundaries boundarie;
    public Boundaries finalBoundarie;
  


    public int maxPlatformsInBound = 6;

    public float waitTimeToStartLevel = 2f;
    public float HORIZONTAL_EDGE_LIMIT = 180f;
    public float PLAYER_JUMP = 120f;




    [Serializable]
    public class Boundaries
    {
        public GameObject prefab;
    }

    [Serializable]
    public class Platform
    {
        public GameObject prefab;
        public float width;
        public float height;
        public float spawnRate;
        [Range(0, 1)] public float horizontalRandomness;

    }
    [Serializable]
    public class Floor
    {
        public GameObject prefab;
        public float width;
        public float height;
        public float spawnRate;
        [Range(0, 1)] public float horizontalRandomness;

    }
    [Serializable]
    public class Coin
    {
        public GameObject prefab;
        public float spawnRate;
    }
    [Serializable]
    public class Cannon
    {
        public GameObject prefab;
        public float spawnRate;

    }





}