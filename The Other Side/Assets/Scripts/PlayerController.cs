using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float moveSpeed = 6;
    public float jumpSpeed = 12;
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;
    public float wallCheckRadius = 0.5f;
    public LayerMask whatIsGround;
    public LayerMask whatIsWall;
    public bool isGrounded;
    public bool isNextToWall;
    public Sprite Real_Bob;
    public Sprite Wall_Bob;
    private Rigidbody2D rb2d;
    private Animator animator;
    private bool isWallMerged;
    private Color Real_World_Color;
    private Color Wall_World_Color;

    // Start is called before the first frame update
    void Start() {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        isWallMerged = false;
        Real_World_Color = GetComponent<SpriteRenderer>().color;
        Wall_World_Color = new Color(0,0,0,1);
    }

    // Update is called once per frame
    void Update() {
        // Wall merging?
        if (Input.GetKeyDown(KeyCode.J)) {
            isWallMerged = !isWallMerged;
            if (isWallMerged) {
                gameObject.layer = LayerMask.NameToLayer("Wall World");
            } else {
                gameObject.layer = LayerMask.NameToLayer("Real World");
            }
        }
        // environments
        // isGrounded = Physics2D.OverlapCircle(groundCheck.position, 
        //                                     groundCheckRadius, whatIsGround);
        isGrounded = Mathf.Abs(rb2d.velocity.y) < 0.1f;  // peek of a jump == isGounded!!

        LayerMask curr_layer = isWallMerged ? LayerMask.NameToLayer("Wall World") : LayerMask.NameToLayer("Real World");
        isNextToWall = Physics2D.OverlapCircle(
            groundCheck.position, 
            wallCheckRadius, 
            curr_layer);
            // );
        // isNextToWall = Physics2D.IsTouchingLayers(rb2d.GetComponent<Collider2D>(), curr_layer);

        ChangeSpeedWForce();
        updateBobsLook();

        /*
        if (Input.GetAxisRaw("Horizontal") > 0f) {
            // move right
            rb2d.velocity = new Vector2(moveSpeed, rb2d.velocity.y);
        } else if (Input.GetAxisRaw("Horizontal") < 0f) {
            // move left
            rb2d.velocity = new Vector2(-moveSpeed, rb2d.velocity.y);
        } else {
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
        }
        */

        animator.SetFloat("Speed", Mathf.Abs(rb2d.velocity.x));
        animator.SetBool("isGrounded", isGrounded);
        animator.SetBool("isWallMerged", isWallMerged);
    }


    void updateBobsLook() {
        // change local scale depending upon sprite's scale values
        if (Input.GetAxisRaw("Horizontal") > 0f) {  // moving right
            transform.localScale = new Vector3(1.55f,2.22f,1f);
        } else if (Input.GetAxisRaw("Horizontal") < 0f) {   // moving left
            transform.localScale = new Vector3(-1.55f,2.22f,1f);
        }
        // wall merged look
        if (isWallMerged) {
            gameObject.GetComponent<SpriteRenderer>().color = Wall_World_Color;
        } else {
            gameObject.GetComponent<SpriteRenderer>().color = Real_World_Color;
        }
    }

    // one way to control, does not get stuck on wall
    // change Bob's speed using force
    // dont not feel very actiony tho, dragging a bit
    void ChangeSpeedWForce() {
        // left or right
        if (Input.GetAxisRaw("Horizontal") > 0f) {
            // move right
            rb2d.AddForce(new Vector2(moveSpeed, 0));
            // too fast
            if (rb2d.velocity.x > moveSpeed) {
                rb2d.velocity = new Vector2(moveSpeed, rb2d.velocity.y);
            }
        } else if (Input.GetAxisRaw("Horizontal") < 0f) {
            // move left
            rb2d.AddForce(new Vector2(-moveSpeed, 0));
            // too fast
            if (rb2d.velocity.x < -moveSpeed) {
                rb2d.velocity = new Vector2(-moveSpeed, rb2d.velocity.y);
            }
        } else {
            rb2d.velocity = new Vector2(rb2d.velocity.x / 1.09f, rb2d.velocity.y);
        }

        // jump
        if (Input.GetButtonDown("Jump")) {
            if (isGrounded) {
                rb2d.velocity = new Vector2(rb2d.velocity.x, jumpSpeed);
            } else if (isNextToWall) {
                Debug.Log("wall jumping");
                // wall jump
                // flip direction!
                rb2d.velocity = new Vector2(-rb2d.velocity.x, jumpSpeed);
            }
        }
    }
}
