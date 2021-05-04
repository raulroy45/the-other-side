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
    private bool isWallMerged;
    public bool isGrounded;
    public bool hasWallJumped;
    public bool isAlongWall;
    public bool isRight;
    public Transform groundCheck;
    public Transform wallCheck;
    public Stopwatch stopwatch;
    public LayerMask whatIsGround;
    public LayerMask RW_Wall;
    public LayerMask WW_Wall;
    private Color Real_World_Color;
    private Color Wall_World_Color;
    private Vector3 Scale;

    // Start is called before the first frame update
    void Start() {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        isWallMerged = false;
        isRight = true;
        stopwatch = new Stopwatch();
        Real_World_Color = GetComponent<SpriteRenderer>().color;
        Wall_World_Color = new Color(0,0,0,1);
        Scale = transform.localScale;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.J)) {
            HandleWallMerging();
        }
        SetNearbyParameters();
        if (!stopwatch.IsRunning || stopwatch.ElapsedMilliseconds > 200) {
            hasWallJumped = false;
            stopwatch.Reset();
            HandleMovement();
        }
        if (Input.GetButtonDown("Jump")) {
            HandleJump();
        }
        SetAnimParameters();
        // ChangeSpeedWForce();
    }

    private void SetNearbyParameters() {
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
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
        } else if (isAlongWall && !hasWallJumped) {
            rb2d.velocity = new Vector2(0, rb2d.velocity.y * 0.1f);
        }
    }

    private void HandleJump() {
        if (isGrounded) {
            UnityEngine.Debug.Log("??");
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpSpeed);
        } else if (isAlongWall && !hasWallJumped) {
            stopwatch.Start();
            hasWallJumped = true;
            float jumpComp = jumpSpeed * (1f / Mathf.Sqrt(2));
            float direction = isRight ? -1f : 1f;
            rb2d.velocity = new Vector2(jumpComp * direction, jumpComp);
            Flip();
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
