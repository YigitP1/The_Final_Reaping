using UnityEngine;
using UnityEngine.UI;
public class Score : MonoBehaviour
{
    public Text enemyScoreText;

    public Text playerScoreText;


    void Update()
    {
        int playerScore = GameManager.playerScore;
        int enemyScore = GameManager.enemyScore;

        playerScoreText.text = "Player Score: " + playerScore.ToString();

        enemyScoreText.text = "Enemy Score: " + enemyScore.ToString();
    }
}
