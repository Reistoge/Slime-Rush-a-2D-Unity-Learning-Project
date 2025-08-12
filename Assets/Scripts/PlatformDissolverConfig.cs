using UnityEngine;

// ScriptableObject template
[CreateAssetMenu(fileName = "new DissolverPlatform", menuName = "ScriptableObjects/PlatformDissolverConfig", order = 1)]
public class PlatformDissolverConfig : ScriptableObject
{

    public float timeDissolved;
    public float timeUnDissolved;
    public bool onStart;
    

    
}