using UnityEditor;
using UnityEngine;
[CreateAssetMenu(fileName = "loadScene", menuName = "ScriptableObjects/LoadSceneVariant", order = 1)]

public class LoadSceneWithTransitionSO : ScriptableObject
{
    
    public SceneAsset sceneAsset;
    public float secondsDelay = 2f;
    public GameObject transitionPrefab;

}