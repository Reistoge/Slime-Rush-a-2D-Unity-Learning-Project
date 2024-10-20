using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public class DefaultCannon : Cannon
{
    bool isCharging;
    protected Queue<IEnumerator> coroutineQueue = new Queue<IEnumerator>();
    protected void createQueueRotateAngle(RotationBehaviour[] values)
    {
        IEnumerator coroutine = null;

        foreach (RotationBehaviour v in values)
        {
            coroutine = rotateToAngle(v.Angles, v.Velocity, v.dashForce);
            coroutineQueue.Enqueue(coroutine);
        }
    }

    protected IEnumerator DequeueCoroutines(Queue<IEnumerator> CoroutineQueue, float delay, RotationBehaviour[] values)
    {
        // this is to
        coroutineQueue.Clear();
        createQueueRotateAngle(RotationVariables);
        while (coroutineQueue.Count > 0)
        {

            yield return new WaitForSeconds(0.1f + delay);

            yield return coroutineQueue.Dequeue();

            if (coroutineQueue.Count == 1 && AlwaysRotate && isRotating == false)
            {
                createQueueRotateAngle(values);
            }


        }
        showArrow();


    }
    [Serializable]
    public class RotationBehaviour
    {
        [SerializeField] public float Angles;
        [SerializeField] public float Velocity;
        [SerializeField] public float dashForce;
    }


    [SerializeField] protected RotationBehaviour[] rotationVariables;
    [SerializeField] protected bool alwaysRotate;
    IEnumerator RotateCannon;
    IEnumerator rotateInit;

    const float chargeMultiplier = 1.3f;

    private new void Start()
    {
        base.Start();
        createQueueRotateAngle(RotationVariables);
        initRot = transform.rotation.eulerAngles.z;

    }

    void Update()
    {
        shootListener();
        if (transform.eulerAngles.z <= 180 && transform.eulerAngles.z >= 90)
        {
            sprite.flipX = true;
        }
        else
        {
            sprite.flipX = false;
        }
        //

        // if (isCharging == true && !IsFinal)
        // {

        //     transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.rotation.y, transform.eulerAngles.z + 1 * 100 * Time.deltaTime * -GameManager.instance.MovXButtons);
        //     insideObject.transform.rotation = transform.rotation;
        //     insideObject.transform.position = transform.position;
        //     if (GameManager.instance.MovXButtons != 0)
        //     {
        //         isRotating = true;
        //     }
        // }

    }
    public void switchCharging()
    {
        isCharging = !isCharging;

    }

    public void shootListener()
    {
        if ((Input.GetKeyUp(KeyCode.Space) || ActionButton.onPressActionButton) && inBarrel)
        {
            // needed for precise input
            ActionButton.onPressActionButton = false;
            
            if (canShoot)
            {
                insideCannonAction();
            }
            gameObject
                .GetComponent<Animator>()
                .SetFloat(
                    "chargeSpeed",
                    gameObject.GetComponent<Animator>().GetFloat("chargeSpeed") * chargeMultiplier
                );
        }
        if (inBarrel)
        {
            //    Vector3 barrel_pos = new Vector3(transform.position.x,transform.position.y+1,transform.position.z);

            insideObject.transform.position = transform.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        canShoot = true;
        if (rotateInit != null)
        {
            StopCoroutine(rotateInit);
        }
        RotateCannon = DequeueCoroutines(coroutineQueue, rotDelay, RotationVariables);
        StartCoroutine(RotateCannon);
        enterInsideCannon(collision);

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        gameObject.GetComponent<Animator>().SetFloat("chargeSpeed", 1);
        StopCoroutine(RotateCannon);
        createQueueRotateAngle(RotationVariables);
        Invoke("rotateToInitialRotation", 0.5f);
    }

    public void rotateToInitialRotation()
    {


        rotateInit = rotateToAngle(initRot, 360);

        StartCoroutine(rotateInit);


    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //here could be something // make the barrel stops and then shoots

            string components = transform.up.ToString();
            // GameManager.instance.TransformUpVectorChars = components;
            collision.gameObject.GetComponent<Transform>().rotation = transform.rotation;
        }

        // pensar en un sistema automatizado de niveles.
    }
    public RotationBehaviour[] RotationVariables { get => rotationVariables; set => rotationVariables = value; }
    public bool AlwaysRotate { get => alwaysRotate; set => alwaysRotate = value; }
}
