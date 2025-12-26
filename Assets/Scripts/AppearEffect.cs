using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearEffect : MonoBehaviour
{

    [SerializeField] Animator animator;

    // Start is called before the first frame update
    public void playSmoke1Effect()
    {
        animator.Play("smoke1", -1, 0f);
    }
    public void playSmoke2Effect()
    {
        animator.Play("smoke2", -1, 0f);
    }
}
