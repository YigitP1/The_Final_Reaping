using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HitDetection : MonoBehaviour
{
    private EnemyAI enemyController;
    private PlayerController playerController;
    private GameManager gameManager;
    private CameraShake cameraShake;
    public AudioClip slash;
    public MusicSound musicSound;

    private void Start()
    {
        musicSound = GameObject.FindGameObjectWithTag("Music").GetComponent<MusicSound>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        enemyController = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyAI>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        cameraShake = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShake>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.CompareTag("Enemy"))
        {
            if (enemyController.guardActive && (playerController.upperStance == enemyController.upperStance))
            {
                enemyController.DeactivateGuard();
                gameManager.FreezeFrame(0.15f);
                StartCoroutine(cameraShake.Shake(0.15f, 0.2f));
                enemyController.GuardBreak(enemyController.upperStance);
            }
            else
            {
                musicSound.SFX(slash);
                gameManager.PlayerScoreIncrement();
                gameManager.FreezeFrame(0.3f);
                StartCoroutine(cameraShake.Shake(0.3f, 0.3f));
                Debug.Log("Hit");
                
                gameManager.OnHitScored();

            }
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            if (playerController.guardActive && (playerController.upperStance == enemyController.upperStance))
            {
                playerController.DeactivateGuard();
                gameManager.FreezeFrame(0.15f);
                StartCoroutine(cameraShake.Shake(0.15f, 0.2f));
                playerController.GuardBreak(playerController.upperStance);
            }
            else
            {
                musicSound.SFX(slash);
                gameManager.EnemyScoreIncrement();
                gameManager.FreezeFrame(0.3f);
                StartCoroutine(cameraShake.Shake(0.3f, 0.3f));
                Debug.Log("Hit");

                gameManager.OnHitScored();
            }
        }
    }


}

