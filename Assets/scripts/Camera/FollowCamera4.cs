using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UIElements;
 
public class FollowCamera4 : MonoBehaviour
{

    [SerializeField] Transform UpLimit, LowerLimit;
    [SerializeField] GameObject PlayerReference;
    [SerializeField] bool impulse,follow;
     float speed=5;
    float height=640;

    public bool Impulse { get => impulse; set => impulse = value; }
    public bool Follow { get => follow; set => follow = value; }

    private void Start()
    {
        follow = true;
        
        PlayerReference= GameObject.FindGameObjectWithTag("Player");
        if(PlayerReference == null)
        {
            //testing
            PlayerReference = GameManager.instance.SelectedPlayer;
        }
        
   
    }
    private float lerpTime = 0;
    private Vector3 startPos, endPos;

    private void LateUpdate()
    {
        if (PlayerReference.transform.position.y >= UpLimit.position.y && follow)
        {
            StartCoroutine(LerpCamera());
        }
    }
    // i dont understand this shit.
    private IEnumerator LerpCamera()
    {
        lerpTime = 0;
        startPos = transform.position;
        endPos = new Vector3(transform.position.x, transform.position.y + height,-10);
        while (lerpTime < 1)
        {
            transform.position = Vector3.Lerp(startPos, endPos, lerpTime);
            lerpTime += Time.deltaTime;
            yield return null;
        }
        transform.position = endPos;
    

    }



}
