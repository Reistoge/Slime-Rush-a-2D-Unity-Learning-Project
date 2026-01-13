using System.ComponentModel;
using Unity.Collections;
using UnityEngine;
using ReadOnlyAttribute = Unity.Collections.ReadOnlyAttribute;

// ScriptableObject template
[CreateAssetMenu(fileName = "InstantiateBoundariesConfig", menuName = "ScriptableObjects/InstantiateBoundariesConfig", order = 1)]

/// <summary> ScriptableObject that store fixed values for the dangerZoneLevel strategies </summary>
public class InstantiateBoundariesConfig : ScriptableObject
{
    public float increaseSpeed = 1.2f;

    public float boundarieVerticalSize = 640f;

    public float nextHorizontal = 0f;
    public float heightOffset = 320f;

    public float boundVerticalSizeOffset = -70f;

    [SerializeField,ReadOnly]   float adjustedBoundVerticalSize;
    public float AdjustedBoundVerticalSize
    {
        get => adjustedBoundVerticalSize;
    }

    public float boundarieInstantiationSeparationRatio = 0.5f;



    [SerializeField,ReadOnly] float boundarieInstantiationSeparationOffset;
    public float BoundarieInstantiationSeparationOffset
    {
        get => boundarieInstantiationSeparationOffset;
    }


    private void OnValidate()
    {
        adjustedBoundVerticalSize = boundarieVerticalSize + boundVerticalSizeOffset;
        boundarieInstantiationSeparationOffset = adjustedBoundVerticalSize * boundarieInstantiationSeparationRatio;

    }
    private void Reset()
    {
        adjustedBoundVerticalSize = boundarieVerticalSize + boundVerticalSizeOffset;
        boundarieInstantiationSeparationOffset = adjustedBoundVerticalSize * boundarieInstantiationSeparationRatio;

    }




    public float getBoundInitHeight(float height)
    {
        return heightOffset + height;
    }
}