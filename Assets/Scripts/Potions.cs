using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potions : MonoBehaviour
{
    public bool healthPotion;
    public bool manaPotion;
    public bool attributePotion;

    PlayerStats playerStats;

    private void Start()
    {
        playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if (healthPotion)
            {
                int health = (int)(playerStats.maxHealth * .3f);
                if (playerStats.currentHealth + health > playerStats.maxHealth)
                    playerStats.currentHealth = playerStats.maxHealth;
                else
                    playerStats.currentHealth += health;
            }
            else if (manaPotion)
            {
                int mana = (int)(playerStats.maxMana * .3f);
                if (playerStats.mana + mana > playerStats.maxMana)
                    playerStats.mana = playerStats.maxMana;
                else
                    playerStats.mana += mana;
            }
            else
                GameObject.Find("Player").GetComponent<PlayerStats>().attributePoints++;
            GameObject.Find("Player").GetComponent<PlayerMovement>().pickupSound.Play();
            Destroy(gameObject);
        }
    }
}
