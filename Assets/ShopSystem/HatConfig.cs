using UnityEngine;

// ScriptableObject template
[CreateAssetMenu(fileName = "Hat asset", menuName = "ScriptableObjects/hat", order = 1)]
public class HatConfig : ScriptableObject
{
    public Sprite hatIcon;
    public GameObject associatedPrefab;
    public bool purchased;

    public int price;

}