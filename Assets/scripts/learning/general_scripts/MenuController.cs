using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    GameObject SpawnPoint;




    public TextMeshPro text;


    public void returnHome()
    {
        // Invoke("executeTransition1", 2);
        SceneManager.LoadScene(GameManager.instance.MainMenuSceneName);
        // GameManager.instance.ResetTriggerTrans1();





    }
    public void selectCharacterScene()
    {
        //Invoke("executeTransition1", 2);
        SceneManager.LoadScene(GameManager.instance.CharacterSelector);

    }
    private void Start()
    {

        if (SceneManager.GetActiveScene().name == GameManager.instance.MainMenuSceneName)
        {

            text.text = "0";
            if (GameManager.instance != null)
            {
                text.text = GameManager.instance.Highscore.ToString();
            }


        }



    }


    public void PlayGame()
    {

        // if (SpawnPoint != null)
        // {

        //     // Play game spawn the object in mainMenu using a reference in inspector of the spawnposition in the scene.
        //     //GameObject Player=GameManager.instance.spawnPlayer(SpawnPoint.transform.position,"Main_Menu");
        //     //Player.GetComponent<SpriteRenderer>().enabled = false;

        // }
        // else if (SpawnPoint == null)
        // {
        //     print("Set the spawnPoint gameObject reference in the button object in the inspector" % Colorize.Orange);
        // }
        // // starts loading the scene.
        // Invoke("executeTransition1", 0);


        StartCoroutine(GameManager.instance.LoadSceneIn(0, GameManager.instance.MainGame.name));
        // when the player press the start button it pass 6 seconds and loads the new scene


    }
    // void executeTransition1()
    // {
    //     GameManager.instance.executeTransition1();
    // }



}

