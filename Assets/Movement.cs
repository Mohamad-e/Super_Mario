using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField]
    private int speed = 1;
    [SerializeField]
    private int height = 3;
    [SerializeField]
    private float GroundDistanceToJump = 3.0f;
    public Animator animator;
    SpriteRenderer m_SpriteRenderer;
    //private bool On_Ground = true;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("Attack", false);
        animator.SetBool("In_Air", false);

        //Debug.Log(Input.GetAxis("Horizontal"));
        float h = Input.GetAxis("Horizontal") * speed;
        animator.SetFloat("Velocity", h);

        this.transform.Translate(h * Time.deltaTime, 0, 0);
        if (h < 0)
        {
            m_SpriteRenderer.flipX = true;
        }
        else if (h > 0)
        {
            m_SpriteRenderer.flipX = false;
        }
        //if (rb.velocity == 0) { 

        if(Input.GetButtonDown("Jump") && Is_on_ground())
        {
            rb.velocity = Vector3.up * height;
            animator.SetBool("In_Air", true);
        }

        // Setting variables for animations, Enable collider for attack
        if (Input.GetButtonDown("Fire1"))
        {
            animator.SetBool("Attack", true);
            Debug.Log(animator.GetBool("Attack"));

            List<RaycastHit2D> hits = new List<RaycastHit2D>();
            if (m_SpriteRenderer.flipX == false)
            {
                Vector3 startArea = this.transform.position + 0.3f * Vector3.up + 0.5f * Vector3.right;
                hits.Add(Physics2D.Raycast(startArea, Vector3.right , 1.0f));
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
                hits.Add(Physics2D.Raycast(startArea, -Vector3.right , 1.2f));
                Debug.DrawRay(startArea, -Vector3.right * 1.2f, Color.white, 10.0f);

                startArea = this.transform.position + (-0.6f) * Vector3.up - 0.5f * Vector3.right;
                hits.Add(Physics2D.Raycast(startArea, -Vector3.right , 1.3f));
                Debug.DrawRay(startArea, -Vector3.right * 1.3f, Color.white, 10.0f);

                startArea = this.transform.position + (-0.9f) * Vector3.up - 0.5f * Vector3.right;
                hits.Add(Physics2D.Raycast(startArea, -Vector3.right, 1.3f));
                Debug.DrawRay(startArea, -Vector3.right * 1.3f, Color.white, 10.0f);
            }


            for (int i = 0; i < 5; ++i)
            {
                if(hits[i].collider == null)
                {
                    Debug.Log("No Hit");
                }else if(hits[i].collider.tag == "Enemy")
                {
                    Debug.Log("Hit enemy");

                }
                else
                {
                    Debug.Log("Hit no enemy");
                }
            }

            hits = new List<RaycastHit2D>();
        } 
    }

    private bool Is_on_ground()
    {
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, -Vector2.up, GroundDistanceToJump);
        
        if (hit.collider == null)
        {
            //Debug.DrawLine(this.transform.position, this.transform.position - (GroundDistanceToJump * Vector3.up), Color.green, 10.0f);
            Debug.DrawRay(this.transform.position, -Vector2.up * GroundDistanceToJump, Color.green, 10.0f);
            return false;
        }
        Debug.Log(hit.collider.tag);
        if (hit.collider.tag == "ground")
        {

            //Debug.Log("Ground");
            Debug.DrawRay(this.transform.position, -Vector2.up * GroundDistanceToJump, Color.red, 10.0f);
            return true;
        }
        else
        {
            //Debug.Log("No_Ground");
            Debug.DrawRay(this.transform.position, -Vector2.up * GroundDistanceToJump, Color.white, 10.0f);
            return false;
        }
    }
}
