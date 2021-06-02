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
    public bool isDead;
    public bool isWallMerged;  // FreezeWW script depend on this
    public bool isGrounded;
    public bool isGrabbing;
    public bool isAlongWall;
    public bool isRight;
    public bool isPaused;
    public bool jumpWait;
    public bool canGrab;
    private Transform groundCheck;
    private Transform wallCheck;
    private LayerMask whatIsGround;
    private LayerMask RW_Wall;
    private LayerMask WW_Wall;
    private LayerMask RW_WallJump;
    private LayerMask WW_WallJump;
    private LayerMask RW_Objects, WW_Objects, RW_WW_Object;
    private Vector3 Scale;
    private SpriteRenderer bobRenderer;

    // Start is called before the first frame update
    void Start() {
        InitRefsFromCode();
        isDead = false;
        isGrabbing = false;
        isRight = true;
        Scale = transform.localScale;
        isPaused = false;
        isWallMerged = false;
        wallMergesLeft = wallMergesLimit;
        gravityScale = rb2d.gravityScale;
        jumpWait = false;
        canGrab = false;
        // set layer automatically
        gameObject.layer = LayerMask.NameToLayer("RW_Bob");
        bobRenderer.sortingLayerName = "Real_Bob";
    }
    
    private void InitRefsFromCode() {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        
        groundCheck = transform.Find("GroundChecker");
        wallCheck = transform.Find("WallChecker");

        whatIsGround = LayerMask.GetMask("Ground");
        RW_Wall = LayerMask.GetMask("Real World");
        WW_Wall = LayerMask.GetMask("Wall World");

        RW_WallJump = LayerMask.GetMask("RW_WallJump");
        WW_WallJump = LayerMask.GetMask("WW_WallJump");

        RW_Objects = LayerMask.GetMask("RW_Objects");
        WW_Objects = LayerMask.GetMask("WW_Objects");
        RW_WW_Object = LayerMask.GetMask("RW_WW_Object");

        bobRenderer = GetComponent<SpriteRenderer>();
        // FUTURE: other public params that are fixed
    }


    // Update is called once per frame
    void Update() {
        // is game paused? could get reference to PauseButtonsHandler 
        // to figure out, or something like this check
        if (Time.timeScale == 0.0f) {
            return;  // paused
        }
        // is dead yet?
        if (isDead) {
            SetAnimParameters();
            return;  // cannot do anything
        }

        if (Input.GetKeyDown(COMMON.WALL_MERGE_KEY)) {
            HandleWallMerging();
        }
        SetParameters();
        if (!isPaused) {
            if (isGrabbing) {
                HandleWallJump();
            } else {
                if (wallJumpCount <= 0) {
                    HandleMovement();
                } else {
                    wallJumpCount -= Time.deltaTime;
                }
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

    private void SetParameters() {
        // isGrabbing = false;
        bool ground = Physics2D.OverlapCircle(groundCheck.position, 
                                            groundCheckRadius, whatIsGround);
        bool wallGround = false;
        if (isWallMerged) {
            isAlongWall = Physics2D.OverlapCircle(wallCheck.position,
                                            wallCheckRadius, WW_WallJump);
            wallGround = Physics2D.OverlapCircle(groundCheck.position,
                                            groundCheckRadius, WW_Wall) ||
                         Physics2D.OverlapCircle(groundCheck.position, 
                                            groundCheckRadius, WW_Objects);
        } else {
            isAlongWall = Physics2D.OverlapCircle(wallCheck.position,
                                            wallCheckRadius, RW_WallJump);
            wallGround = Physics2D.OverlapCircle(groundCheck.position,
                                            groundCheckRadius, RW_Wall) ||
                         Physics2D.OverlapCircle(groundCheck.position, 
                                            groundCheckRadius, RW_Objects);
        }
        // also detect standing on RWWWWALLs
        wallGround = wallGround || Physics2D.OverlapCircle(groundCheck.position,
                                            groundCheckRadius, RW_WW_Object);
        
        isGrounded = (ground || wallGround);

        if (!isGrounded) {
            if (!isAlongWall) {
                canGrab = true;
            }
        } else {
            canGrab = false;
        }

        if (isAlongWall && canGrab) {
            if ((isRight && Input.GetAxisRaw("Horizontal") > 0) ||
                (!isRight && Input.GetAxisRaw("Horizontal") < 0)) {
                isGrabbing = true;
                jumpWait = true;
            }
        }
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
            bobRenderer.sortingLayerName = "Wall_Bob";
        } else {
            gameObject.layer = LayerMask.NameToLayer("RW_Bob");
            bobRenderer.sortingLayerName = "Real_Bob";
        }
        LoggingController.LevelMerge();
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
    }

    private void HandleWallJump() {
        if (Input.GetButtonDown("Jump") || 
        ((isRight && Input.GetAxisRaw("Horizontal") < 0) ||
        (!isRight && Input.GetAxisRaw("Horizontal") > 0))) {
            wallJumpCount = wallJumpTime;
            isGrabbing = false;
            rb2d.gravityScale = gravityScale;
            float jumpComp = jumpSpeed * (1f / Mathf.Sqrt(2));
            float direction = isRight ? -1f : 1f;
            rb2d.velocity = new Vector2(jumpComp * direction, jumpComp);
            Flip();
            canGrab = true;
            LoggingController.LevelWallJump();
        } else if ((isRight && Input.GetAxisRaw("Horizontal") > 0) ||
        (!isRight && Input.GetAxisRaw("Horizontal") < 0)) {
            rb2d.velocity = new Vector2(0f, 0f);
            rb2d.gravityScale = 0f;
        } else {
            if (jumpWait) {
                grabCount = grabTime;
                jumpWait = false;
                rb2d.velocity = new Vector2(0f, 0f);
                rb2d.gravityScale = 0f;
            } else {
                if (grabCount <= 0) {
                    isGrabbing = false;
                } else {
                    grabCount -= Time.deltaTime;
                }
            }
        }
    }

    private void HandleJump() {
        if (isGrounded) {
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpSpeed);
            LoggingController.LevelJump();
        }
    }

    private void SetAnimParameters() {
        animator.SetFloat("Speed", Mathf.Abs(rb2d.velocity.x));
        animator.SetBool("isGrounded", isGrounded);
        animator.SetBool("isWallMerged", isWallMerged);
        animator.SetBool("isDead", isDead);
        animator.SetBool("isGrabbing", isGrabbing);
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

    private float airSpeedDivider = 20;
    private float airDragMultiplier = 0.15f;
    private float airDragMax = 0.1f;
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
        rb2d.velocity = new Vector2(0, 0);
        isPaused = true;
    }

    public void resumeMovement() {
        isPaused = false;
    }

}
