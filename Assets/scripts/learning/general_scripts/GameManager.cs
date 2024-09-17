using System;
using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{

    /// <summary>
    /// refactor initialization of variables in start, awake etc..
    /// </summary>
    //instance of an object
    public static GameManager instance;
    [SerializeField] GameObject coinReference;
    [Header("PLAYER SETTINGS")]
    //reference of the player
    [SerializeField] private GameObject selectedPlayer;
    [SerializeField] private GameObject defaultPlayer;
    [SerializeField] GameObject playerInScene;
    [SerializeField] private Vector3 playerPos;
    [SerializeField] private float playerSpeed;
    [SerializeField] private float playerScore;
    [SerializeField] int totalCoins;
    [SerializeField] private int playerLife;
    [SerializeField] private int playerPurchasedHearts = 3;
    [SerializeField] private int playerCoins;
    [SerializeField] GameObject screenController;

    [Header("Game Manager")]
    [SerializeField] string currentScene;
    //reference of the player life
    [SerializeField] float movXButtons;

    [SerializeField] private int charIndex = 0;
    [SerializeField] private float playerHighscore = 0;

    // [Header("CANNON VARIABLES")]
    // [SerializeField] private bool firstBarrelPass;
    // [SerializeField] private Vector3 inBarrelPos;
    // [SerializeField] private Vector3 rewardSpawnPos = new Vector3(0, -6.31f, 1);
    [SerializeField] SceneAsset mainGame;
    [SerializeField] SceneAsset mainMenu;
    [SerializeField] SceneAsset mainCharacterSelector;
    [SerializeField] GameObject lastUsedBarrel;

    [SerializeField] bool inBarrel;
    [SerializeField] int levelCount;
    [SerializeField] bool canMove;
    Vector3 startPos;

    private void OnEnable()
    {
        PlayerScript.OnPlayerDied += pauseGame;
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
        singletonLogic();

    }
    void Start()
    {
        if (playerInScene == null)
        {
            playerInScene = GameObject.FindWithTag("Player");
            if (playerInScene == null && GameObject.Find("LevelObjectsManager").activeInHierarchy==false)
            {
                instantiatePlayer();
            }
        }
        
    }
    void Update(){

        if(Input.GetKeyDown(KeyCode.LeftShift)){
            Time.timeScale=0.5f;

        }
        if(Input.GetKeyUp(KeyCode.LeftShift)){
            Time.timeScale=1f;
        }
    }
 

    public void instantiatePlayer()
    {
        if (selectedPlayer == null)
        {
            selectedPlayer = defaultPlayer;
        }
        // debugging
        if (SceneManager.GetActiveScene().name.Equals(MainGameSceneName))
        {
            PlayerInScene = Instantiate(selectedPlayer, startPos, Quaternion.identity);
            PlayerInScene.transform.parent = null;
        }
    }
    public void instantiatePlayer(Transform pos)
    {
        if (selectedPlayer == null)
        {
            selectedPlayer = defaultPlayer;
        }
        // debugging
        if (SceneManager.GetActiveScene().name.Equals(MainGameSceneName))
        {

            PlayerInScene = Instantiate(selectedPlayer, pos.position, pos.rotation);

        }
    }
    private void OnDisable()
    {
        // when the object is disable desuscribe from the event
        PlayerScript.OnPlayerDied -= pauseGame;
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }
    void singletonLogic()
    {
        if (instance == null)
        {
            // if the object "instance does not exist", the object instance equals to gamemanager class
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            //if already exist, destroy everytime an scene is created
            Destroy(this.gameObject);
        }
    }
    public void stopCannonDash()
    {
        if (LastUsedBarrel1 != null)
        {
            LastUsedBarrel1.GetComponent<Cannon>().stopCannonDash();
        }
    }
    public void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        // if (scene.name == MainGameSceneName && GameObject.FindGameObjectWithTag("Player") == null)
        // {
        //     instantiatePlayer();

        // }
        currentScene = scene.name;
    }
    public void shakeCamera(shakeType s)
    {
        // we call the camera and set the new state.
        Camera.main.GetComponent<FollowCamera>().ShakeBehaviour = s;
        shakeCamera();
    }
    public void shakeCamera()
    {
        // shake camera with the current shakeState.
        Camera.main.GetComponent<FollowCamera>().shakeCamera();
    }
    public void shakeCamera(float time, float magnitude)
    {
        // we call the camera and set the new state.

        Camera.main.GetComponent<FollowCamera>().shakeCamera(time, magnitude);
    }
    public void lerpCamera()
    {

        if (Camera.main.GetComponent<FollowCamera>() != null)
        {
            Camera.main.GetComponent<FollowCamera>().lerp();

        }
    }
    public void lerpCamera(float startPosY, float height)
    {

        if (Camera.main.GetComponent<FollowCamera>() != null)
        {
            Camera.main.GetComponent<FollowCamera>().lerp(startPosY, height);

        }
    }
    public void nextMiniLevel()
    {

        GameObject levelManager = GameObject.Find("LevelObjectsManager");
        
        if (levelManager.activeInHierarchy)
        {
            Camera.main.GetComponent<FollowCamera>().startZoom();
            if (levelManager != null)
            {
                LevelObjectManager manager = levelManager.GetComponent<LevelObjectManager>();
                // lerpCamera(manager.Level * 640, 640);
                if (manager != null)
                {
                    manager.Level += 1;
                    LevelCount += 1;

                    manager.instantiateNextLevel();

                }
            }
        }
        else{
            print("testing");
        }


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


    public float PlayerScore { get => playerScore; set => playerScore = value; }
    public float PlayerSpeed { get => playerSpeed; set => playerSpeed = value; }
    public int PlayerCoins { get => playerCoins; set => playerCoins = value; }
    public GameObject SelectedPlayer { get => selectedPlayer; set => selectedPlayer = value; }
    public int PlayerLife { get => playerLife; set => playerLife = value; }
    public float Highscore { get => playerHighscore; set => playerHighscore = value; }
    public int PlayerPurchasedHearts { get => playerPurchasedHearts; set => playerPurchasedHearts = value; }
    // public string TransformUpVectorChars
    // {
    //     get { return transformUpVectorChars; }
    //     set { transformUpVectorChars = value; }
    // }
    public int Char_Index
    {
        //get and setter
        get { return charIndex; }
        set { charIndex = value; }
    }
    public string MainGameSceneName
    {
        get { return mainGame.name; }
    }
    public string MainMenuSceneName
    {
        get { return mainMenu.name; }
    }
    public string CharacterSelector
    {
        get { return mainCharacterSelector.name; }
    }
    public float MovXButtons { get => movXButtons; set => movXButtons = value; }
    public GameObject LastUsedBarrel { get => LastUsedBarrel1; set => LastUsedBarrel1 = value; }
    public SceneAsset MainGame { get => mainGame; set => mainGame = value; }
    public bool InBarrel { get => inBarrel; set => inBarrel = value; }
    public Vector3 StartPos { get => startPos; set => startPos = value; }
    public GameObject PlayerInScene { get => playerInScene; set => playerInScene = value; }
    public GameObject ScreenController { get => screenController; set => screenController = value; }
    public int LevelCount { get => levelCount; set => levelCount = value; }
   
    public GameObject LastUsedBarrel1 { get => lastUsedBarrel; set => lastUsedBarrel = value; }
    public bool CanMove { get => canMove; set => canMove = value; }
}

