using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeMovement : MonoBehaviour
{
    public Transform targetPlayer;
    public float speed;
    public float distance;
    private bool direction;

    public Animator anim;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 displacement = (targetPlayer.position - transform.position).normalized;
        if(Vector3.Distance(targetPlayer.position, transform.position) <= distance)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                new Vector2(targetPlayer.position.x, transform.position.y),
                speed * Time.deltaTime);

            anim.SetBool("moveToPlayer", true);

        }
        else
        {
            anim.SetBool("moveToPlayer", false);
        }
            


        if (transform.position.x < targetPlayer.position.x && !direction)
        {
            direction = !direction;
            transform.Rotate(0f, 180f, 0f);
        }
        else if(transform.position.x > targetPlayer.position.x && direction)
        {
            direction = !direction;
            transform.Rotate(0f, 180f, 0f);
        }
            
    }
}
