using System.Drawing;
using TMPro;
using UnityEngine;

public class UICoinManager : MonoBehaviour
{
 
     
    [SerializeField]
    TextMeshPro coinText;
    [SerializeField] Animator coinImage;
    private void Start()
    {
        UpdateCoinText(0);
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
        coinImage.Play("playerGetCoin",-1,0f);
        // if the coins value is 1 add 1;
        // there could be a coin that value more;
        currentCoin+=value;
        GameManager.instance.PlayerCoins = currentCoin;
        coinText.text = currentCoin.ToString()+"x";
         
    }
}
