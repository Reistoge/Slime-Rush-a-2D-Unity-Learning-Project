using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class FloorPart : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] SpriteRenderer sr;
    [SerializeField] BoxCollider2D breakDetectionCol;
    [SerializeField] BoxCollider2D platformSolidCollider;
    Vector2 initialSize;
    BreakableFloor floor;
    [SerializeField] FloorSide side;
    bool isBreak;
    [SerializeField] float verticalClampPosition = 20;
    const float SMOOTH_CLAMP_DURATION = 200; 

    public SpriteRenderer Sr { get => sr; set => sr = value; }
    public Vector2 InitialSize { get => initialSize; set => initialSize = value; }
    public BoxCollider2D BreakDetectionCol { get => breakDetectionCol; set => breakDetectionCol = value; }
    public BoxCollider2D PlatformSolidCollider { get => platformSolidCollider; set => platformSolidCollider = value; }

    void Start()
    {

        initialSize = sr.size;
        floor = transform.parent.GetComponent<BreakableFloor>();
    }

    // Update is called once per frame

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.CompareTag("playerDetector") && floor && isBreak == false)
        {   

            if (col.transform.parent.GetComponent<Rigidbody2D>())
            {
                float vel = col.transform.parent.GetComponent<Rigidbody2D>().velocity.y;
                PlayerScript player = col.transform.parent.GetComponent<PlayerScript>();
                Vector3 finalPos = new Vector3(player.transform.position.x, transform.position.y + verticalClampPosition);


                if (vel >= floor.MinVelocity)
                {

                    if (floor.DetectDash)
                    {

                        if (player != null)
                        {
                            if (player.IsDashing == false) return;
                        }
                    }

                    floor.breakObject(col.transform.position.x);
                    player.lerpPosition(finalPos,Quaternion.identity, SMOOTH_CLAMP_DURATION); // lerp the player position to make sure he lands on the next level.
                }
            }



        }
    }

    public void recalculateBounds(float collisionPoint)
    {
        isBreak = true;
        float tempSize = Sr.size.x;
        switch (side)
        {
            case FloorSide.left:
                if (collisionPoint <= 0) Sr.size = new Vector2(((floor.PlatformSize / 2) + collisionPoint - (floor.DefaultHoleSize / 2)), initialSize.y);
                else Sr.size = new Vector2(((floor.PlatformSize / 2) + collisionPoint - (floor.DefaultHoleSize / 2)), initialSize.y);
                //BreakDetectionCol.size = Sr.size;
                BreakDetectionCol.size = new Vector2(Sr.size.x, BreakDetectionCol.size.y);
                BreakDetectionCol.offset += new Vector2((sr.size.x - tempSize) / 2, 0);
                platformSolidCollider.size = BreakDetectionCol.size;
                platformSolidCollider.offset = BreakDetectionCol.offset;

                break;
            case FloorSide.rigth:
                if (collisionPoint <= 0) Sr.size = new Vector2(((floor.PlatformSize / 2) - collisionPoint - (floor.DefaultHoleSize / 2)), initialSize.y);
                else Sr.size = new Vector2(((floor.PlatformSize / 2) - collisionPoint - (floor.DefaultHoleSize / 2)), initialSize.y);
                BreakDetectionCol.size = new Vector2(Sr.size.x, BreakDetectionCol.size.y);
                BreakDetectionCol.offset -= new Vector2((sr.size.x - tempSize) / 2, 0);
                platformSolidCollider.size = BreakDetectionCol.size;
                platformSolidCollider.offset = BreakDetectionCol.offset;
                break;
        }

        // PlatformSolidCollider.usedByEffector=false;
    }
    public enum FloorSide
    {
        left,
        rigth,
    }






}
