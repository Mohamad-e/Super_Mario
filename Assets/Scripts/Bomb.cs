using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField]
    private float timeAlive = 5;
    private float timer = 0;
    private Rigidbody2D rb;

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
}
