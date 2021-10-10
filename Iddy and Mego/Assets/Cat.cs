using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : MonoBehaviour
{
    [SerializeField] float rspeed = 1; // speed when going right or right momentum
    [SerializeField] float lspeed = 1; // speed going left or left momentum
    [SerializeField] float max = 5; // maximum speed of both left and right. So player doesnt speed up forever
    [SerializeField] float min = 1; // base speed for both left and right speed
    float hValue; // Will be 1 if going right, -1 if going left, and 0 if neither left/right buttons are pressed.
//    float jForce; not used for now
    Rigidbody2D rb;
    
    const float groundCheckRadius = 0.2f;  // variables used for ground check collider
    [SerializeField] Transform groundCheckCollider;
    [SerializeField] LayerMask groundLayer;

    bool facingRight = true; // used to know if cat is facing right/left to flip the cat sprite to the appropriate direction
    bool jump; // Jumpflag used to know if cat is jumping
    [SerializeField] bool isGrounded; // checks to see if cat is grounded

    void Awake() // like start()
    {
        rb = GetComponent<Rigidbody2D>(); // refers to the player
    }

    void Update()
    {
        hValue = Input.GetAxisRaw("Horizontal"); // hvalue recieves directional input of player. 1 = right. -1 = left.

        if (Input.GetButton("Jump")) // conditional statements for jumping
            jump = true;
        else if (Input.GetButtonUp("Jump"))
            jump = false;
    }

    private void FixedUpdate()
    {
        GroundCheck(); //checks if player is grounded
        Move(hValue, jump); // affects movement as well as jumping
    }

    void GroundCheck()
    {
        isGrounded = false;
        Collider2D[] coll = Physics2D.OverlapCircleAll(groundCheckCollider.position, groundCheckRadius, groundLayer);
        if(coll.Length > 0)
            isGrounded = true;
    }

    void Move(float dir, bool jumpFlag)
    {
        float lVal = dir * lspeed * 100 * Time.fixedDeltaTime; // value used to move cat in the left direction. dir = -1
        float rVal = dir * rspeed * 100 * Time.fixedDeltaTime; // used to move cat to the right. dir = 1
        #region Jumping
        if (isGrounded && jumpFlag)
        {
            isGrounded = false;
            jumpFlag = false;
            if(dir > 0) // uses rspeed when jumping when dir = 1, which is right
                rb.AddForce(new Vector2(0f, rspeed/2 * 100));
            if(dir < 0) // uses lspeed when jumping when dir = -1, which is left
                rb.AddForce(new Vector2(0f, lspeed/2 * 100));
            if(dir == 0) // when not moving horizontally, just uses minimum speed for jumping
                rb.AddForce(new Vector2(0f, min * 100));
        }
        #endregion
        #region Movement

        // Makes sure that when not pressing any directional button, momentum still takes affect
        if (dir == 0)
        {
            if(rspeed > lspeed) // if right momentum is greater
            {
                Vector2 tVelocity = new Vector2(rspeed * 100 * Time.fixedDeltaTime, rb.velocity.y); // moves right until right speed is back at minimum speed
                rb.velocity = tVelocity;
                rspeed -= 0.1f;

                if (rspeed < min) // if below minimum speed set both left and right momentum to 1 so rspeed == lspeed
                {
                    lspeed = min;
                    rspeed = min;
                }
            }

            if(rspeed < lspeed) // is left momentum is greater
            {
                Vector2 tVelocity = new Vector2(-1 * lspeed * 100 * Time.fixedDeltaTime, rb.velocity.y); // moves left until back at minimum speed
                rb.velocity = tVelocity;
                lspeed -= 0.1f;

                if (lspeed < min) // if below min. speed, sets lspeed == rspeed
                {
                    rspeed = min;
                    lspeed = min;
                }
            }
        }

        // When switchinng directional buttons without having to let go of both, these conditionals are used
        if(dir > 0) // going right
        {
            Vector2 tVelocity = new Vector2(rVal, rb.velocity.y); // move right using rVal
            rb.velocity = tVelocity;
            
            rspeed += .1f; // gains right speed 
            lspeed -= .1f; // loses left speed

            if(rspeed > max) // cap right speed at max speed
            {
                rspeed = max;
            }

            if(lspeed < -max) // cap left speed at -max speed
            {
                lspeed = -max;
            }
        }
        
        if(dir < 0) // going left
        {
            Vector2 tVelocity = new Vector2(lVal, rb.velocity.y); // moves left
            rb.velocity = tVelocity;
            lspeed += .1f; // gains left speed
            rspeed -= .1f; // loses right speed

            if (lspeed > max) // cap speed at max
            {
                lspeed = max;
            }

            if (rspeed < -max) // cap speed at -max
            {
                rspeed = -max;
            }
        }

        // Conditionals used chnage direction sprite is facing
        if(facingRight && dir < 0) // if facingRight is true, but facing left
        {
            transform.localScale = new Vector3(-1, 1, 1); // flips sprite to face the left
            facingRight = false; // change to false since not facing right
        }
        else if(!facingRight && dir > 0) // if facingRight is false, but facing right
        {
            transform.localScale = new Vector3(1, 1, 1); // flips sprite to face the right
            facingRight = true; // change facingRight back to true since it is facing right
        }
        #endregion
    }
}
