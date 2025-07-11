using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

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


    [SerializeField] private RotationBehaviour[] rotationVariables;
    [SerializeField] protected bool alwaysRotate;
    IEnumerator RotateCannon;
    int indexRot;

    public RotationBehaviour[] RotationVariables { get => rotationVariables; set => rotationVariables = value; }

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
        int maxIndex = RotationVariables.Length - 1;
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

        transform.rotation = Quaternion.Euler(0, 0, RotationVariables[indexRot].Angles);
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
        if (index >= RotationVariables.Length) return;
        RotationVariables[index] = new RotationBehaviour();
        RotationVariables[index].Angles = angle;
        RotationVariables[index].Velocity = velocity;
        RotationVariables[index].dashForce = dashForce;

    }
    public void addRotation(string message)
    {
        string[] values = message.Split(',');
        float angle = float.Parse(values[0]);
        float velocity = float.Parse(values[1]);
        float dashForce = float.Parse(values[2]);
        if (RotationVariables.Length >= 10) return;
        if (RotationVariables.Length == 0)
        {
            RotationVariables = new RotationBehaviour[1];
            RotationVariables[0] = new RotationBehaviour();
            RotationVariables[0].Angles = angle;
            RotationVariables[0].Velocity = velocity;
            RotationVariables[0].dashForce = dashForce;
            return;
        }
        RotationBehaviour[] newRotationVariables = new RotationBehaviour[RotationVariables.Length + 1];
        for (int i = 0; i < RotationVariables.Length; i++)
        {
            newRotationVariables[i] = RotationVariables[i];
        }
        newRotationVariables[RotationVariables.Length] = new RotationBehaviour();
        newRotationVariables[RotationVariables.Length].Angles = angle;
        newRotationVariables[RotationVariables.Length].Velocity = velocity;
        newRotationVariables[RotationVariables.Length].dashForce = dashForce;
        RotationVariables = newRotationVariables;
    }
    public void removeRotation(string message)
    {
        if (message.ToLower() == "all")
        {
            RotationVariables = new RotationBehaviour[0];
            return;
        }

        int index = int.Parse(message);
        RotationBehaviour[] newRotationVariables = new RotationBehaviour[RotationVariables.Length - 1];
        for (int i = 0; i < RotationVariables.Length; i++)
        {
            if (i < index)
            {
                newRotationVariables[i] = RotationVariables[i];
            }
            else if (i > index)
            {
                newRotationVariables[i - 1] = RotationVariables[i];
            }
        }
        RotationVariables = newRotationVariables;
    }


    // Update is called once per frame
    void Update()
    {
        if (inBarrel && isCharging == false)
        {

            getHorizontalInput();

        }
        shootListener();

    }

    private void OnTriggerEnter2D(Collider2D col)
    {

        canShoot = true;

        enterInsideCannon(col);
        if (col.CompareTag("Player")) GameManager.Instance.CanMove = false;


    }

    private void OnTriggerExit2D(Collider2D col)
    {
        // Invoke("rotateToInitialRotation", 0.5f);

        if (col.CompareTag("Player")) GameManager.Instance.CanMove = true;
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
#if UNITY_EDITOR
[CustomEditor(typeof(ManualDirectionCannon)), CanEditMultipleObjects]
public class MDCannonEditor : Editor
{

    ManualDirectionCannon handler;
    private void OnEnable()
    {
        handler = (ManualDirectionCannon)target;

    }
    // public override void OnInspectorGUI()
    // {
    //     DrawDefaultInspector();

    //     ManualDirectionCannon handler = (ManualDirectionCannon)target;

    //     if (GUILayout.Button("Add Rotation"))
    //     {
    //         float rotation = handler.transform.rotation.eulerAngles.z;
    //         float velocity = 360f;
    //         float dashForce = 100f;

    //         handler.addRotation("" + rotation + "," + velocity + "," + dashForce);
    //     }

    // }
    public override VisualElement CreateInspectorGUI()
    {
        VisualElement root = new VisualElement();
        
    
        Button addRot = new Button(() => handler.addRotation("" + handler.transform.rotation.eulerAngles.z + "," + 360f + "," + 100f))
        {
            text = "Add Rotation",
            style =
            {
                backgroundColor = new Color(0.2f, 0.8f, 0.2f),
                color = Color.white,
                fontSize = 14,
                unityTextAlign = TextAnchor.MiddleCenter,
                height = 30
            }


        };
        root.Add(addRot);
        root.Add(new VisualElement { style = { height = 10 } });
        InspectorElement.FillDefaultInspector(root, serializedObject, this);
       

        return root;
    }
 

 
}
#endif
