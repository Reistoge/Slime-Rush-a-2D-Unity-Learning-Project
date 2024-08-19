using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class PauseController : MonoBehaviour
{
    [SerializeField] Sprite[] buttonSprites;
   [SerializeField] Button pauseResumeButton;
    [SerializeField] Animator[] animElements;
    [SerializeField] GameObject screenController;
    [SerializeField] float resumeTime;

    public static bool isGamePaused;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            onClickButtonAction();
        }
         

    }

    public void onClickButtonAction()
    {
        // si esta pausado y presiona el boton de pausar.
        if (isGamePaused == false)
        {

            isGamePaused = true;
            Time.timeScale = 0f;
            pauseResumeButton.image.sprite=buttonSprites[1];
            animElements[0].gameObject.transform.parent.gameObject.SetActive(true);
     
            screenController.SetActive(false);



        }
        else
        {
 
            foreach(Animator a in animElements){
                a.Play("fadeOut",-1,0);
            }

            StartCoroutine(ResumeIn(resumeTime));









        }

    }


    IEnumerator ResumeIn(float time)
    {
        //double check ????
        if (isGamePaused == true)
        {

            pauseResumeButton.interactable=false;
            yield return new WaitForSecondsRealtime(time);
            animElements[0].gameObject.transform.parent.gameObject.SetActive(false);
            pauseResumeButton.image.sprite = buttonSprites[0];

 
            
            Time.timeScale = 1;
            isGamePaused = false;
            pauseResumeButton.interactable=true;
            screenController.SetActive(true);

        }

    }
    
    




}

