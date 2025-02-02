using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PVPGameManager : MonoBehaviour
{
    private bool isFrozen = false;
    public GameObject player;
    public GameObject enemy;
    public Vector3 playerStartPosition;
    public Vector3 enemyStartPosition;
    PlayerController playerController;
    SecondPlayerController enemyAI;
    public Timer timer;
    GameObject timerText;
    static public int enemyScore = 0;
    static public int playerScore = 0;
    private static int timerExpiryCount = 0;

    public CanvasGroup fadeScreen;

    private void Awake()
    {

    }
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        // Save initial positions
        playerStartPosition = player.transform.position;
        enemyStartPosition = enemy.transform.position;
        playerController = player.GetComponent<PlayerController>();
        enemyAI = enemy.GetComponent<SecondPlayerController>();
        // Ensure the fade screen starts transparent
        fadeScreen.alpha = 0f;
        timer = GameObject.FindGameObjectWithTag("Timer").GetComponent<Timer>();
        timerText = GameObject.FindGameObjectWithTag("TimerText");
    }
    public void FreezeFrame(float duration)
    {
        if (isFrozen) return; // Prevent overlapping freezes
        StartCoroutine(FreezeCoroutine(duration));
    }

    private IEnumerator FreezeCoroutine(float duration)
    {
        isFrozen = true;

        // Save the current time scale
        float originalTimeScale = Time.timeScale;
        Time.timeScale = 0f;

        // Wait for the freeze duration in real-time seconds
        yield return new WaitForSecondsRealtime(duration);

        // Restore the original time scale
        Time.timeScale = originalTimeScale;
        isFrozen = false;
    }

    public void OnHitScored()
    {
        // Trigger the fade and reset sequence
        StartCoroutine(FadeAndReset());
    }

    private IEnumerator FadeAndReset()
    {

        // Fade to black
        yield return StartCoroutine(FadeScreen(1f));

        // Reset positions
        ResetPositions();

        // Wait briefly (optional, for dramatic effect)
        yield return new WaitForSeconds(0.5f);
        CheckWinCondition();


        // Fade back in
        yield return StartCoroutine(FadeScreen(0f));
        //play animation
        enemy.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        player.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        playerController.ChangeAnimationState("Player_Lower_Guard");
        enemyAI.ChangeAnimationState("Enemy_Lower_Guard");
        enemyAI.enabled = false;
        playerController.enabled = false;
        timerText.SetActive(false);
        yield return new WaitForSeconds(3f);
        //enable scripts
        timer.RestartTimer();
        timerText.SetActive(true);
        playerController.enabled = true;
        enemyAI.enabled = true;
    }

    private IEnumerator FadeScreen(float targetAlpha)
    {
        float duration = 0.2f; // Adjust fade duration as needed
        float startAlpha = fadeScreen.alpha;
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            fadeScreen.alpha = Mathf.Lerp(startAlpha, targetAlpha, timeElapsed / duration);
            yield return null;
        }

        fadeScreen.alpha = targetAlpha;
    }

    private void ResetPositions()
    {
        // Reset player and enemy positions
        player.transform.position = playerStartPosition;
        enemy.transform.position = enemyStartPosition;
        playerController.guardActive = true;
        playerController.upperStance = false;
        playerController.parryable = false;
        playerController.attackIsPressed = false;
        playerController.isAttacking = false;
        playerController.parryIsPressed = false;
        playerController.isParrying = false;
        playerController.isDashing = false;
        playerController.dashIsPressed = false;
        playerController.parrySuccessful = false;
        playerController.canMove = true;
        playerController.isGuardBreaking = false;
        playerController.isGettingParried = false;
        playerController.ableToMove = true;

        enemyAI.guardActive = true;
        enemyAI.upperStance = false;
        enemyAI.parryable = false;
        enemyAI.attackIsPressed = false;
        enemyAI.isAttacking = false;
        enemyAI.parryIsPressed = false;
        enemyAI.isParrying = false;
        enemyAI.isDashing = false;
        enemyAI.dashIsPressed = false;
        enemyAI.parrySuccessful = false;
        enemyAI.canMove = true;
        enemyAI.isGuardBreaking = false;
        enemyAI.isGettingParried = false;
        enemyAI.ableToMove = true;

        // Reset any other necessary states (e.g., animations, health)

        Debug.Log("Positions reset");
    }
    public void PlayerScoreIncrement()
    {
        playerScore++;

    }
    public void EnemyScoreIncrement()
    {
        enemyScore++;

    }

    public void CheckWinCondition()
    {
        if (enemyScore >= 2)
        {
            // A player reached 15 points, go to a different scene
            GoToEndScene();
        }
        if (playerScore >= 2)
        {
            GoToNextScene();
        }
    }

    public static void TimerExpired()
    {
        timerExpiryCount++;
        if (timerExpiryCount >= 3)
        {
            // Timer expired 3 times, go to a different scene
            if (playerScore > enemyScore)
            {
                GoToNextScene();
            }
            else
            {
                GoToEndScene();
            }
        }
    }

    private static void GoToEndScene()
    {
        SceneManager.LoadScene("GameOver"); // Replace "EndScene" with your desired scene name
    }
    private static void GoToNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}

