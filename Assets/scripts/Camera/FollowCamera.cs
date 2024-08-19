using System;
using System.Collections;
using System.Drawing;
using System.Timers;
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

    float lerpTime = 0;

    private void Start()
    {

        playerReference = GameObject.FindGameObjectWithTag("Player");

        if (playerReference == null)
        {
            playerReference = GameManager.instance.SelectedPlayer;
            print("the player was not instantiated ");
        }

    }

    void LateUpdate()
    {
        onCameraBehaviour();
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


    private IEnumerator Zoom(SmoothZoom zoomCameraVariables)
    {

        // Smoothly follow the player
        GetComponent<PixelPerfectCamera>().enabled = false; // config
        cameraType = cameraBehaviour.zoomToPlayer;
        float elapsed = 0;


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
    public void lerpToCenter()
    {
        cameraType = cameraBehaviour.centerCamera;
        ZoomCamera.Target = null;
        moving = true;
        if (GameObject.Find("LevelObjectsManager").GetComponent<LevelObjectManager>() != null && GameObject.Find("LevelObjectsManager").GetComponent<LevelObjectManager>().isActiveAndEnabled)
        {
            StartCoroutine(LerpCamera(this.transform.position, GameObject.Find("LevelObjectsManager").GetComponent<LevelObjectManager>().CurrentLevel.transform.position, 2, 2));


        }
        else
        {
            cameraType = cameraBehaviour.centerCamera;
            ZoomCamera.Target = null;
            moving = true;
            StartCoroutine(LerpCamera(this.transform.position, new Vector3(0,GameManager.instance.LevelCount*640,-10), 2, 2));
        }
    }



    public void startZoom()
    {


        StartCoroutine(centerLogic());

    }
    private IEnumerator centerLogic()
    {

        isCenteringCamera = true; // bool to say that the coroutine is running or not
        GameManager.instance.ScreenController.SetActive(false);

        ZoomCamera.Target = GameManager.instance.PlayerInScene.transform;

        StartCoroutine(Zoom(ZoomCamera));
        yield return new WaitUntil(() => GameManager.instance.LastUsedBarrel.GetComponent<Cannon>().IsFirst == true);
        // aqui tendria que suceder el cañon autamatico ?, donde estara el cañon ?
        yield return new WaitForSeconds(0.5f);
        if (GameObject.Find("LevelObjectsManager").GetComponent<LevelObjectManager>() != null)
        {
            cameraType = cameraBehaviour.centerCamera;
            ZoomCamera.Target = null;
            moving = true;
            StartCoroutine(LerpCamera(this.transform.position, GameObject.Find("LevelObjectsManager").GetComponent<LevelObjectManager>().CurrentLevel.transform.position, 2, 2));


        }

        yield return new WaitUntil(() => moving == false);
        yield return new WaitForSeconds(0.5f);
        cameraType = cameraBehaviour.centerCamera;

        IsZoom = true;
        StartCoroutine(Zoom(UnZoomCamera));
        yield return new WaitUntil(() => IsZoom == false);
        GetComponent<PixelPerfectCamera>().enabled = true;

        GameManager.instance.ScreenController.SetActive(true);
        isCenteringCamera = false;

    }
    public void onCameraBehaviour()
    {
        if (playerReference != null)
        {
            switch (CameraType)
            {
                case cameraBehaviour.followCharacter:
                    // this just follow the character
                    Vector3 newpos = new Vector3(playerReference.transform.position.x, playerReference.transform.position.y, -10);
                    transform.position = newpos;
                    break;
                case cameraBehaviour.riseWhenReachesHeight:
                    // if the character surpases the limit of a certain point of the camera we are going to lerp the camera up.
                    if ((playerReference.transform.position.y) >= (UpLimit.transform.position.y) && (moving == false))
                    {

                        StartCoroutine(LerpCamera());
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
                case cameraBehaviour.centerCamera:

                    break;








            }
        }

    }
    public void shakeCamera()
    {

        switch (ShakeBehaviour)
        {
            case shakeType.strong:
                StartCoroutine(Shake(2, 10));
                break;
            case shakeType.medium:
                StartCoroutine(Shake(1, 5));
                break;
            case shakeType.explosion:
                StartCoroutine(Shake(5, 1));
                break;
            case shakeType.lite:
                StartCoroutine(Shake(0.5f, 2));
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
        print(originalPos);
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



    }
    [System.Serializable]
    public class SmoothZoom
    {
        float smoothSpeed = 0.8f; // Speed of the camera's smooth transition

        private Transform target; // Reference to the player's transform
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
 

    public shakeType ShakeBehaviour { get => shakeBehaviour; set => shakeBehaviour = value; }
    public cameraBehaviour CameraType { get => cameraType; set => cameraType = value; }

    public float TimeShake { get => timeShake; set => timeShake = value; }
    public float MagnitudeShake { get => magnitudeShake; set => magnitudeShake = value; }
    public SmoothZoom ZoomCamera { get => zoomCamera; set => zoomCamera = value; }
    public SmoothZoom UnZoomCamera { get => unZoomCamera; set => unZoomCamera = value; }
    public bool IsZoom { get => isZoom; set => isZoom = value; }
}
public enum shakeType
{
    strong,
    medium,
    explosion,
    lite,

}
public enum cameraBehaviour
{
    followCharacter,
    riseWhenReachesHeight,
    zoomToPlayer,
    centerCamera,



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
            GameManager.instance.shakeCamera(camera.TimeShake, camera.MagnitudeShake);
            Repaint();
        }
        if (GUILayout.Button("Zoom Player"))
        {

            if (trigger == false)
            {
                 
                trigger = true;
                camera.ZoomCamera.Target = GameManager.instance.PlayerInScene.transform;
                camera.triggerZoom(camera.ZoomCamera);
            }
            else if(trigger ==true )
            {
                trigger = false;
                camera.ZoomCamera.Target = null;
                camera.triggerZoom(camera.UnZoomCamera);
                camera.lerpToCenter();

            }
            Repaint();
        }
    }




}


