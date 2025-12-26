using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DissolvePlatform : MonoBehaviour
{

    [SerializeField] DissolvePlatformAnimHandler anim;

    [SerializeField] Collider2D col;

    [Tooltip("true: the platform starts in the invisible state\nfalse: the platforms start in the default state")][SerializeField] bool startDissolved = false;

    public bool StartDissolved { get => startDissolved; set => startDissolved = value; }
    void OnEnable()
    {
        if (startDissolved)
        {
            playDissolved();
        }
    }

    public void dissolve()
    {
        anim.playDissolve();
    }
    public void unDissolve()
    {
        anim.playUnDissolve();
    }
    public void playDissolved()
    {

        anim.playDissolved();
    }


    public void enableCollider()
    {
        col.enabled = true;
    }
    public void disableCollider()
    {
        col.enabled = false;
    }
    public float getDissolveAnimTime()
    {
        return anim.getDissolveAnimTime();
    }
    public float getUnDissolveAnimTime()
    {
        return anim.getUnDissolveAnimTime();
    }

    






}

