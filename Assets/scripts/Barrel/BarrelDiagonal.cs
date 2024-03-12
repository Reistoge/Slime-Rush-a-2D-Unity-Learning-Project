using System.Collections;
using System.Collections.Generic;
 
using UnityEngine;
using UnityEngine.UIElements;

public class BarrelDiagonal : MonoBehaviour
{
  
    // Adjust this to change speed
    private float _speed = 2f;

    // Adjust this to change how high it goes
    private float _heigth = 0.5f;

    // The initial offset of the object
    private float _offsetY;
    private float _offsetX; 

    [SerializeField]
    int _direction=1;

    public int Direction
    {
        get { return _direction; }
        set { _direction = value; }
    }

    void Start()
    {
       
        _offsetY = transform.position.y;
        _offsetX = transform.position.x;
    }


    // Update is called once per frame
    void Update()
    {
        // Get the current position of the object
        Vector3 pos = transform.position;


        // Calculate the new y position using a sine function
        float newY = _offsetY + _heigth * Mathf.Sin(Time.time * _speed);
        float newX= _offsetX + _heigth * Mathf.Sin(Time.time * _speed);

        // Set the new position of the object
        transform.position = new Vector3(_direction*newX, newY, pos.z) ;
    }

}
