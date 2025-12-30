using System;
using System.Collections;
using System.Drawing;
using System.Numerics;
using System.Timers;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEditor.Experimental;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;


public class FollowCamera : MonoBehaviour
{

    [SerializeField] Transform top, bottom,center;
    [SerializeField] GameObject playerReference;
    [SerializeField] shakeType shakeBehaviour;
    [SerializeField] CameraBehaviour selectedBehaviour;
    [SerializeField] float height = 640;
    [SerializeField] float magnitudeShake;
    [SerializeField] float timeShake;
    [SerializeField] SmoothZoom zoomCamera;
    [SerializeField] SmoothZoom unZoomCamera;
    [SerializeField] bool moving;
    [SerializeField] bool isZoom;
    [SerializeField] bool isCenteringCamera;
    [SerializeField] AlwaysRise rise;
    [SerializeField] Coroutine shakeRoutine;
    [SerializeField] Coroutine behaviourRoutine;

 





    float lerpTime = 0;
    [SerializeField] private bool followHorizontal = false;
    [SerializeField] private bool followVertical = true;
    [SerializeField] private float minVerticalPosition = 0;

    private void Start()
    {

        PlayerReference = GameObject.FindGameObjectWithTag("Player");

        if (PlayerReference == null)
        {
            PlayerReference = GameManager.Instance.SelectedPlayer;
            // print("the player was not instantiated ");
        }


    }

    void OnEnable()
    {

        LegacyEvents.GameEvents.onSceneChanged += () => { selectedBehaviour = CameraBehaviour.stop; };
        LegacyEvents.GameEvents.onGameIsRestarted += StopAllCoroutines;

    }
    void OnDisable()
    {
        LegacyEvents.GameEvents.onSceneChanged -= () => { selectedBehaviour = CameraBehaviour.stop; };
        LegacyEvents.GameEvents.onGameIsRestarted -= StopAllCoroutines;
    }


