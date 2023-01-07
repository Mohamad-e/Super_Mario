using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector3 mVelocity = Vector3.zero;

    //Player Movement
    [SerializeField]
    private float speed = 3f;
    private float horizontalMove;
    private float jumpForce = 7;
    
    [SerializeField]
    private float dashingPower = 6f;
    [SerializeField]
    private float attackStrength = 10f;
    [SerializeField]
    private float strikeStrength = 30f;
    [SerializeField]
    private float blockingFactor = 0.3f;

    private bool doubleJump = true;
    private bool isGrounded = true;
    private bool facingRight = true;
    private bool isDashing = false;
    private bool isCrouching = false;
    private bool isAttacking = false;
    private bool isStriking = false;
    private bool isFireballing = false;
    private bool isParrying = false;
    public bool parryFrameAcive = false;
    private bool blockReady = false;
    public bool isBlocking = false;

    
    

    //Player Status
    public bool dizzy = false;

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
        //death
        if(this.gameObject.GetComponent<PlayerStats>().currentHealth <= 0)
        {
            animator.SetBool("dead", true);
        }

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
        if (Input.GetKeyDown(KeyCode.E) && isGrounded && !isCrouching && !isParrying && !isBlocking)
        {
            speed = 10f;
            //Debug.Log("sprinting");
            //rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity*2f, ref mVelocity, .05f);
        }else if(Input.GetKeyUp(KeyCode.E) && !isCrouching && !isParrying && !isBlocking)
        {
            //Debug.Log("stop sprinting");
            speed = 3f;
        }

        
        //crouching
        if(Input.GetKeyDown(KeyCode.S) && isGrounded && !isAttacking && !isDashing && !isStriking && !isFireballing && !isBlocking && !blockReady && !isParrying)
        {
            animator.SetBool("crouching", true);
            speed = 0;
            isCrouching = true;
        } 
        else if(Input.GetKeyUp(KeyCode.S) && isGrounded && !isAttacking && !isDashing && !isStriking && !isFireballing && !isBlocking && !blockReady && !isParrying)
        {
            animator.SetBool("crouching", false);
            speed = 3f;
            isCrouching = false;
        }


        //Jumping
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isCrouching && !isDashing && !isBlocking && !blockReady)
        { 
            isGrounded = false;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            animator.SetBool("jumping", true);
            doubleJump = true;
        }
        //double Jump
        else if(Input.GetKeyDown(KeyCode.Space) && doubleJump && !isCrouching && !isBlocking && !blockReady)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            animator.SetBool("jumping", true);
            doubleJump = false;
        }

        //attacking
        if (Input.GetKeyDown(KeyCode.K) && !isDashing && !isAttacking && !isCrouching && !isFireballing && !isStriking && !isBlocking && !blockReady &&  !isParrying)
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
        if (Input.GetKeyDown(KeyCode.L) && isGrounded && !isDashing && !isAttacking && !isStriking && !isCrouching && !isFireballing && !isBlocking && !blockReady)
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

        //parrying + blocking
        if(Input.GetKeyDown(KeyCode.B) && !isDashing && !isAttacking && !isStriking && !isCrouching && !isFireballing && !isParrying)
        {
            animator.SetTrigger("parrying");
            isParrying = true;
            
        }
        if (Input.GetKey(KeyCode.B) && blockReady && isGrounded)
        {
            animator.SetBool("blocking", true);
            isBlocking = true;
            speed = 1f;
        }
        else if(Input.GetKeyUp(KeyCode.B) && !isCrouching && !isFireballing && !isParrying)
        {
            animator.SetBool("blocking", false);
            isBlocking = false;
            blockReady = false;
            speed = 3f;
        }

        //fireball
        if (Input.GetKeyDown(KeyCode.F) && !isDashing && !isAttacking && !isCrouching && !isStriking && !isFireballing && !isParrying && !isBlocking && !blockReady)
        {
            animator.SetTrigger("fireball");
            isFireballing = true;
            /*//List<Object> Objects = new List<Object>();
            GameObject fireBall = Instantiate(fireball, fireballSpawn.transform.position, transform.rotation);
            fireBall.SetActive(true);
            isFireballing = true;*/
        }

        //striking
        if (Input.GetKeyDown(KeyCode.J) && isGrounded && !isDashing && !isAttacking && !isCrouching && !isStriking && !isFireballing && !isBlocking && !blockReady)
        {
            animator.SetTrigger("striking");
            isStriking = true;
        }

        if (dizzy == true)
        {
            animator.SetTrigger("dizzy");
            dizzy = false;
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
        else if (collision.gameObject.tag == "CaveEntrance")
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        else if (collision.gameObject.tag == "CaveExit")
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    private void flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }


    private void attackArea()
    {
        List<RaycastHit2D[]> hits = new List<RaycastHit2D[]>();
        if (facingRight)
        {
            Vector3 startArea = this.transform.position + 0.3f * Vector3.up + 0.5f * Vector3.right;
            hits.Add(Physics2D.RaycastAll(startArea, Vector3.right, 1.0f));
            Debug.DrawRay(startArea, Vector3.right * 1.0f, Color.white, 10.0f);

            startArea = this.transform.position + 0.0f * Vector3.up + 0.5f * Vector3.right;
            hits.Add(Physics2D.RaycastAll(startArea, Vector3.right, 1.1f));
            Debug.DrawRay(startArea, Vector3.right * 1.1f, Color.white, 10.0f);

            startArea = this.transform.position + (-0.3f) * Vector3.up + 0.5f * Vector3.right;
            //Debug.Log(startArea);
            //Debug.Log(Vector3.right);
            hits.Add(Physics2D.RaycastAll(startArea, Vector3.right, 1.2f));
            Debug.DrawRay(startArea, Vector3.right * 1.2f, Color.white, 10.0f);

            startArea = this.transform.position + (-0.6f) * Vector3.up + 0.5f * Vector3.right;
            hits.Add(Physics2D.RaycastAll(startArea, Vector3.right, 1.3f));
            Debug.DrawRay(startArea, Vector3.right * 1.3f, Color.white, 10.0f);

            startArea = this.transform.position + (-0.9f) * Vector3.up + 0.5f * Vector3.right;
            hits.Add(Physics2D.RaycastAll(startArea, Vector3.right, 1.3f));
            Debug.DrawRay(startArea, Vector3.right * 1.3f, Color.white, 10.0f);
        }
        else
        {
            Vector3 startArea = this.transform.position + 0.3f * Vector3.up - 0.5f * Vector3.right;
            hits.Add(Physics2D.RaycastAll(startArea, -Vector3.right, 1.0f));
            Debug.DrawRay(startArea, -Vector3.right * 1.0f, Color.white, 10.0f);

            startArea = this.transform.position + 0.0f * Vector3.up - 0.5f * Vector3.right;
            hits.Add(Physics2D.RaycastAll(startArea, -Vector3.right, 1.1f));
            Debug.DrawRay(startArea, -Vector3.right * 1.1f, Color.white, 10.0f);

            startArea = this.transform.position + (-0.3f) * Vector3.up - 0.5f * Vector3.right;
            hits.Add(Physics2D.RaycastAll(startArea, -Vector3.right, 1.2f));
            Debug.DrawRay(startArea, -Vector3.right * 1.2f, Color.white, 10.0f);

            startArea = this.transform.position + (-0.6f) * Vector3.up - 0.5f * Vector3.right;
            hits.Add(Physics2D.RaycastAll(startArea, -Vector3.right, 1.3f));
            Debug.DrawRay(startArea, -Vector3.right * 1.3f, Color.white, 10.0f);

            startArea = this.transform.position + (-0.9f) * Vector3.up - 0.5f * Vector3.right;
            hits.Add(Physics2D.RaycastAll(startArea, -Vector3.right, 1.3f));
            Debug.DrawRay(startArea, -Vector3.right * 1.3f, Color.white, 10.0f);
        }
        List<Collider2D> objectsHit = new List<Collider2D>();
        for (int i = 0; i < 5; ++i)
        {
            //Debug.Log(hits[i].Length);
            for (int j = 0; j < hits[i].Length; j++)
            {
                if (hits[i][j].collider == null)
                {
                    //Debug.Log("No Hit");
                }
                else if (hits[i][j].collider.tag == "Enemy")    
                {
                    //Debug.Log("Hit enemy");
                    if (!objectsHit.Contains(hits[i][j].collider))
                    {
                        objectsHit.Add(hits[i][j].collider);
                        hits[i][j].collider.GetComponent<enemy>().health -= (int)((float)(attackStrength)); // + this.gameObject.GetComponent<PlayerStats>().playerStrength 
                    }
                    //Debug.Log(hits[i][j].collider.name);
                }
                else
                {
                    //Debug.Log("Hit no enemy");
                }
            }
        }
        objectsHit = new List<Collider2D>();
        hits = new List<RaycastHit2D[]>();
    }

    private void strikeArea()
    {
        List<RaycastHit2D[]> hits = new List<RaycastHit2D[]>();
        if (facingRight)
        {
            Vector3 startArea = this.transform.position + (-0.45f) * Vector3.up + 0.5f * Vector3.right;
            hits.Add(Physics2D.RaycastAll(startArea, Vector3.right, 1.3f));
            Debug.DrawRay(startArea, Vector3.right * 1.3f, Color.white, 10.0f);

            startArea = this.transform.position + (-0.75f) * Vector3.up + 0.5f * Vector3.right;
            hits.Add(Physics2D.RaycastAll(startArea, Vector3.right, 1.3f));
            Debug.DrawRay(startArea, Vector3.right * 1.3f, Color.white, 10.0f);

            startArea = this.transform.position + (-1.05f) * Vector3.up + 0.5f * Vector3.right;
            hits.Add(Physics2D.RaycastAll(startArea, Vector3.right, 1.3f));
            Debug.DrawRay(startArea, Vector3.right * 1.3f, Color.white, 10.0f);
        }    
        else
        {
            Vector3 startArea = this.transform.position + (-0.45f) * Vector3.up - 0.5f * Vector3.right;
            hits.Add(Physics2D.RaycastAll(startArea, -Vector3.right, 1.3f));
            Debug.DrawRay(startArea, -Vector3.right* 1.3f, Color.white, 10.0f);

            startArea = this.transform.position + (-0.75f) * Vector3.up - 0.5f * Vector3.right;
            hits.Add(Physics2D.RaycastAll(startArea, -Vector3.right, 1.3f));
            Debug.DrawRay(startArea, -Vector3.right * 1.3f, Color.white, 10.0f);

            startArea = this.transform.position + (-1.05f) * Vector3.up - 0.5f * Vector3.right;
            hits.Add(Physics2D.RaycastAll(startArea, -Vector3.right, 1.3f));
            Debug.DrawRay(startArea, -Vector3.right* 1.3f, Color.white, 10.0f);
        }

        List<Collider2D> objectsHit = new List<Collider2D>();

        for (int i = 0; i < 3; ++i)
        {
            //Debug.Log(hits[i].Length);
            for (int j = 0; j < hits[i].Length; j++)
            {

                if (hits[i][j].collider == null)
                {
                    //Debug.Log("No Hit");
                }
                else if (hits[i][j].collider.tag == "Enemy")
                {

                    //Debug.Log("Hit enemy");
                    if (!objectsHit.Contains(hits[i][j].collider))
                    {
                        objectsHit.Add(hits[i][j].collider);
                        hits[i][j].collider.GetComponent<enemy>().health -= (int)((float)(strikeStrength)); // + this.gameObject.GetComponent<PlayerStats>().playerStrength 
                    }

                    //Debug.Log(hits[i][j].collider.name);

                }
                else
                {
                    //Debug.Log("Hit no enemy");
                }
            }
        }
        objectsHit = new List<Collider2D>();
        hits = new List<RaycastHit2D[]>();
    }

    public void applyDamage(float dmg, RaycastHit hitPoint)
    {
        if (parryFrameAcive)
        {
            if (hitPoint.point.x > this.transform.position.x + 1 && facingRight)
            {

                //play animation for succesful parrying
                
            }
            else if(hitPoint.point.x < this.transform.position.x - 1 && !facingRight)
            {
                //play animation for succesful parrying
            }
        } 
        else if(isBlocking)
        {
            if (hitPoint.point.x > this.transform.position.x + 1 && facingRight)
            {
                //play animation for succesful blocking
                this.gameObject.GetComponent<PlayerStats>().currentHealth -= (int)(dmg*blockingFactor);
            }
            else if (hitPoint.point.x < this.transform.position.x - 1 && !facingRight)
            {
                //play animation for succesful blocking
                this.gameObject.GetComponent<PlayerStats>().currentHealth -= (int)(dmg * blockingFactor);
            }
        }
        else
        {
            this.gameObject.GetComponent<PlayerStats>().currentHealth -= (int)(dmg);
        }
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

    private void endParrying()
    {
        isParrying = false;
        blockReady = true;
    }

    private void startParryingFrame()
    {
        parryFrameAcive = true;
    }

    private void endParryingFrame()
    {
        parryFrameAcive = false;
    }

    private void fireballing()
    {
        //List<Object> Objects = new List<Object>();
        GameObject fireBall = Instantiate(fireball, fireballSpawn.transform.position, transform.rotation);
        fireBall.SetActive(true);
    }

    private void resetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
