using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector3 mVelocity = Vector3.zero;


    private float speed = 3f;
    private float jumpForce = 7;

    private float dashingPower = 15;

    private bool doubleJump = true;
    private bool isGrounded = true;
    private bool facingRight = true;
    private bool isDashing = false;

    public Animator animator;

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
        float horizontalMove = Input.GetAxis("Horizontal") * speed;
        //transform.Translate(horizontalMove * Time.deltaTime, 0, 0);
        
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

        //Jumping
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            isGrounded = false;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            animator.SetBool("jumping", true);
            doubleJump = true;
        }
        //double Jump
        else if(Input.GetKeyDown(KeyCode.Space) && doubleJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            animator.SetBool("jumping", true);
            doubleJump = false;
        }

        //attacking
        if (Input.GetKeyDown(KeyCode.K))
        {
            animator.SetTrigger("attacking");
        }

        //jumpAttack
        /*if (Input.GetKeyDown(KeyCode.K) && !isGrounded)
        {
            animator.SetTrigger("attacking");
        }*/

        //dashing
        if (Input.GetKey(KeyCode.L) && isGrounded && !isDashing)
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
        if (Input.GetKeyDown(KeyCode.F))
        {
            animator.SetTrigger("fireball");
            List<Object> Objects = new List<Object>();
            GameObject fireBall = Instantiate(fireball, fireballSpawn.transform.position, transform.rotation);
            fireBall.SetActive(true);
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

    private void notDashing()
    {
        isDashing = false;
    }
}
