using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField]
    private float timeAlive = 5;
    private float timer = 0;
    private Rigidbody2D rb;
    public bool explosion = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
    private void OnCollisionStay2D(Collision2D collision)
    {
        //Debug.Log("explosion");
        if (explosion == true)
        {
            
            if (collision.collider.tag == "weakWall")
            {
                collision.collider.GetComponent<weakWall>().explosion = true;
            }
        }
    }

    private void DestoryBomb()
    {
        Destroy(gameObject);
    }

    private void activateExplosion()
    {
        explosion = true;
    }
}