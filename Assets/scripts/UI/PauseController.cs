using System;
using System.Collections;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class PauseController : MonoBehaviour
{
    // pause system is trash, improve it.

    [SerializeField] Button pauseResumeButton;
    [SerializeField] Sprite pause;
    [SerializeField] Sprite resume;
    [SerializeField] GameObject UIPauseContainer;
    // [SerializeField] GameObject screenController;

    [SerializeField] pauseState state;

    [SerializeField] UnityEvent onClickResume;
    [SerializeField] UnityEvent onResume;
    [SerializeField] UnityEvent onPause;
    
    

    public static Action OnPause;
    public static Action OnResume;
    float maxTime;
    // [SerializeField] float resumeTime;



    public static bool isGamePaused;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            onClickPause();
        }


    }
    void Start()
    {
        // 

    }
    void OnEnable()
    {
        restartButton.StopCoroutines += StopAllCoroutines;
    }
    void OnDisable()
    {
        restartButton.StopCoroutines -= StopAllCoroutines;
    }


    public void onClickPause()
    {
        // si esta pausado y presiona el boton de pausar.
        if (isGamePaused == false)
        {

            isGamePaused = true;
            OnPause?.Invoke();
            pauseResumeButton.image.sprite = resume;
            //animElements[0].gameObject.transform.parent.gameObject.SetActive(true);
            UIPauseContainer.SetActive(true);
            InputManager.Instance.gameObject.SetActive(false);
            Time.timeScale = 0f;
            //screenController.SetActive(false);



        }
        else
        {


            InputManager.Instance.gameObject.SetActive(true);
            OnResume?.Invoke();
            StartCoroutine(ResumeIn(0));









        }

    }


    IEnumerator ResumeIn(float time)
    {

        //double check ????
        maxTime = 0f;
        string name = "";
       
        foreach (Transform child in UIPauseContainer.transform)
        
        {
            if (child.GetComponent<Animator>())
            {
                child.GetComponent<Animator>().Play("OnResume", -1, 0f);
                if (child.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length >= maxTime){

                    maxTime = child.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
                    name=child.name;
                }

            }

        }
        print(maxTime);
        print(name);

        if (isGamePaused == true)
        {

            pauseResumeButton.interactable = false;

            yield return new WaitForSecondsRealtime(maxTime);

            UIPauseContainer.SetActive(false);
            // animElements[0].gameObject.transform.parent.gameObject.SetActive(false);
            pauseResumeButton.image.sprite = pause;



            Time.timeScale = 1;
            isGamePaused = false;
            pauseResumeButton.interactable = true;
            //  screenController.SetActive(true);

        }

    }
    public void processClick(pauseState state)
    {
        switch (state)
        {
            case pauseState.onResume:
                break;
            case pauseState.playingPause:
                break;
            case pauseState.onPause:
                break;
            case pauseState.playingResume:
                break;

        }
    }
    public enum pauseState
    {
        playingPause,
        onPause,
        playingResume,
        onResume,

    }







}

