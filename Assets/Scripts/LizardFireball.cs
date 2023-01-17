using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LizardFireball : MonoBehaviour
{
    public GameObject targetPlayer;
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private int damage = 30;
    [SerializeField]
    private float timeAlive = 5;
    private float timer = 0;
    private Rigidbody2D rb;

    private Vector2 rotationFromEnemyToPlayer;
    private void Start()
    {
        
        rb = GetComponent<Rigidbody2D>();
        rotationFromEnemyToPlayer = targetPlayer.transform.position - transform.position;
        //rotationFromEnemyToPlayer.x = Mathf.Abs(rotationFromEnemyToPlayer.x);
        //rotationFromEnemyToPlayer.y = Mathf.Abs(rotationFromEnemyToPlayer.y);

        //transform.rotation = new Quaternion(rotationFromEnemyToPlayer.x, rotationFromEnemyToPlayer.y, 1, 1);
        //Vector3 movement = transform.rotation * Vector3.forward; ;
        //rb.velocity = movement * 5;

        float tmpNormalisieren = Mathf.Abs(Mathf.Sqrt(rotationFromEnemyToPlayer.x * rotationFromEnemyToPlayer.x + rotationFromEnemyToPlayer.y * rotationFromEnemyToPlayer.y));
        rotationFromEnemyToPlayer.x = rotationFromEnemyToPlayer.x / tmpNormalisieren;
        rotationFromEnemyToPlayer.y = rotationFromEnemyToPlayer.y / tmpNormalisieren;
        rotationFromEnemyToPlayer *= 5;
        rb.velocity = rotationFromEnemyToPlayer;

    }

    // Update is called once per frame
    void Update()
    {

        if (timer < timeAlive)
        {
            timer += Time.deltaTime;
        }
        else if (timer >= timeAlive)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!collision.gameObject.GetComponent<PlayerMovement>().isGettingHurtFrame)
            {
                //  && ((collision.collider.transform.position.x > transform.position.x && collision.collider.transform.rotation.y == -180) || (collision.collider.transform.position.x < transform.position.x && collision.collider.transform.rotation.y == 0))
                if (collision.collider.GetComponent<PlayerMovement>().parryFrameAcive)
                {
                    Debug.Log("Parried");
                }
                else if (collision.collider.GetComponent<PlayerMovement>().isBlocking)
                {
                    Debug.Log("Blocked");
                    collision.collider.GetComponent<PlayerStats>().currentHealth -= damage / 2;
                }
                else
                {
                    Debug.Log("neither Parried nor Blocked");
                    collision.collider.GetComponent<PlayerStats>().currentHealth -= damage;
                    collision.gameObject.GetComponent<PlayerMovement>().isGettingHurt = true;
                }
            }
        }
        Destroy(gameObject);
    }
}