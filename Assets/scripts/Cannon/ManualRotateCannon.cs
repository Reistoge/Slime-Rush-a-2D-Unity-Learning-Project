using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO.Compression;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class ManualRotateCannon : Cannon
{
    // Start is called before the first frame update
    const float chargeMultiplier = 1.3f;
    IEnumerator rotateInit;
    [SerializeField] float minRot, maxRot;
    [SerializeField] bool restrict;
    [SerializeField] float rotVel = 300f;

 

    new void Start()
    {
        base.Start();
    }
    new void OnEnable()
    {
        base.OnEnable();

        InputManager.OnTouchCenter += chargeAndShoot;
    }
    new void OnDisable()
    {
        base.OnDisable();
        InputManager.OnTouchCenter -= chargeAndShoot;
    }

    // Update is called once per frame
    void Update()
    {
        if (inBarrel && InputManager.Instance.HorizontalAxis != 0)
        {
            float input = -InputManager.Instance.HorizontalAxisRaw;
            float currentZ = transform.eulerAngles.z;
            float targetZ = currentZ + rotVel * input * Time.deltaTime;

            if (restrict)
            {
                // Normalize angles to [0,360)
                targetZ = (targetZ + 360f) % 360f;
                float min = (minRot + 360f) % 360f;
                float max = (maxRot + 360f) % 360f;

                if (min < max)
                {
                    targetZ = Mathf.Clamp(targetZ, min, max);
                }
                else // Wrap-around case, e.g., min=300, max=60
                {
                    if (!(targetZ >= min || targetZ <= max))
                    {
                        // Clamp to the nearest bound
                        float distToMin = Mathf.DeltaAngle(targetZ, min);
                        float distToMax = Mathf.DeltaAngle(targetZ, max);
                        targetZ = Mathf.Abs(distToMin) < Mathf.Abs(distToMax) ? min : max;
                    }
                }
            
            }
          
            // Directly set the angle for more responsive and predictable rotation
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.rotation.eulerAngles.y, targetZ);
            insideObject.transform.SetPositionAndRotation(transform.position, transform.rotation);

            if (InputManager.Instance.HorizontalAxis != 0)
            {
                isRotating = true;
            }
        }
        shootListener();

    }

    private void OnTriggerEnter2D(Collider2D col)
    {

        canShoot = true;
        enterInsideCannon(col);


    }
    private void OnTriggerExit2D(Collider2D col)
    {
        Invoke("rotateToInitialRotation", 0.5f);
    }
    public void rotateToInitialRotation()
    {
        if (isRotating == false)
        {

            rotateInit = rotateToAngle(initRot, 360);

            StartCoroutine(rotateInit);
        }
    }
    public void chargeAndShoot()
    {


        if (inBarrel)
        {
            if (canShoot)
            {
                insideCannonAction();

            }
            gameObject.GetComponent<Animator>().SetFloat("chargeSpeed", gameObject.GetComponent<Animator>().GetFloat("chargeSpeed") * chargeMultiplier);

        }
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
