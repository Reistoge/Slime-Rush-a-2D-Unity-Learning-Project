using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class BreakAndRepair : MonoBehaviour
{
    // it's used to create a simple mechanic of breaking and repairing objects in the game.
    // the object needs an animator or it will trigger the default anim.

    const string breakAnimName = "break";
    const string repairAnimName = "repair";
    [SerializeField] float dissapearTime;
    [SerializeField] float breakTimer;
    AnimationClip breakAnimation;
    AnimationClip repairAnimation;
    [SerializeField] Collider2D collider;
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
        collider=GetComponent<Collider2D>();
        anim=GetComponent<Animator>();
        sr=GetComponent<SpriteRenderer>();
        //breakAnimation = searchClip("break");
        // repairAnimation = searchClip("repair");
    }
    void OnTriggerEnter2D(Collider2D col){
        if(isDefault){
            breakObject();
        }
    }
    void OnCollisionEnter2D(Collision2D col){

            if(isDefault){
                breakObject();
        }
    }
    AnimationClip searchClip(string s)
    {
        AnimationClip[] animationClips = anim.runtimeAnimatorController.animationClips;
        if (animationClips != null)
        {
            foreach (AnimationClip clip in animationClips)
            {
                if (clip.name == s) return clip;
            }

        }
        return null;


    }

    void Update()
    {

        if (IsBroken)
        {
            breakTimer += Time.deltaTime;
            if (breakTimer >= dissapearTime)
            {
                breakTimer=dissapearTime;
                repairObject();
            }
        }
    }

    public void breakObject()
    {

        if (anim && breakAnimation)
        {
            anim.Play(breakAnimation.name);

        }
        else
        {
            // default
            StartCoroutine(DisappearObject());
        }
    }
    public void repairObject()
    {


        if (anim && repairAnimation)
        {
            anim.Play(repairAnimation.name);
        }
        else
        {
            // default 
            if (IsBreaking == false)
            {


                StartCoroutine(ReappearObject());

            }
        }

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
            collider.enabled = true;
        }
        else
        {
            GetComponent<PlatformEffector2D>().enabled = true;
            collider.enabled = true;
        }

        IsBroken = false;
        transform.localScale = targetScale;

    }
    IEnumerator DisappearObject()
    {
        if (GetComponent<PlatformEffector2D>() == null)
        {
            collider.enabled = false;
        }
        else
        {
            GetComponent<PlatformEffector2D>().enabled = false;
            collider.enabled = false;
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
