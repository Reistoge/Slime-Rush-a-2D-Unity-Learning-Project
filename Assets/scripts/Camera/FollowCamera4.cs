using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class FollowCamera4 : MonoBehaviour
{

    [SerializeField] Transform UpLimit, LowerLimit;
    [SerializeField] GameObject PlayerReference;
    [SerializeField] shakeType shakeBehaviour;
    [SerializeField] bool impulse, follow;



    float speed = 5;
    float height = 640;

    public bool Impulse { get => impulse; set => impulse = value; }
    public bool Follow { get => follow; set => follow = value; }
    public shakeType ShakeBehaviour { get => shakeBehaviour; set => shakeBehaviour = value; }

    private void Start()
    {


        PlayerReference = GameObject.FindGameObjectWithTag("Player");
        if (PlayerReference == null)
        {
            //testing
            PlayerReference = GameManager.instance.SelectedPlayer;
        }


    }
    private float lerpTime = 0;
    private Vector3 startPos, endPos;


    private void LateUpdate()
    {
        if (PlayerReference.transform.position.y >= UpLimit.position.y && !follow)
        {
            StartCoroutine(LerpCamera());
        }
        if (follow)
        {
            Vector3 newpos = new Vector3(PlayerReference.transform.position.x, PlayerReference.transform.position.y, -10);
            transform.position = newpos;
        }
    }
    // i dont understand this shit.
    private IEnumerator LerpCamera()
    {
        lerpTime = 0;
        startPos = transform.position;
        endPos = new Vector3(transform.position.x, transform.position.y + height, -10);
        while (lerpTime < 1)
        {
            transform.position = Vector3.Lerp(startPos, endPos, lerpTime);
            lerpTime += Time.deltaTime;
            yield return null;
        }
        transform.position = endPos;


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

        Vector3 originalPos = transform.position;
        float elapsed = 0.0f;
        while ((elapsed < duration))
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            transform.localPosition = new Vector3(x, y, originalPos.z);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = originalPos;

    }




}
public enum shakeType
{
    strong,
    medium,
    explosion,
    lite,

}

