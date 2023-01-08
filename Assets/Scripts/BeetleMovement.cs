using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeetleMovement : MonoBehaviour
{
    public float movingSpeed = 2;
    public float fMinX;
    public float fMaxX;
    private int direction = -1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(direction == -1)
        {
            if (transform.localPosition.x > fMinX)
            {
                //flip();
                GetComponent<Rigidbody2D>().velocity = new Vector2(-movingSpeed, GetComponent<Rigidbody2D>().velocity.y);
            }
            else
            {
                Debug.Log("direction -1");
                direction = 1;
                flip();
            }
        }
        else if(direction == 1)
        {
            if (transform.localPosition.x < fMaxX)
            {
                //flip();
                GetComponent<Rigidbody2D>().velocity = new Vector2(movingSpeed, GetComponent<Rigidbody2D>().velocity.y);
            }
            else
            {
                Debug.Log("direction 1");
                direction = -1;
                flip();
            }
        }
        
    }

    private void flip()
    {
            transform.Rotate(0f, 180f, 0f);
    }
}
