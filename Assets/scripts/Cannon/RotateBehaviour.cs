using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBehaviour : MonoBehaviour
{
    [Serializable]
    public class RotationBehaviour
    {
        [SerializeField] public float Angles;
        [SerializeField] public float Velocity;
    }

    [SerializeField]  RotationBehaviour[] rotationVariables;
    [SerializeField]  bool alwaysRotate;
    [SerializeField]  bool isRotating;
    [SerializeField][Min(0)][Tooltip("Time that take to the object to start rotating again (in the opposite direction) ")] public float rotDelay;
     float initRot;
    protected Queue<IEnumerator> coroutineQueue = new Queue<IEnumerator>();
    protected void createQueueRotateAngle(RotationBehaviour[] values)
    {
        IEnumerator coroutine = null;

        foreach (RotationBehaviour v in values)
        {
            coroutine = rotateToAngle(v.Angles, v.Velocity);
            coroutineQueue.Enqueue(coroutine);
        }
    }
    protected IEnumerator DequeueCoroutines(Queue<IEnumerator> CoroutineQueue, float delay, RotationBehaviour[] values)
    {
        // this is to
        coroutineQueue.Clear();
        createQueueRotateAngle(rotationVariables);
        while (coroutineQueue.Count > 0)
        {

            yield return new WaitForSeconds(0.1f + delay);

            yield return coroutineQueue.Dequeue();

            if (coroutineQueue.Count == 1 && alwaysRotate && isRotating == false)
            {
                createQueueRotateAngle(values);
            }


        }


    }
    protected IEnumerator rotateToAngle(float finalRotation, float rotSpeed)
    {

        isRotating = true;

        if (finalRotation == 180 || finalRotation == -180)
        {
            finalRotation *= -1;
        }


        float currentRotation = transform.rotation.eulerAngles.z;
        float rot = finalRotation - currentRotation;
        /*
            logic before rotation start like sound or something like that

        */
        while (transform.rotation != Quaternion.Euler(0f, 0f, finalRotation))
        {
            
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0f, 0f, finalRotation), rotSpeed * Time.deltaTime);
            // gameObject.GetComponent<CannonSoundSystem>().downVolume(0.005f);
            yield return new WaitForEndOfFrame(); // Yield to the next frame


        }
         /*
            logic after rotation start like sound or something like that

        */
 
        transform.rotation = Quaternion.Euler(0f, 0f, finalRotation);
        isRotating = false;




    }
    
}
