using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    Animator animator;

    private Vector2 movementInput = Vector2.zero;

    //for momentum movement
    [SerializeField] float maxSpeed = 5f;
    [SerializeField] float accel = 20f;

    //jumping
    [SerializeField] float jumpSpeed = 14f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] bool grounded;

    //wall climbing
    [SerializeField] float slideFactor = 2f; // climbing modifier
    [SerializeField] float climbFactor = 25f; // sliding modifier
    [SerializeField] bool wall;

    //for sprite flipping
    float horizontalValue;
    bool facingRight = true;

    private void Start() 
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public void OnMove(InputValue input)
    {
        Vector2 inputVec = input.Get<Vector2>();
        movementInput = new Vector2(inputVec.x, inputVec.y);
    }

    public void OnJump()
    {
        Jump();
    }

    void Update()
    {
        horizontalValue = movementInput.x;
    }

    private void FixedUpdate()
    {
        float dir = horizontalValue;

        WallCheck();
        IsGrounded();
        Move(dir);

        //turning
        Vector3 currentScale = transform.localScale;
        if (facingRight && dir < 0) {
            currentScale.x *= -1;
            facingRight = false;
        }
        else if (!facingRight && dir > 0) {
            currentScale.x = Math.Abs(currentScale.x);
            facingRight = true;
        }
        transform.localScale = currentScale;
    }

    //player position checks

    //checks for ground
    bool IsGrounded()
    {
        Vector2 positionLeft = new Vector2(transform.position.x - 0.52f, transform.position.y);
        Vector2 positionRight = new Vector2(transform.position.x + 0.52f, transform.position.y);
        Vector2 positionMid = new Vector2(transform.position.x, transform.position.y);
        Vector2 direction = Vector2.down;
        float distance = 0.76f;

        Debug.DrawRay(positionLeft, direction, Color.green);
        Debug.DrawRay(positionRight, direction, Color.green);
        Debug.DrawRay(positionMid, direction, Color.green);
        RaycastHit2D hitLeft = Physics2D.Raycast(positionLeft, direction, distance, groundLayer);
        RaycastHit2D hitRight = Physics2D.Raycast(positionRight, direction, distance, groundLayer);
        RaycastHit2D hitMid = Physics2D.Raycast(positionMid, direction, distance, groundLayer);

        if (hitLeft.collider != null || hitRight.collider != null || hitMid.collider != null) {
            grounded = true;
            return true;
        }

        grounded = false;
        return false;
    }

    //checks for wall in front
    bool WallCheck()
    {
        Vector2 position = transform.position;
        Vector2 direction = Vector2.right;
        float distance = 0.95f;

        Debug.DrawRay(position, direction, Color.green);
        RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, groundLayer);

        if (hit.collider != null) {
            wall = true;
            return true;
        }
        wall = false;
        return false;
    }
    
    void Jump()
    {
        if (IsGrounded()) {
            //rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
            rb.AddForce(new Vector2(0, jumpSpeed), ForceMode2D.Impulse);
        }
    }
    
    void Move(float dir)
    {
        //current horizontal speed
        float xSpeed = rb.velocity.x * Time.fixedDeltaTime * 100;

        //movement with AddForce
        //if holding "right"
        if (dir > 0f) 
        {
            //if current speed is above max speed
            if (xSpeed > maxSpeed) {
                //set current speed to max speed
                rb.velocity = new Vector2(maxSpeed * dir, rb.velocity.y);
            }
            else {
                //accelerate player by accel constant
                rb.AddForce(new Vector2(accel * dir, 0), ForceMode2D.Force);
            }
        }
        //if holding "left"
        else if (dir < 0f) 
        {
            //if current speed is above max speed
            if (xSpeed < -maxSpeed) {
                //set current speed to max speed
                rb.velocity = new Vector2(maxSpeed * dir, rb.velocity.y);
            }
            else {
                //accelerate player by accel constant
                rb.AddForce(new Vector2(accel * dir, 0), ForceMode2D.Force);
            }
        }
        //if holding neither "left" nor "right"
        else if (dir == 0) {
            //if current velocity is greater than 0.01
            if (xSpeed > 0.01f) {
                //decelerate player by accel constant
                rb.AddForce(new Vector2(-(accel), 0));
            }
            //if current velocity is less than -0.01
            else if (xSpeed < -0.01f) {
                rb.AddForce(new Vector2(accel, 0));
            }
            else {
                //if the current velocity is between 0.01 and -0.01, then set the instant
                //velocity to 0
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
        }

        animator.SetFloat("xVelocity", Math.Abs(xSpeed));
    }
}