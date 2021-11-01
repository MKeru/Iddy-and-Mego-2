using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(PlayerInput))]
public class Cat : MonoBehaviour
{
    private Vector2 movementInput = Vector2.zero;
    private PlayerInput controller;
    [SerializeField] float rspeed = 1;
    [SerializeField] float lspeed = 1;
    [SerializeField] float slideFactor = 2f; // climbing modifier
    [SerializeField] float climbFactor = 25f; // sliding modifier
    [SerializeField] float max = 3; //max speed
    [SerializeField] float min = 1; //min speed
    float hValue; //Direction of player movement
    float jForce; //unused
    float runSpeedModifier = 2f;
    const float groundCheckRadius = 0.2f;
    const float wallCheckRadius = 0.1f;


    Rigidbody2D rb;
    Animator animator;
    [SerializeField] Transform groundCheckCollider;
    [SerializeField] LayerMask groundLayer;

    [SerializeField] Transform gwallCheckCollider;
    [SerializeField] Transform backCheckCollider;
    [SerializeField] Transform wallCheckCollider;
    [SerializeField] LayerMask wallLayer;

    bool facingRight = true;
    bool jump = false;
    [SerializeField] bool isGrounded; // on ground
    [SerializeField] bool ghitWall; // ground/ledge wall
    [SerializeField] bool bhitWall; // back side hits wall
    [SerializeField] bool hitWall; // front side hits wall
    [SerializeField] bool isRunning = false;
    private void Start()
    {
        controller = gameObject.GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }
    public void onJump(InputAction.CallbackContext context)
    {
        jump = context.action.triggered; //triggered on frame

    }
    void Update()
    {
        hValue = movementInput.x;
        //if Lshift is clicked enable isRunning
      /*  if (Input.GetKeyDown(KeyCode.LeftShift))
            isRunning = true;
        //if LShift is released disable isRunning
        if (Input.GetKeyUp(KeyCode.LeftShift))
            isRunning = false;*/
    }

    private void FixedUpdate()
    {
        GroundCheck();
        gWallCheck();
        WallCheck();
        BackCheck();
        WallMovement(jump);
        Move(hValue, jump);
    }

    void GroundCheck()
    {
        isGrounded = false;
        Collider2D[] coll = Physics2D.OverlapCircleAll(groundCheckCollider.position, groundCheckRadius, groundLayer);
        if(coll.Length > 0)
            isGrounded = true;
    }

    void gWallCheck()
    {
        ghitWall = false;
        Collider2D[] gwcoll = Physics2D.OverlapCircleAll(gwallCheckCollider.position, wallCheckRadius, groundLayer);
        if (gwcoll.Length > 0)
            ghitWall = true;
    }

    void WallCheck()
    {
        hitWall = false;
        Collider2D[] wcoll = Physics2D.OverlapCircleAll(wallCheckCollider.position, wallCheckRadius, wallLayer);
   
        if (wcoll.Length > 0)
            hitWall = true;
    }

    void BackCheck()
    {
        bhitWall = false;
        Collider2D[] bgwcoll = Physics2D.OverlapCircleAll(backCheckCollider.position, wallCheckRadius, groundLayer);
        Collider2D[] bwcoll = Physics2D.OverlapCircleAll(backCheckCollider.position, wallCheckRadius, wallLayer);
        if (bgwcoll.Length > 0 || bwcoll.Length > 0)
            bhitWall = true;
    }

