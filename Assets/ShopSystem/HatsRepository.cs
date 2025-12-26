using System.Collections.Generic;
using UnityEngine;

// ScriptableObject template
[CreateAssetMenu(fileName = "Hats", menuName = "ScriptableObjects/Hats", order = 1)]
public class HatsRepository : ScriptableObject
{
    public HatConfig[] hats;
    public int loadedHatIndex;
    public int selectedHatIndex;
    
    
}