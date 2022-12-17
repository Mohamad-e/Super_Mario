using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatMovement : MonoBehaviour
{
    public Transform targetPlayer;
    public float speed;
    public float distance;
    private bool direction;

    public Animator anim;


    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(targetPlayer.position, transform.position) <= distance)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                new Vector2(targetPlayer.position.x, targetPlayer.position.y - .5f),
                speed * Time.deltaTime);

            anim.SetBool("moveToPlayer", true);

        }



        if (transform.position.x < targetPlayer.position.x && !direction)
        {
            direction = !direction;
            transform.Rotate(0f, 180f, 0f);
        }
        else if (transform.position.x > targetPlayer.position.x && direction)
        {
            direction = !direction;
            transform.Rotate(0f, 180f, 0f);
        }

    }
}
