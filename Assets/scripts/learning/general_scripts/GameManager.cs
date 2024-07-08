
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{



    //instance of an object

    public static GameManager instance;
    [Header("ENEMIES SETTINGS")]
    [SerializeField] int enemyAmount;
    [SerializeField] float enemySeparation;
    [SerializeField] private int enemiesKilled;


    [Header("PLAYER SETTINGS")]
    //reference of the player
    [SerializeField] private GameObject selectedPlayer;
    [SerializeField] private GameObject defaultPlayer;
    [SerializeField] private Vector3 playerPos;
    [SerializeField] private float playerSpeed;
    [SerializeField] private float playerScore;
    [SerializeField] private int playerLife;
    [SerializeField] private int playerPurchasedHearts = 3;
    [SerializeField] private int playerCoins1;
    [SerializeField] private bool isInRewardScene;
    [SerializeField] private bool inBarrel;




    [Header("Game Manager")]
    [SerializeField] string currentScene;
    private string mainMenuNameScene = "MAIN_MENU";
    private string characterSelectorNameScene = "CHARACTER_SELECTOR_SCENE";
    //reference of the player life
    [SerializeField] GameObject transitionManager;
    [SerializeField] int totalCoins;
    [SerializeField] int movXButtons;
    [SerializeField] int waves;
    [SerializeField] private int charIndex = 0;
    [SerializeField] private bool isInMainMenu;
    [SerializeField] private float playerHighscore = 0;
    [SerializeField] bool setPlayerValuesInScenes;

    [Header("CANNON VARIABLES")]
    [SerializeField] private bool firstBarrelPass;
    [SerializeField] private Vector3 inBarrelPos;
    [SerializeField] private Vector3 rewardSpawnPos = new Vector3(0, -6.31f, 1);


    private string transformUpVectorChars = "";


    [Header("MAIN GAME")]
    [SerializeField] float playerMainGameScale = 1f;
    [SerializeField] float playerMainGameSpeed = 300f;
    [SerializeField] float playerMainGameMass = 300f;
    [SerializeField] float playerMainGameDrag = 0;
    [SerializeField] float playerMainGameGravityScale = 100;
    [SerializeField] float playerMainGameAngularDrag = 0.05f;

    [SerializeField] float finalBarrelForce = 0;//10
    [SerializeField] float finalBarrelCooldown = 0;//5
    private string mainGameNameScene = "GAME_SCENE";



    [Header("REWARD SCENE")]
    [SerializeField] float playerRewardSceneMass = 1f;
    [SerializeField] float playerRewardSceneScale = 0.3f;
    [SerializeField] float playerRewardSceneGravity = 0.5f;
    [SerializeField] float playerRewardSceneLinearDrag = 5f;
    [SerializeField] float playerRewardSceneSpeed = 200f;
    private string rewardScene1Name = "REWARD_SCENE_1";

    [Header("MAIN MENU SCENE")]
    [SerializeField] float playerMainMenuGravityScale = 0.1f;
    [SerializeField] float playerMainMenuDrag = 0.1f;
    [SerializeField] float playerMainMenuMass = 3f;
    [SerializeField] float playerMainMenuScale = 1.5f;
    [SerializeField] float playerMainMenuSpeed = 500f;
    [SerializeField] float mainMenuBarrelForce;
    [SerializeField] float mainMenuBarrelCooldown;
    [SerializeField]GameObject lastUsedBarrel;



    private void OnEnable()
    {

        if (instance == null)
        {
            // if the object "instance does not exist", the object instance equals to gamemanager class
            instance = this;
            DontDestroyOnLoad(gameObject);


        }
        else
        {
            //if already exist, destroy everytime an scene is created
            Destroy(gameObject);
        }

        if (selectedPlayer == null)
        {
            selectedPlayer = defaultPlayer;
        }
        if (SceneManager.GetActiveScene().name.Equals(mainGameNameScene))
        {


            Instantiate(selectedPlayer);
            setPlayerValuesIn(selectedPlayer, mainGameNameScene);
            // Time.timeScale = 0;


        }





        // subscribes to the event this events happens when the scene loads
        PlayerScript.OnPlayerDied += pauseGame;
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }
    private void OnDisable()
    {
        // when the object is disable desuscribe from the event
        PlayerScript.OnPlayerDied -= pauseGame;
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;

    }



    public void stopCannonDash()
    {
        if(lastUsedBarrel != null)
        {
            lastUsedBarrel.GetComponent<Cannon>().StopCannonDash();
        }
    }
    public void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        
        if (scene.name == mainGameNameScene && GameObject.FindGameObjectWithTag("Player") == null)
        {
            // create a function that will execute when the event is called
            GameObject Pointer = GameObject.FindGameObjectWithTag("pointers");
            if (Pointer != null)
            {
                GameManager.instance.PlayerScore = 0;
                Vector3 SpawnPoint = Pointer.transform.GetChild(0).transform.position;
                GameObject.Find("TapToStart")?.SetActive(true);

                spawnPlayer(SpawnPoint, "Main_Game");

            }

            Time.timeScale = 0;


        }


        if (scene.name == rewardScene1Name && GameObject.FindGameObjectWithTag("Player") == null)
        {
            spawnPlayer(rewardSpawnPos, "reward_scene");


        }
        currentScene = scene.name;
    }
    public void shakeCamera(shakeType s)
    {
        // we call the camera and set the new state.
        Camera.main.GetComponent<FollowCamera4>().ShakeBehaviour = s;
        shakeCamera();

    }
    public void shakeCamera()
    {
        // shake camera with the current shakeState.
        Camera.main.GetComponent<FollowCamera4>().shakeCamera();
    }
    public GameObject spawnPlayer(Vector3 position, string option)
    {


        // normal means that the spawn point is defined
        // when the option is not normal the vector could be anyone
        if (SelectedPlayer != null)
        {
            // reference the object if you want to change some value or somethning afer instantaite it.

            GameObject player = Instantiate(SelectedPlayer, position, Quaternion.identity);
            if (option == "normal")
            {

                print("player instantiate at " + position.ToString());
                return player;

            }
            else if (setPlayerValuesInScenes == true)
            {
                if (option == "Main_Menu")
                {

                    setPlayerValuesIn(player, mainMenuNameScene);



                    print("player instantiate in mainMenu at " + position.ToString());
                    return player;

                }
                else if (option == "Main_Game")
                {
                    IsInRewardScene = false;

                    setPlayerValuesIn(player, mainGameNameScene);



                    print("player instantiate in gamescene at " + position.ToString());
                    return player;


                }
                else if (option == "reward_scene")
                {

                    setPlayerValuesIn(player, rewardScene1Name);

                    player.GetComponent<Rigidbody2D>().AddForce(Vector3.up * 100f, ForceMode2D.Impulse);
                    isInRewardScene = true;

                    print("player instantiate in reward scene at " + position.ToString());
                    return player;

                }
            }


        }
        return defaultPlayer;


    }

    public IEnumerator LoadSceneIn(float seconds, string scene)
    {

        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene(scene);
    }
    void pauseGame()
    {
        Debug.Log("player died " % Colorize.DarkOrange);
        Time.timeScale = 0;
    }

    public void reloadscene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    public void executeTransition1()
    {
        transitionManager.GetComponent<TransitionManager>().ExecuteTransition1();
    }
    public void ResetTriggerTrans1()
    {
        transitionManager.GetComponent<TransitionManager>().ResetTriggerTrans1();
    }

    void setPlayerValuesIn(GameObject Player, string scene)
    {
        if (scene == GameManager.instance.mainGameNameScene)
        {
            Player.transform.localScale = Vector3.one * playerMainGameScale;
            Player.GetComponent<Rigidbody2D>().mass = playerMainGameMass;
            Player.GetComponent<Rigidbody2D>().drag = playerMainGameDrag;
            Player.GetComponent<Rigidbody2D>().gravityScale = playerMainGameGravityScale;



        }
        else if (scene == GameManager.instance.mainMenuNameScene)
        {


            //playerSpeed = PlayerMainMenuSpeed;
            //Player.transform.localScale = Vector3.one * PlayerMainMenuScale;
            //Player.GetComponent<Rigidbody2D>().gravityScale = PlayerMainMenuGravityScale1;
            //Player.GetComponent<Rigidbody2D>().drag = PlayerMainMenuDrag;
            //Player.GetComponent<Rigidbody2D>().mass = PlayerMainMenuMass;

        }
        else if (scene == GameManager.instance.rewardScene1Name)
        {
            Player.transform.localScale = Vector3.one * playerRewardSceneScale;



            Player.GetComponent<Rigidbody2D>().AddForce(Vector3.up * 100f, ForceMode2D.Impulse);
            Player.GetComponent<Rigidbody2D>().gravityScale = playerRewardSceneGravity;
            Player.GetComponent<Rigidbody2D>().drag = playerRewardSceneLinearDrag;
            playerSpeed = playerRewardSceneSpeed;

            Player.GetComponent<Rigidbody2D>().mass = playerRewardSceneMass;
        }
    }




    public int EnemiesKilled { get => enemiesKilled; set => enemiesKilled = value; }
    public float PlayerScore { get => playerScore; set => playerScore = value; }
    public bool MainMenu { get => isInMainMenu; set => isInMainMenu = value; }
    public float PlayerSpeed { get => playerSpeed; set => playerSpeed = value; }
    public bool FirstBarrelPas { get => firstBarrelPass; set => firstBarrelPass = value; }
    public bool InBarrel { get => inBarrel; set => inBarrel = value; }
    public Vector3 BarrelPos { get => inBarrelPos; set => inBarrelPos = value; }
    public Vector3 PlayerPos { get => playerPos; set => playerPos = value; }
    public int EnemiesKilled1 { get => enemiesKilled; set => enemiesKilled = value; }
    public int PlayerCoins1 { get => playerCoins1; set => playerCoins1 = value; }
    public bool IsInMainMenu { get => isInMainMenu; set => isInMainMenu = value; }

    public Vector3 RewardSpawnPos { get => rewardSpawnPos; set => rewardSpawnPos = value; }
    public GameObject SelectedPlayer { get => selectedPlayer; set => selectedPlayer = value; }
    public int PlayerLife { get => playerLife; set => playerLife = value; }
    public float Highscore { get => playerHighscore; set => playerHighscore = value; }
    public int PlayerPurchasedHearts { get => playerPurchasedHearts; set => playerPurchasedHearts = value; }
    public int Waves { get => waves; set => waves = value; }
    public float EnemySeparation { get => enemySeparation; set => enemySeparation = value; }

    public int EnemyAmount { get => enemyAmount; set => enemyAmount = value; }
    public int TotalCoins { get => totalCoins; set => totalCoins = value; }
    public string TransformUpVectorChars
    {
        get { return transformUpVectorChars; }
        set { transformUpVectorChars = value; }
    }
    public int Char_Index
    {
        //get and setter
        get { return charIndex; }
        set { charIndex = value; }

    }
    public string Main_game_name
    {
        get { return mainGameNameScene; }
        set { mainGameNameScene = value; }
    }
    public bool IsInRewardScene
    {
        get { return isInRewardScene; }
        set { isInRewardScene = value; }
    }
    public string Main_menu_name
    {
        get { return mainMenuNameScene; }
        set { mainMenuNameScene = value; }
    }
    public string Character_selector_name
    {
        get { return characterSelectorNameScene; }
        set { characterSelectorNameScene = value; }
    }

    public int MovXButtons { get => movXButtons; set => movXButtons = value; }
    public float PlayerMainGameScale { get => playerMainGameScale; set => playerMainGameScale = value; }
    public float PlayerMainGameSpeed { get => playerMainGameSpeed; set => playerMainGameSpeed = value; }
    public float PlayerMainGameMass { get => playerMainGameMass; set => playerMainGameMass = value; }
    public float PlayerMainGameDrag { get => playerMainGameDrag; set => playerMainGameDrag = value; }
    public float PlayerMainGameGravityScale { get => playerMainGameGravityScale; set => playerMainGameGravityScale = value; }
    public float PlayerMainMenuGravityScale { get => PlayerMainMenuGravityScale1; set => PlayerMainMenuGravityScale1 = value; }

    public float FinalBarrelForce { get => finalBarrelForce; set => finalBarrelForce = value; }
    public float FinalBarrelCooldown { get => finalBarrelCooldown; set => finalBarrelCooldown = value; }

    public float PlayerMainMenuGravityScale1 { get => playerMainMenuGravityScale; set => playerMainMenuGravityScale = value; }
    public float PlayerMainMenuDrag { get => playerMainMenuDrag; set => playerMainMenuDrag = value; }
    public float PlayerMainMenuMass { get => playerMainMenuMass; set => playerMainMenuMass = value; }
    public float PlayerMainMenuScale { get => playerMainMenuScale; set => playerMainMenuScale = value; }
    public float PlayerMainMenuSpeed { get => playerMainMenuSpeed; set => playerMainMenuSpeed = value; }
    public float MainMenuBarrelForce { get => mainMenuBarrelForce; set => mainMenuBarrelForce = value; }
    public float MainMenuBarrelCooldown { get => mainMenuBarrelCooldown; set => mainMenuBarrelCooldown = value; }
    public GameObject LastUsedBarrel { get => lastUsedBarrel; set => lastUsedBarrel = value; }
}



