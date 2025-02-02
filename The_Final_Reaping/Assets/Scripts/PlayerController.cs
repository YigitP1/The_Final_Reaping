using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //implement getting parried animation
    //initializing stuff
    private Animator myAnimator;
    private Rigidbody2D myRigidBody;
    private float speed = 2f;
    public string currentState;
    private float inputX = 0;
    private float dashSpeed = 3f;
    private float attackCooldown = 1.3f;
    private float timeSinceLastAttack = 0;
    private float parryCooldown = 0.8f;
    private float timeSinceLastParry = 0;

    private GameObject myEnemy;

    //stances
    public bool guardActive = true;
    public bool upperStance = false;

    //current status effects
    public bool parryable = false;
    public bool ableToMove = true;

    //bools to use with inputs 
    public bool attackIsPressed;
    public bool isAttacking = false;
    public bool parryIsPressed = false;
    public bool isParrying = false;
    public bool isDashing = false;
    public bool dashIsPressed = false;
    public bool parrySuccessful = false;
    public bool canMove = true;
    public bool isGuardBreaking = false;
    public bool isGettingParried = false;


    //Animation handles
    const string PLAYER_UPPER_GUARD = "Player_Upper_Guard_Idle";
    const string PLAYER_LOWER_GUARD = "Player_Lower_Guard_Idle";
    const string PLAYER_UPPER_ATTACK = "Player_Upper_Attack";
    const string PLAYER_LOWER_ATTACK = "Player_Lower_Attack";
    const string PLAYER_LOWER_WALK_FORWARD = "Player_Walk_Forward";
    const string PLAYER_UPPER_WALK_FORWARD = "Player_Upper_Walk";
    const string PLAYER_UPPER_WALK_BACK = "Player_Upper_Walk_Backward";
    const string PLAYER_UPPER_GETTING_PARRIED = "Player_Upper_Getting_Parried";
    const string PLAYER_LOWER_GETTING_PARRIED = "Player_Lower_Getting_Parried";
    const string PLAYER_WALK_BACK = "Player_Walk_Back";
    const string PLAYER_LUNGE_ATTACK = "Player_Lunge_Attack";
    const string PLAYER_UPPER_PARRY = "Player_Upper_Parry";
    const string PLAYER_LOWER_PARRY = "Player_Lower_Parry";
    const string PLAYER_UPPER_DASH_FORWARD = "Player_Upper_Dash_Forward";
    const string PLAYER_LOWER_DASH_FORWARD = "Player_Lower_Dash_Forward";
    const string PLAYER_UPPER_DASH_BACKWARD = "Player_Upper_Dash_Backward";
    const string PLAYER_LOWER_DASH_BACKWARD = "Player_Lower_Dash_Backward";
    const string PLAYER_LOWER_GUARDBREAK = "Player_Lower_GuardBreak";
    const string PLAYER_UPPER_GUARDBREAK = "Player_Upper_GuardBreak";




    void Start()
    {

        myEnemy = GameObject.FindGameObjectWithTag("Enemy");
        myAnimator = GetComponent<Animator>();
        myRigidBody = GetComponent<Rigidbody2D>();


    }



    void Update()
    {

        //getting movement input
        inputX = Input.GetAxis("Horizontal");
        //getting attack input
        if (Input.GetKeyDown(KeyCode.RightArrow) && (Time.time - timeSinceLastAttack) > attackCooldown && !isDashing && !isGettingParried && !isParrying)
        {
            attackIsPressed = true;
            timeSinceLastAttack = Time.time;

        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            upperStance = false;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            upperStance = true;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) && ((Time.time - timeSinceLastParry) > parryCooldown) && !isParrying && !isDashing && !isAttacking)
        {
            parryIsPressed = true;
            timeSinceLastParry = Time.time;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isParrying && !isDashing && !isAttacking && !isGettingParried)
        {
            dashIsPressed = true;
        }

    }
    //animation and controls
    private void FixedUpdate()
    {

        //move and change animation if isn't attacking

        if (ableToMove && canMove && !isDashing && !isGettingParried)
        {

            //TODO: make it smoother??
            if (inputX < 0)
            {
                myRigidBody.linearVelocity = new Vector2(inputX * speed, myRigidBody.linearVelocity.y);
                if (!upperStance)
                {
                    ChangeAnimationState(PLAYER_WALK_BACK);
                    //ChangeAnimationState(PLAYER_LOWER_GUARD_MOVE);
                }
                else if (upperStance)
                {
                    ChangeAnimationState(PLAYER_UPPER_WALK_BACK);
                    //ChangeAnimationState(PLAYER_UPPER_GUARD_MOVE);
                }
            }

            else if (inputX > 0)
            {
                if (myEnemy != null)
                {
                    if ((Mathf.Abs(myEnemy.transform.position.x - transform.position.x) < 3.2f))
                    {
                        myRigidBody.linearVelocity = new Vector2(0, myRigidBody.linearVelocity.y);

                    }
                    else
                    {
                        myRigidBody.linearVelocity = new Vector2(inputX * speed, myRigidBody.linearVelocity.y);

                        if (!upperStance)
                        {
                            ChangeAnimationState(PLAYER_LOWER_WALK_FORWARD);
                            //ChangeAnimationState(PLAYER_LOWER_GUARD_MOVE);
                        }
                        else if (upperStance)
                        {
                            ChangeAnimationState(PLAYER_UPPER_WALK_FORWARD);
                            //ChangeAnimationState(PLAYER_UPPER_GUARD_MOVE);
                        }
                    }
                }
                //if enemy doesn't exist delete later
                else
                {
                    myRigidBody.linearVelocity = new Vector2(inputX * speed, myRigidBody.linearVelocity.y);
                    if (!upperStance)
                    {
                        ChangeAnimationState(PLAYER_LOWER_WALK_FORWARD);
                        //ChangeAnimationState(PLAYER_LOWER_GUARD_MOVE);
                    }
                    else if (upperStance)
                    {

                        ChangeAnimationState(PLAYER_LOWER_WALK_FORWARD);
                        //ChangeAnimationState(PLAYER_UPPER_GUARD_MOVE);
                    }

                }
            }
            else
            {
                //change guard stance
                myRigidBody.linearVelocity = Vector2.zero;
                if (!upperStance)
                {
                    ChangeAnimationState(PLAYER_LOWER_GUARD);

                }
                else if (upperStance)
                {
                    ChangeAnimationState(PLAYER_UPPER_GUARD);
                }
            }

        }

        //IMPORTANT NOTE:might have to change to include parry and feints
        //attack
        if (attackIsPressed && canMove && !isParrying)
        {
            //cant attack again while attacking
            //deactivate guard
            //TO DO: go forward



            if (!isAttacking)
            {

                attackIsPressed = false;
                ableToMove = false;
                guardActive = false;
                //upper or lower stance attack
                if (upperStance)
                {
                    myRigidBody.linearVelocity = Vector2.zero;
                    isAttacking = true;
                    ChangeAnimationState(PLAYER_UPPER_ATTACK);
                    StartCoroutine(AttackCooldown(1.1f));
                }
                else if (!upperStance)
                {
                    myRigidBody.linearVelocity = Vector2.zero;
                    isAttacking = true;
                    ChangeAnimationState(PLAYER_LOWER_ATTACK);
                    StartCoroutine(AttackCooldown(1.1f));

                }

            }
        }
        //parry
        if (parryIsPressed)
        {
            parryIsPressed = false; // Reset the input immediately
            StartParry();
        }

        if (dashIsPressed)
        {
            isDashing = true;
            Dash();



        }

    }
    IEnumerator AttackCooldown(float cooldownTime)
    {
        isAttacking = true;
        yield return new WaitForSeconds(cooldownTime);
        AttackComplete();
    }


    //complete attacking
    public void AttackComplete()
    {
        isAttacking = false;
        ableToMove = true;
        //activate guard and set attacking false
        guardActive = true;
        Debug.Log("Player attack complete");
    }

    void Dash()
    {
        // Set dash flags
        dashIsPressed = false; // Clear input flag
        isDashing = true;      // Set dashing status
        ableToMove = false;
        // Disable movement during dash

        // Determine dash direction and animation
        Vector2 dashDirection;
        string dashAnimation;

        if (Input.GetKey(KeyCode.D)) // Forward dash
        {
            dashDirection = new Vector2(1, 0);

            if (upperStance)
            {
                dashAnimation = PLAYER_UPPER_DASH_FORWARD;
            }
            else
            {
                dashAnimation = PLAYER_LOWER_DASH_FORWARD;
            }
        }
        else // Backward dash
        {
            dashDirection = new Vector2(-1, 0);

            if (upperStance)
            {
                dashAnimation = PLAYER_UPPER_DASH_BACKWARD;
            }
            else
            {
                dashAnimation = PLAYER_LOWER_DASH_BACKWARD;
            }
        }
        ChangeAnimationState(dashAnimation);

        myRigidBody.linearVelocity = dashDirection * dashSpeed;

    }

    public void GuardBreak(bool upperStance)
    {
        myRigidBody.linearVelocity = new Vector2(0,0);
        isGuardBreaking = true;
        ableToMove = false;
        canMove = false;
        if (upperStance)
        {
            ChangeAnimationState(PLAYER_UPPER_GUARDBREAK);
        }
        else
        {
            ChangeAnimationState(PLAYER_LOWER_GUARDBREAK);
        }
    }
    void GuardBreakComplete()
    {
        isGuardBreaking = false;
        ableToMove = true;
        canMove = true;
        guardActive = true;
    }

    void dashComplete()
    {
        // Stop dashing
        myRigidBody.linearVelocity = Vector2.zero; // Stop movement
        isDashing = false;
        ableToMove = true; // Re-enable movement
    }
    //parry successful
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
    //change animation state without using animation controller
    public void ChangeAnimationState(string newState)
    {
        if (currentState == newState)
        {
            return;
        }

        myAnimator.Play(newState);

        currentState = newState;
    }

    public void DeactivateGuard()
    {

        guardActive = false;
        canMove = false;

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
    }
    IEnumerator parryBonusEnd()
    {
        yield return new WaitForSeconds(1f);
        parrySuccessful = false;
    }
    private void StartParry()
    {
        GuardBreakComplete();
        isParrying = true;
        ableToMove = false;
        guardActive = false;

        // Determine stance and play corresponding parry animation
        if (upperStance)
        {
            myRigidBody.linearVelocity = Vector2.zero;
            ChangeAnimationState(PLAYER_UPPER_PARRY);
        }
        else
        {
            myRigidBody.linearVelocity = Vector2.zero;
            ChangeAnimationState(PLAYER_LOWER_PARRY);
        }


    }
    private void EndParry()
    {
        isParrying = false;
        ableToMove = true;
        guardActive = true;
    }

    public void GettingParried(bool upperStance)
    {
        isGettingParried = true;   
        canMove = false;
        Debug.Log("GettingParried");

        if (upperStance)
        {
            ChangeAnimationState(PLAYER_UPPER_GETTING_PARRIED);
        }
        else
        {
            ChangeAnimationState(PLAYER_LOWER_GETTING_PARRIED);
        }
    }
    void GettingParriedComplete()
    {
        isGettingParried = false;
        canMove = true;
    }
}
