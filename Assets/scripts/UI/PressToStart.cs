using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PressToStart : MonoBehaviour
{
    // Start is called before the first frame update
    // Dialogue dialogue;
    Animator anim;

    void Start()
    {
        // Time.timeScale = 0;
        anim = GetComponent<Animator>();    
    }
    void Update()
    {
        if (Input.anyKeyDown  && anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            startGame();


        }
    }

    public void destroyButton(float time)
    {
        Destroy(this.gameObject, time);

    }
    public void startGame()
    {
        GameManager.Instance.instantiatePlayer();
        anim.Play("keyPressed",-1,0f);
        


    }








}
