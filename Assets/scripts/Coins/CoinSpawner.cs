using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject coinPrefab;
    GameObject spawnCoin;
    [SerializeField]
    float timer;
    [SerializeField]
    private float separation;

    [SerializeField]
    float coinYVelocity;






    [SerializeField]
    Transform playerSpawnPoint;
    // Start is called before the first frame update
    int counter;

    // Update is called once per frame
    private void Start()
    {
        timer = GameManager.instance.EnemiesKilled * 10;

        //coinYVelocity = (GameManager.instance.getPlayerCoin1()+GameManager.instance.EnemiesKilled)*100/GameManager.instance.PlayerScore;
        float positionY = playerSpawnPoint.position.y;
        InvokeRepeating("SpawnCoin", 0, 1f);
        GameManager.instance.Waves++;

    }
    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            CancelInvoke("SpawnCoin");
            StartCoroutine(GameManager.instance.LoadSceneIn(3f, "GAME_SCENE"));
        }
    }
    void SpawnCoin()
    {

        float positionY = playerSpawnPoint.position.y;

        //for (int i = 0; i < totalCoins; i++)

        // there is a better way to check this.
        // reset the position instead of instantiate.
        positionY += separation;

        spawnCoin = Instantiate(coinPrefab, new Vector3(Random.Range(-2f, 2f), positionY, coinPrefab.transform.position.z), Quaternion.identity);
        spawnCoin.GetComponent<Rigidbody2D>().velocity = -Vector2.up * coinYVelocity;
        spawnCoin.gameObject.transform.SetParent(transform, true);

        spawnCoin = Instantiate(coinPrefab, new Vector3(Random.Range(-2f, 2f), positionY, coinPrefab.transform.position.z), Quaternion.identity);
        spawnCoin.GetComponent<Rigidbody2D>().velocity = -Vector2.up * coinYVelocity;
        spawnCoin.gameObject.transform.SetParent(transform, true);

        spawnCoin = Instantiate(coinPrefab, new Vector3(Random.Range(-2f, 2f), positionY, coinPrefab.transform.position.z), Quaternion.identity);
        spawnCoin.GetComponent<Rigidbody2D>().velocity = -Vector2.up * coinYVelocity;
        spawnCoin.gameObject.transform.SetParent(transform, true);


        //if (i == totalCoins - 1)

        //{
        //  finalCoin = spawnCoin;
        //}






    }




    public float CoinYVelocity
    {
        get { return coinYVelocity; }
        set { coinYVelocity = value; }
    }


}
