using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ManualRotateCannon : Cannon
{
    // Start is called before the first frame update
    const float chargeMultiplier = 1.3f;
    IEnumerator rotateInit;

    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (inBarrel)
        {

            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.rotation.y, transform.eulerAngles.z + 1 * 100 * Time.deltaTime * -GameManager.instance.MovXButtons);
            insideObject.transform.rotation = transform.rotation;
            insideObject.transform.position = transform.position;
            if(GameManager.instance.MovXButtons!=0){
                isRotating=true;
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
    public void shootListener()
    {
        if (Input.GetKeyUp(KeyCode.Space) || ActionButton.onPressActionButton && inBarrel)
        {
            //needed for precise input
            ActionButton.onPressActionButton=false;
            
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
