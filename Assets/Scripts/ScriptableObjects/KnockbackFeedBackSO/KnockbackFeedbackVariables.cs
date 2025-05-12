using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "KnockbackFeedBackVariables", menuName = "ScriptableObjects/KnockbackFeedBackSO", order = 1)]
public class KnockbackFeedBackVariables : ScriptableObject
{
    
    public UnityEvent OnBegin, OnDone;
    [Header("Feedback Settings")]
    public float horizontalStrength = 10f;
    public float verticalStrength = 10f;
    public float delay = 0.5f;
    public float gravityMultiplier = 0.5f;

    [Header("Clamps")]
    public float minHorizontalForce = 5f;
    public float maxVerticalForce = 15f;
}