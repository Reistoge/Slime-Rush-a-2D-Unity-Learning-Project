using System;
using System.Collections;

using Unity.VisualScripting;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
[DefaultExecutionOrder(-100)]
public class GameManager : MonoBehaviour
{

    /// <summary>
    /// refactor initialization of variables in start, awake etc..
    /// </summary>
    //instance of an object
    private static GameManager instance;
    GameObject managerObject;
    [SerializeField] GameObject coinReference;
    [Header("PLAYER SETTINGS")]
    //reference of the player
    [SerializeField] private GameObject selectedPlayer;
    [SerializeField] private GameObject defaultPlayer;
    [SerializeField] GameObject playerInScene;
    [SerializeField] private Vector3 playerPos;
    [SerializeField] private int playerCoins;
    [SerializeField] private int playerInGameCoins;


    [Header("Game Manager")]
    [SerializeField] string currentScene;
    //reference of the player life
    [SerializeField] float movXButtons;
    [SerializeField] float movYButtons;

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
    [SerializeField] GameObject appearEffect;
    [SerializeField] GhostPoolManager ghostPoolManager;
    Vector3 startPos;
    [SerializeField] GameObject[] transitions;
    [SerializeField] GameObject transitionLoaded;
    [SerializeField] PlayerSO playerConfig;

    [SerializeField] PlayerScript playerScript;
    [SerializeField] PlayerRuntimeData playerRuntimeData;
    Coroutine transitionCoroutine;



