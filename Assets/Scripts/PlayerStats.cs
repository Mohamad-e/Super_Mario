using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerStats : MonoBehaviour
{
    //current Player health
    public int health = 3;

    //maximum lives a player can have
    public int numberOfLives = 3;

    //images of lives
    public Image[] lives;
    public Sprite fullPotion;
    public Sprite emptyPotion;

    //Player Mana
    public const float maxMana = 100;
    public float mana = maxMana;

    //Player experience points
    public float currentExperience = 0;
    public float experienceToNextLvl = 20;

    //Player Stats
    private int playerStrength = 1;
    private int playerArmor = 1;
    private int playerIntelligence = 1;

    //Player Status
    private bool dizzy = false;
    private void Update()
    {
        healthManager();
        checkLevelUp();
    }

    //manage health
    private void healthManager()
    {
        //check if player would exceed live limit
        if (health > numberOfLives)
            health = numberOfLives;

        //check current lives
        for (int i = 0; i < lives.Length; i++)
        {
            if (i < health)
            {
                lives[i].sprite = fullPotion;
            }
            else
            {
                lives[i].sprite = emptyPotion;
            }

            if (i < numberOfLives)
            {
                lives[i].enabled = true;
            }
            else
            {
                lives[i].enabled = false;
            }
        }
    }

    private void checkLevelUp()
    {
        if(currentExperience >= experienceToNextLvl)
        {
            //reset experience, difference between current experience and experience to next level
            currentExperience = currentExperience - experienceToNextLvl;

            //change experience to next level
            experienceToNextLvl += 20;

            //level up stats
            playerStrength += Random.Range(0, 3);
            playerArmor += Random.Range(0, 3);
            playerIntelligence += Random.Range(0, 3);
        }
    }


}
