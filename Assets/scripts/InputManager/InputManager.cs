 
using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;
[DefaultExecutionOrder(-1)]
public class InputManager : GenericSingleton<InputManager>
{
    #region Events
    public delegate void StartTouch(Vector2 position, float time);
    public event StartTouch OnStartTouch;
    public delegate void EndTouch(Vector2 position, float time);
    public event StartTouch OnEndTouch;
    private Camera mainCamera;

    #endregion


    private PlayerControls playerControls;
    private void Awake()
    {
        playerControls = new PlayerControls();
        mainCamera = Camera.main;
    }
    private void OnEnable()
    {
        playerControls.Enable();
    }
    private void OnDisable()
    {
        playerControls.Disable();
    }


    void Start()
    {
        playerControls.Touch.PrimaryContact.started += ctx => StartTouchPrimary(ctx);
        playerControls.Touch.PrimaryContact.canceled += ctx => EndTouchPrimary(ctx);
    }
    void StartTouchPrimary(InputAction.CallbackContext ctx)
    {
        if(OnStartTouch!=null) OnStartTouch(Utils.ScreenToWorld(mainCamera,playerControls.Touch.PrimaryPosition.ReadValue<Vector2>()),(float )ctx.startTime );

  

    }
      void EndTouchPrimary(InputAction.CallbackContext ctx)
    {
        if(OnEndTouch!=null) OnEndTouch(Utils.ScreenToWorld(mainCamera,playerControls.Touch.PrimaryPosition.ReadValue<Vector2>()),(float )ctx.time );

  

    }
    public Vector2 PrimaryPosition(){
        return Utils.ScreenToWorld(mainCamera, playerControls.Touch.PrimaryPosition.ReadValue<Vector2>());

    }   


   





}
