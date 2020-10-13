using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float runSpeed = 5.0f;
    [SerializeField] float jumpSpeed = 28.0f;
    [SerializeField] float climbSpeed = 5.0f;

    Rigidbody2D myRigidBody;
    CapsuleCollider2D myCollider;
    Animator myAnimator;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();

        // Jump only when on ground
        myCollider = GetComponent<CapsuleCollider2D>();

        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Run();
        Jump();
        ClimbLadder();
    }


    // Making player able to run
    // Attach to player
    // To stop player rotating -> rigid body 2d in player -> constraints -> freez z  
    private void Run()
    {
        // Use the input manager edit->project settings-> input manager
        float xDir = Input.GetAxis("Horizontal");
        // Direction * run speed with rigid body
        Vector2 playerVelocity = new Vector2(xDir * runSpeed, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;

        // Math.Epsilon -> smallest val > 0
        // Nedd absolute value of movement
        // Making the player face in direction moving
        // Flip sprite using a bool val
        bool playerHasHorizontalMovement = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;

        if(playerHasHorizontalMovement){
            // -1 = left || +1 = right
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), 1.0f);
            myAnimator.SetBool("isRunning", playerHasHorizontalMovement);
        }
        else{
            myAnimator.SetBool("isRunning", false);
        }
    }

    // Getting player to jump
    private void Jump(){
        // Allowing player to jump only when on the ground
        // Cant do multiple jumps at the same time
        if(!myCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))){
            myAnimator.SetBool("isClimbing", false);
            return;
        }

        // Player jump based on the input 
        if(Input.GetButtonDown("Jump")){
            // Change vel in y direction
            Vector2 jumpVelocityToAdd = new Vector2(0f, jumpSpeed);
            myRigidBody.velocity += jumpVelocityToAdd;
        }
    }

    private void ClimbLadder(){
        
         if(!myCollider.IsTouchingLayers(LayerMask.GetMask("Ladder"))){
            return;
        }

        float yDir = Input.GetAxis("Vertical");
        Vector2 climbVelocity = new Vector2(myRigidBody.velocity.x, yDir * climbSpeed);
        myRigidBody.velocity = climbVelocity;

        // Check if there is vertical movement then change bool
        bool playerHadVerticalSpeed = Mathf.Abs(myRigidBody.velocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("isClimbing", playerHadVerticalSpeed);

    }
}
