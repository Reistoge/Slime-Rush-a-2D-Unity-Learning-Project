

using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Numerics;

using UnityEngine;
using UnityEngine.Events;
using Color = UnityEngine.Color;
using Debug = UnityEngine.Debug;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

[DefaultExecutionOrder(-1)]
public class SwipeDetection : MonoBehaviour
{

    [SerializeField]
    private float minimunDistance;
    [SerializeField]
    private float maximumTime;

    [SerializeField, Range(0f, 1f)]
    private float directionThreshold;

    [SerializeField]
    private GameObject trail;
    [SerializeField] GameObject trailPrefab;


    InputManager inputManager;
    [SerializeField] private Vector2 startPosition;
    private float startTime;
    [SerializeField] Vector2 currentPosition;
    private Vector2 endPosition;
    private float endTime;
    private Coroutine coroutine;
    public UnityEvent OnSwipeLeft, OnSwipeRight, OnSwipeDown, OnSwipeUp;

    public static Action<Vector2, Vector2> OnSwipeLine;
    public static Action OnSwipeStart;
    public static Action OnSwipeEnd;

    public Vector2 StartPosition { get => startPosition; set => startPosition = value; }
    public Vector2 CurrentPosition { get => currentPosition; set => currentPosition = value; }
    public Vector2 EndPosition { get => endPosition; set => endPosition = value; }

    void Awake()
    {

    }
    private void OnEnable()
    {

        inputManager = InputManager.Instance;
        inputManager.OnStartTouch += SwipeStart;
        inputManager.OnEndTouch += SwipeEnd;

    }
    private void OnDisable()
    {
        inputManager.OnStartTouch -= SwipeStart;
        inputManager.OnEndTouch -= SwipeEnd;


    }
    private void SwipeStart(Vector2 position, float time)
    {
        if(trail==null){
            trail = Instantiate(trailPrefab);

        }
        StartPosition = position;
        startTime = time;
        trail.transform.position = position;
        trail.SetActive(true);
        OnSwipeStart?.Invoke();
        coroutine = StartCoroutine(Trail());


    }
    private IEnumerator Trail()
    {

        while (true)
        {


            //GameManager.instance.PlayerInScene

            trail.transform.position = inputManager.PrimaryPosition();
            CurrentPosition=trail.transform.position;
            yield return null;
        }
    }
    private void SwipeEnd(Vector2 position, float time)
    {


        if (trail && trail.activeInHierarchy && coroutine != null)
        {
            trail.SetActive(false);
            StopCoroutine(coroutine);
            EndPosition = position;

            endTime = time;
            OnSwipeEnd?.Invoke();
            DetectSwipe();
        }

    }

    private void DetectSwipe()
    {

        if ((Vector3.Distance(StartPosition, EndPosition) >= minimunDistance) && ((endTime - startTime) <= maximumTime))
        {
            // Debug.Log("Swipe detected");
            // 
            Debug.DrawLine(StartPosition, EndPosition, Color.red, 5f);
            Vector3 direction = EndPosition - StartPosition;
            Vector2 direction2D = new Vector2(direction.x, direction.y).normalized;
            SwipeDirection(direction2D);
            OnSwipeLine?.Invoke(StartPosition, EndPosition);

        }




    }
    private void SwipeDirection(Vector2 direction)
    {

        // this will not check if the directions are in 45 degree angles, it will just check if the direction is in the same quadrant as the swipe

        if (Vector2.Dot(Vector2.up, direction) > directionThreshold)
        {
            //Debug.Log("Swipe up");
            OnSwipeUp?.Invoke();

        }
        else if (Vector2.Dot(Vector2.down, direction) > directionThreshold)
        {
            // Debug.Log("Swipe down");
            OnSwipeDown?.Invoke();

        }
        else if (Vector2.Dot(Vector2.left, direction) > directionThreshold)
        {
            //Debug.Log("Swipe left");
            OnSwipeLeft?.Invoke();

        }
        else if (Vector2.Dot(Vector2.right, direction) > directionThreshold)
        {
            //Debug.Log("Swipe right");
            OnSwipeRight?.Invoke();

        }


    }

}
