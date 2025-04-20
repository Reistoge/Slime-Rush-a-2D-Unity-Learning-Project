using System;
using TMPro;
using UnityEngine;

public class LifeScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] TextMeshProUGUI text_object;

    [SerializeField] public string text_content;

    private void OnEnable()
    {
        PlayerScript.OnEnemyIsDamaged += updateLifeText;
        PlayerScript.OnEnemyIsDamaged += enemyDamage;
    }
    private void OnDisable()
    {
        PlayerScript.OnEnemyIsDamaged -= updateLifeText;
        PlayerScript.OnEnemyIsDamaged -= enemyDamage;
    }




    public void updateLifeText(int damage)
    {
        // HERES THE TEXT UPDATE OR MAY BE AN ICON IDK
        if(GameManager.instance.PlayerInScene.GetComponent<PlayerScript>())
        text_content = Convert.ToString(GameManager.instance.PlayerInScene.GetComponent<PlayerScript>());
        text_object.text = text_content;

    }
    public void enemyDamage(int damage)
    {
        print("Enemy damage Player, minus " + damage + " HP" % Colorize.Orange);

    }

}
