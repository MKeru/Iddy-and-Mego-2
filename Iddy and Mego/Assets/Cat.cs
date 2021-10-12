using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(PlayerInput))]
public class Cat : MonoBehaviour
{
    public Animator animator;
    private Vector2 movementInput = Vector2.zero;
    private PlayerInput controller;
    [SerializeField] float rspeed = 1;
    [SerializeField] float lspeed = 1;
    [SerializeField] float max = 3; //max speed
    [SerializeField] float min = 1; //min speed
    float hValue; //Direction of player movement
    float jForce; //unused
    const float groundCheckRadius = 0.2f;
    float runSpeedModifier = 2f;

    Rigidbody2D rb;
    Animator animator;
    [SerializeField] Transform groundCheckCollider;
    [SerializeField] LayerMask groundLayer;

    bool facingRight = true;
    bool jump = false;
    [SerializeField] bool isGrounded;
    [SerializeField] bool isRunning = false;

    


    private void Start()
    {


        controller = gameObject.GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

   // void Awake()
   // {
        
    //}
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
        //hValue = Input.GetAxisRaw("Horizontal");
        hValue = movementInput.x;
        animator.SetFloat("Speed", rb.velocity.x);
        animator.SetBool("isGrounded", isGrounded);

        //if (Input.GetButton("Jump"))
        //    jump = true;
        //else if (Input.GetButtonUp("Jump"))
        //jump = false;
<<<<<<< HEAD

        //if Lshift is enabled, isRunning enabled
        if (Input.GetKeyDown(KeyCode.LeftShift))
            isRunning = true;
        //otherwise disable isRunning
        if (Input.GetKeyUp(KeyCode.LeftShift))
            isRunning = false;
=======
>>>>>>> 72d1e32d86ba0e9e129a41918d81d55fa94aba5c
    }

    private void FixedUpdate()
    {
        GroundCheck();
        
        Move(hValue, jump);
        animator.SetFloat("Speed", rb.velocity.x);
        animator.SetBool("isGrounded", isGrounded);
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
        float lVal = dir * lspeed * 100 * Time.fixedDeltaTime;
        float rVal = dir * rspeed * 100 * Time.fixedDeltaTime;
        #region Running

        //float xVal = dir * runSpeedModifier * 100 * Time.deltaTime;

        if (isRunning)
        {
            lVal *= runSpeedModifier;
            rVal *= runSpeedModifier;
        }

        //set float xVelocity to x value of the rigidbody2d velocity.


        #endregion
        #region Jumping
        if (isGrounded && jumpFlag)
        {
            isGrounded = false;
            jumpFlag = false;
            if(dir > 0)
                rb.AddForce(new Vector2(0f, rspeed/2 * 100));
            if(dir < 0)
                rb.AddForce(new Vector2(0f, lspeed/2 * 100));
            if(dir == 0)
                rb.AddForce(new Vector2(0f, min * 100));

        }
        #endregion
        #region Movement


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

        if(dir > 0) // going right
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
        
        if(dir < 0) // going left
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
            transform.localScale = new Vector3(-1, 1, 1);
            facingRight = false;
        }
        else if(!facingRight && dir > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            facingRight = true;
        }
        #endregion

     

        animator.SetFloat("xVelocity", Mathf.Abs(rb.velocity.x));
    }
}
