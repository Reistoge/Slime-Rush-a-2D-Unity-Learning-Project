using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using Vector2 = UnityEngine.Vector2;
using System.Collections.Generic;
using System;
using UnityEngine.Events;
using System.Security.Cryptography;

[DefaultExecutionOrder(-100)]
public class InputManager : GenericSingleton<InputManager>
{
    #region Events
    public delegate void StartTouch(Vector2 position, float time);
    public event StartTouch OnStartTouch;
    public delegate void EndTouch(Vector2 position, float time);
    public event StartTouch OnEndTouch;

    [SerializeField] GameObject pad;
    public static Action OnTouchLeft;
    public static Action OnTouchRight;
    public static Action OnTouchCenter;

    [SerializeField] UnityEvent OnTouchLeftEvent;
    [SerializeField] UnityEvent OnTouchRightEvent;
    [SerializeField] UnityEvent OnTouchCenterEvent;


    [SerializeField] float horizontalAxisRaw;
    [SerializeField] float horizontalAxis;
    [SerializeField] bool touchingPad;




    private Camera mainCamera;

    #endregion


    private PlayerControls playerControls;

    [SerializeField] float timeElapsed;
    [SerializeField] float duration;
    [SerializeField] int maxSpeed = 1;
    [SerializeField] AnimationCurve speedCurve;
    [SerializeField] float speed;

    public float HorizontalAxisRaw { get => horizontalAxisRaw; set => horizontalAxisRaw = value; }
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


    void Start()
    {

        playerControls.Touch.PrimaryContact.started += ctx => StartTouchPrimary(ctx);
        playerControls.Touch.PrimaryContact.canceled += ctx => EndTouchPrimary(ctx);


    }
 
    void Update()
    {

        getHorizontalInputRaw();
        getHorizontalInput();
        //horizontalAxis = Input.GetAxisRaw("Horizontal");
    }
    void getHorizontalInput()
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
    public void getHorizontalInputRaw()
    {
        if (touchingPad == false)
        {
            horizontalAxisRaw = Input.GetAxisRaw("Horizontal");
   

        }
 
    }
    void StartTouchPrimary(InputAction.CallbackContext ctx)
    {
        if (Camera.main == null) return;
        Vector2 touchPosition = playerControls.Touch.PrimaryPosition.ReadValue<Vector2>();
        if (IsPointerOverUIObject())
        {
            touchingPad = checkIfPadTouched();
            if (touchingPad)
            {
                CheckScreenSection(touchPosition);
                return;
            }



        }
        if (OnStartTouch != null) OnStartTouch(Utils.ScreenToWorld(Camera.main, playerControls.Touch.PrimaryPosition.ReadValue<Vector2>()), (float)ctx.startTime);

        // CheckScreenSection(touchPosition);

    }
    void EndTouchPrimary(InputAction.CallbackContext ctx)
    {
        if (Camera.main == null) return;
        Vector2 touchPosition = playerControls.Touch.PrimaryPosition.ReadValue<Vector2>();
        if (checkIfPadTouched())
        {
            touchingPad = false;
        }
        if (OnEndTouch != null)
        {
            OnEndTouch(Utils.ScreenToWorld(Camera.main, playerControls.Touch.PrimaryPosition.ReadValue<Vector2>()), (float)ctx.time);
        }
        horizontalAxisRaw = 0;




    }

    public Vector2 PrimaryPosition()
    {
        if (Camera.main)
        {
            return Utils.ScreenToWorld(Camera.main, playerControls.Touch.PrimaryPosition.ReadValue<Vector2>());
        }
        return Vector2.zero;

    }
    private void CheckScreenSection(Vector2 touchPosition)
    {

        float screenWidth = Screen.width;
        float sectionWidth = screenWidth / 3;

        if (touchPosition.x < sectionWidth)
        {
            Debug.Log("Left section pressed");
            OnTouchLeft?.Invoke();
            OnTouchLeftEvent?.Invoke();
            horizontalAxisRaw = -1;
        }
        else if (touchPosition.x < sectionWidth * 2)
        {
            Debug.Log("Center section pressed");
            OnTouchCenter?.Invoke();
            OnTouchCenterEvent?.Invoke();
            horizontalAxisRaw = 0;
        }
        else if (touchPosition.x < sectionWidth * 3)
        {
            Debug.Log("Right section pressed");
            OnTouchRight?.Invoke();
            OnTouchRightEvent?.Invoke();
            horizontalAxisRaw = 1;
        }
        // else
        // {
        //     GameManager.instance.MovXButtons = 0;
        // }



    }
    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        return results.Count > 0;

    }
    private bool checkIfPadTouched()
    {
        // pad check
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();

        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        foreach (RaycastResult result in results)
        {
            print(result.gameObject.name);
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
