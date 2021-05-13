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
    public float grabTime = 0.33f;
    public static int wallMergesLimit = -1; // infinite wall merges
    public int wallMergesLeft;
    public float wallJumpCount;
    public float grabCount;
    public float gravityScale;
    public bool isWallMerged;  // FreezeWW script depend on this
    public bool isGrounded;
    public bool isGrabbing;
    public bool isAlongWall;
    public bool isRight;
    public bool isPaused;
    public bool jumpWait;
    public Transform groundCheck;
    public Transform wallCheck;
    public LayerMask whatIsGround;
    public LayerMask RW_Wall;
    public LayerMask WW_Wall;
    public LayerMask RW_WallJump;
    public LayerMask WW_WallJump;
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
        isWallMerged = false;
        wallMergesLeft = wallMergesLimit;
        gravityScale = rb2d.gravityScale;
        jumpWait = false;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.J)) {
            HandleWallMerging();
        }
        SetNearbyParameters();
        if (!isPaused) {
            if (grabCount <= 0 && wallJumpCount <= 0) {
                HandleMovement();
            } else {
                grabCount -= Time.deltaTime;
                wallJumpCount -= Time.deltaTime;
            }
            if (Input.GetButtonDown("Jump")) {
                HandleJump();
            } else if (Input.GetButtonUp("Jump")) {
                // short hop, unrelated to time
                // cut vY in half when release "space key"
                // dirty fix: not half speed when falling
                if (rb2d.velocity.y > 0) {
                    rb2d.velocity = new Vector2(rb2d.velocity.x, rb2d.velocity.y / 2);
                }
            }
        }
        SetAnimParameters();
    }

    private void SetNearbyParameters() {
        // isGrabbing = false;
        bool ground = Physics2D.OverlapCircle(groundCheck.position, 
                                            groundCheckRadius, whatIsGround);
        bool wallGround = false;
        if (isWallMerged) {
            isAlongWall = Physics2D.OverlapCircle(wallCheck.position,
                                            wallCheckRadius, WW_WallJump);
            wallGround = Physics2D.OverlapCircle(groundCheck.position,
                                            groundCheckRadius, WW_Wall);                              
        } else {
            isAlongWall = Physics2D.OverlapCircle(wallCheck.position,
                                            wallCheckRadius, RW_WallJump);
            wallGround = Physics2D.OverlapCircle(groundCheck.position,
                                            groundCheckRadius, RW_Wall);                                   
        }
        isGrounded = (ground || wallGround);
    }

    private void HandleWallMerging() {
        if (wallMergesLeft == 0) {
            // ooh no more merges
            return;
        }
        wallMergesLeft--;
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
        rb2d.gravityScale = gravityScale;
        if (!isGrounded) {
            AirMovement();
        } else {
            // on ground
            GroundMovement();
        }
        // v is updated, now possibly need to flip sprite
        // OptionalFlipOnXVelocity();
        
        if (isAlongWall && !isGrounded) {
            if ((isRight && Input.GetAxisRaw("Horizontal") > 0) ||
                (!isRight && Input.GetAxisRaw("Horizontal") < 0)) {
                isGrabbing = true;
                jumpWait = true;
                rb2d.velocity = new Vector2(0f, 0f);
                rb2d.gravityScale = 0f;
            } else if (jumpWait) {
                grabCount = grabTime;
                jumpWait = false;
                rb2d.velocity = new Vector2(0f, 0f);
                rb2d.gravityScale = 0f;
            }
        }
    }

    private void HandleJump() {
        if (isGrounded) {
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpSpeed);
        } else if (isAlongWall && isGrabbing) {
            wallJumpCount = wallJumpTime;
            isGrabbing = false;
            rb2d.gravityScale = gravityScale;
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
        transform.localScale = Scale;
    }

    private void OptionalFlipOnXVelocity() {
        if ((rb2d.velocity.x > 0f && !isRight) ||
            (rb2d.velocity.x < 0f && isRight)) {
            Flip();
        }
    }

    private void GroundMovement() {
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
        } else {
            // is grounded & no input
            isGrabbing = false;
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
        }
    }

    public float airSpeedDivider = 20;
    public float airDragMultiplier = 0.15f;
    public float airDragMax = 0.1f;
    private void AirMovement() {
        // mid air, FIXED: arbitrary divide by 10 (make it a param?)
        // FIXED: arbitrary 0.1f (make it a param?)
        float desiredDeltaSpeed = Input.GetAxisRaw("Horizontal") * moveSpeed;
        if (isRight && desiredDeltaSpeed < 0f || !isRight && desiredDeltaSpeed > 0f) {
            Flip();
        }
        float vX = rb2d.velocity.x;
        float vY = rb2d.velocity.y;
        vX += desiredDeltaSpeed / airSpeedDivider;


        vX = Mathf.Clamp(vX, -moveSpeed, moveSpeed);
        rb2d.velocity = new Vector2(vX, vY);
        // air drag
        float deltaVx = - Mathf.Sign(rb2d.velocity.x) * Mathf.Min(airDragMax, Mathf.Abs(rb2d.velocity.x * airDragMultiplier));
        rb2d.velocity = new Vector2(rb2d.velocity.x + deltaVx , rb2d.velocity.y);
    }

    public void pauseMovement() {
        isPaused = true;
    }

    public void resumeMovement() {
        isPaused = false;
    }

}