    void Awake()
    {
        singletonLogic();
    }
    private void OnEnable()
    {
        PlayerScript.OnPlayerDied += pauseGame;
        LegacyEvents.GameEvents.onGameIsRestarted += destroyRuntimeData;
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }
    private void OnDisable()
    {
        PlayerScript.OnPlayerDied -= pauseGame;
        LegacyEvents.GameEvents.onGameIsRestarted -= destroyRuntimeData;
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void Start()
    {
        if (playerInScene == null)
        {
            playerInScene = GameObject.FindWithTag("Player");
            if (playerInScene == null) //&& GameObject.Find("LevelObjectsManager").activeInHierarchy == false)
            {
                // instantiatePlayer();



            }
        }

    }

    public IEnumerator enumerateThis(Action func, float seconds)
    {
        
        yield return new WaitForSeconds(seconds);
        func?.Invoke();
        
    }
    
    
 
    public LevelObjectManager getLevelObjectManager()
    {
        return (LevelObjectManager)GameObject.FindAnyObjectByType(typeof(LevelObjectManager));
    }
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Time.timeScale = 0.5f;

        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            Time.timeScale = 1f;
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Time.timeScale = 5f;
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            Time.timeScale = 1f;
        }


    }


    public void instantiatePlayer()
    {
        // selectedPlayer = GameObject.FindWithTag("Player");
        // if (selectedPlayer == null)
        // {
        //     selectedPlayer = defaultPlayer;
        // }

        if (GameObject.Find("startPos"))
        {
            startPos = GameObject.Find("startPos").transform.position;
            PlayerInScene = Instantiate(selectedPlayer, startPos, Quaternion.identity);
            PlayerInScene.transform.SetParent(null);

        }

    }
    public void instantiateDefaultPlayer()
    {
        startPos = GameObject.Find("startPos").transform.position;
        PlayerInScene = Instantiate(defaultPlayer, startPos, Quaternion.identity);
        PlayerInScene.transform.SetParent(null);
    }
    public void instantiatePlayer(Transform pos)
    {
        selectedPlayer = GameObject.FindWithTag("Player");
        if (selectedPlayer == null)
        {
            selectedPlayer = defaultPlayer;
        }
        if (selectedPlayer.activeInHierarchy == false)
        {
            selectedPlayer.SetActive(true);
            return;
        }
        // debugging
        if (SceneManager.GetActiveScene().name.Equals(MainGameSceneName))
        {

            PlayerInScene = Instantiate(selectedPlayer, pos.position, pos.rotation);

        }
    }

    void singletonLogic()
    {
        if (Instance == null)
        {
            // if the object "instance does not exist", the object instance equals to gamemanager class
            Instance = this;
            managerObject = this.gameObject;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            //if already exist, destroy everytime an scene is created

            Destroy(this.gameObject);
        }
    }
    public GameManager getInstance()
    {
        singletonLogic();
        return instance;
    }
    public void stopCannonDash()
    {
        if (LastUsedBarrel != null)
        {
            LastUsedBarrel.GetComponent<Cannon>().stopCannonDash();
        }
    }

    public void destroyTransition()
    {
        // used in the timeline
        if (transitionLoaded) Destroy(transitionLoaded);
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
    public shakeType cameraState()
    {
        return Camera.main.GetComponent<FollowCamera>().ShakeBehaviour;
    }
    public void changeCameraBehaviour(CameraBehaviour behaviour)
    {
        Camera.main.GetComponent<FollowCamera>().CameraType = behaviour;
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




    public void instantiateAppearEffect(Transform pos, int effectIndex)
    {
        // 0 : random spawn effect.
        GameObject effect = Instantiate(appearEffect, pos.position, Quaternion.identity);
        effect.transform.parent = null;

        if (effectIndex == 0) effect.GetComponent<Animator>().Play("smoke" + Random.Range(1, 3), -1, 0f);
        else effect.GetComponent<Animator>().Play("smoke" + effectIndex, -1, 0f);
        Destroy(effect, effect.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0).Length + 1f);

    }

    public void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {

        if (GameObject.Find("startPos"))
        { startPos = GameObject.Find("startPos").transform.position; }

        currentScene = scene.name;
        if (transitionLoaded)
        {

            transitionLoaded.GetComponent<PlayableDirector>().Resume();

        }
        if (GameObject.FindWithTag("PressToStart") == null)
        {

            instantiatePlayer();
        }
        checkSceneLoaded(scene);

    }

    void checkSceneLoaded(Scene scene)
    {

        switch (scene.name)
        {
            case "MainGame":
                LegacyEvents.GameEvents.triggerOnMainGameSceneLoaded();
                break;
            case "InGameShop":
                LegacyEvents.GameEvents.triggerOnInGameShopSceneLoaded();
                break;
            case "Menu":
                LegacyEvents.GameEvents.triggerOnMainMenuSceneLoaded();
                break;


        }
    }

    public IEnumerator LoadSceneIn(float seconds, string scene)
    {
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene(scene);
    }
    public void loadScene(string scene)
    {

        // if (transitionLoaded)
        // {
        //     transitionLoaded.GetComponent<PlayableDirector>().Pause();
        // }

        SceneManager.LoadScene(scene);

    }

    IEnumerator loadSceneWithTransitionCoroutine(string args)
    {


        string[] parts = args.Split(",");

        savePlayerRuntimeData();



        float waitTime = 0;
        if (parts.Length == 2) waitTime = float.Parse(parts[1]);
        yield return new WaitForSecondsRealtime(waitTime);

        transitionLoaded = Instantiate(transitions[0]);
        transitionLoaded.transform.position = new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y);
        Transition t = transitionLoaded.GetComponent<Transition>();
        DontDestroyOnLoad(transitionLoaded);
        yield return new WaitForSecondsRealtime(3);

        // yield return new WaitUntil(() => t.TransitionFinished == true);
        // t.TransitionFinished = false;
        loadScene(parts[0]);
        transitionCoroutine = null;



    }
    IEnumerator loadSceneWithTransitionCoroutine(LoadSceneWithTransitionSO config)
    {


        savePlayerRuntimeData();


        yield return new WaitForSecondsRealtime(config.secondsDelay);

        transitionLoaded = config.transitionPrefab;
        if (transitionLoaded != null)
        {
            transitionLoaded = Instantiate(config.transitionPrefab);
        }
        else
        {
            transitionLoaded = Instantiate(transitions[0]);
            Debug.LogError("Transition prefab is null, using default transition prefab.");
        }
        transitionLoaded.transform.position = new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y);
        Transition t = transitionLoaded.GetComponent<Transition>();

        DontDestroyOnLoad(transitionLoaded);
        yield return new WaitUntil(() => t.SceneCanBeLoaded == true);
        //Time.timeScale = 1.0f;
        // t.TransitionFinished = false;

        loadScene(config.sceneAsset.name);
        transitionCoroutine = null;



    }


    private void savePlayerRuntimeData()
    {
        if (playerInScene != null)
        {
            PlayerScript playerScript = playerInScene.GetComponent<PlayerScript>();
            if (playerScript != null)
            {
                if (playerRuntimeData == null)
                {
                    playerRuntimeData = new PlayerRuntimeData();
                }
                playerRuntimeData.playerInGameCoins = playerScript.Coins;
                playerRuntimeData.playerHp = playerScript.Hp;

            }
        }
    }

    public PlayerRuntimeData getRuntimeData()
    {
        if (playerRuntimeData == null || playerRuntimeData.playerHp <= 0)
        {
            playerRuntimeData = new PlayerRuntimeData();
            playerRuntimeData.playerInGameCoins = 0;
            playerRuntimeData.playerHp = playerConfig.maxHp;

        }
        return playerRuntimeData;
    }
    public void resetPlayerRuntimeData()
    {
        playerRuntimeData = new PlayerRuntimeData();
        playerRuntimeData.playerInGameCoins = 0;
        playerRuntimeData.playerHp = playerConfig.maxHp;
    }
    public void destroyRuntimeData()
    {

        // playerRuntimeData = new PlayerRuntimeData();
        // playerRuntimeData.playerInGameCoins = 0;
        // playerRuntimeData.playerHp = playerConfig.maxHp;

        //  savePlayerRuntimeData();
    }

    public void loadSceneWithTransition(string args)
    {
        if (transitionLoaded == null && transitionCoroutine == null)
        {
            transitionCoroutine = StartCoroutine(loadSceneWithTransitionCoroutine(args));
        }
    }
    public void loadSceneWithTransition(LoadSceneWithTransitionSO config)
    {
        if (transitionLoaded == null && transitionCoroutine == null)
        {
            transitionCoroutine = StartCoroutine(loadSceneWithTransitionCoroutine(config));
        }
    }




    public void pauseGame()
    {

        Time.timeScale = 0;




    }
    public void resumeGame()
    {
        Time.timeScale = 1;

    }
    public void reloadscene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void tutorialEnd(string arg)
    {



    }
    IEnumerator finishedTutorialCoroutine()
    {
        PlayerScript playerScript = playerInScene.GetComponent<PlayerScript>();
        yield return new WaitUntil(() => playerScript.IsDashing == false);
        playerScript.Rb.isKinematic = true;
        playerInScene.SetActive(false);





    }
    public int getPlayerCoins()
    {
        return playerConfig.totalCoins;
    }
    public void setPlayerCoins(int coins)
    {
        playerConfig.totalCoins = coins;


    }
    public void onPlayerGetCoins(int value)
    {
        if (playerInScene != null)
        {
            playerInScene.GetComponent<PlayerScript>().playerGetCoin(value);
        }
    }
    [Serializable]
    public class PlayerRuntimeData
    {

        public int playerInGameCoins;
        public int playerHp;
        public int redHearts;
        public int blueHearts;
        public int playerMaxHp;
    }









    //public float PlayerScore { get => playerScore; set => playerScore = value; }

    public GameObject SelectedPlayer { get => selectedPlayer; set => selectedPlayer = value; }
    //public int PlayerLife { get => playerLife; set => playerLife = value; }
    public float Highscore { get => playerHighscore; set => playerHighscore = value; }
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
    public GameObject LastUsedBarrel { get => lastUsedBarrel; set => lastUsedBarrel = value; }
    public SceneAsset MainGame { get => mainGame; set => mainGame = value; }
    public bool InBarrel { get => inBarrel; set => inBarrel = value; }
    public Vector3 StartPos { get => startPos; set => startPos = value; }
    public GameObject PlayerInScene { get => playerInScene; set => playerInScene = value; }
    public int LevelCount { get => levelCount; set => levelCount = value; }

    //public GameObject LastUsedBarrel1 { get => lastUsedBarrel; set => lastUsedBarrel = value; }
    public bool CanMove { get => canMove; set => canMove = value; }
    public float MovYButtons { get => movYButtons; set => movYButtons = value; }
    public float MovYButtons1 { get => movYButtons; set => movYButtons = value; }
    public GhostPoolManager GhostPoolManager { get => ghostPoolManager; set => ghostPoolManager = value; }
    public PlayerSO PlayerConfig { get => playerConfig; set => playerConfig = value; }
    public static GameManager Instance { get => instance; set => instance = value; }
    //public int InitCoins { get => initCoins; set => initCoins = value; }
}

