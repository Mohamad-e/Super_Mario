using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    public int health = 1000;

    public GameObject fireball;
    public Transform fireballSpawn;

    public int experience;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("testProjectiles", 2.0f, 5.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if(health <= 0)
        {
            Destroy(gameObject);
            GameObject.Find("Player").GetComponent<PlayerStats>().currentExperience += experience; 
        }
    }

    private void testProjectiles()
    {
        Instantiate(fireball, this.gameObject.transform.position, this.gameObject.transform.rotation).SetActive(true) ;
        GameObject fireBall = Instantiate(fireball, fireballSpawn.transform.position, transform.rotation);
    }
}
