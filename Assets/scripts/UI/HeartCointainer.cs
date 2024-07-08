using UnityEngine;

public class HeartCointainer : MonoBehaviour
{
    [SerializeField]
    GameObject redHeart;
    [SerializeField]
    Transform heartInitpos;
    [SerializeField]
    int Childs;
    int brokenHearts;
    int healthHearts;
    int fixHearts;

    // Start is called before the first frame update
    void Start()
    {
        // CHARGE THE HEARTS THAT THE PLAYER HAS
        LoadHearts(redHeart);
        // ADD THE FUNCTION TO THE EVENt
        PlayerScript.OnPlayerIsDamaged += BrokeHearts;
    }
    private void OnDisable()
    {
        PlayerScript.OnPlayerIsDamaged -= BrokeHearts;
    }

    // Update is called once per frame

    private void FixHeart()
    {

    }
    void BrokeHearts(int damage)
    {
        //we start at child and not chld-1 because of the position child


        for (int i = Childs; i >= 0; i--)
        {

            GameObject heart = gameObject.transform.GetChild(i).gameObject;
            if (heart.GetComponent<Animator>())
            {
                if (!heart.GetComponent<Animator>().GetBool("brokeHeart"))
                {
                    heart.GetComponent<Animator>().SetBool("brokeHeart", true);
                    damage--;
                    brokenHearts++;
                }


                if (damage == 0)
                {
                    break;
                }

            }

            else
            {
                Debug.Log("No more hearts to break, player has to die");
            }

        }
    }
    void LoadHearts(GameObject Heart)
    {
        // in canvas something happens with the positions
        float x = 0;
        Childs = 0;
        Vector3 HeartInipos = heartInitpos.position;
        for (int i = 0; i < GameManager.instance.PlayerPurchasedHearts; i++)
        {
            float Xpos = HeartInipos.x;
            Vector3 newPos = new Vector3(Xpos + x, HeartInipos.y, 1);
            GameObject spawnHeart = Instantiate(Heart, newPos, Quaternion.identity, transform);
            Childs++;
            x += 32f;
        }
    }

}




