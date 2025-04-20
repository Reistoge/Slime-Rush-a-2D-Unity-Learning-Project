using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squish : MonoBehaviour
{
    // Start is called before the first frame update
    [System.Serializable]
    public class ScaleLerp
    {
        public Vector2 finalScale;
        public float duration;

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            print("asdasd");
            squish();
        }
    }
    [SerializeField] GameObject objectToSquish;
    [SerializeField] ScaleLerp[] scalesToLerp;
    [SerializeField] Coroutine squishCoroutine;
     


    Queue<IEnumerator> coroutineQueue = new Queue<IEnumerator>();
    void createQueueLerpScale(ScaleLerp[] values)
    {
        IEnumerator coroutine = null;
        foreach (ScaleLerp v in values)
        {
            coroutine = LerpScaleByAmount(v.finalScale, v.duration);
            coroutineQueue.Enqueue(coroutine);
        }
    }
    IEnumerator DequeCoroutines()
    {
               
        if (objectToSquish.GetComponent<Animator>())
        {
            objectToSquish.GetComponent<Animator>().enabled = false;
        }
        coroutineQueue.Clear();
        createQueueLerpScale(scalesToLerp);
        while (coroutineQueue.Count > 0)
        {
            yield return coroutineQueue.Dequeue();


        }
        if (objectToSquish.GetComponent<Animator>())
        {
            objectToSquish.GetComponent<Animator>().enabled = true;
        }
        
        
        squishCoroutine = null;

    }
    public void squish()
    {

        if (squishCoroutine == null) squishCoroutine = StartCoroutine(DequeCoroutines());


    } 


    private IEnumerator LerpScaleByAmount(Vector2 scaleIncrease, float lerpDuration)
    {
        // Store the starting scale and calculate the target scale
        Vector3 startScale = objectToSquish.transform.localScale;
        Vector3 targetScale = startScale + new Vector3(scaleIncrease.x, scaleIncrease.y, startScale.z);

        // Lerp over time
        float elapsedTime = 0f;

        while (elapsedTime < lerpDuration)
        {
            objectToSquish.transform.localScale = Vector3.Lerp(startScale, targetScale, elapsedTime / lerpDuration);

            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Ensure the scale reaches the exact target at the end
        objectToSquish.transform.localScale = targetScale;
        // bubbleRepairRoutine = null;
    }

}
