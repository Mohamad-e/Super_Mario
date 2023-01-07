using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawMovement : MonoBehaviour
{
    public float startPosition;
    public float speed;
    public int direction;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float y = direction * Mathf.PingPong(Time.time * speed, 3) + startPosition;
        transform.position = new Vector3(transform.position.x, y, 0);

        transform.Rotate(new Vector3(0, 0, 20) * Time.deltaTime *2.5f);
    }
}
