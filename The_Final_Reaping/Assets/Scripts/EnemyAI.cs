using System.Collections;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private Animator myAnimator;
    private Rigidbody2D myRigidBody;
    private GameObject player;
    Vector2 dashDirection = new Vector2(1, 0);

    // AI Parameters
    private float speed = 2f;
    private float attackRange = 3.2f;
    float timeSinceLastAttack = 0;
    public bool isAttacking = false;
    float attackCooldown = 1.6f;
    private float dashSpeed = 3f;
    public bool isParrying = false;
    public bool canAttack = true;
    public bool upperStance = true;
    public bool guardActive = true;
    public bool parryable = false;
    public bool canMove = true;
    public bool isGuardBreaking = false;
    public bool isGettingParried = false;
    private bool parrySuccessful;

    [SerializeField]
    int succesfulEvasion = 7;
    private string currentState;

    // State Control
    private enum AIState
    {
        MoveForward,
        WalkBackward,
        Stop,
    }

    private bool isDashing;
    private AIState currentAIState = AIState.Stop;

    // Stance cooldown
    private float stanceSwitchCooldown = 3.0f;
    private float nextStanceSwitchTime = 0f;


    // Animation handles
    const string AI_UPPER_GUARD = "Enemy_Upper_Idle";
    const string AI_LOWER_GUARD = "Enemy_Idle";
    const string AI_UPPER_ATTACK = "Enemy_Upper_Attack";
    const string AI_LOWER_ATTACK = "Enemy_Lower_Attack";
    const string AI_UPPER_PARRY = "Enemy_Upper_Parry";
    const string AI_LOWER_PARRY = "Enemy_Lower_Parry";
    const string AI_WALK_BACK = "Enemy_Walking_Back";
    const string AI_WALK = "Enemy_Walking_Forward";
    const string AI_UPPER_WALK = "Enemy_Upper_Walking_Forward";
    const string AI_UPPER_WALK_BACK = "Enemy_Upper_Walking_Back";
    const string AI_DASH = "Enemy_Upper_Forward_Dash";
    //const string AI_UPPER_FORWARD_DASH = "Enemy_Upper_Forward_Dash";
    //const string AI_LOWER_FORWARD_DASH = "Enemy_Lower_Forward_Dash";
    const string AI_UPPER_BACKWARD_DASH = "Enemy_Upper_Backwards_Dash";
    const string AI_LOWER_BACKWARD_DASH = "Enemy_Lower_Backwards_Dash";
    const string AI_LOWER_GUARDBREAK = "Enemy_Lower_GuardBreak";
    const string AI_UPPER_GUARDBREAK = "Enemy_Upper_GuardBreak";
    const string AI_UPPER_GETTING_PARRIED = "Enemy_Upper_Getting_Parried";
    const string AI_LOWER_GETTING_PARRIED = "Enemy_Lower_Getting_Parried";

    void Start()
    {
        myAnimator = GetComponent<Animator>();
        myRigidBody = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");

        StartCoroutine(RandomBehavior());
    }

    void FixedUpdate()
    {
        if (player != null && !isAttacking && !isParrying && !isDashing && !isGuardBreaking && !isGettingParried)
        {
            DecideStance();



            if (Vector2.Distance(transform.position, player.transform.position) <= attackRange)
            {
                AttemptAttackOrParry();
            }
            else
            {
                switch (currentAIState)
                {
                    case AIState.MoveForward:
                        MoveTowardPlayer();
                        break;
                    case AIState.WalkBackward:
                        WalkBackward();
                        break;
                    case AIState.Stop:
                        StopMoving();
                        break;
                }
            }
        }
        if (!isAttacking && myRigidBody.linearVelocity.magnitude == 0 && !isParrying && !isGettingParried && !isGuardBreaking)
        {
            if (upperStance)
            {

                ChangeAnimationState(AI_UPPER_GUARD);
            }
            else
            {
                ChangeAnimationState(AI_LOWER_GUARD);

            }
        }

    }

    void ReactToPlayerAttack(PlayerController playerController)
    {
        // Randomly decide to parry or dash
        if (Random.Range(0, 10) < succesfulEvasion)
        {
            if (Random.Range(0, 1) == 0 && playerController.parryable)
            {
                upperStance = playerController.upperStance;
                Debug.Log("Trying to parry");
                StartParry();
            }
            else
            {
                if (isDashing) return;
                Debug.Log("Trying to dash");
                if (upperStance)
                {
                    PerformUpperBackwardDash();
                }
                else
                {
                    PerformLowerBackwardDash();
                }

            }
        }
    }

    IEnumerator RandomBehavior()
    {
        while (true)
        {
            Debug.Log("Deciding behaviour");
            // Randomly choose a new state
            int action = Random.Range(0, 100);
            if (action > 30 && canMove)
            {
                Debug.Log("Move Forward");
                currentAIState = AIState.MoveForward;
            }
            else if (action > 20 && canMove)
            {
                Debug.Log("Walk Back");
                currentAIState = AIState.WalkBackward;
            }
            else
            {
                Debug.Log("Stop");
                currentAIState = AIState.Stop;
            }

            // Wait a random amount of time before deciding again
            yield return new WaitForSeconds(Random.Range(1f, 2f));
        }
    }

    void MoveTowardPlayer()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;
        myRigidBody.linearVelocity = new Vector2(direction.x * speed, myRigidBody.linearVelocity.y);

        if (upperStance)
            ChangeAnimationState(AI_UPPER_WALK);
        else
            ChangeAnimationState(AI_WALK);
    }

    void WalkBackward()
    {
        // Check if AI is at or beyond the backward limit
        if (transform.position.x >= 7.7f)
        {
            // Stop movement if limit is reached
            StopMoving();
            return;
        }

        // Move backward if not at the limit
        Vector2 direction = (transform.position - player.transform.position).normalized;
        myRigidBody.linearVelocity = new Vector2(direction.x * speed, myRigidBody.linearVelocity.y);
        if(upperStance){
            ChangeAnimationState(AI_UPPER_WALK_BACK);
        }
        else{

        ChangeAnimationState(AI_WALK_BACK);
        }
    }

    void StopMoving()

    {
        myRigidBody.linearVelocity = Vector2.zero;

        if (upperStance)
            ChangeAnimationState(AI_UPPER_GUARD);
        else
            ChangeAnimationState(AI_LOWER_GUARD);
    }

    void AttemptAttackOrParry()
    {
        

        if (isAttacking || isParrying || isGettingParried || isGuardBreaking) return; // Skip logic if already attacking or parrying

        // Stop movement while deciding action
        myRigidBody.linearVelocity = Vector2.zero;
        
        canMove = false;
        
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null && playerController.isAttacking && playerController.parryable)
        {
            Debug.Log("Parry or dash");
            ReactToPlayerAttack(playerController);
            timeSinceLastAttack = Time.time;
        }
        else if (!isAttacking && canAttack && (Time.time - timeSinceLastAttack) > attackCooldown)
        {
            Debug.Log("Attack");
            timeSinceLastAttack = Time.time;
            PerformAttack();
        }
    }
    public void GuardBreak(bool upperStance)
    {
        isGuardBreaking = true;
        currentAIState = AIState.Stop;
        canMove = false;
        if (upperStance)
        {
            ChangeAnimationState(AI_UPPER_GUARDBREAK);
        }
        else
        {
            ChangeAnimationState(AI_LOWER_GUARDBREAK);
        }
    }
    void GuardBreakComplete()
    {
        Debug.Log("Guard break");
        isGuardBreaking = false;
        canMove = true;
        guardActive = true;
        canAttack = true;
    }
    public void GettingParried(bool upperStance)
    {
        isGettingParried = true;
        canMove = false;
        canAttack = false;
        currentAIState = AIState.Stop;
        Debug.Log("GettingParried");

        if (upperStance)
        {
            ChangeAnimationState(AI_UPPER_GETTING_PARRIED);
        }
        else
        {
            ChangeAnimationState(AI_LOWER_GETTING_PARRIED);
        }
    }
    void GettingParriedComplete()
    {
        isGettingParried = false;
        canMove = true;
        canAttack = true;
    }


    void PerformAttack()
    {
        if (isAttacking) return; // Prevent multiple attack triggers

        isAttacking = true;
        guardActive = false; // Guard is down during the attack

        if (upperStance)
        {
            ChangeAnimationState(AI_UPPER_ATTACK);
        }
        else
        {
            ChangeAnimationState(AI_LOWER_ATTACK);
        }


    }


    void StartParry()
    {
        GuardBreakComplete();
        Debug.Log("parry");
        GuardBreakComplete();
        guardActive = false;
        isParrying = true;
        if (upperStance)
            ChangeAnimationState(AI_UPPER_PARRY);
        else
            ChangeAnimationState(AI_LOWER_PARRY);


    }


    /*void PerformDash()
    {
        isDashing = true;
        currentAIState = AIState.Stop;
        Debug.Log("Dashed");
        ChangeAnimationState(AI_DASH);
        myRigidBody.linearVelocity = new Vector2 (1 * 3, myRigidBody.linearVelocity.y);
    }*/
    void StartDash(string animationState)
    {

        if (isDashing) return;

        isDashing = true;
        currentAIState = AIState.Stop;

        // Play dash animation
        ChangeAnimationState(animationState);

        // Apply dash movement
        myRigidBody.linearVelocity = dashDirection * dashSpeed;



    }
    void PerformUpperBackwardDash()
    {
        StartDash(AI_UPPER_BACKWARD_DASH);
    }

    void PerformLowerBackwardDash()
    {
        StartDash(AI_LOWER_BACKWARD_DASH);
    }

    void CompleteDash()
    {
        isDashing = false;
        myRigidBody.linearVelocity = Vector2.zero;
    }

    void EndParry()
    {
        guardActive = true;
        isParrying = false;
    }

    void DecideStance()
    {

        if (Time.time >= nextStanceSwitchTime)
        {
            Debug.Log("Change Stance");
            upperStance = Random.Range(0, 2) == 0;
            nextStanceSwitchTime = Time.time + stanceSwitchCooldown;
        }
    }

    public void ChangeAnimationState(string newState)
    {
        if (currentState == newState)
            return;

        myAnimator.Play(newState);
        currentState = newState;
    }

    public void DeactivateGuard()
    {
        currentAIState = AIState.Stop;
        guardActive = false;
        canMove = false;
        canAttack = false;
        //TO DO:
        //go back
        //play some animation to indicate that guard is down
        //StartCoroutine(guardStun());
    }

    IEnumerator guardStun()
    {
        yield return new WaitForSeconds(2f);
        guardActive = true;
        canMove = true;
        canAttack = true;
    }

    public void AttackComplete()
    {
        Debug.Log("Attack Complete");
        //activate guard and set attacking false
        canMove = true;
        guardActive = true;
        isAttacking = false;
    }
    IEnumerator AttackCooldown(float cooldownTime)
    {
        isAttacking = true;
        yield return new WaitForSeconds(cooldownTime);
        AttackComplete();
    }
    public void ParrySuccessful()
    {
        EndParry();
        parrySuccessful = true;
        StartCoroutine(parryBonusEnd());

        //play some particle effect
        //start coroutine for faster attack animation (counter attack)
    }
    //parry unsuccessful
    public void ParryUnsuccessful()
    {
        EndParry();

    }
    IEnumerator parryBonusEnd()
    {
        yield return new WaitForSeconds(1f);
        parrySuccessful = false;
    }
}
