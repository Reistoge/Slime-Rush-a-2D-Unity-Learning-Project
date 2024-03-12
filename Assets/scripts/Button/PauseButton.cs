using System.Collections;
using UnityEngine;
using UnityEngine.UI;
 

public class PauseButton : MonoBehaviour
{
    [SerializeField]
    private Sprite[] button_sprites;
    [SerializeField]
    private GameObject[] button_container;
    [SerializeField]
    public static bool game_paused = false;
  
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {

            Pause_and_resume_Game();
        }
        
    }

    public void Pause_and_resume_Game()
    {
        // si esta pausado y presiona el boton de pausar.
        if (game_paused == false)
        {

            //animacion
            // si esta pausado cambia el sprite de pausa a resume
            Time.timeScale = 0f;
            gameObject.GetComponent<Image>().sprite = button_sprites[1];
            for(int i = 0; i < button_container.Length; i++)
            {
                

                 
                 
               
                    button_container[i].SetActive(true);
              
                 

            }
            
            game_paused = true;



        }
        else if(game_paused == true) 
        {

            for (int i = 0; i < button_container.Length; i++)
            {
                if (button_container[i].GetComponent<Animator>() != null)
                {
                    button_container[i].GetComponent<Animator>().SetTrigger("fadeout");
                }
                
            }
            // time that takes to resume the game
            StartCoroutine(ResumeIn(3));
            
            game_paused = false;

             


            //animacion

            // we change the image of the paused button



        }

    }
     
 

    
     
     


    IEnumerator ResumeIn(float time)
    {
        //double check ????
        if (game_paused == true)
        {
            
          
            yield return new WaitForSecondsRealtime(time);
            gameObject.GetComponent<Image>().sprite = button_sprites[0];

            for (int i = 0; i < button_container.Length; i++)
            {
                button_container[i].SetActive(false);


            }
            Time.timeScale = 1;
        }
        
    }

     


}
 
