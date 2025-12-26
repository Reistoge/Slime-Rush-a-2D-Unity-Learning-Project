using TMPro;
using Unity.Mathematics;
using UnityEngine;


public class scoreManager : MonoBehaviour
{
    public TextMeshProUGUI highscoreNumber;
    public TextMeshProUGUI scoreNumber;
    [SerializeField] PlayerScript player;
    float score = 0;

    


    // Update is called once per frame
    void Update()
    {
        checkScore();
    }
    public void checkScore()
    {
        if(GameManager.Instance.PlayerInScene == null)
        {
            return;
        }
        if (player == null)
        {
            player = GameManager.Instance.PlayerInScene.GetComponent<PlayerScript>();
        }

        else
        {
            if (player.PlayerScore >= score)
            {
                score = player.PlayerScore;
                player.PlayerScore = score;
            }


            score = math.round(score);
            scoreNumber.text = "Score: " + score.ToString();
            highscoreNumber.text = "Highscore: " + GameManager.Instance.Highscore.ToString();
            if (score >= GameManager.Instance.Highscore)
            {
                // new highscore !!
                GameManager.Instance.Highscore = score;
            }

        }



    }
}
