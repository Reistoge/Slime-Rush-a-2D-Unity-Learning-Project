using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    public delegate void Game_events();
    public static event Game_events GameStarted;

    [SerializeField] private int spawnInt;

    // Start is called before the first frame 
    [SerializeField] private GameObject barrelPrefab;
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject Barrel;
    [SerializeField] private Transform spawnPlayer;

    // value to send the player to the other sector



    // vectors references
    [SerializeField] private Vector2 currentPos;
    [SerializeField] private Vector2 currentVel;
    [SerializeField] private Quaternion currentRot;
    [SerializeField] private Vector3 initialPos;
    [SerializeField] private Quaternion initialRot;





    // the rigidbody of the player
    Rigidbody2D rb;
    private Transform PlayerPos;

    EnemyClass Minion = new EnemyClass("JUAN", 3, 4, "BOW");
    Warrior Warrior1 = new Warrior("Guts", 5, 10, "MegaSword", "Impolute Platinum");



    IEnumerator Start()
    {


        if (GameStarted != null)
        {
            GameStarted();
        }

        Player = GameObject.FindWithTag("Player");
        yield return new WaitForSeconds(0);
        Player.transform.position = spawnPlayer.position;
        spawnBarrel(spawnInt);
        rb = Player.GetComponent<Rigidbody2D>();
        PlayerPos = Player.transform;
        initialPos = spawnPlayer.position;
        initialRot = Player.transform.rotation;



        //warrior class examples 
        // the player class has an attack function with a parameter i pass the name of warrior1 class warrior
        Minion.attack(Warrior1.Name);
        Minion.Health = 5;
        Minion.getHealth();


        Warrior1.attack(Minion.Name);
        Warrior1.Health = 10;
        Warrior1.DisplayArmor();

        Warrior1.talk();
        Minion.talk();




    }


    // Update is called once per frame
    void Update()

    {





        // here goes the behaviour
        if (Input.GetKeyDown(KeyCode.H))
        {
            Player.transform.localScale -= Vector3.one;
            if (Player.transform.localScale.x <= 1 && Player.transform.localScale.y <= 1)
            {
                Player.transform.localScale = Vector3.one;

            }

        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            Player.transform.localScale += Vector3.one;
            if (Player.transform.localScale.x >= 5 && Player.transform.localScale.y >= 5)
            {
                Player.transform.localScale = Vector3.one * 5;

            }
        }

        home();

        if (Input.GetKeyDown(KeyCode.P))
        {

            spawnBarrel(1);

        }


        if (Input.GetKeyDown(KeyCode.R))
        {


        }
        // current vel and position of the object
        currentRot = Player.transform.rotation;
        currentPos = Player.transform.position;
        currentVel = Player.GetComponent<Rigidbody2D>().velocity;


    }
    void spawnBarrel(int n)
    {
        for (int i = 0; i < n; i++)
        {
            Vector2 randomSpawnPosition = new Vector2(Random.Range(-6, 6), Random.Range(-2.5f, 2.5f));
            // you have to set the rotation of the object.
            //transform=vector3+quaternion
            Instantiate(barrelPrefab, randomSpawnPosition, Quaternion.identity);

        }


    }

    void home()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("Main Menu2");
        }

    }



}

