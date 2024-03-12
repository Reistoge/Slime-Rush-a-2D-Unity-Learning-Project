using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionControl : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    Vector2 movX;
    [SerializeField]
    int speedMov=1;
    void Start()
    {
          
    }

    // Update is called once per frame
    void Update()
    {
        movX = Input.acceleration;
        movX.y = 0;

        transform.Translate(movX*speedMov);
    }
}
