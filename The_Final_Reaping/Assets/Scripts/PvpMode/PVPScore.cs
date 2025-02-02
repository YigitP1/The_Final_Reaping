using UnityEngine;
using UnityEngine.UI;
public class PVPScore : MonoBehaviour
{
    public Text enemyScoreText;

    public Text playerScoreText;


    void Update()
    {
        int playerScore = PVPGameManager.playerScore;
        int enemyScore = PVPGameManager.enemyScore;

        playerScoreText.text = "Player Score: " + playerScore.ToString();

        enemyScoreText.text = "Enemy Score: " + enemyScore.ToString();
    }
}
