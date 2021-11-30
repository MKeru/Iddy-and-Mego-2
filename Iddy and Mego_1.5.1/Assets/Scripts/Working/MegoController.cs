using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class MegoController : MonoBehaviour
{
    Rigidbody2D rb;
    Animator animator;
    LevelManager gameLevelManager;

    //spawn point
    public Vector3 spawnPoint;

    //for momentum movement
    [SerializeField] float maxSpeed = 8f;
    [SerializeField] float accel = 20f;

    //jumping
    [SerializeField] float jumpSpeed = 14f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] bool grounded;

    [SerializeField] GameObject attack;
    bool isAttacking;

    //wall climbing
    //[SerializeField] float slideFactor = 2f; // climbing modifier
    //[SerializeField] float climbFactor = 25f; // sliding modifier
    //[SerializeField] bool wall;

    //for sprite flipping
    //float horizontalValue;
    bool facingRight = true;

    private void Start() 
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spawnPoint = transform.position;
        gameLevelManager = FindObjectOfType<LevelManager>();
        attack.SetActive(false);

        Debug.Log("Mego start complete");
    }

    public void Swipe() {
        isAttacking = true;
        StartCoroutine(DoSwipe(.4f));
    }

    IEnumerator DoSwipe(float sec)
    {
        attack.SetActive(true);
        yield return new WaitForSeconds(sec);
        attack.SetActive(false);
        isAttacking = false;
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        WallCheck();
        IsGrounded();

        //animator
        animator.SetFloat("yVelocity", rb.velocity.y);
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
            //wall = true;
            return true;
        }
        //wall = false;
        return false;
    }
    
    

    public void Jump()
    {
        if (IsGrounded()) {
            //rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
            rb.AddForce(new Vector2(0, jumpSpeed), ForceMode2D.Impulse);
        }
    }

    void Land()
    {

    }
    
    public void Move(float dir)
    {
        //current horizontal speed
        float xSpeed = rb.velocity.x * Time.fixedDeltaTime * 100;

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

        //animator
        if (dir != 0) {
            animator.SetBool("LRHeld", true);
        } else {
            animator.SetBool("LRHeld", false);
        }
        
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
            if (xSpeed > 0.5f) {
                //decelerate player by accel constant
                rb.AddForce(new Vector2(-(accel), 0));
            }
            //if current velocity is less than -0.01
            else if (xSpeed < -0.5f) {
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

    void OnTriggerStay2D(Collider2D other) {
        if (other.tag == "Enemy") {
            gameLevelManager.Respawn();
        }
    }
}
