using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public Animator animator;
    private bool touched = false;

    public GameObject[] potions;
    private int potionPos;
    public float forceMultiplier;

    // Update is called once per frame
    void Update()
    {
        //Spawn Items
        if (touched)
        {
            float choosePotion = Random.Range(0f, 1f);
            Debug.Log(choosePotion);
            if (choosePotion <= 0.1f)
                potionPos = 0;  //Attribute point
            else if (choosePotion >= 0.1f && choosePotion <= .55f)
                potionPos = 1;  //health potion
            else
                potionPos = 2;  //mana potion
            Instantiate(potions[potionPos], transform.position, Quaternion.identity).GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-.5f, .5f), 3) * forceMultiplier); 

            touched = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            this.GetComponent<Collider2D>().enabled = false;
            animator.Play("ChestAnimation");
            touched = true;
        }
    }
}
