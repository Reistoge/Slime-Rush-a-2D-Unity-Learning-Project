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

        SceneManager.LoadScene(GameManager.Instance.MainMenuSceneName);






    }
    public void selectCharacterScene()
    {

        SceneManager.LoadScene(GameManager.Instance.CharacterSelector);

    }
    private void Start()
    {

        if (SceneManager.GetActiveScene().name == GameManager.Instance.MainMenuSceneName)
        {

            text.text = "0";
            if (GameManager.Instance != null)
            {
                text.text = GameManager.Instance.Highscore.ToString();
            }


        }



    }


    public void PlayGame()
    {



        StartCoroutine(GameManager.Instance.LoadSceneIn(0, GameManager.Instance.MainGame.name));


    }
    public void LoadSceneWithTransition(string args)
    {
        GameManager.Instance.loadSceneWithTransition(args);
    }




}

