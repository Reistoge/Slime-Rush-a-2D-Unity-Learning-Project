using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PressToStart : MonoBehaviour
{
    // Start is called before the first frame update
    // Dialogue dialogue;
    Animator anim;
    [SerializeField] UnityEvent onGameStarted;
    void Start()
    {
        Time.timeScale = 1.0f;
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
        InputManager.Instance.gameObject.SetActive(true);
        try
        {
            GameManager.Instance.instantiatePlayer();

        }
        catch
        {
            #if UNITY_EDITOR
                        Debug.LogWarning("Failed to instantiate player. Instantiating default player instead.");
                        GameManager.Instance.instantiateDefaultPlayer();
            #endif
        }
        anim.Play("keyPressed",-1,0f);
        


    }








}
