using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LizardFireball : MonoBehaviour
{
    public Transform targetPlayer;
    public Transform enemyPosition;
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private int damage = 30;

    private Vector2 fromEnemyToPlayer;
    private void Start()
    {
        fromEnemyToPlayer = targetPlayer.position;
    }

    // Update is called once per frame
    void Update()
    {

        transform.position = Vector2.MoveTowards(transform.position, fromEnemyToPlayer, speed * Time.deltaTime);
        if (transform.position.Equals(fromEnemyToPlayer))
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //  && ((collision.collider.transform.position.x > transform.position.x && collision.collider.transform.rotation.y == -180) || (collision.collider.transform.position.x < transform.position.x && collision.collider.transform.rotation.y == 0))
            if (collision.collider.GetComponent<PlayerMovement>().parryFrameAcive  )
            {
                Debug.Log("Parried");
            }
            else if (collision.collider.GetComponent<PlayerMovement>().isBlocking)
            {
                Debug.Log("Blocked");
                collision.collider.GetComponent<PlayerStats>().currentHealth -= damage/2;
            }
            else
            {
                Debug.Log("neither Parried nor Blocked");
                collision.collider.GetComponent<PlayerStats>().currentHealth -= damage;
                collision.gameObject.GetComponent<PlayerMovement>().isGettingHurt = true;
            }

        }
        Destroy(gameObject);
    }
}
