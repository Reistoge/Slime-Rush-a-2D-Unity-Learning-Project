using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using Vector2 = UnityEngine.Vector2;
using UnityEngine.Events;
 

/// <summary>
/// Centralized input management system handling keyboard, touch, and controller input.
/// Singleton that provides unified input access for all control schemes.
/// </summary>
[DefaultExecutionOrder(-100)]
public class InputManager : GenericSingleton<InputManager>
{
    #region Events
    /// <summary>Delegate for touch start events.</summary>
    public delegate void StartTouch(Vector2 position, float time);
    /// <summary>Event fired when a touch begins.</summary>
    public event StartTouch OnStartTouch;

    /// <summary>Delegate for touch end events.</summary>
    public delegate void EndTouch(Vector2 position, float time);
    /// <summary>Event fired when a touch ends.</summary>
    public event StartTouch OnEndTouch;

    [SerializeField] private GameObject pad;

    /// <summary>Static event for left side touch.</summary>
    public static Action OnTouchLeft;
    /// <summary>Static event for right side touch.</summary>
    public static Action OnTouchRight;
    /// <summary>Static event for center touch.</summary>
    public static Action OnTouchCenter;

    [SerializeField] private UnityEvent OnTouchLeftEvent;
    [SerializeField] private UnityEvent OnTouchRightEvent;
    [SerializeField] private UnityEvent OnTouchCenterEvent;

    [SerializeField] private float horizontalAxisRaw;
    [SerializeField] private float horizontalAxis;
    [SerializeField] private bool touchingPad;
    #endregion

    private PlayerControls playerControls;

    [SerializeField] private float timeElapsed;
    [SerializeField] private float duration;
    [SerializeField] private int maxSpeed = 1;
    [SerializeField] private AnimationCurve speedCurve;
    [SerializeField] private float speed;

    /// <summary>Gets or sets the raw horizontal input (-1, 0, or 1).</summary>
    public float HorizontalAxisRaw { get => horizontalAxisRaw; set => horizontalAxisRaw = value; }

    /// <summary>Gets or sets the smoothed horizontal input with acceleration.</summary>
    public float HorizontalAxis { get => horizontalAxis; set => horizontalAxis = value; }

    new void Awake()
    {
        base.Awake();
        playerControls = new PlayerControls();


    }
    private void OnEnable()
    {
        if (playerControls != null) playerControls.Enable();

    }

    private void OnDisable()
    {
        if (playerControls != null) playerControls.Disable();

    }


    private void Start()
    {
        playerControls.Touch.PrimaryContact.started += ctx => StartTouchPrimary(ctx);
        playerControls.Touch.PrimaryContact.canceled += ctx => EndTouchPrimary(ctx);
    }

    private void Update()
    {
        getHorizontalInputRaw();
        getHorizontalInput();
    }

    /// <summary>
    /// Applies acceleration curve to horizontal input for smooth movement.
    /// </summary>
    private void getHorizontalInput()
    {
       
        if (horizontalAxisRaw != 0)
        {
            timeElapsed += Time.deltaTime;
            float normalizedTime = Mathf.Clamp01(timeElapsed / duration);
            speed = speedCurve.Evaluate(normalizedTime) * maxSpeed;
            horizontalAxis = speed * horizontalAxisRaw;
        }
        else
        {
            timeElapsed = 0;
            horizontalAxis = horizontalAxisRaw;
        }
    }

    /// <summary>
    /// Gets raw horizontal input from keyboard.
    /// Skips if currently touching virtual pad.
    /// </summary>
    public void getHorizontalInputRaw()
    {
        if (touchingPad == false)
        {
            horizontalAxisRaw = Input.GetAxisRaw("Horizontal");
        }
    }

    /// <summary>
    /// Handles the start of a touch input.
    /// </summary>
    /// <param name="ctx">Input action callback context</param>
    private void StartTouchPrimary(InputAction.CallbackContext ctx)
    {
        
        if (Camera.main == null) return;

        Vector2 touchPosition = playerControls.Touch.PrimaryPosition.ReadValue<Vector2>();

        if (IsPointerOverUIObject())
        {
            print("pointer is over UI object");
            touchingPad = checkIfPadTouched();

            if (touchingPad)
            {
                CheckScreenSection(touchPosition);
                
                return;
            }
        }
        if (OnStartTouch != null)
        {
            
            OnStartTouch(Utils.ScreenToWorld(Camera.main, playerControls.Touch.PrimaryPosition.ReadValue<Vector2>()), (float)ctx.startTime);
        }
        // CheckScreenSection(touchPosition);

 
    }

    /// <summary>
    /// Handles the end of a touch input.
    /// </summary>
    /// <param name="ctx">Input action callback context</param>
    private void EndTouchPrimary(InputAction.CallbackContext ctx)
    {
        if (Camera.main == null) return;

        if (checkIfPadTouched())
        {
            touchingPad = false;
        }

        if (OnEndTouch != null)
        {
            Vector2 touchPosition = playerControls.Touch.PrimaryPosition.ReadValue<Vector2>();
            OnEndTouch(Utils.ScreenToWorld(Camera.main, touchPosition), (float)ctx.time);
        }

        horizontalAxisRaw = 0;
    }

    /// <summary>
    /// Gets the current primary touch position in world space.
    /// </summary>
    /// <returns>Touch position in world coordinates</returns>
    public Vector2 PrimaryPosition()
    {
        if (Camera.main)
        {
            return Utils.ScreenToWorld(Camera.main, playerControls.Touch.PrimaryPosition.ReadValue<Vector2>());
        }
        return Vector2.zero;
    }

    /// <summary>
    /// Checks which section of the screen was touched (left, center, or right).
    /// </summary>
    /// <param name="touchPosition">Screen position of the touch</param>
    private void CheckScreenSection(Vector2 touchPosition)
    {
        float screenWidth = Screen.width;
        float sectionWidth = screenWidth / 3;

        if (touchPosition.x < sectionWidth)
        {
            OnTouchLeft?.Invoke();
            OnTouchLeftEvent?.Invoke();
            horizontalAxisRaw = -1;
        }
        else if (touchPosition.x < sectionWidth * 2)
        {
            OnTouchCenter?.Invoke();
            OnTouchCenterEvent?.Invoke();
            horizontalAxisRaw = 0;
        }
        else
        {
            OnTouchRight?.Invoke();
            OnTouchRightEvent?.Invoke();
            horizontalAxisRaw = 1;
        }
    }

    /// <summary>
    /// Checks if the pointer is over any UI element.
    /// </summary>
    /// <returns>True if over UI, false otherwise</returns>
    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(UnityEngine.EventSystems.EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        UnityEngine.EventSystems.EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        return results.Count > 0;
    }

    /// <summary>
    /// Checks if the virtual control pad was touched.
    /// </summary>
    /// <returns>True if pad was touched, false otherwise</returns>
    private bool checkIfPadTouched()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(UnityEngine.EventSystems.EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();

        UnityEngine.EventSystems.EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.name == "Pad")
            {
                pad = result.gameObject;
                touchingPad = true;
                return true;
            }
        }

        pad = null;
        return false;
    }
}
