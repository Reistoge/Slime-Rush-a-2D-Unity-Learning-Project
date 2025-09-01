using System;
using UnityEngine;

public class CircularMotionMovement : MonoBehaviour
{


    [SerializeField]
    private float radius = 2f;
    [SerializeField]
    private float period = 2f;

    private Vector3 center;
    [SerializeField] private float angle;
    [SerializeField] private float initAngle;

    public float Period { get => period; set => period = value; }
    public float Radius { get => radius; set => radius = value; }
    public float Angle { get => angle; set => angle = value; }
    public float InitAngle { get => initAngle; set => initAngle = value; }

    public void stopMovement()
    {
        period *= -1; 
    }
    public void resumeMovement(){
        if(period<=0){
            period *= -1;
        }
    }

    void Start()
    {
        center = transform.position;
        initAngle = angle;
        
        // angle = 0f;
    }

    void Update()
    {
        if (Period <= 0f) return;

        angle += (2 * Mathf.PI / Period) * Time.deltaTime;
        float x = Mathf.Cos(angle) * Radius;
        float y = Mathf.Sin(angle) * Radius;
        transform.position = center + new Vector3(x, y, 0f);
    }
}