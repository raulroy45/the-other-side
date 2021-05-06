using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    private Rigidbody2D rb2d;
    private Animator animator;
    public float moveSpeed = 6;
    public float jumpSpeed = 12;
    public float groundCheckRadius = 0.1f;
    public float wallCheckRadius = 0.3f;
    public float wallJumpTime = 0.2f;
    public float grabTime = 0.5f;
    public float wallJumpCount;
    public float grabCount;
    private bool isWallMerged;
    public bool isGrounded;
    public bool isGrabbing;
    public bool isAlongWall;
    public bool isRight;
    public bool isPaused;
    public Transform groundCheck;
    public Transform wallCheck;
    public LayerMask whatIsGround;
    public LayerMask RW_Wall;
    public LayerMask WW_Wall;
    public int wallMergeLimit = -1;  // -1: infinite wall merges 
    private Color Real_World_Color;
    private Color Wall_World_Color;
    private Vector3 Scale;

    // Start is called before the first frame update
    void Start() {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        isGrabbing = false;
        isRight = true;
        Real_World_Color = GetComponent<SpriteRenderer>().color;
        Wall_World_Color = new Color(0,0,0,1);
        Scale = transform.localScale;
        isPaused = false;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.J)) {
            HandleWallMerging();
        }
        SetNearbyParameters();
        if (!isPaused) {
            if (grabCount <= 0 && wallJumpCount <= 0) {
                // isGrabbing = false;
                HandleMovement();
            } else {
                grabCount -= Time.deltaTime;
                wallJumpCount -= Time.deltaTime;
            }
            if (Input.GetButtonDown("Jump")) {
                    HandleJump();
            }
        }
        SetAnimParameters();
        // ChangeSpeedWForce();
    }

    private void SetNearbyParameters() {
        // isGrabbing = false;
        bool ground = Physics2D.OverlapCircle(groundCheck.position, 
                                            groundCheckRadius, whatIsGround);
        bool wallGround = false;
        if (isWallMerged) {
            isAlongWall = Physics2D.OverlapCircle(wallCheck.position,
                                            wallCheckRadius, WW_Wall);
            wallGround = Physics2D.OverlapCircle(groundCheck.position,
                                            groundCheckRadius, WW_Wall);                              
        } else {
            isAlongWall = Physics2D.OverlapCircle(wallCheck.position,
                                            wallCheckRadius, RW_Wall);
            wallGround = Physics2D.OverlapCircle(groundCheck.position,
                                            groundCheckRadius, RW_Wall);                                   
        }
        isGrounded = (ground || wallGround);
    }

    private void HandleWallMerging() {
        if (wallMergeLimit == 0) {
            // ooh no more merges
            return;
        }
        wallMergeLimit--;
        isWallMerged = !isWallMerged;
        if (isWallMerged) {
            gameObject.layer = LayerMask.NameToLayer("WW_Bob");
            gameObject.GetComponent<SpriteRenderer>().color = Wall_World_Color;
        } else {
            gameObject.layer = LayerMask.NameToLayer("RW_Bob");
            gameObject.GetComponent<SpriteRenderer>().color = Real_World_Color;
        }
}

    private void HandleMovement() {
        if (Input.GetAxisRaw("Horizontal") > 0f) {
            rb2d.velocity = new Vector2(moveSpeed, rb2d.velocity.y);
            if (!isRight) {
                Flip();
            }
        } else if (Input.GetAxisRaw("Horizontal") < 0f) {
            rb2d.velocity = new Vector2(-moveSpeed, rb2d.velocity.y);
            if (isRight) {
                Flip();
            }
        } else if (isGrounded) {
            isGrabbing = false;
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
        }
        if (isAlongWall && !isGrounded) {
            if ((isRight && Input.GetAxisRaw("Horizontal") > 0) ||
                (!isRight && Input.GetAxisRaw("Horizontal") < 0)) {
                isGrabbing = true;
                rb2d.velocity = new Vector2(0, rb2d.velocity.y * 0.1f);
            } else {
                grabCount = grabTime;
            }
        }
    }

    private void HandleJump() {
        if (isGrounded) {
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpSpeed);
        } else if (isAlongWall && isGrabbing) {
            UnityEngine.Debug.Log("Ok");
            if ((isRight && Input.GetAxisRaw("Horizontal") < 0) ||
                (!isRight && Input.GetAxisRaw("Horizontal") > 0)) {
                wallJumpCount = wallJumpTime;
                isGrabbing = false;
                // float jumpComp = jumpSpeed * (1f / Mathf.Sqrt(2));
                float direction = isRight ? -1f : 1f;
                rb2d.velocity = new Vector2(moveSpeed * direction, jumpSpeed);
                Flip();
            }
        }
    }

    private void SetAnimParameters() {
        animator.SetFloat("Speed", Mathf.Abs(rb2d.velocity.x));
        animator.SetBool("isGrounded", isGrounded);
        animator.SetBool("isWallMerged", isWallMerged);
    }

    private void Flip() {
        isRight = !isRight;
        Scale.x = -Scale.x;
        if (isRight) {
            transform.localScale = Scale;
        } else {

            transform.localScale = Scale;
        }
    }

    public void pauseMovement() {
        isPaused = true;
    }

    public void resumeMovement() {
        isPaused = false;
    }


    // one way to control, does not get stuck on wall
    // change Bob's speed using force
    // dont not feel very actiony tho, dragging a bit
    // void ChangeSpeedWForce() {
    //     if (Input.GetAxisRaw("Horizontal") > 0f) {
    //         // move right
    //         rb2d.AddForce(new Vector2(moveSpeed, 0));
    //         // too fast
    //         if (rb2d.velocity.x > moveSpeed) {
    //             rb2d.velocity = new Vector2(moveSpeed, rb2d.velocity.y);
    //         }
    //     } else if (Input.GetAxisRaw("Horizontal") < 0f) {
    //         // move left
    //         rb2d.AddForce(new Vector2(-moveSpeed, 0));
    //         // too fast
    //         if (rb2d.velocity.x < -moveSpeed) {
    //             rb2d.velocity = new Vector2(-moveSpeed, rb2d.velocity.y);
    //         }
    //     } else {
    //         rb2d.velocity = new Vector2(rb2d.velocity.x / 1.09f, rb2d.velocity.y);
    //     }
    // }
}
