using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float force;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Rigidbody2D>().AddForce(Input.GetAxis("Horizontal")*Vector2.right*force,ForceMode2D.Impulse);
        GetComponent<Rigidbody2D>().AddForce(Input.GetAxis("Vertical")*Vector2.up*force,ForceMode2D.Impulse);
        if(Input.GetKey(KeyCode.Space)){
            GetComponent<Rigidbody2D>().AddForce(Vector2.up*force*100,ForceMode2D.Impulse);
        }
    }
}
