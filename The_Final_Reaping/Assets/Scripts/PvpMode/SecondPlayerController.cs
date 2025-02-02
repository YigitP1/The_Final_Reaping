using System.Collections;
using UnityEngine;

public class SecondPlayerController : MonoBehaviour
{
    //implement getting parried animation
    //initializing stuff
    private Animator myAnimator;
    private Rigidbody2D myRigidBody;
    private float speed = 2f;
    public string currentState;
    private float inputX = 0;
    private float inputY = 0;
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
    const string AI_UPPER_GUARD = "Enemy_Upper_Guard";
    const string AI_LOWER_GUARD = "Enemy_Lower_Guard";
    const string AI_UPPER_ATTACK = "Enemy_Upper_Attack";
    const string AI_LOWER_ATTACK = "Enemy_Lower_Attack";
    const string AI_UPPER_PARRY = "Enemy_Upper_Parry";
    const string AI_LOWER_PARRY = "Enemy_Lower_Parry";
    const string AI_WALK_BACK = "Enemy_Walk_Back";
    const string AI_WALK = "Enemy_Walk_Forward";
    const string AI_DASH = "Enemy_Dash";
    const string AI_UPPER_FORWARD_DASH = "Enemy_Upper_Forward_Dash";
    const string AI_LOWER_FORWARD_DASH = "Enemy_Lower_Forward_Dash";
    const string AI_UPPER_BACKWARD_DASH = "Enemy_Upper_Backward_Dash";
    const string AI_LOWER_BACKWARD_DASH = "Enemy_Lower_Backward_Dash";
    const string AI_LOWER_GUARDBREAK = "Enemy_Lower_GuardBreak";
    const string AI_UPPER_GUARDBREAK = "Enemy_Upper_GuardBreak";
    const string AI_UPPER_GETTING_PARRIED = "Enemy_Upper_Getting_Parried";
    const string AI_LOWER_GETTING_PARRIED = "Enemy_Lower_Getting_Parried";




    void Start()
    {

        myEnemy = GameObject.FindGameObjectWithTag("Player");
        myAnimator = GetComponent<Animator>();
        myRigidBody = GetComponent<Rigidbody2D>();


    }



    void Update()
    {

        //getting movement input
        inputX = Input.GetAxis("ControllerHorizontal");
        //getting attack input
        if (Input.GetButtonDown("ControllerAttack") && (Time.time - timeSinceLastAttack) > attackCooldown && !isDashing && !isGettingParried && !isParrying)
        {
            attackIsPressed = true;
            timeSinceLastAttack = Time.time;

        }
        inputY = Input.GetAxis("ControllerLowerGuard");
        if (inputY<0){

            upperStance = true;
        }
        else if (inputY>0){ upperStance = false; }
        //if (Input.GetKeyDown(KeyCode.DownArrow))
        //{
        //    upperStance = false;
        //}
        //if (Input.GetKeyDown(KeyCode.UpArrow))
        //{
        //    upperStance = true;
        //}
        if (Input.GetButtonDown("ControllerParry") && ((Time.time - timeSinceLastParry) > parryCooldown) && !isParrying && !isDashing && !isAttacking)
        {
            parryIsPressed = true;
            timeSinceLastParry = Time.time;
        }
        if (Input.GetButtonDown("ControllerDash") && !isParrying && !isDashing && !isAttacking && !isGettingParried)
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
                if ((Mathf.Abs(myEnemy.transform.position.x - transform.position.x) < 2.5f))
                {
                    myRigidBody.linearVelocity = new Vector2(0, myRigidBody.linearVelocity.y);

                }
                else
                {
                    myRigidBody.linearVelocity = new Vector2(inputX * speed, myRigidBody.linearVelocity.y);
                    if (!upperStance)
                    {
                        ChangeAnimationState(AI_WALK_BACK);
                        //ChangeAnimationState(PLAYER_LOWER_GUARD_MOVE);
                    }
                    else if (upperStance)
                    {
                        ChangeAnimationState(AI_WALK_BACK);
                        //ChangeAnimationState(PLAYER_UPPER_GUARD_MOVE);
                    }   
                }
                
            }

            else if (inputX > 0)
            {
                if (myEnemy != null)
                {
                    
                    {
                        myRigidBody.linearVelocity = new Vector2(inputX * speed, myRigidBody.linearVelocity.y);

                        if (!upperStance)
                        {
                            ChangeAnimationState(AI_WALK);
                            //ChangeAnimationState(PLAYER_LOWER_GUARD_MOVE);
                        }
                        else if (upperStance)
                        {
                            ChangeAnimationState(AI_WALK);
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
                        ChangeAnimationState(AI_WALK);
                        //ChangeAnimationState(PLAYER_LOWER_GUARD_MOVE);
                    }
                    else if (upperStance)
                    {

                        ChangeAnimationState(AI_WALK);
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
                    ChangeAnimationState(AI_LOWER_GUARD);

                }
                else if (upperStance)
                {
                    ChangeAnimationState(AI_UPPER_GUARD);
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
                    ChangeAnimationState(AI_UPPER_ATTACK);
                    StartCoroutine(AttackCooldown(1.1f));
                }
                else if (!upperStance)
                {
                    myRigidBody.linearVelocity = Vector2.zero;
                    isAttacking = true;
                    ChangeAnimationState(AI_LOWER_ATTACK);
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
                dashAnimation = AI_UPPER_FORWARD_DASH;
            }
            else
            {
                dashAnimation = AI_LOWER_FORWARD_DASH;
            }
        }
        else // Backward dash
        {
            dashDirection = new Vector2(-1, 0);

            if (upperStance)
            {
                dashAnimation = AI_UPPER_BACKWARD_DASH;
            }
            else
            {
                dashAnimation = AI_LOWER_BACKWARD_DASH;
            }
        }
        ChangeAnimationState(dashAnimation);

        myRigidBody.linearVelocity = dashDirection * dashSpeed;

    }

    public void GuardBreak(bool upperStance)
    {
        myRigidBody.linearVelocity = new Vector2(0, 0);
        isGuardBreaking = true;
        ableToMove = false;
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
            ChangeAnimationState(AI_UPPER_PARRY);
        }
        else
        {
            myRigidBody.linearVelocity = Vector2.zero;
            ChangeAnimationState(AI_LOWER_PARRY);
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
    }
}


