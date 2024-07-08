using UnityEngine;

public class Coin : MonoBehaviour
{



    public void playerGetCoin()
    {
        this.GetComponent<Animator>().SetTrigger("getCoin");
    }
    public void destroyCoin()
    {
        this.gameObject.SetActive(false);
    }

}