    void WallMovement(bool jumpFlag)
    {
        Vector2 v = rb.velocity;
        // climbing
        if((hitWall || (ghitWall && !isGrounded))  && Mathf.Abs(hValue) > 0 && !isGrounded && (rspeed > min || lspeed > min))
        {
            // going right
            if(hValue > 0 && rspeed > min)
            {
                lspeed = min;
                v.y = rspeed * climbFactor * Time.fixedDeltaTime;
                rb.velocity = v;
                if (jumpFlag) // wall jump
                {
                    jumpFlag = false;
                    rb.velocity = Vector2.up * rspeed / 3 * 5;
                }
                    
                rspeed -= .1f;

                if (rspeed <= min)
                    rspeed = 1;
            }

            // going left
            if (hValue < 0 && lspeed > min)
            {
                rspeed = min;
                v.y = lspeed * climbFactor * Time.fixedDeltaTime;
                rb.velocity = v;
                if (jumpFlag)
                {
                    jumpFlag = false;
                    rb.velocity = Vector2.up * lspeed / 3 * 5;
                }
                lspeed -= .1f;

                if (lspeed <= min)
                    lspeed = 1;
            }
        }
        //sliding
        if ((hitWall || (ghitWall && !isGrounded)) && Mathf.Abs(hValue) > 0 && rb.velocity.y < 0 && !isGrounded)
        {
            v.y = -slideFactor;
            rb.velocity = v;
        }

    }

    void Move(float dir, bool jumpFlag)
    {
        float lVal = dir * lspeed * 100 * Time.fixedDeltaTime;
        float rVal = dir * rspeed * 100 * Time.fixedDeltaTime;

       //running = multiply with running modifer.
        if (isRunning)
        {
            lVal *= runSpeedModifier;
            rVal *= runSpeedModifier;
        }

        #region Jumping
        if ((isGrounded) && jumpFlag)
        {
            isGrounded = false;
            jumpFlag = false;

            if (dir > 0)
                rb.velocity = Vector2.up * rspeed / 2 * 5;
  
            if(dir < 0)
                rb.velocity = Vector2.up * lspeed / 2 * 5;
    
            if (dir == 0)
                rb.velocity = Vector2.up * min * 5;

        }
        #endregion
        #region Movement
        // if hitting wall, momentum set to 0(speeds set to minimum)
        if (((hitWall && isGrounded) && !bhitWall) || (!hitWall && bhitWall) || (ghitWall && !bhitWall) || (!ghitWall && bhitWall))
        {
            rspeed = min;
            lspeed = min;
        }

        if (dir == 0)
        {
            if(rspeed > lspeed) // if right momentum is greater
            {
                Vector2 tVelocity = new Vector2(rspeed * 100 * Time.fixedDeltaTime, rb.velocity.y);
                rb.velocity = tVelocity;
                rspeed -= 0.1f;

                if (rspeed < min)
                {
                    lspeed = min;
                    rspeed = min;
                }
            }

            if(rspeed < lspeed) // is left momentum is greater
            {
                Vector2 tVelocity = new Vector2(-1 * lspeed * 100 * Time.fixedDeltaTime, rb.velocity.y);
                rb.velocity = tVelocity;
                lspeed -= 0.1f;

                if (lspeed < min)
                {
                    rspeed = min;
                    lspeed = min;
                }
            }
        }

        if(dir > 0 && !hitWall) // going right
        {
            Vector2 tVelocity = new Vector2(rVal, rb.velocity.y);
            rb.velocity = tVelocity;
            
            rspeed += .1f;
            lspeed -= .1f;

            if(rspeed > max) // cap speed at max
            {
                rspeed = max;
            }

            if(lspeed < -max) // cap speed at -max
            {
                lspeed = -max;
            }
        }
        
        if(dir < 0 && !hitWall) // going left
        {
            Vector2 tVelocity = new Vector2(lVal, rb.velocity.y);
            rb.velocity = tVelocity;
            lspeed += .1f;
            rspeed -= .1f;

            if (lspeed > max) // cap speed at max
            {
                lspeed = max;
            }

            if (rspeed < -max) // cap speed at -max
            {
                rspeed = -max;
            }
        }

        if(facingRight && dir < 0)
        {
            transform.localScale = new Vector3(-0.5f, 0.5f, 1);
            facingRight = false;
        }
        else if(!facingRight && dir > 0)
        {
            transform.localScale = new Vector3(.5f, .5f, 1);
            facingRight = true;
        }
        #endregion


        // thresholds for idle, walking and running
        //set float xVelocity according to the x value of the RigidBody2D Velocity
        animator.SetFloat("xVelocity", Mathf.Abs(rb.velocity.x));
    }
}
