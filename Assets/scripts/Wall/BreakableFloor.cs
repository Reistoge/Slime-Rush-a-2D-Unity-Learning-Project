using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using Vector2 = UnityEngine.Vector2;

public class BreakableFloor : MonoBehaviour, IBreakable
{
    // [System.Serializable]
    // class FloorPart
    // {
    //     public Transform transform;
    //     public SpriteRenderer Sr;
    //     public BoxCollider2D collider;
    //     public Vector2 initialSize;



    // }


    [SerializeField] FloorPart leftPart;
    [SerializeField] FloorPart rightPart;
    [SerializeField] bool isBreak;

    Coroutine coroutine;

    [SerializeField] float platformSize = 360;

    [SerializeField] float defaultHoleSize = 70;
    [SerializeField] ParticleSystem particles;
    [SerializeField] SoundSystem sound;
    [SerializeField] EdgeCollider2D edgeCollider;
    [SerializeField] float minVelocity;
    [SerializeField] UnityEvent OnBreakFloor; 
    int calls;
    [SerializeField] bool detectDash;

    public bool IsBreak { get => isBreak; set => isBreak = value; }
    public int Calls { get => calls; set => calls = value; }
    public float PlatformSize { get => platformSize; set => platformSize = value; }
    public float DefaultHoleSize { get => defaultHoleSize; set => defaultHoleSize = value; }
    public float MinVelocity { get => minVelocity; set => minVelocity = value; }
    public bool DetectDash { get => detectDash; set => detectDash = value; }

    void Start()
    {
        // first we get the sprite of each object
        calls = 0;

    }
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.R))
        // {

        //     //repairObject();
        //     //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        // }
    }


 

    IEnumerator fixHole(float collisionPoint)
    {
        yield return new WaitForSeconds(1);

        leftPart.PlatformSolidCollider.excludeLayers = new LayerMask();
        rightPart.PlatformSolidCollider.excludeLayers = new LayerMask();



    }


    public void breakObject(float collisionPoint)
    {


        leftPart.recalculateBounds(collisionPoint);
        rightPart.recalculateBounds(collisionPoint);
        particles.transform.position= new Vector2(collisionPoint,particles.transform.position.y);
        breakObject();
        OnBreakFloor?.Invoke();
        edgeCollider.enabled=true;
        // if (isBreak == false && coroutine == null)
        // {
        //     coroutine = StartCoroutine(breakRoutine());

        // }

    }
    public void breakObject()
    {

        particles.Play();
        IsBreak = true;
        sound.playRandom();
    }
    IEnumerator breakRoutine()
    {

        yield return new WaitForSeconds(2);
    }

    public void repairObject()
    {
        leftPart.Sr.size = leftPart.InitialSize;
        rightPart.Sr.size = rightPart.InitialSize;
        isBreak = false;

        // do nothing for now 
    }
}
