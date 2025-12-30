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

    private List<Action> actionsAfterCentering = new List<Action>();
    private List<Action> actionsBeforeCentering = new List<Action>();
    private List<Action<Transform>> actionsBeforeCenteringTarget = new List<Action<Transform>>();
    private List<Action<Transform>> actionsAfterCenteringTarget = new List<Action<Transform>>();

    /// <summary>Actions to execute after an object is centered (no parameters).</summary>
    public List<Action> ActionsAfterCentering { get => actionsAfterCentering; set => actionsAfterCentering = value; }
    /// <summary>Actions to execute before an object is centered (no paramaters) </summary>
    public List<Action> ActionsBeforeCentering { get => actionsBeforeCentering; set => actionsBeforeCentering = value; }

    /// <summary>Actions to execute before centering begins (receives target transform).</summary>
    public List<Action<Transform>> ActionsBeforeCenteringTarget { get => actionsBeforeCenteringTarget; set => actionsBeforeCenteringTarget = value; }

    /// <summary>Actions to execute after centering completes (receives target transform).</summary>
    public List<Action<Transform>> ActionsAfterCenteringTarget { get => actionsAfterCenteringTarget; set => actionsAfterCenteringTarget = value; }
    public float CenterObjectDuration { get => centerObjectDuration; set => centerObjectDuration = value; }

    private void Start()
    {
        colTrigger.EvenTriggerCollider.AddListener(centerObject);
    }

    /// <summary>
    /// Centers an object that enters the portal.
    /// </summary>

    public void centerObject(Collider2D collision)
    {
        portalCoroutine = StartCoroutine(lerpPositionAndRotation(collision.gameObject.transform, portalCenter, centerObjectDuration));
    }

    /// <summary>
    /// Smoothly interpolates an object's position and rotation to the portal center.
    /// </summary>
    private IEnumerator lerpPositionAndRotation(Transform target, Transform center, float duration)
    {

        actionsBeforeCentering?.ForEach((a) => a?.Invoke());
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

        actionsAfterCentering?.ForEach((Action a) => a?.Invoke());
        actionsAfterCenteringTarget?.ForEach((Action<Transform> a) => a?.Invoke(target));

    }
}
