using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Portal : MonoBehaviour
{

    Coroutine portalCoroutine;
    [SerializeField] Transform portalCenter;
    [SerializeField] float centerObjectDuration;
    [SerializeField] ColliderTriggerEvent colTrigger;
    List<Action> simpleActionsAfterCentering = new List<Action>();
    List<Action<Transform>> actionsBeforeCenteringTarget = new List<Action<Transform>>();
    List<Action<Transform>> actionsAfterCenteringTarget = new List<Action<Transform>>();

    public List<Action> SimpleActionsAfterCentering { get => simpleActionsAfterCentering; set => simpleActionsAfterCentering = value; }
    public List<Action<Transform>> ActionsBeforeCenteringTarget { get => actionsBeforeCenteringTarget; set => actionsBeforeCenteringTarget = value; }
    public List<Action<Transform>> ActionsAfterCenteringTarget { get => actionsAfterCenteringTarget; set => actionsAfterCenteringTarget = value; }

    void Start()
    {

        colTrigger.EvenTriggerCollider.AddListener(centerObject);
        

    }



    // public void changeScene(string args)
    // {
    //     GameManager.Instance.loadSceneWithTransition(args);
    // }
    // public void changeScene(LoadSceneWithTransitionSO so)
    // {
    //     GameManager.Instance.loadSceneWithTransition(so);
    // }
    public void centerObject(Collider2D collision)
    {
        portalCoroutine = StartCoroutine(lerpPositionAndRotation(collision.gameObject.transform, portalCenter, centerObjectDuration));
    }
    

    IEnumerator lerpPositionAndRotation(Transform target, Transform center, float duration)
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
