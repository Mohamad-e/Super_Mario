using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector3 mVelocity = Vector3.zero;

    //Player Movement
    private float speed = 3f;
    private float horizontalMove;
    private float jumpForce = 7;
    
    [SerializeField]
    private float dashingPower = 6;

    private bool doubleJump = true;
    private bool isGrounded = true;
    private bool facingRight = true;
    private bool isDashing = false;
    private bool isCrouching = false;
    private bool isAttacking = false;
    private bool isStriking = false;
    private bool isFireballing = false;

    //animator Object
    public Animator animator;

    //Fireball
    public GameObject fireball;
    public Transform fireballSpawn;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Player Movement
        horizontalMove = Input.GetAxis("Horizontal") * speed;
        Vector3 targetVelocity = new Vector2(horizontalMove, rb.velocity.y);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref mVelocity, .05f);
        animator.SetFloat("speed", Mathf.Abs(horizontalMove));

        //change direction of player facing
        if (horizontalMove > 0 && !facingRight)
        {
            flip();
        } 
        else if(horizontalMove < 0 && facingRight)
        {
            flip();
        }


        //sprinting
        if (Input.GetKey(KeyCode.E) && isGrounded)
        {
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity*2f, ref mVelocity, .05f);
        }

        
        //crouching
        if(Input.GetKeyDown(KeyCode.S) && isGrounded && !isAttacking && !isDashing && !isStriking && !isFireballing)
        {
            animator.SetBool("crouching", true);
            speed = 0;
            isCrouching = true;
        } 
        else if(Input.GetKeyUp(KeyCode.S) && isGrounded)
        {
            animator.SetBool("crouching", false);
            speed = 3f;
            isCrouching = false;
        }


        //Jumping
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isCrouching && !isDashing )
        {
            isGrounded = false;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            animator.SetBool("jumping", true);
            doubleJump = true;
        }
        //double Jump
        else if(Input.GetKeyDown(KeyCode.Space) && doubleJump && !isCrouching)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            animator.SetBool("jumping", true);
            doubleJump = false;
        }

        //attacking
        if (Input.GetKeyDown(KeyCode.K) && !isDashing && !isAttacking && !isCrouching && !isFireballing && !isStriking)
        {
            animator.SetTrigger("attacking");
            isAttacking = true;
        }

        //jumpAttack
        /*if (Input.GetKeyDown(KeyCode.K) && !isGrounded)
        {
            animator.SetTrigger("attacking");
        }*/

        //dashing
        if (Input.GetKeyDown(KeyCode.L) && isGrounded && !isDashing && !isAttacking && !isStriking && !isCrouching && !isFireballing)
        {
            animator.SetTrigger("dashing");
            isDashing = true;
            if (facingRight)
            {
                //rb.velocity += new Vector2( dashingPower, 0f);
                rb.AddForce(Vector2.right * dashingPower, ForceMode2D.Impulse);
            }
            else
            {
                //rb.velocity += new Vector2(-dashingPower, 0f);
                rb.AddForce(-Vector2.right * dashingPower, ForceMode2D.Impulse);
            }
        }

        //blocking
        if(Input.GetKeyDown(KeyCode.B))
        {
            animator.SetTrigger("blocking");
        }

        //fireball
        if (Input.GetKeyDown(KeyCode.F) && !isDashing && !isAttacking && !isCrouching && !isStriking && !isFireballing)
        {
            animator.SetTrigger("fireball");
            isFireballing = true;
            /*//List<Object> Objects = new List<Object>();
            GameObject fireBall = Instantiate(fireball, fireballSpawn.transform.position, transform.rotation);
            fireBall.SetActive(true);
            isFireballing = true;*/
        }

        //striking
        if (Input.GetKeyDown(KeyCode.J) && isGrounded && !isDashing && !isAttacking && !isCrouching && !isStriking && !isFireballing)
        {
            animator.SetTrigger("striking");
            isStriking = true;
        }

        //check if player touches ground
        if (isGrounded)
        {
            animator.SetBool("jumping", false);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ground")
            isGrounded = true;
    }

    private void flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }


    private void attackArea()
    {
        List<RaycastHit2D> hits = new List<RaycastHit2D>();
        if (facingRight)
        {
            Vector3 startArea = this.transform.position + 0.3f * Vector3.up + 0.5f * Vector3.right;
            hits.Add(Physics2D.Raycast(startArea, Vector3.right, 1.0f));
            Debug.DrawRay(startArea, Vector3.right * 1.0f, Color.white, 10.0f);

            startArea = this.transform.position + 0.0f * Vector3.up + 0.5f * Vector3.right;
            hits.Add(Physics2D.Raycast(startArea, Vector3.right, 1.1f));
            Debug.DrawRay(startArea, Vector3.right * 1.1f, Color.white, 10.0f);

            startArea = this.transform.position + (-0.3f) * Vector3.up + 0.5f * Vector3.right;
            Debug.Log(startArea);
            Debug.Log(Vector3.right);
            hits.Add(Physics2D.Raycast(startArea, Vector3.right, 1.2f));
            Debug.DrawRay(startArea, Vector3.right * 1.2f, Color.white, 10.0f);

            startArea = this.transform.position + (-0.6f) * Vector3.up + 0.5f * Vector3.right;
            hits.Add(Physics2D.Raycast(startArea, Vector3.right, 1.3f));
            Debug.DrawRay(startArea, Vector3.right * 1.3f, Color.white, 10.0f);

            startArea = this.transform.position + (-0.9f) * Vector3.up + 0.5f * Vector3.right;
            hits.Add(Physics2D.Raycast(startArea, Vector3.right, 1.3f));
            Debug.DrawRay(startArea, Vector3.right * 1.3f, Color.white, 10.0f);
        }
        else
        {
            Vector3 startArea = this.transform.position + 0.3f * Vector3.up - 0.5f * Vector3.right;
            hits.Add(Physics2D.Raycast(startArea, -Vector3.right, 1.0f));
            Debug.DrawRay(startArea, -Vector3.right * 1.0f, Color.white, 10.0f);

            startArea = this.transform.position + 0.0f * Vector3.up - 0.5f * Vector3.right;
            hits.Add(Physics2D.Raycast(startArea, -Vector3.right, 1.1f));
            Debug.DrawRay(startArea, -Vector3.right * 1.1f, Color.white, 10.0f);

            startArea = this.transform.position + (-0.3f) * Vector3.up - 0.5f * Vector3.right;
            hits.Add(Physics2D.Raycast(startArea, -Vector3.right, 1.2f));
            Debug.DrawRay(startArea, -Vector3.right * 1.2f, Color.white, 10.0f);

            startArea = this.transform.position + (-0.6f) * Vector3.up - 0.5f * Vector3.right;
            hits.Add(Physics2D.Raycast(startArea, -Vector3.right, 1.3f));
            Debug.DrawRay(startArea, -Vector3.right * 1.3f, Color.white, 10.0f);

            startArea = this.transform.position + (-0.9f) * Vector3.up - 0.5f * Vector3.right;
            hits.Add(Physics2D.Raycast(startArea, -Vector3.right, 1.3f));
            Debug.DrawRay(startArea, -Vector3.right * 1.3f, Color.white, 10.0f);
        }
        List<Collider2D> objectsHit = new List<Collider2D>();
        for (int i = 0; i < 5; ++i)
        {
            if (hits[i].collider == null)
            {
                Debug.Log("No Hit");
            }
            else if (hits[i].collider.tag == "Enemy")
            {
                Debug.Log("Hit enemy");
                objectsHit.Add(hits[i].collider);
            }
            else
            {
                Debug.Log("Hit no enemy");
            }
        }
        hits = new List<RaycastHit2D>();
    }

    private void endAttacking()
    {
        isAttacking = false;
    }

    private void endDashing()
    {
        isDashing = false;
    }

    private void endStriking()
    {
        isStriking = false;
    }

    private void endFireballing()
    {
        isFireballing = false;
    }

    private void fireballing()
    {
        //List<Object> Objects = new List<Object>();
        GameObject fireBall = Instantiate(fireball, fireballSpawn.transform.position, transform.rotation);
        fireBall.SetActive(true);
    }
}
