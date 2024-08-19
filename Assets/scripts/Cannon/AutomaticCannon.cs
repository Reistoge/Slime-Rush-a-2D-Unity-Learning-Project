using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AutomaticCannon : Cannon
{
    // Start is called before the first frame update
    const float chargeMultiplier = 1.3f;
    IEnumerator rotateInit;

    public Transform target; // The target object to face and follow
    public float rotationSpeed = 5f; // Speed of rotation

    float offset;
    bool isRotating;
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (inBarrel)
        {

            insideObject.transform.rotation = transform.rotation;
            insideObject.transform.position = transform.position;
            // transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.rotation.y, transform.eulerAngles.z + 1 * 100 * Time.deltaTime * -GameManager.instance.MovXButtons);

        }
        // shootListener();

    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        
      
            canShoot = true;
            enterInsideCannon(col);
            StartCoroutine(seekAndShoot());

       



    }
    private void OnTriggerExit2D(Collider2D col)
    {
        rotateToInitialRotation();
    }


    IEnumerator seekAndShoot()
    {
        isRotating = true;
        arrowAnim.SetBool("canShoot", false);
        soundSystem.playRotateSfx();
        StartCoroutine(seekTarget());
        yield return new WaitUntil(() => isRotating == false);
        arrowAnim.SetBool("canShoot", true);
        soundSystem.stop();
        insideCannonAction(2);



    }
    public void rotateToInitialRotation()
    {
        if (isRotating == false)
        {
            rotateInit = rotateToAngle(initRot, 360);

            StartCoroutine(rotateInit);
        }
    }


    IEnumerator seekTarget()
    {
        if (target != null)
        {
            // Rotate towards the target

            Vector3 direction = target.position - transform.position;
            float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) - 90;
            Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
            float elapsed = 0;


            while (elapsed < 2)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
                elapsed += Time.deltaTime * rotationSpeed;
                yield return new WaitForEndOfFrame();
            }
            transform.rotation = targetRotation;
            isRotating = false;
            // Continuously follow the target's rotation
        }
        print("no target");
    }




    public void shootListener()
    {
        if (Input.GetKeyUp(KeyCode.Space) || ActionButton.onPressActionButton && inBarrel)
        {
            //needed for precise input
            ActionButton.onPressActionButton = false;

            if (canShoot)
            {

                insideCannonAction();




            }
            gameObject.GetComponent<Animator>().SetFloat("chargeSpeed", gameObject.GetComponent<Animator>().GetFloat("chargeSpeed") * chargeMultiplier);

        }
        if (inBarrel)
        {
            //    Vector3 barrel_pos = new Vector3(transform.position.x,transform.position.y+1,transform.position.z);

            insideObject.transform.position = transform.position;

        }
    }



}
