using UnityEngine;

public class RotateAroundBarrel : MonoBehaviour
{

    [SerializeField]
    Vector3 vector1 = new Vector3();
    [SerializeField]
    Vector3 vector2 = new Vector3();

    [SerializeField]
    public float random_speed = 3f;

    [SerializeField]
    Transform center;

    [SerializeField]
    private bool activateRotateAround = true;

    public bool ActivateRotateAround
    {
        get
        {
            return activateRotateAround;
        }
        set
        {
            activateRotateAround = value;
        }
    }


    // Update is called once per frame
    void Update()
    {
        // THE FIRST PARAMETER CONTROLLS IN 2D ROTATION, IF YOU CHANGE THE VECTOR NUMBERS THE CIRCUNFERENCE WILL BE BIGGER.
        // THE SECOND VECTOR PARAMETER IS NOT FOR 2D, IN SOME CASES IF YOU WANT TOU FLIP IT LIKE A CARD.
        // THE THIRD PARAMETERS IS FOR SPEED OF THE ROTATION.
        if (activateRotateAround)
        {
            //transform.RotateAround(vector1, vector2, rotationSpeed);
            transform.RotateAround(center.position, Vector3.forward, 50 * Time.deltaTime * random_speed);
        }
        else if (!activateRotateAround)
        {
            transform.position = transform.position;
        }


    }
    public void SwitchRotateState()
    {
        ActivateRotateAround = !ActivateRotateAround;
    }

}
