using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField]
    private int speed = 1;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
  
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Input.GetAxis("Horizontal"));
        float h = Input.GetAxis("Horizontal") * speed;
        this.transform.Translate(h * Time.deltaTime, 0, 0);
        //if (rb.velocity == 0) { }
    }
}
