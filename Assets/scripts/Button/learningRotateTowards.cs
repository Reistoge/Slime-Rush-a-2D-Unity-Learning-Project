using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class learningRotateTowards : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)) {

            Rotate(1);
        }
        if (Input.GetMouseButtonDown(1))
        {

            Rotate(-1);
        }
    }
    private void Rotate(float direction)
    {
        
       
    }
}
