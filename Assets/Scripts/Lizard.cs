using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lizard : MonoBehaviour
{
    public Transform targetPlayer;
    public Transform fireballSpawn;
    public GameObject fireball;

    public float distance;
    private bool direction;
    public float cooldownMax = 3f;
    public float cooldown = 3f;
    public Animator anim;

    private void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        //InvokeRepeating
        if (Vector3.Distance(targetPlayer.position, transform.position) <= distance)
        { 

            if (cooldown <= 0)
            {
                

                anim.SetTrigger("shooting");

                cooldown = cooldownMax;
            }
        }
        cooldown -= Time.deltaTime;
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

    private void shootFireball()
    {
        Instantiate(fireball, fireballSpawn.position, Quaternion.identity).SetActive(true);
    }

}