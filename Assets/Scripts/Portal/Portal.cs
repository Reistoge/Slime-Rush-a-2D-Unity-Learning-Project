using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Manages portal behavior, centering objects and triggering actions.
/// Handles smooth transitions for objects entering the portal.
/// </summary>
public class Portal : MonoBehaviour
{
    private Coroutine portalCoroutine;

    [SerializeField] private Transform portalCenter;
    [SerializeField] private float centerObjectDuration;
    [SerializeField] private ColliderTriggerEvent colTrigger;

    private List<Action> simpleActionsAfterCentering = new List<Action>();
    private List<Action<Transform>> actionsBeforeCenteringTarget = new List<Action<Transform>>();
    private List<Action<Transform>> actionsAfterCenteringTarget = new List<Action<Transform>>();

    /// <summary>Actions to execute after an object is centered (no parameters).</summary>
    public List<Action> SimpleActionsAfterCentering { get => simpleActionsAfterCentering; set => simpleActionsAfterCentering = value; }

    /// <summary>Actions to execute before centering begins (receives target transform).</summary>
    public List<Action<Transform>> ActionsBeforeCenteringTarget { get => actionsBeforeCenteringTarget; set => actionsBeforeCenteringTarget = value; }

    /// <summary>Actions to execute after centering completes (receives target transform).</summary>
    public List<Action<Transform>> ActionsAfterCenteringTarget { get => actionsAfterCenteringTarget; set => actionsAfterCenteringTarget = value; }

    private void Start()
    {
        colTrigger.EvenTriggerCollider.AddListener(centerObject);
    }

    /// <summary>
    /// Centers an object that enters the portal.
    /// </summary>
    /// <param name="collision">The collider that entered the portal</param>
    public void centerObject(Collider2D collision)
    {
        portalCoroutine = StartCoroutine(lerpPositionAndRotation(collision.gameObject.transform, portalCenter, centerObjectDuration));
    }

    /// <summary>
    /// Smoothly interpolates an object's position and rotation to the portal center.
    /// </summary>
    /// <param name="target">The transform to move</param>
    /// <param name="center">The target center position</param>
    /// <param name="duration">Time in seconds for the interpolation</param>
    private IEnumerator lerpPositionAndRotation(Transform target, Transform center, float duration)
    {
        
        actionsBeforeCenteringTarget?.ForEach((Action<Transform> a) => a?.Invoke(target));
        float timeElapsed = 0;
        Vector3 startPosition = target.position;
        Vector3 endPosition = center.position;
        Quaternion startRotation = target.rotation;
        Quaternion endRotation = center.rotation;

        while (timeElapsed < duration)
        {
            float t = timeElapsed / duration;
            t = t * t * (3f - 2f * t); // Smoothstep
            target.position = Vector3.Lerp(startPosition, endPosition, t);
            target.rotation = Quaternion.Slerp(startRotation, endRotation, t);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        target.position = endPosition;
        target.rotation = endRotation;
        portalCoroutine = null;

        simpleActionsAfterCentering?.ForEach((Action a) => a?.Invoke());
        actionsAfterCenteringTarget?.ForEach((Action<Transform> a) => a?.Invoke(target));

    }
}