    void LateUpdate()
    {

        processCameraBehaviour();
    }
    private IEnumerator LerpCamera(float startPosY, float height)
    {


        Vector3 startPos, endPos;
        moving = true;
        lerpTime = 0;
        startPos = transform.position;
        startPos.y = startPosY;
        startPos.x = 0;
        endPos = new Vector3(transform.position.x, startPosY + height, -10);


        while (lerpTime < 1)
        {
            transform.position = Vector3.Lerp(startPos, endPos, lerpTime);
            lerpTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        transform.position = endPos;
        moving = false;




    }



    public void lerp()
    {

        StopAllCoroutines();
        StartCoroutine(LerpCamera());


    }
    public void lerp(float startPosY, float height)
    {

        StopAllCoroutines();
        StartCoroutine(LerpCamera(startPosY, height));
    }

    private IEnumerator LerpCamera()
    {

        Vector3 startPos, endPos;
        moving = true;
        lerpTime = 0;
        startPos = transform.position;
        startPos.x = 0;
        endPos = new Vector3(transform.position.x, transform.position.y + height, -10);


        while (lerpTime < 1)
        {
            transform.position = Vector3.Lerp(startPos, endPos, lerpTime);
            lerpTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        transform.position = endPos;
        moving = false;


    }
    private IEnumerator LerpUpCameraInUnscaledTime()
    {

        Vector3 startPos, endPos;
        moving = true;
        lerpTime = 0;
        startPos = transform.position;
        startPos.x = 0;
        endPos = new Vector3(transform.position.x, transform.position.y + height, -10);

        Time.timeScale = 0;
        while (lerpTime < 1)
        {
            transform.position = Vector3.Lerp(startPos, endPos, lerpTime);
            lerpTime += Time.unscaledDeltaTime;
            yield return new WaitForEndOfFrame();
        }
        transform.position = endPos;
        Time.timeScale = 1;
        moving = false;


    }
    private IEnumerator LerpDownCameraInUnscaledTime()
    {

        Vector3 startPos, endPos;
        moving = true;
        lerpTime = 0;
        startPos = transform.position;
        startPos.x = 0;
        endPos = new Vector3(transform.position.x, transform.position.y - height, -10);

        Time.timeScale = 0;
        while (lerpTime < 1)
        {
            transform.position = Vector3.Lerp(startPos, endPos, lerpTime);
            lerpTime += Time.unscaledDeltaTime;
            yield return new WaitForEndOfFrame();
        }
        transform.position = endPos;
        Time.timeScale = 1;
        moving = false;


    }
    public void changeCameraBehaviour(int args)
    {
        StopAllCoroutines();
        selectedBehaviour = (CameraBehaviour)args;
    }

    public void stopCameraBehaviourForSeconds(float seconds)
    {
        if (selectedBehaviour != CameraBehaviour.stop)
        {
            // StartCoroutine(stopCameraBehaviourCoroutine(seconds));
            CameraBehaviour temp = selectedBehaviour;
            StopCoroutine(Rise.alwaysRiseRoutine);
            selectedBehaviour = CameraBehaviour.stop;
            StartCoroutine(GameManager.Instance.enumerateThis(() => { selectedBehaviour = temp; }, seconds));

        }

    }
    public IEnumerator stopCameraBehaviourCoroutine(float seconds)
    {

        CameraBehaviour temp = selectedBehaviour;
        StopCoroutine(Rise.alwaysRiseRoutine);
        selectedBehaviour = CameraBehaviour.stop;
        yield return new WaitForSeconds(seconds);
        selectedBehaviour = temp;
    }

    public void lerp(Transform target)
    {
        lerp(transform.position.y, target.transform.position.y - transform.position.y);
    }

    private IEnumerator alwaysRise(Vector3 startPos, Vector3 endPos)
    {
        float lerpTime = 0;
        moving = true;
        //rise.riseTotalDuration = math.abs(startPos.magnitude - endPos.magnitude) * (1 / rise.speed);
        while ((Vector2)transform.position != (Vector2)(endPos))
        {
            transform.position = Vector3.MoveTowards(new Vector3(startPos.x, startPos.y, -10), new Vector3(endPos.x, endPos.y, -10), lerpTime * Rise.speed);

            lerpTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        transform.position = new Vector3(endPos.x, endPos.y, -10);
        moving = false;


    }

    private IEnumerator Zoom(SmoothZoom zoomCameraVariables)
    {

        // Smoothly follow the player
        GetComponent<PixelPerfectCamera>().enabled = false; // config
        selectedBehaviour = CameraBehaviour.zoomToPlayer;
        float elapsed = 0;

        // add to follow the target.
        while (elapsed < zoomCameraVariables.Duration)
        {

            // float desiredZoom = Mathf.Lerp(zoomCameraVariables.maxZoom, zoomCameraVariables.minZoom,zoomCameraVariables.zoomSpeed);
            // GetComponent<Camera>().orthographicSize = Mathf.Lerp(GetComponent<Camera>().orthographicSize, desiredZoom, Time.deltaTime);
            GetComponent<Camera>().orthographicSize = Mathf.Lerp(GetComponent<Camera>().orthographicSize, zoomCameraVariables.FinalZoom, elapsed * zoomCameraVariables.ZoomSpeed);
            elapsed += Time.deltaTime * zoomCameraVariables.ZoomSpeed;
            yield return new WaitForEndOfFrame();

        }
        GetComponent<Camera>().orthographicSize = zoomCameraVariables.FinalZoom;
        IsZoom = false;


    }
    public void triggerZoom(SmoothZoom smooth)
    {
        StartCoroutine(Zoom(smooth));
    }

    #region "Camera Behaviour"
    public void processCameraBehaviour()
    {
        if (PlayerReference != null)
        {
            switch (CameraType)
            {
                case CameraBehaviour.followCharacter:
                    // this just follow the character

                    float newPosX = playerReference.transform.position.x;
                    float newPosY = playerReference.transform.position.y;
                    Vector3 newPos = new Vector3(0, 0, -10);
                    if (followHorizontal)
                    {
                        newPos.x = newPosX;
                    }
                    if (followVertical)
                    {
                        newPos.y = newPosY;
                    }
                    // Vector3 newpos = new Vector3(PlayerReference.transform.position.x, PlayerReference.transform.position.y, -10);
                    transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime);


                    break;
                case CameraBehaviour.riseWhenReachesHeight:
                    // if the character surpases the limit of a certain point of the camera we are going to lerp the camera up.
                    if ((PlayerReference.transform.position.y) >= (top.transform.position.y) && (moving == false))
                    {
                        if (shakeRoutine != null)
                        {
                            StopCoroutine(shakeRoutine);
                        }
                        StartCoroutine(LerpUpCameraInUnscaledTime());
                    }
                    if ((PlayerReference.transform.position.y) >= (-300) && (PlayerReference.transform.position.y) <= (bottom.transform.position.y) && (moving == false))
                    {
                        if (shakeRoutine != null)
                        {
                            StopCoroutine(shakeRoutine);
                        }
                        StartCoroutine(LerpDownCameraInUnscaledTime());
                    }

                    break;
                case CameraBehaviour.alwaysRise:

                    if (Rise.alwaysRiseRoutine == null)
                    {
                        PlayerReference = GameObject.FindWithTag("Player");
                        Rise.alwaysRiseRoutine = StartCoroutine(alwaysRise(Camera.main.transform.position, new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y + Rise.alwaysRiseEndPos)));

                    }
                    else // the coroutine is running
                    {

                        if (PlayerReference)
                        {
                            float dif = (PlayerReference.transform.position.y - transform.position.y);
                            if (dif <= -Rise.verticalDamp)
                            {

                                StopCoroutine(Rise.alwaysRiseRoutine);
                                Rise.speed = Rise.slowSpeed;
                                Rise.alwaysRiseRoutine = StartCoroutine(alwaysRise(Camera.main.transform.position, new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y + Rise.alwaysRiseEndPos)));


                            }
                            else if (dif > Rise.verticalDamp)
                            {
                                StopCoroutine(Rise.alwaysRiseRoutine);
                                Rise.speed = Rise.fastSpeed;
                                Rise.alwaysRiseRoutine = StartCoroutine(alwaysRise(Camera.main.transform.position, new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y + Rise.alwaysRiseEndPos)));

                            }
                            else
                            {
                                StopCoroutine(Rise.alwaysRiseRoutine);
                                Rise.speed = Rise.normalSpeed;
                                Rise.alwaysRiseRoutine = StartCoroutine(alwaysRise(Camera.main.transform.position, new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y + Rise.alwaysRiseEndPos)));
                            }
                        }
                    }
                    break;


                case CameraBehaviour.zoomToPlayer:
                    // if we zoom to the player we also want to follow them
                    if (ZoomCamera.Target != null)
                    {
                        Vector3 desiredPosition = ZoomCamera.Target.position + ZoomCamera.Offset;
                        Vector3 smoothedPosition = Vector2.Lerp(transform.position, desiredPosition, ZoomCamera.SmoothSpeed);
                        smoothedPosition.z = -10f;
                        transform.position = smoothedPosition;

                    }


                    break;








            }
        }

    }
    #endregion

    public void stopRise()
    {   
        
        selectedBehaviour = CameraBehaviour.stop;
        StopCoroutine(rise.alwaysRiseRoutine);

    }
    public void shakeCamera()
    {

        switch (ShakeBehaviour)
        {
            case shakeType.strong:
                shakeRoutine = StartCoroutine(Shake(2, 10));
                break;
            case shakeType.medium:
                shakeRoutine = StartCoroutine(Shake(1, 5));
                break;
            case shakeType.explosion:
                shakeRoutine = StartCoroutine(Shake(5, 1));
                break;
            case shakeType.lite:
                shakeRoutine = StartCoroutine(Shake(0.5f, 2));
                break;

        }
    }
    public void shakeCamera(float duration, float magnitude)
    {
        StartCoroutine(Shake(duration, magnitude));

    }


    IEnumerator Shake(float duration, float magnitude)
    {

        Vector3 originalPos = transform.localPosition;
        //print(originalPos);
        float elapsed = 0.0f;
        while ((elapsed < duration))
        {

            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x + originalPos.x, y + originalPos.y, originalPos.z);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
        shakeBehaviour = shakeType.none;



    }


    [System.Serializable]
    public class SmoothZoom
    {
        float smoothSpeed = 0.8f; // Speed of the camera's smooth transition

        [SerializeField] private Transform target; // Reference to the player's transform
        [SerializeField] Vector3 offset; // Offset from the player
        [SerializeField] float zoomSpeed = 2f; // Speed of the zoom effect
        [SerializeField] float finalZoom = 5f; // Minimum zoom level
        [SerializeField] float initZoom = 10f; // Maximum zoom level
        [SerializeField] float duration = 1;

        public float SmoothSpeed { get => smoothSpeed; set => smoothSpeed = value; }
        public Transform Target { get => target; set => target = value; }
        public Vector3 Offset { get => offset; set => offset = value; }
        public float ZoomSpeed { get => zoomSpeed; set => zoomSpeed = value; }
        public float FinalZoom { get => finalZoom; set => finalZoom = value; }
        public float InitZoom { get => initZoom; set => initZoom = value; }
        public float Duration { get => duration; set => duration = value; }

    }
    [System.Serializable]
    public class AlwaysRise
    {
        public Coroutine alwaysRiseRoutine;
        public float alwaysRiseEndPos;

        public float verticalDamp;
        public float speed;
        public float normalSpeed;
        public float fastSpeed;
        public float slowSpeed;

        public void increaseSpeed(float multiplier)
        {
            slowSpeed *= multiplier;
            normalSpeed *= multiplier;
            fastSpeed *= multiplier;
        }
        public void decreaseSpeed(float multiplier)
        {
            slowSpeed /= multiplier;
            normalSpeed /= multiplier;
            fastSpeed /= multiplier;
        }
 






    }


    public shakeType ShakeBehaviour { get => shakeBehaviour; set => shakeBehaviour = value; }
    public CameraBehaviour CameraType { get => selectedBehaviour; set => selectedBehaviour = value; }

    public float TimeShake { get => timeShake; set => timeShake = value; }
    public float MagnitudeShake { get => magnitudeShake; set => magnitudeShake = value; }
    public SmoothZoom ZoomCamera { get => zoomCamera; set => zoomCamera = value; }
    public SmoothZoom UnZoomCamera { get => unZoomCamera; set => unZoomCamera = value; }
    public bool IsZoom { get => isZoom; set => isZoom = value; }
    public GameObject PlayerReference { get => playerReference; set => playerReference = value; }

    public bool Moving { get => moving; set => moving = value; }
    public AlwaysRise Rise { get => rise; set => rise = value; }
    public CameraBehaviour CameraType1 { get => selectedBehaviour; set => selectedBehaviour = value; }
    public Transform Center { get => center; set => center = value; }
    public Transform Bottom { get => bottom; set => bottom = value; }
}
public enum shakeType
{
    strong,
    medium,
    explosion,
    lite,
    none,

}

[Serializable]
public enum CameraBehaviour
{
    followCharacter,
    riseWhenReachesHeight,
    alwaysRise,
    zoomToPlayer,
    stop,
    shaking,





}

[CustomEditor(typeof(FollowCamera)), CanEditMultipleObjects]
class CameraEditor : Editor
{


    bool trigger;
    public override void OnInspectorGUI()
    {
        FollowCamera camera = (FollowCamera)target;
        DrawDefaultInspector();
        if (GUILayout.Button("Shake"))
        {
            GameManager.Instance.shakeCamera(camera.TimeShake, camera.MagnitudeShake);
            Repaint();
        }
        if (GUILayout.Button("Zoom Player"))
        {

            if (trigger == false)
            {

                trigger = true;
                camera.ZoomCamera.Target = GameManager.Instance.PlayerInScene.transform;
                camera.triggerZoom(camera.ZoomCamera);
            }
            // else if (trigger == true)
            // {
            //     trigger = false;
            //     camera.ZoomCamera.Target = null;
            //     camera.triggerZoom(camera.UnZoomCamera);
            //     camera.lerpToCenter();

            // }
            Repaint();
        }
    }




}


