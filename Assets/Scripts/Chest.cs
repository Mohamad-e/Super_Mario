using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public Animator animator;
    private bool touched = false;

    public GameObject[] items;
    public float forceMultiplier;

    public GameObject bomb;
    public bool bombChest;
    public bool respawnBomb;
    
    // Update is called once per frame
    void Update()
    {
        //Spawn Items
        if (touched)
        {
            foreach(GameObject item in items)
            {
                Instantiate(item, new Vector2(transform.position.x, transform.position.y + .5f), Quaternion.identity).GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-.5f, .5f), 3) * forceMultiplier);
            }
            Instantiate(bomb, new Vector2(transform.position.x, transform.position.y + .5f), Quaternion.identity).GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-.5f, .5f), 3) * forceMultiplier);
            touched = false;
        }
        if(GameObject.Find("Player").GetComponent<PlayerStats>().bombCount == 0 & respawnBomb)
        {
            Instantiate(bomb, new Vector2(transform.position.x, transform.position.y + .5f), Quaternion.identity).GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-.5f, .5f), 3) * forceMultiplier);
            respawnBomb = false;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            this.GetComponent<Collider2D>().enabled = false;
            animator.SetTrigger("open");
            touched = true;
        }
    }
}
