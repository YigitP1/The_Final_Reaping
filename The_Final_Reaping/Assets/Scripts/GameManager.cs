using NUnit.Framework;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool isFrozen = false;
    public GameObject player;
    public GameObject enemy;
    public Vector3 playerStartPosition;
    public Vector3 enemyStartPosition;
    PlayerController playerController;
    EnemyAI enemyAI;
    Timer timer;
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
        enemyAI = enemy.GetComponent<EnemyAI>();
        // Ensure the fade screen starts transparent
        fadeScreen.alpha = 0f;
        timer = GameObject.FindGameObjectWithTag("Timer").GetComponent<Timer>();
        timerText = GameObject.FindGameObjectWithTag("TimerText");

        fadeScreen.alpha = 1f;

        // Start fade-in
        StartCoroutine(FadeScreen(0f));


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

        if (CheckWinCondition() == 0){
            GoFade();
        }
        
    }

    private void GoFade(){
        StartCoroutine(FadeAndReset());
    }
    private IEnumerator FadeAndReset()
    {

        // Fade to black
        yield return StartCoroutine(FadeScreen(1f));

        // Reset positions
        ResetPositions();
        enemy.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        player.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        playerController.ChangeAnimationState("Player_Lower_Guard_Idle");
        enemyAI.ChangeAnimationState("Enemy_Idle");
        enemyAI.enabled = false;
        playerController.enabled = false;

        // Wait briefly (optional, for dramatic effect)
        yield return new WaitForSeconds(0.5f);
        
        CheckWinCondition();
        // Fade back in
        yield return StartCoroutine(FadeScreen(0f));
        //play animation
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
        float duration = 0.4f; // Adjust fade duration as needed
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

        // Reset any other necessary states (e.g., animations, health)
        EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
        enemyAI.isAttacking = false;
        enemyAI.isParrying= false;
        enemyAI.canAttack = true;
        enemyAI.upperStance = false;
        enemyAI.guardActive = true;
        enemyAI.parryable = false;
        enemyAI.canMove = true;
        enemyAI.isGuardBreaking = false;
        enemyAI.isGettingParried = false;
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

    public int CheckWinCondition()
    {
        if (enemyScore >= 15)
        {
            // A player reached 15 points, go to a different scene
            GoToEndScene();
            return 0;
        }
        if (playerScore >= 15)
        {
            StartCoroutine(PlayEndAnimation());
            return 1;
        }
        else{
            return 0;
        }
    }

    public void TimerExpired()
    {
        timerExpiryCount++;
        if (timerExpiryCount >= 3)
        {
            // Timer expired 3 times, go to a different scene
            if (playerScore > enemyScore)
            {
                StartCoroutine(PlayEndAnimation());
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
        playerScore = 0;
        enemyScore = 0;
    }
    private IEnumerator PlayEndAnimation(){
        enemyAI.ChangeAnimationState("Enemy_Fly");
        enemyAI.enabled = false;
        enemy.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(0, 3); 
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(FadeScreen(1f));
        yield return new WaitForSeconds(1.5f);
        GoToNextScene();
    }
}

