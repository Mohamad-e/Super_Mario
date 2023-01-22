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
    public bool isStriking = false;
    private bool strikingCooldown = false;
    private bool isFireballing = false;
    private bool isParrying = false;
    private bool afterParry = false;
    public bool parryFrameAcive = false;
    private bool blockReady = false;
    public bool isBlocking = false;
    public bool isGettingHurt = false;
    private bool isGettingHurtAnimation = false;
    public bool isGettingHurtFrame = false;
    private bool invincibilityCooldown = false;

    public bool respawnBomb;

    // Timer
    [SerializeField]
    private float strikeTimerAlive = 10.0f;
    private float strikeTimer = 0.0f;
    [SerializeField]
    private float invincibilityFrameTimerAlive = 3.0f;
    private float invincibilityFrameTimer = 0;

    //Player Status
    public bool dizzy = false;

    //animator Object
    public Animator animator;

    //Fireball
    public GameObject fireball;
    public GameObject bomb;
    public Transform fireballSpawn;


    //Audio
    public AudioSource attackSound;
    public AudioSource pickupSound;
    public AudioSource levelupSound;
    public AudioSource jumpSound;
    public AudioSource fireballSound;

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
        if (Input.GetKeyDown(KeyCode.E) && isGrounded && !isCrouching && !isParrying && !isBlocking && !isGettingHurtAnimation)
        {
            speed = 6f;
            //Debug.Log("sprinting");
            //rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity*2f, ref mVelocity, .05f);
        }else if(Input.GetKeyUp(KeyCode.E) && !isCrouching && !isParrying && !isBlocking && !isGettingHurtAnimation)
        {
            //Debug.Log("stop sprinting");
            speed = 3f;
        }

        
        //crouching
        /*if(Input.GetKeyDown(KeyCode.S) && isGrounded && !isAttacking && !isDashing && !isStriking && !isFireballing && !isBlocking && !blockReady && !isParrying && !isGettingHurtAnimation)
        {
            animator.SetBool("crouching", true);
            speed = 0;
            isCrouching = true;
        } 
        else if(Input.GetKeyUp(KeyCode.S) && isGrounded && !isAttacking && !isDashing && !isStriking && !isFireballing && !isBlocking && !blockReady && !isParrying && !isGettingHurtAnimation)
        {
            animator.SetBool("crouching", false);
            speed = 3f;
            isCrouching = false;
        }*/


        //Jumping
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isCrouching && !isDashing && !isBlocking && !blockReady && !isGettingHurtAnimation)
        { 
            isGrounded = false;

            float raySize = 1.4f;
            RaycastHit2D[] hits = new RaycastHit2D[3];
            Debug.DrawRay(gameObject.transform.position, Vector2.down * raySize, Color.white, 5);
            hits[0] = Physics2D.Raycast(gameObject.transform.position, Vector2.down * raySize);

            Debug.DrawRay(gameObject.transform.position + (Vector3.left*0.25f), Vector2.down * raySize, Color.white, 5);
            hits[1] = Physics2D.Raycast(gameObject.transform.position + (Vector3.left * 0.25f), Vector2.down * raySize);

            Debug.DrawRay(gameObject.transform.position + (Vector3.right * 0.25f), Vector2.down * raySize, Color.white, 5);
            hits[2] = Physics2D.Raycast(gameObject.transform.position + (Vector3.right * 0.25f), Vector2.down * raySize);
            
            bool onGround = false;
            for(int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider != null)
                {
      
                    if (hits[i].collider.tag == "ground")
                    {
                        Debug.Log("raycast");
                        onGround = true;
                        break;
                    }
                }
            }
            if (onGround)
            {
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                animator.SetBool("jumping", true);
                doubleJump = true;
                jumpSound.Play();
            }
            
        }
        //double Jump
        else if(Input.GetKeyDown(KeyCode.Space) && doubleJump && !isCrouching && !isBlocking && !blockReady && !isGettingHurtAnimation)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            animator.SetBool("jumping", true);
            doubleJump = false;
            jumpSound.Play();
        }

        //attacking
        if (Input.GetKeyDown(KeyCode.K) && !isDashing && !isAttacking && !isCrouching && !isFireballing && !isStriking && !isBlocking && !blockReady &&  !isParrying && !isGettingHurtAnimation)
        {
            animator.SetTrigger("attacking");
            isAttacking = true;
            if (isGrounded)
            {
                speed = 0f;
            }
            attackSound.Play();
        }

        //jumpAttack
        /*if (Input.GetKeyDown(KeyCode.K) && !isGrounded)
        {
            animator.SetTrigger("attacking");
        }*/

        //dashing
        /*if (Input.GetKeyDown(KeyCode.L) && isGrounded && !isDashing && !isAttacking && !isStriking && !isCrouching && !isFireballing && !isBlocking && !blockReady && !isGettingHurtAnimation)
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
        }*/

        //parrying + blocking
        if(Input.GetKeyDown(KeyCode.B) && !isDashing && !isAttacking && !isStriking && !isCrouching && !isFireballing && !isParrying && !isGettingHurtAnimation)
        {
            animator.SetTrigger("parrying");
            isParrying = true;
            afterParry = true;
        }
        if (Input.GetKey(KeyCode.B) && blockReady && isGrounded)
        {
            animator.SetBool("blocking", true);
            isBlocking = true;
            speed = 1f;
        }
        else if(!(Input.GetKey(KeyCode.B)) && !isCrouching && !isFireballing && !isParrying && afterParry)
        {
            animator.SetBool("blocking", false);
            isBlocking = false;
            blockReady = false;
            afterParry = false;
            speed = 3f;
        }

        //fireball
        if (Input.GetKeyDown(KeyCode.F) && !isDashing && !isAttacking && !isCrouching && !isStriking && !isFireballing && !isParrying && !isBlocking && !blockReady && !isGettingHurtAnimation && this.gameObject.GetComponent<PlayerStats>().mana >= 25)
        {
            animator.SetTrigger("fireball");
            isFireballing = true;
            this.gameObject.GetComponent<PlayerStats>().mana -= 25;
            fireballSound.Play();
            /*//List<Object> Objects = new List<Object>();
            GameObject fireBall = Instantiate(fireball, fireballSpawn.transform.position, transform.rotation);
            fireBall.SetActive(true);
            isFireballing = true;*/
        }

        //striking
        if (Input.GetKeyDown(KeyCode.J) && isGrounded && !isDashing && !isAttacking && !isCrouching && !isStriking && !isFireballing && !isBlocking && !blockReady && !isGettingHurtAnimation && !strikingCooldown)
        {
            animator.SetTrigger("striking");
            speed = 0f;
            
            strikingCooldown = true;
            isStriking = true;
        }
            // Timer
        if(strikingCooldown)
            if (strikeTimer < strikeTimerAlive)
            {
                strikeTimer += Time.deltaTime;
            }
            else if (strikeTimer >= strikeTimerAlive)
            {
                strikeTimer = 0;
                strikingCooldown = false;
            }

        if (dizzy)
        {
            animator.SetTrigger("dizzy");
            dizzy = false;
        }

        //check if player touches ground
        if (isGrounded)
        {
            animator.SetBool("jumping", false);
        }

        // Hurt
        if (isGettingHurt && !isGettingHurtAnimation)
        {
            isDashing = false;
            isCrouching = false;
            isAttacking = false;
            isStriking = false;
            isFireballing = false;
            isParrying = false;
            afterParry = false;
            parryFrameAcive = false;
            blockReady = false;
            isBlocking = false;

            isGettingHurtFrame = true;
            isGettingHurtAnimation = true;
            isGettingHurt = false;

            invincibilityCooldown = true;

            animator.SetTrigger("hurt");
        }
        // Timer
        if (invincibilityCooldown)
            if (invincibilityFrameTimer < invincibilityFrameTimerAlive)
            {
                invincibilityFrameTimer += Time.deltaTime;
            }
            else if (invincibilityFrameTimer >= invincibilityFrameTimerAlive)
            {
                invincibilityFrameTimer = 0;
                invincibilityCooldown = false;
                isGettingHurtFrame = false;
            }

        //bomb
        if(Input.GetKeyDown(KeyCode.R))
        {
            if (gameObject.GetComponent<PlayerStats>().bombCount > 0)
            {
                GameObject bombe = Instantiate(bomb, fireballSpawn.transform.position, transform.rotation);
                if (facingRight)
                {
                    
                    bombe.GetComponent<Rigidbody2D>().velocity = (new Vector2(1, 1)) * 5;
                   
                }else
                {
                    bombe.GetComponent<Rigidbody2D>().velocity = (new Vector2(-1, 1)) * 5;
                }

                gameObject.GetComponent<PlayerStats>().bombCount -= 1;
                gameObject.GetComponent<PlayerStats>().updateStats();
                if(gameObject.GetComponent<PlayerStats>().bombCount == 0)
                {
                    respawnBomb = true;
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ground")
            isGrounded = true;
        else if (collision.gameObject.tag == "CaveEntrance")
        {
            GameObject.Find("Player").GetComponent<PlayerStats>().saveStats();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }  
        else if (collision.gameObject.tag == "CaveExit")
        {
            GameObject.Find("Player").GetComponent<PlayerStats>().saveStats();
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex - 1);
        }
        else if (collision.gameObject.tag == "ExitToCredits")
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        }
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
                        hits[i][j].collider.GetComponent<enemy>().health -= (int)((float)(attackStrength)) + (int)(GameObject.Find("Player").GetComponent<PlayerStats>().playerStrength * .5f); // + this.gameObject.GetComponent<PlayerStats>().playerStrength 
                        if (facingRight)
                        {
                            hits[i][j].collider.gameObject.GetComponent<Rigidbody2D>().velocity = (new Vector2(1, 1)) ;
                        }
                        else
                        {
                            hits[i][j].collider.gameObject.GetComponent<Rigidbody2D>().velocity = (new Vector2(-1, 1));
                        }
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
                        if (facingRight)
                        {
                            hits[i][j].collider.gameObject.GetComponent<Rigidbody2D>().velocity = (new Vector2(1, 1));
                        }
                        else
                        {
                            hits[i][j].collider.gameObject.GetComponent<Rigidbody2D>().velocity = (new Vector2(-1, 1));
                        }
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
        speed = 3f;
        isAttacking = false;
    }

    private void endDashing()
    {
        isDashing = false;
    }

    private void endStriking()
    {
        speed = 3f;
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

    private void endIsGettingHurtAnimation()
    {
         isGettingHurtAnimation = false;
    }

    private void endIsGettingHurtFrame()
    {
        // isGettingHurtFrame = false;
    }

    private void fireballing()
    {
        //List<Object> Objects = new List<Object>();
        GameObject fireBall = Instantiate(fireball, fireballSpawn.transform.position, transform.rotation);
        fireBall.SetActive(true);
    }

    private void resetGame()
    {
        GameObject.Find("Player").GetComponent<PlayerStats>().saveStats();
        PlayerPrefs.SetFloat("Health", GameObject.Find("Player").GetComponent<PlayerStats>().maxHealth);
        PlayerPrefs.SetFloat("Mana", GameObject.Find("Player").GetComponent<PlayerStats>().maxMana);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}