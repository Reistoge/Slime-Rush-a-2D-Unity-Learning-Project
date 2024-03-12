using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LifeScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] TextMeshProUGUI text_object;

    [SerializeField] public string text_content;

    private void OnEnable()
    {
        PlayerScript.OnEnemyIsDamaged += update_life_text;
        PlayerScript.OnEnemyIsDamaged += enemy_damage;
    }
    private void OnDisable()
    {
        PlayerScript.OnEnemyIsDamaged -= update_life_text;
        PlayerScript.OnEnemyIsDamaged -= enemy_damage;
    }




    public void update_life_text(int damage)
    {
        // HERES THE TEXT UPDATE OR MAY BE AN ICON IDK
        text_content =  Convert.ToString(GameManager.instance.PlayerLife);
        text_object.text = text_content;
        
    }
    public void enemy_damage(int damage)
    {
        print("Enemy damage Player, minus " + damage + " HP" % Colorize.Orange);

    }

}
