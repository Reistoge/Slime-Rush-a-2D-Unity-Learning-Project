using System.Collections;
using System.Drawing;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class UICoinManager : MonoBehaviour
{


    [SerializeField] TextMeshProUGUI coinText;
    [SerializeField] Animator coinImage;
    private void OnEnable()
    {


        PlayerScript.OnPlayerInstantiated += initializeCoins;
        PlayerScript.OnPlayerGetCoin += UpdateCoinText;
    }
    private void OnDisable()
    {

        PlayerScript.OnPlayerGetCoin -= UpdateCoinText;
        PlayerScript.OnPlayerInstantiated -= initializeCoins;
    }
    public void UpdateCoinText(int value)
    {

        int currentCoin = GameManager.Instance.getPlayerCoins();

        
        // if the coins value is 1 add 1;
        // there could be a coin that value more;
        int oldValue = currentCoin;
        int newValue = currentCoin + value;

        GameManager.Instance.setPlayerCoins(newValue);

        updateCoinText(oldValue, newValue);

    }
    void updateCoinText(int oldValue, int newValue)
    {
        // Start the coroutine to update the coin text with animation
        StartCoroutine(updateCoinTextCoroutine(oldValue, newValue));
    }
    IEnumerator updateCoinTextCoroutine(int oldValue, int newValue)
    {
        float waitTime = .01f; // Duration of the animation in seconds
    
        if (Mathf.Abs(newValue - oldValue) > 5)
        {
            int temp = oldValue;
            
            while(temp != newValue)
            {
                if(temp > newValue)
                {
                    temp -= 1;
                }
                else if(temp < newValue)
                {
                    temp += 1;
                }
                coinImage.Play("playerGetCoin", -1, 0f);
                coinText.text = temp.ToString() + " x";
                yield return new WaitForSeconds(waitTime);
            }
        }

        coinText.text = newValue.ToString() + " x";
    }
    public void initializeCoins()
    {
        coinImage.gameObject.SetActive(true);
        int currentCoin = GameManager.Instance.getPlayerCoins();
        coinImage.Play("playerGetCoin", -1, 0f);
        // if the coins value is 1 add 1;
        // there could be a coin that value more;
        coinText.text = currentCoin.ToString() + " x";

    }
}
