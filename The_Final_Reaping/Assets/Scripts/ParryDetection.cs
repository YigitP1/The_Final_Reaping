using UnityEngine;

public class ParryDetection : MonoBehaviour
{
    private EnemyAI enemyController;
    private PlayerController playerController;
    private GameManager gameManager;
    private CameraShake cameraShake;
    public AudioClip sword;
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
            if (enemyController.parryable && (playerController.upperStance == enemyController.upperStance))
            {
                musicSound.SFX(sword);
                enemyController.AttackComplete();
                //enemyController.DeactivateGuard();
                StartCoroutine(cameraShake.Shake(0.1f, 0.2f));
                gameManager.FreezeFrame(0.1f);
                enemyController.GettingParried(enemyController.upperStance);
                Debug.Log("Parry");
            }
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            if (playerController.parryable && (playerController.upperStance == enemyController.upperStance))
            {
                musicSound.SFX(sword);
                playerController.AttackComplete();
                //playerController.DeactivateGuard();
                StartCoroutine(cameraShake.Shake(0.1f, 0.2f));
                gameManager.FreezeFrame(0.1f);
                playerController.GettingParried(playerController.upperStance);
                Debug.Log("Parryyyy");
            }
        }
    }


}
