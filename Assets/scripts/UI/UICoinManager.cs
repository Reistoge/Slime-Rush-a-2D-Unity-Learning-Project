using TMPro;
using UnityEngine;

public class UICoinManager : MonoBehaviour
{
 
     
    [SerializeField]
    TextMeshProUGUI coinText;
    private void Start()
    {
        UpdateCoinText(GameManager.instance.PlayerCoins);
    }
    private void OnEnable()
    {
        PlayerScript.OnPlayerGetCoin += UpdateCoinText;
    }
    private void OnDisable()
    {
        PlayerScript.OnPlayerGetCoin -= UpdateCoinText;
    }
    public void UpdateCoinText(int value)

    {
        int currentCoin = GameManager.instance.PlayerCoins;
        // if the coins value is 1 add 1;
        // there could be a coin that value more;
        currentCoin+=value;
        GameManager.instance.PlayerCoins = currentCoin;
        coinText.text = currentCoin.ToString();
         
    }
}
