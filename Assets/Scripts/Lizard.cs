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
    private float cooldown = 5f;
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
            
            anim.SetTrigger("shooting");
        }
        cooldown -= Time.deltaTime;
        if(cooldown <= 0)
        {
            Instantiate(fireball, fireballSpawn.position, Quaternion.identity);
            cooldown = 4f;
        }
            
    }
}
