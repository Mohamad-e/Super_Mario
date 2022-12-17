using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;
    private Vector3 mVelocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(player.position.x + offset.x, Mathf.Clamp(player.position.y + offset.y, -0.5f, 5) , offset.z), ref mVelocity, .5f); // Camera follows the player with specified offset position
        
    }
}
