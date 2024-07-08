using System.Collections;
using UnityEngine;

public class BarrelRotation : MonoBehaviour
{
    [SerializeField] float rotationSpeed; // Degrees per second


    [SerializeField] float rotation;
    float initRot;

    private void Start()
    {
        initRot = transform.rotation.eulerAngles.z;
        // Initialize the target rotation (90 degrees clockwise)
        StartCoroutine(RotateFor2(rotation, rotationSpeed));
    }
    private void Update()
    {
        print(transform.rotation);



    }



    protected IEnumerator RotateFor2(float rotation, float speed)
    {

        float delay = 1f;
        yield return new WaitForSeconds(delay);
        // NOW IS EXACTLY, THE REASON IS QUATERNION.ROTATETOWARDS, the method used before was gameobject.rotate(), less accurate.



        double sum = 0;
        rotation += initRot;
        while (true)
        {

            // Rotate clockwise
            while (transform.rotation != Quaternion.Euler(0f, 0f, rotation))
            {



                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0f, 0f, rotation), speed * Time.deltaTime);
                sum = transform.rotation.z;
                //print(sum);

                yield return new WaitForEndOfFrame(); // Yield to the next frame


            }

            double middle = sum;
            // Pause for a moment
            yield return new WaitForSeconds(0.5f);




            while (transform.rotation != Quaternion.Euler(0f, 0f, -rotation))
            {



                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0f, 0f, -rotation), speed * Time.deltaTime);
                sum = transform.rotation.z;
                //print(sum);



                yield return new WaitForEndOfFrame(); // Yield to the next frame
            }




            // Pause again
            yield return new WaitForSeconds(0.5f);
        }
    }
}