using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolvePlatformAnimHandler : MonoBehaviour
{
    [SerializeField] DissolvePlatform dissolvePlatform;
    [SerializeField] LittleGemSpawn gems;
    [SerializeField] Animator anim;


    static readonly int DISSOLVE_HASH = Animator.StringToHash("dissolvePlatform");
    static readonly int UNDISSOLVE_HASH = Animator.StringToHash("unDissolvePlatform");
    static readonly int IDLE_HASH = Animator.StringToHash("idle");



    public void enableCollider()
    {
        dissolvePlatform.enableCollider();
    }
    public void disableCollider()
    {
        dissolvePlatform.disableCollider();
    }

    public void playDissolve()
    {

        anim.Play(DISSOLVE_HASH);
    }
    public void playUnDissolve()
    {
        anim.Play(UNDISSOLVE_HASH);
    }
    public void playDissolved()
    {

        anim.Play(DISSOLVE_HASH, 0, 0.99f);
        gems.desactivateGemsNoAnimation();



    }
    public void playIdle()
    {

        anim.Play(IDLE_HASH);

    }
    public float getDissolveAnimTime()
    {
        return Utils.getAnimationClipDuration(anim, DISSOLVE_HASH);
    }
    public float getUnDissolveAnimTime()
    {
        return Utils.getAnimationClipDuration(anim, UNDISSOLVE_HASH);
    }
}
