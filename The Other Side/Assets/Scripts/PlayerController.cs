using System.Collections;
using System.Collections.Generic;
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
    public LayerMask whatIsGround;
    public LayerMask RW_Wall;
    public LayerMask WW_Wall;
    private Color Real_World_Color;
    private Color Wall_World_Color;

    // Start is called before the first frame update
    void Start() {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        isWallMerged = false;
        isRight = true;
        Real_World_Color = GetComponent<SpriteRenderer>().color;
        Wall_World_Color = new Color(0,0,0,1);
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.J)) {
            handleWallMerging();
        }
        SetNearbyParameters();
        HandleMovement();
        if (Input.GetButtonDown("Jump")) {
            HandleJump();
        }
        SetAnimParameters();
        // ChangeSpeedWForce();
    }

    private void SetNearbyParameters() {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 
                                            groundCheckRadius, whatIsGround);
        if (isGrounded) {
            hasWallJumped = false;
        }
        if (isWallMerged) {
            isAlongWall = Physics2D.OverlapCircle(groundCheck.position,
                                            wallCheckRadius, WW_Wall);
        } else {
            isAlongWall = Physics2D.OverlapCircle(groundCheck.position,
                                            wallCheckRadius, RW_Wall);            
        }
    }

    private void handleWallMerging() {
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
        if (isGrounded && !isAlongWall) {
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpSpeed);
        }
        if (isAlongWall) {
            hasWallJumped = true;
            float jumpComp = jumpSpeed * (1f / Mathf.Sqrt(2));
            rb2d.velocity = new Vector2(-jumpComp, jumpComp);
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
        if (isRight) {
            transform.localScale = new Vector3(1.55f,2.22f,1f);
        } else {
            transform.localScale = new Vector3(-1.55f,2.22f,1f);
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
