using System;
using System.Collections;
using System.Drawing;
using System.Timers;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEditor.Experimental;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Random = UnityEngine.Random;


public class FollowCamera : MonoBehaviour
{

    [SerializeField] Transform UpLimit, LowerLimit;
    [SerializeField] GameObject playerReference;
    [SerializeField] shakeType shakeBehaviour;
    [SerializeField] cameraBehaviour cameraType;
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

    private void Start()
    {

        PlayerReference = GameObject.FindGameObjectWithTag("Player");

        if (PlayerReference == null)
        {
            PlayerReference = GameManager.Instance.SelectedPlayer;
            print("the player was not instantiated ");
        }

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
    public void changeCameraBehaviour(int args){
        StopAllCoroutines();
        cameraType = (cameraBehaviour)args;
    }
    public void stopCameraBehaviour(){
        StopAllCoroutines();
        cameraType = cameraBehaviour.stop;
    }
    public void lerp(Transform target){
        lerp(transform.position.y, target.transform.position.y-transform.position.y);        
    }




    private IEnumerator LerpCamera(Vector3 startPos, Vector3 endPos, float duration, float speed)
    {

        float lerpTime = 0;
        moving = true;
        while (lerpTime < duration)
        {
            transform.position = Vector3.Lerp(new Vector3(startPos.x, startPos.y, -10), new Vector3(endPos.x, endPos.y, -10), lerpTime * speed);

            lerpTime += Time.deltaTime * speed;
            yield return new WaitForEndOfFrame();
        }
        transform.position = new Vector3(endPos.x, endPos.y, -10);
        moving = false;


    }
    private IEnumerator LerpCamera(Vector3 startPos, Vector3 endPos, float speed)
    {

        float lerpTime = 0;
        moving = true;
        float duration = math.abs(startPos.magnitude - endPos.magnitude) / speed;
        while (lerpTime < duration)
        {
            transform.position = Vector3.Lerp(new Vector3(startPos.x, startPos.y, -10), new Vector3(endPos.x, endPos.y, -10), lerpTime * speed);

            lerpTime += Time.deltaTime * speed;
            yield return new WaitForEndOfFrame();
        }
        transform.position = new Vector3(endPos.x, endPos.y, -10);
        moving = false;


    }
    private IEnumerator alwaysRise(Vector3 startPos, Vector3 endPos)
    {

        float lerpTime = 0;
        moving = true;
        //rise.riseTotalDuration = math.abs(startPos.magnitude - endPos.magnitude) * (1 / rise.speed);
        while ((Vector2)transform.position != (Vector2)(endPos))
        {
            transform.position = Vector3.MoveTowards(new Vector3(startPos.x, startPos.y, -10), new Vector3(endPos.x, endPos.y, -10), lerpTime * rise.speed);

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
        cameraType = cameraBehaviour.zoomToPlayer;
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
                case cameraBehaviour.followCharacter:
                    // this just follow the character
                    Vector3 newpos = new Vector3(PlayerReference.transform.position.x, PlayerReference.transform.position.y, -10);
                    transform.position = newpos;
                    break;
                case cameraBehaviour.riseWhenReachesHeight:
                    // if the character surpases the limit of a certain point of the camera we are going to lerp the camera up.
                    if ((PlayerReference.transform.position.y) >= (UpLimit.transform.position.y) && (moving == false))
                    {
                        if (shakeRoutine != null)
                        {
                            StopCoroutine(shakeRoutine);
                        }
                        StartCoroutine(LerpUpCameraInUnscaledTime());
                    }
                    if ((PlayerReference.transform.position.y) >= (-300) && (PlayerReference.transform.position.y) <= (LowerLimit.transform.position.y) && (moving == false))
                    {
                        if (shakeRoutine != null)
                        {
                            StopCoroutine(shakeRoutine);
                        }
                        StartCoroutine(LerpDownCameraInUnscaledTime());
                    }

                    break;
                case cameraBehaviour.alwaysRise:

                    if (rise.alwaysRiseRoutine == null)
                    {
                        PlayerReference = GameObject.FindWithTag("Player");
                        rise.alwaysRiseRoutine = StartCoroutine(alwaysRise(Camera.main.transform.position, new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y + rise.alwaysRiseEndPos)));

                    }
                    else // the coroutine is running
                    {

                        if (PlayerReference)
                        {
                            float dif = (PlayerReference.transform.position.y - transform.position.y);
                            if (dif <= -rise.verticalDamp)
                            {

                                StopCoroutine(rise.alwaysRiseRoutine);
                                rise.speed = rise.slowSpeed;
                                rise.alwaysRiseRoutine = StartCoroutine(alwaysRise(Camera.main.transform.position, new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y + rise.alwaysRiseEndPos)));


                            }
                            else if (dif > rise.verticalDamp)
                            {
                                StopCoroutine(rise.alwaysRiseRoutine);
                                rise.speed = rise.fastSpeed;
                                rise.alwaysRiseRoutine = StartCoroutine(alwaysRise(Camera.main.transform.position, new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y + rise.alwaysRiseEndPos)));

                            }
                            else
                            {
                                StopCoroutine(rise.alwaysRiseRoutine);
                                rise.speed = rise.normalSpeed;
                                rise.alwaysRiseRoutine = StartCoroutine(alwaysRise(Camera.main.transform.position, new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y + rise.alwaysRiseEndPos)));
                            }
                        }
                    }
                    break;
                

                case cameraBehaviour.zoomToPlayer:
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
 

    }


    public shakeType ShakeBehaviour { get => shakeBehaviour; set => shakeBehaviour = value; }
    public cameraBehaviour CameraType { get => cameraType; set => cameraType = value; }

    public float TimeShake { get => timeShake; set => timeShake = value; }
    public float MagnitudeShake { get => magnitudeShake; set => magnitudeShake = value; }
    public SmoothZoom ZoomCamera { get => zoomCamera; set => zoomCamera = value; }
    public SmoothZoom UnZoomCamera { get => unZoomCamera; set => unZoomCamera = value; }
    public bool IsZoom { get => isZoom; set => isZoom = value; }
    public GameObject PlayerReference { get => playerReference; set => playerReference = value; }
    private AlwaysRise Rise { get => rise; set => rise = value; }
    public bool Moving { get => moving; set => moving = value; }
    
}
public enum shakeType
{
    strong,
    medium,
    explosion,
    lite,
    none,

}

public enum cameraBehaviour
{
    followCharacter ,
    riseWhenReachesHeight ,
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


