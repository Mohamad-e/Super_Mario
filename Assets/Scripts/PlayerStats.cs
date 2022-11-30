using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    //canvas and buttons to show/hide
    public Canvas statsMenu;
    public Button healthButton;
    public Button manaButton;
    public Button strenghtButton;
    public Button armorButton;
    public Button intelligenceButton;

    //current Player health
    public int health = 3;

    //maximum lives a player can have
    public int numberOfLives = 3;

    //images of lives
    public Image[] lives;
    public Sprite fullPotion;
    public Sprite emptyPotion;

    //Player Mana
    public float maxMana = 100;
    public float mana = 100;

    //Player experience points
    public float currentExperience = 0;
    public float experienceToNextLvl = 20;

    //Player Stats
    private int playerLevel = 1;
    private int attributePoints = 0;
    private int specialAttributePoints = 0;
    private int playerStrength = 1;
    private int playerArmor = 1;
    private int playerIntelligence = 1;

    //texts
    public TMP_Text playerLevelText;
    public TMP_Text healthText;
    public TMP_Text manaText;
    public TMP_Text strengthText;
    public TMP_Text armorText;
    public TMP_Text intelligenceText;
    public TMP_Text attributePointsText;
    public TMP_Text specialPointsText;

    private void Start()
    {
        updateStats();
        statsMenu.enabled = false;
        healthButton.gameObject.SetActive(false);
        manaButton.gameObject.SetActive(false);
        strenghtButton.gameObject.SetActive(false);
        armorButton.gameObject.SetActive(false);
        intelligenceButton.gameObject.SetActive(false);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            statsMenu.enabled = !statsMenu.enabled;

        healthManager();
        checkLevelUp();
        checkAttributes();

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
            //updare player level and reset experience, difference between current experience and experience to next level
            playerLevel++;
            currentExperience = currentExperience - experienceToNextLvl;

            //change experience to next level
            experienceToNextLvl += 20;

            //give attribute point and check for health/mana point
            attributePoints++;

            if(playerLevel % 5 == 0)
            {
                specialAttributePoints++;
            }
            
            updateStats();
        }
    }

    public void incrStrength()
    {
        playerStrength++;
        attributePoints--;
        updateStats();
    }

    public void incrArmor()
    {
        playerArmor++;
        attributePoints--;
        updateStats();
    }

    public void incrIntelligence()
    {
        playerIntelligence++;
        attributePoints--;
        updateStats();
    }

    public void incrHealth()
    {
        numberOfLives++;
        specialAttributePoints--;
        updateStats();
    }

    public void incrMana()
    {
        maxMana += 10;
        specialAttributePoints--;
        updateStats();
    }

    private void updateStats()
    {
        playerLevelText.text = playerLevel.ToString();
        healthText.text = numberOfLives.ToString();
        manaText.text = maxMana.ToString();
        strengthText.text = playerStrength.ToString();
        armorText.text = playerArmor.ToString();
        intelligenceText.text = playerIntelligence.ToString();
        attributePointsText.text = attributePoints.ToString();
        specialPointsText.text = specialAttributePoints.ToString();
    }

    private void checkAttributes()
    {
        if(attributePoints > 0)
        {
            strenghtButton.gameObject.SetActive(true);
            armorButton.gameObject.SetActive(true);
            intelligenceButton.gameObject.SetActive(true);
        } 
        else if (attributePoints == 0)
        {
            strenghtButton.gameObject.SetActive(false);
            armorButton.gameObject.SetActive(false);
            intelligenceButton.gameObject.SetActive(false);
        }

        if(specialAttributePoints > 0)
        {
            healthButton.gameObject.SetActive(true);
            manaButton.gameObject.SetActive(true);
        }
        else if (specialAttributePoints == 0)
        {
            healthButton.gameObject.SetActive(false);
            manaButton.gameObject.SetActive(false);
        }
    }
}
