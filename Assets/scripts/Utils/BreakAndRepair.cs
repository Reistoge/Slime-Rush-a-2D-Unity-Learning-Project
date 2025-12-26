using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[RequireComponent(typeof(Collider2D))]


public class BreakAndRepair : MonoBehaviour
{
    // it's used to create a simple mechanic of breaking and repairing objects in the game.
    // the object needs an animator or it will trigger the default anim.

    // better solution is to use the animator with the animation in pixel art
    // (start the animation while breaking it and in the end trigger the other function to break)


    const string aboveEntity = "OnEntityAbove";
    [SerializeField] float dissapearTime;
    [SerializeField] float breakTimer;

    [SerializeField] Collider2D breakableCollider;
    [SerializeField] Animator anim;
    [SerializeField] SpriteRenderer sr;
    [SerializeField] bool isDefault;
    Vector2 startPos;



    bool isBroken;
    bool isBreaking; // for the coroutines.

    public bool IsBroken { get => isBroken; set => isBroken = value; }
    public bool IsBreaking { get => isBreaking; set => isBreaking = value; }

    void Start()
    {
        // Initialize variables
        IsBroken = false;
        startPos = transform.position;
        breakableCollider = GetComponent<Collider2D>();
        //breakAnimation = searchClip("break");
        // repairAnimation = searchClip("repair");
    }
    // void OnTriggerEnter2D(Collider2D col)
    // {
    //     if (isDefault)
    //     {
    //         breakObject();
    //     }
    // }
    // void OnCollisionEnter2D(Collision2D col)
    // {

    //     if (isDefault)
    //     {
    //         breakObject();
    //     }
    // }
    // AnimationClip searchClip(string s)
    // {
    //     AnimationClip[] animationClips = anim.runtimeAnimatorController.animationClips;
    //     if (animationClips != null)
    //     {
    //         foreach (AnimationClip clip in animationClips)
    //         {
    //             if (clip.name == s) return clip;
    //         }

    //     }
    //     return null;


    // }

    void Update()
    {
        // we just adjust the reappear mechanic ( we dont know how the object is broken)
        // so we just use a simple timer for the reappear time
        if (IsBroken)
        {
            breakTimer += Time.deltaTime;
            // Increment emission intensity while the object is broken
            

            if (breakTimer >= dissapearTime)
            {
                breakTimer = dissapearTime;
                reappearObject();
            }
        }
    }
    

    public void breakObject()
    {
        if(isDefault){
            StartCoroutine(DisappearObject());
        }
  

    }

    public void reappearObject(){

        if (IsBreaking == false)
        {
            if(isDefault){

                StartCoroutine(ReappearObject());
            }

        }
    }
    public void playEntityAboveAnim()
    {
        anim.Play(aboveEntity, -1, 0f);
    }

    IEnumerator ReappearObject()
    {

        sr.enabled = true;
        float duration = 0.5f;
        float timer = 0f;
        Vector3 initialScale = transform.localScale;
        Vector3 targetScale = new Vector3(1f, 1f, 1f);
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration; // calculate a normalized time value (0 to 1)
            t = t * t; // optional: add a curve to the interpolation (e.g., quadratic ease-out)
            transform.localScale = Vector3.Lerp(initialScale, targetScale, t);
            yield return new WaitForEndOfFrame();

        }

        if (GetComponent<PlatformEffector2D>() == null)
        {
            breakableCollider.enabled = true;
        }
        else
        {
            GetComponent<PlatformEffector2D>().enabled = true;
            breakableCollider.enabled = true;
        }

        IsBroken = false;
        transform.localScale = targetScale;

    }

    IEnumerator DisappearObject()
    {
        if (GetComponent<PlatformEffector2D>() == null)
        {
            breakableCollider.enabled = false;
        }
        else
        {
            GetComponent<PlatformEffector2D>().enabled = false;
            breakableCollider.enabled = false;
        }
        IsBreaking = true;
        breakTimer = 0f;
        float duration = 0.5f; // adjust this value to change the duration
        float timer = 0f;
        Vector3 initialScale = transform.localScale;
        Vector3 targetScale = new Vector3(0f, 0f, 0f);

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration; // calculate a normalized time value (0 to 1)
            t = t * t; // optional: add a curve to the interpolation (e.g., quadratic ease-out)
            transform.localScale = Vector3.Lerp(initialScale, targetScale, t);

            yield return new WaitForEndOfFrame();
        }
        transform.localScale = targetScale;
        IsBreaking = false;

        IsBroken = true;
        sr.enabled = false;

        // this that is neccesary by state.

    }


}
