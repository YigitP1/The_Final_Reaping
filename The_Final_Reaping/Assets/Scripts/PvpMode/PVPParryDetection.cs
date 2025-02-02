using UnityEngine;

public class PVPParryDetection : MonoBehaviour
{
    private SecondPlayerController enemyController;
    private PlayerController playerController;
    private GameManager gameManager;
    private CameraShake cameraShake;



    private void Start()
    {

        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        enemyController = GameObject.FindGameObjectWithTag("Enemy").GetComponent<SecondPlayerController>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        cameraShake = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShake>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.CompareTag("Enemy"))
        {
            if (enemyController.parryable && (playerController.upperStance == enemyController.upperStance))
            {
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
