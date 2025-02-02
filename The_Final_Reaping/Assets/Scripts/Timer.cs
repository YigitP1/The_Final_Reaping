using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public Text timerText;
    private float timeRemaining = 180f; // 3 minutes in seconds
    private bool timerIsRunning = true;
    GameManager gameManager;


    void Start()
    {
        UpdateTimerDisplay(timeRemaining);
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                UpdateTimerDisplay(timeRemaining);
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
                TimerEnded();
            }
        }
    }

    void UpdateTimerDisplay(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void TimerEnded()
    {
        Debug.Log("Time's up!");
        gameManager.TimerExpired();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Restart scene or end game
    }
    public void RestartTimer()
    {
        timeRemaining = 180f;
    }
}
