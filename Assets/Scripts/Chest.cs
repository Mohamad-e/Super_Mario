using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public Animator animator;
    private bool touched = false;

    public GameObject potion;
    public float forceMultiplier;

    // Update is called once per frame
    void Update()
    {
        //Spawn Items
        if (touched)
        {
            Instantiate(potion, new Vector2(transform.position.x, transform.position.y + .5f), Quaternion.identity).GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-.5f, .5f), 3) * forceMultiplier); 
            touched = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            this.GetComponent<Collider2D>().enabled = false;
            animator.SetTrigger("open");
            //animator.Play("ChestAnimation");
            touched = true;
        }
    }
}
