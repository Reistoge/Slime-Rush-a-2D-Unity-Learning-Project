using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ManualDirectionCannon : Cannon
{
    // Start is called before the first frame update
    const float chargeMultiplier = 1.3f;
    IEnumerator rotateInit;

    [System.Serializable]
    public class RotationBehaviour
    {
        [SerializeField] public float Angles;
        [SerializeField] public float Velocity;
        [SerializeField] public float dashForce;
    }


    [SerializeField] protected RotationBehaviour[] rotationVariables;
    [SerializeField] protected bool alwaysRotate;
    IEnumerator RotateCannon;
    int indexRot;

    new void OnEnable()
    {
        base.OnEnable();
        InputManager.OnTouchLeft += previousRotation;
        InputManager.OnTouchRight += nextRotation;
        InputManager.OnTouchCenter += chargeAndShoot;
    }
    new void OnDisable()
    {
        base.OnDisable();
        InputManager.OnTouchLeft -= previousRotation;
        InputManager.OnTouchRight -= nextRotation;
        InputManager.OnTouchCenter -= chargeAndShoot;
    }
    new void Start()
    {
        base.Start();
    }

    public void getHorizontalInput()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            rotateCannon(1);

        }

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {

            rotateCannon(-1);
        }

    }
    public void nextRotation()
    {
        if (inBarrel)
        {
            rotateCannon(1);

        }
    }
    public void previousRotation()
    {
        if (inBarrel)
        {
            rotateCannon(-1);

        }
    }

    private void rotateCannon(int v)
    {
        playChangeRotation();
        int maxIndex = rotationVariables.Length - 1;
        int lowIndex = 0;
        int nextIndex = indexRot + v;
        switch (nextIndex)
        {
            case int n when n > maxIndex:
                nextIndex = lowIndex;
                break;
            case int n when n < lowIndex:
                nextIndex = maxIndex;
                break;
        }

        indexRot = nextIndex;

        transform.rotation = Quaternion.Euler(0, 0, rotationVariables[indexRot].Angles);
        insideObject.transform.rotation = transform.rotation;
        playPlayerEnterBarrel();




        // if (indexRot + v < rotationVariables.Length && indexRot + v >= 0)
        // {
        //     indexRot += v;
        //     // RotateCannon = rotateToAngle(rotationVariables[indexRot].Angles, rotationVariables[indexRot].Velocity, rotationVariables[indexRot].dashForce);
        //     transform.rotation = Quaternion.Euler(0, 0, rotationVariables[indexRot].Angles);
        //     insideObject.transform.rotation = transform.rotation;

        //     // StartCoroutine(RotateCannon);
        // }

    }
    public void changeRotation(string message)
    {



        string[] values = message.Split(',');
        int index = int.Parse(values[0]);
        float angle = float.Parse(values[1]);
        float velocity = float.Parse(values[2]);
        float dashForce = float.Parse(values[3]);
        if (index >= rotationVariables.Length) return;
        rotationVariables[index] = new RotationBehaviour();
        rotationVariables[index].Angles = angle;
        rotationVariables[index].Velocity = velocity;
        rotationVariables[index].dashForce = dashForce;

    }
    public void addRotation(string message)
    {
        string[] values = message.Split(',');
        float angle = float.Parse(values[0]);
        float velocity = float.Parse(values[1]);
        float dashForce = float.Parse(values[2]);
        RotationBehaviour[] newRotationVariables = new RotationBehaviour[rotationVariables.Length + 1];
        for (int i = 0; i < rotationVariables.Length; i++)
        {
            newRotationVariables[i] = rotationVariables[i];
        }
        newRotationVariables[rotationVariables.Length] = new RotationBehaviour();
        newRotationVariables[rotationVariables.Length].Angles = angle;
        newRotationVariables[rotationVariables.Length].Velocity = velocity;
        newRotationVariables[rotationVariables.Length].dashForce = dashForce;
        rotationVariables = newRotationVariables;
    }
    public void removeRotation(string message)
    {
        if (message.ToLower() == "all")
        {
            rotationVariables = new RotationBehaviour[0];
            return;
        }

        int index = int.Parse(message);
        RotationBehaviour[] newRotationVariables = new RotationBehaviour[rotationVariables.Length - 1];
        for (int i = 0; i < rotationVariables.Length; i++)
        {
            if (i < index)
            {
                newRotationVariables[i] = rotationVariables[i];
            }
            else if (i > index)
            {
                newRotationVariables[i - 1] = rotationVariables[i];
            }
        }
        rotationVariables = newRotationVariables;
    }


    // Update is called once per frame
    void Update()
    {
        if (inBarrel)
        {

            getHorizontalInput();

        }
        shootListener();

    }

    private void OnTriggerEnter2D(Collider2D col)
    {

        canShoot = true;

        enterInsideCannon(col);
        if (col.CompareTag("Player")) GameManager.instance.CanMove = false;


    }

    private void OnTriggerExit2D(Collider2D col)
    {
        // Invoke("rotateToInitialRotation", 0.5f);

        if (col.CompareTag("Player")) GameManager.instance.CanMove = true;
    }
    // public void rotateToInitialRotation()
    // {
    //     if (isRotating == false)
    //     {

    //         rotateInit = rotateToAngle(initRot, 360);

    //         StartCoroutine(rotateInit);
    //     }
    // }
    public void chargeAndShoot()
    {


        if (canShoot && inBarrel)
        {

            insideCannonAction();
            gameObject.GetComponent<Animator>().SetFloat("chargeSpeed", gameObject.GetComponent<Animator>().GetFloat("chargeSpeed") * chargeMultiplier);
        }
    }
    public void playChangeRotation()
    {
        arrowAnim.Play("showArrow", -1, 0f);
        anim.Play("changeRotation", -1, 0f);
    }

    public void shootListener()
    {
        if (Input.GetKeyUp(KeyCode.Space) && inBarrel)
        {
            //needed for precise input
            // ActionButton.onPressActionButton = false;


            chargeAndShoot();



        }

    }
}
