using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class ManualRotateCannon : Cannon
{
    // Start is called before the first frame update
    const float chargeMultiplier = 1.3f;
    IEnumerator rotateInit;
    [SerializeField] float minRot,maxRot;
    [SerializeField] bool restrict;
    

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
        if (inBarrel )
        {
            float z =  transform.eulerAngles.z + 1 * 100 * Time.deltaTime * -InputManager.Instance.HorizontalAxis;
            if(restrict){
                z = Mathf.Clamp(Mathf.Abs(z),minRot,maxRot);
            
            }
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.rotation.y, z);
            insideObject.transform.rotation = transform.rotation;
            insideObject.transform.position = transform.position;
             
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


        if (canShoot && inBarrel)
        {

            insideCannonAction();
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
