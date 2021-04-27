using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float moveSpeed;
    public float jumpSpeed = 3;
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask whatIsGround;
    public bool isGrounded;
    private Rigidbody2D rb2d;
    private Animator animator;
    private bool isWallMerged = false;

    // Start is called before the first frame update
    void Start() {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 
                                            groundCheckRadius, whatIsGround);
        
        // Wall merge
        if (Input.GetKeyDown(KeyCode.W)) {
            isWallMerged = !isWallMerged;
            if (isWallMerged) {
                gameObject.layer = LayerMask.NameToLayer("Real World");
            } else {
                gameObject.layer = LayerMask.NameToLayer("Wall World");
            }
        }
        if (Input.GetAxisRaw("Horizontal") > 0f) {
            rb2d.velocity = new Vector2(moveSpeed, rb2d.velocity.y);
            // change local scale depending upon sprite's scale values
            transform.localScale = new Vector3(1.55f,2.22f,1f);
        } else if (Input.GetAxisRaw("Horizontal") < 0f) {
            rb2d.velocity = new Vector2(-moveSpeed, rb2d.velocity.y);
            // change local scale depending upon sprite's scale values
            transform.localScale = new Vector3(-1.55f,2.22f,1f);
        } else {
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
        }

        if (Input.GetButtonDown("Jump") && isGrounded) {
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpSpeed);
        }
        animator.SetFloat("Speed", Mathf.Abs(rb2d.velocity.x));
        animator.SetBool("isGrounded", isGrounded);
    }
}
