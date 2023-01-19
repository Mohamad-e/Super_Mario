using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weakWall : MonoBehaviour
{
    public bool explosion = false;
    public bool otherWallExplosion = true;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (explosion)
        {
            // setAnimator Explosion 
            animator.SetTrigger("Explosion");
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (otherWallExplosion == true)
        {
            Debug.Log(collision.collider.name);
            if (collision.collider.tag == "weakWall")
            {

                Debug.Log(explosion); ;
                collision.collider.GetComponent<weakWall>().explosion = true;
            }
        }
        
        /*if (collision.collider.GetComponent<Bomb>().explosion == true)
        {
            explosion = true;

        }*/
    }

    private void destroyWall()
    {
        Destroy(gameObject);
    }

    private void activateOtherWallExplosion()
    {
        otherWallExplosion = true;
    }
}
