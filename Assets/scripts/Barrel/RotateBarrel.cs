using UnityEngine;

[RequireComponent(typeof(Cannon))]
public class RotateBarrel : MonoBehaviour
{

    // script for default_barrel
    [SerializeField] float defaultRot;
    [SerializeField] float maxNegativeRot;
    [SerializeField] float maxPositiveRot;
    [SerializeField] private float rotationSpeed;
    [SerializeField] int randomDirection = 0;
    [SerializeField] bool random = true;

    [SerializeField] private Vector3 rotateComponents = new Vector3(0, 0, 1);
    [SerializeField] private Vector3 upVectors;

    [SerializeField] private float objectRotation;
    [SerializeField] private int switchDirection = 1;

    private bool activateRotation = true;
    public bool ActivateRotation { get { return activateRotation; } set { activateRotation = value; } }


    private void Start()
    {
        if (maxNegativeRot == -180)
        {
            maxNegativeRot = -179;
        }
        if (maxPositiveRot == 180)
        {
            maxPositiveRot = 179;
        }




        if (random)
        {
            rotationSpeed = Random.Range(50, 100);

            //random_direction... it could be 0 so that the reason i 
            // we make this while to ensure that the randomdirection is not 0 (if its zero the object is not going to rotate).
            while (randomDirection == 0)
            {
                randomDirection = Random.Range(-1, 1);
                maxNegativeRot = -50f;
                maxPositiveRot = 50f;
            }
            //we apply the direction.
        }

        rotationSpeed *= randomDirection;

        if (transform.GetComponent<Cannon>().Is_First)
        {

            randomDirection = 1;
            rotationSpeed = 60f;
        }

    }
    private void Update()
    {

        upVectors = transform.up;
        if (activateRotation)
        {
            transform.Rotate(rotateComponents, switchDirection * rotationSpeed * Time.deltaTime);
            objectRotation = gameObject.GetComponent<Rigidbody2D>().rotation;
            if (objectRotation < maxNegativeRot)
            {
                if (rotationSpeed > 0)
                {
                    switchDirection = 1;
                }
                if (rotationSpeed < 0)
                {
                    switchDirection = -1;
                }

            }
            if (objectRotation > maxPositiveRot)
            {
                if (rotationSpeed > 0)
                {
                    switchDirection = -1;
                }
                if (rotationSpeed < 0)
                {
                    switchDirection = 1;
                }
            }





        }
        if (!activateRotation)
        {
            transform.position = transform.position;
        }






    }
    public void SwitchRotateState()
    {
        activateRotation = !activateRotation;
    }






}






