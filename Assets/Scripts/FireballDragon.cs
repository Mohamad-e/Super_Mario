using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballDragon : MonoBehaviour
{
    private float speed = 20;
    [SerializeField]
    private int fireballDamage = 1;
    // Update is called once per frame
    void Update()
    {
        transform.position += transform.right * Time.deltaTime * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.collider.GetComponent<PlayerMovement>().parryFrameAcive)
        {
            Debug.Log("Parried");
        }
        else if (collision.collider.GetComponent<PlayerMovement>().isBlocking)
        {
            Debug.Log("Blocked");
            collision.collider.GetComponent<PlayerStats>().health -= fireballDamage;
        }
        else
        {
            Debug.Log("neither Parried nor Blocked");
            collision.collider.GetComponent<PlayerStats>().health -= fireballDamage;
        }
        Destroy(gameObject);
    }
}
