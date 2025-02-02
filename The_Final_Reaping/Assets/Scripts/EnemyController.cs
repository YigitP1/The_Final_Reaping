using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //initializing stuff
    private Animator myAnimator;
    private Rigidbody2D myRigidBody;
    // private float speed = 4.0f;
    private string currentState;
    //private float inputX = 0;
    private bool isAttackPressed;
    private bool isAttacking;
    //private float attackDelay;
    //private GameObject myEnemy;
    public bool guardActive = true;
    public bool upperStance = false;


    //Animation handles
    const string PLAYER_UPPER_GUARD = "Upper_Guard";
    const string PLAYER_LOWER_GUARD = "Lower_Guard";
    const string PLAYER_UPPER_ATTACK = "Upper_Attack";
    const string PLAYER_LOWER_ATTACK = "Lower_Attack";
    const string PLAYER_RUN = "Run";





    void Start()
    {

        //myEnemy = GameObject.FindGameObjectWithTag("Enemy");
        myAnimator = GetComponent<Animator>();
        myRigidBody = GetComponent<Rigidbody2D>();


    }



    void Update()
    {

        //getting movement input
        ////inputX = Input.GetAxis("Horizontal");
        ////getting attack input
        if (Input.GetKeyDown(KeyCode.RightControl))
        {
            isAttackPressed = true;
        }
        //if (Input.GetKeyDown(KeyCode.DownArrow))
        //{
        //    upperStance = false;
        //}
        //if (Input.GetKeyDown(KeyCode.UpArrow))
        //{
        //    upperStance = true;
        //}

    }
    //animation and controls
    private void FixedUpdate()
    {

        //move and change animation if isn't attacking
        if (!isAttacking)
        {

            ////TODO: make it smoother??
            //if (inputX < 0)
            //{
            //    myRigidBody.linearVelocity = new Vector2(inputX * speed, myRigidBody.linearVelocity.y);
            //    ChangeAnimationState(PLAYER_RUN);
            //}

            //else if (inputX > 0)
            //{
            //    if (myEnemy != null)
            //    {
            //        if (!(Mathf.Abs(myEnemy.transform.position.x - transform.position.x) < 2.5f))
            //        {
            //            myRigidBody.linearVelocity = new Vector2(inputX * speed, myRigidBody.linearVelocity.y);
            //            ChangeAnimationState(PLAYER_RUN);
            //        }
            //    }
            //    else
            //    {
            //        myRigidBody.linearVelocity = new Vector2(inputX * speed, myRigidBody.linearVelocity.y);
            //        ChangeAnimationState(PLAYER_RUN);

            //    }
            //}
            //else
            //{
            //change guard stance
            myRigidBody.linearVelocity = Vector2.zero;
            if (!upperStance)
            {
                ChangeAnimationState(PLAYER_LOWER_GUARD);
                //yiðit osurdu
            }
            else if (upperStance)
            {
                ChangeAnimationState(PLAYER_UPPER_GUARD);
            }
            //}

        }

        //IMPORTANT NOTE:might have to change to include parry and feints
        //attack
        if (isAttackPressed)
        {
            //cant attack again while attacking
            //deactivate guard
            guardActive = false;
            isAttackPressed = false;

            if (!isAttacking)
            {
                //upper or lower stance attack
                if (upperStance)
                {
                    myRigidBody.linearVelocity = Vector2.zero;
                    isAttacking = true;
                    ChangeAnimationState(PLAYER_UPPER_ATTACK);
                }
                else if (!upperStance)
                {
                    myRigidBody.linearVelocity = Vector2.zero;
                    isAttacking = true;
                    ChangeAnimationState(PLAYER_LOWER_ATTACK);
                }


            }


        }

    }
    //complete attacking
    void AttackComplete()
    {
        //activate guard and set attacking false
        guardActive = true;
        isAttacking = false;
    }
    //change animation state without using animation controller
    void ChangeAnimationState(string newState)
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
        StartCoroutine(guardStun());
    }
    IEnumerator guardStun()
    {
        yield return new WaitForSeconds(2f);
        guardActive = true;
    }

}
