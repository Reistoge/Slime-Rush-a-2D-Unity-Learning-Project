using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class FollowCamera : MonoBehaviour
{

    [SerializeField] Transform UpLimit, LowerLimit;
    [SerializeField] GameObject playerReference;
    [SerializeField] shakeType shakeBehaviour;
    [SerializeField] cameraBehaviour cameraType;
    [SerializeField] float height = 640;
    bool moving;
    float lerpTime = 0;
    private void Start()
    {

        playerReference = GameObject.FindGameObjectWithTag("Player");
         


    }
 
    private void Update()
    {

        onCameraBehaviour();
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
    public void lerp()
    {

            StopAllCoroutines();
            StartCoroutine(LerpCamera());
        

    }
    public void onCameraBehaviour()
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
    
    IEnumerator Shake(float duration, float magnitude)
    {
            Vector3 originalPos = transform.localPosition;
            print(originalPos);
            float elapsed = 0.0f;
            while ((elapsed < duration)  )
            {
                
                float x = Random.Range(-1f, 1f) * magnitude;
                float y = Random.Range(-1f, 1f) * magnitude;
                
                transform.localPosition = new Vector3(x+originalPos.x, y+originalPos.y, originalPos.z);
                elapsed += Time.deltaTime;
                yield return null;
            }
            print(transform.position);
            transform.localPosition = originalPos;
         
       

    }    
    public shakeType ShakeBehaviour { get => shakeBehaviour; set => shakeBehaviour = value; }
    public cameraBehaviour CameraType { get => cameraType; set => cameraType = value; }
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
    


}

