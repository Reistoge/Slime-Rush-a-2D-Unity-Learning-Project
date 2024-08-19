using TMPro;
using Unity.Mathematics;
using UnityEngine;


public class scoreManager : MonoBehaviour
{
    public TextMeshProUGUI highscoreNumber;
    public TextMeshProUGUI scoreNumber;
    float score = 0;




    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.PlayerScore >= score)
        {
            score = GameManager.instance.PlayerScore;
            GameManager.instance.PlayerScore = score;
        }


        score = math.round(score);
        scoreNumber.text = "Score: "+ score.ToString();
        highscoreNumber.text = "Highscore: "+ GameManager.instance.Highscore.ToString();
        if (score >= GameManager.instance.Highscore)
        {
            // new highscore !!
            GameManager.instance.Highscore = score;
        }


    }
}
