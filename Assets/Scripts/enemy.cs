using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    public int health = 1000;

    //public GameObject fireball;
    //public Transform fireballSpawn;

    public int experience;
    // Start is called before the first frame update
    void Start()
    {
        //InvokeRepeating("testProjectiles", 2.0f, 5.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if(health <= 0)
        {
            Destroy(gameObject);
            GameObject.Find("Player").GetComponent<PlayerStats>().currentExperience += experience; 
        }
    }

    /*private void testProjectiles()
    {
        Instantiate(fireball, this.gameObject.transform.position, this.gameObject.transform.rotation).SetActive(true);
        GameObject fireBall = Instantiate(fireball, fireballSpawn.transform.position, transform.rotation);
    }*/
    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if (!collision.gameObject.GetComponent<PlayerMovement>().isGettingHurtFrame)
            {
                collision.gameObject.GetComponent<PlayerStats>().currentHealth -= 10;
                collision.gameObject.GetComponent<PlayerMovement>().isGettingHurt = true;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            
            collision.gameObject.GetComponent<enemy>().health -= 40;
            //collision.gameObject.GetComponent<PlayerMovement>().isGettingHurt = true;
             
        }

        if (collision.gameObject.tag == "Player")
        {
            if (!collision.gameObject.GetComponent<PlayerMovement>().isGettingHurtFrame)
            {
                collision.gameObject.GetComponent<PlayerStats>().currentHealth -= 40;
                collision.gameObject.GetComponent<PlayerMovement>().isGettingHurt = true;
            }
        }
    }
}