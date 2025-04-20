using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Events;

public class Rotable : MonoBehaviour
{


    protected IEnumerator rotateToAngle(float finalRotation, float rotSpeed)
    {
        rotationVariables[index].OnStartRotation?.Invoke();
        isRotating = true;

        if (finalRotation == 180 || finalRotation == -180)
        {
            finalRotation *= -1;
        }
        float currentRotation = transform.rotation.eulerAngles.z;
        float rot = finalRotation - currentRotation;

        while (transform.rotation != Quaternion.Euler(0f, 0f, finalRotation))
        {
            // canShoot = false;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0f, 0f, finalRotation), rotSpeed * Time.deltaTime);

            yield return new WaitForEndOfFrame(); // Yield to the next frame


        }

        transform.rotation = Quaternion.Euler(0f, 0f, finalRotation);
        rotationVariables[index].OnEndRotation?.Invoke();
        // canShoot = true;
        isRotating = false;




    }

    protected void createQueueRotateAngle(RotationBehaviour[] values)
    {
        IEnumerator coroutine = null;

        foreach (RotationBehaviour v in values)
        {
            coroutine = rotateToAngle(v.Angles, v.Velocity);
            coroutineQueue.Enqueue(coroutine);
        }
    }

    protected IEnumerator DequeueCoroutines(RotationBehaviour[] values)
    {
        coroutineQueue.Clear();
        createQueueRotateAngle(RotationVariables);
        index = 0;
        while (coroutineQueue.Count > 0)
        {

//            print("start "+  index);
            yield return new WaitForSeconds(delay);
            yield return coroutineQueue.Dequeue();
  //          print("end "+ index);
            index++;
            if (coroutineQueue.Count == 1 && AlwaysRotate && isRotating == false)
            {   
                print("end cycle");
                index = 0;
                createQueueRotateAngle(values);
            }


        }


    }
  
    public void startRotating()
    {

        rotationCoroutine = StartCoroutine(DequeueCoroutines(rotationVariables));
    }
    public void stopRotating()
    {
        StopCoroutine(rotationCoroutine);
    }
    protected Queue<IEnumerator> coroutineQueue = new Queue<IEnumerator>();
    [Serializable]
    public class RotationBehaviour
    {
        [SerializeField] public float Angles;
        [SerializeField] public float Velocity;
        [SerializeField] public UnityEvent OnStartRotation;
        [SerializeField] public UnityEvent OnEndRotation;

    }


    [SerializeField] RotationBehaviour[] rotationVariables;
    int index = 0;
    [SerializeField] bool alwaysRotate;
    [SerializeField] bool isRotating;
    [SerializeField] float delay;
    Coroutine rotationCoroutine;

    protected RotationBehaviour[] RotationVariables { get => rotationVariables; set => rotationVariables = value; }
    protected bool AlwaysRotate { get => alwaysRotate; set => alwaysRotate = value; }

    // Start is called before the first frame update
    void Start()
    {
        startRotating();
    }


}

