using TMPro;
using UnityEngine;

public class CoinText : MonoBehaviour
{
    // Start is called before the first frame update


    [SerializeField]
    TextMeshProUGUI coinText;
    private void Start()
    {
        UpdateCoinText(GameManager.instance.PlayerCoins1);
    }
    private void OnEnable()
    {
        PlayerScript.OnPlayerGetCoin += UpdateCoinText;
    }
    private void OnDisable()
    {
        PlayerScript.OnPlayerGetCoin -= UpdateCoinText;
    }


    public void UpdateCoinText(int coins)

    {
        int currentCoin = GameManager.instance.PlayerCoins1;
        // if the coins value is 1 add 1;
        // there could be a coin that value more;
        currentCoin++;
        GameManager.instance.PlayerCoins1 = currentCoin;
        coinText.text = currentCoin.ToString();
        Debug.Log(" + 1 coin");
    }
}
