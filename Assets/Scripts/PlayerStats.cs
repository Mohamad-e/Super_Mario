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

    //Player health
    public float currentHealth = 100;
    public float maxHealth = 100;

    //Player Mana
    public float maxMana = 100;
    public float mana = 100;

    //Player experience points
    public float currentExperience = 0;
    public float experienceToNextLvl = 20;

    //Player Stats
    public int playerLevel = 1;
    public int attributePoints = 0;
    public int specialAttributePoints = 0;
    public int playerStrength = 1;
    public int playerArmor = 1;
    public int playerIntelligence = 1;
    public int bombCount = 0;


    //texts
    public TMP_Text playerLevelText;
    public TMP_Text healthText;
    public TMP_Text manaText;
    public TMP_Text strengthText;
    public TMP_Text armorText;
    public TMP_Text intelligenceText;
    public TMP_Text attributePointsText;
    public TMP_Text specialPointsText;
    public TMP_Text levelUpText;
    public TMP_Text bombCountText;

    //Audio
    public AudioSource confirmPointSound;

    private void Awake()
    {
        if(PlayerPrefs.GetInt("Level") == 0)
            saveStats();
            
    }

    private void Start()
    {
        getStats();
        updateStats();
        statsMenu.enabled = false;
        healthButton.gameObject.SetActive(false);
        manaButton.gameObject.SetActive(false);
        strenghtButton.gameObject.SetActive(false);
        armorButton.gameObject.SetActive(false);
        intelligenceButton.gameObject.SetActive(false);
        levelUpText.enabled = false;

    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            statsMenu.enabled = !statsMenu.enabled;

        checkLevelUp();
        checkAttributes();

    }

    private void checkLevelUp()
    {
        if(currentExperience >= experienceToNextLvl)
        {
            StartCoroutine(showLevelUp());
            GameObject.Find("Player").GetComponent<PlayerMovement>().levelupSound.Play();
            //update player level and reset experience, difference between current experience and experience to next level
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
            saveStats();
            updateStats();
        }
    }

    public void incrStrength()
    {
        confirmPointSound.Play();
        playerStrength++;
        attributePoints--;
        saveStats();
        updateStats();
    }

    public void incrArmor()
    {
        confirmPointSound.Play();
        playerArmor++;
        attributePoints--;
        saveStats();
        updateStats();
    }

    public void incrIntelligence()
    {
        confirmPointSound.Play();
        playerIntelligence++;
        attributePoints--;
        saveStats();
        updateStats();
    }

    public void incrHealth()
    {
        confirmPointSound.Play();
        maxHealth += 10;
        currentHealth = maxHealth;
        specialAttributePoints--;
        saveStats();
        updateStats();
    }

    public void incrMana()
    {
        confirmPointSound.Play();
        maxMana += 10;
        mana = maxMana;
        specialAttributePoints--;
        saveStats();
        updateStats();
    }

    public void updateStats()
    {
        playerLevelText.text = playerLevel.ToString();
        healthText.text = maxHealth.ToString();
        manaText.text = maxMana.ToString();
        strengthText.text = playerStrength.ToString();
        armorText.text = playerArmor.ToString();
        intelligenceText.text = playerIntelligence.ToString();
        attributePointsText.text = attributePoints.ToString();
        specialPointsText.text = specialAttributePoints.ToString();
        bombCountText.text = bombCount.ToString() + "x";
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

    private IEnumerator showLevelUp()
    {
        levelUpText.enabled = true;
        yield return new WaitForSeconds(2f);
        levelUpText.enabled = false;
    }


    public void saveStats()
    {
        PlayerPrefs.SetInt("Level", playerLevel);
        PlayerPrefs.SetFloat("Health", currentHealth);
        PlayerPrefs.SetFloat("Mana", mana);
        PlayerPrefs.SetInt("Strength", playerStrength);
        PlayerPrefs.SetInt("Armor", playerArmor);
        PlayerPrefs.SetInt("Intelligence", playerIntelligence);
        PlayerPrefs.SetInt("AttributePoints", attributePoints);
        PlayerPrefs.SetInt("SpecialAttributePoints", specialAttributePoints);
        PlayerPrefs.SetFloat("MaxHealth", maxHealth);
        PlayerPrefs.SetFloat("MaxMana", maxMana);
        PlayerPrefs.SetFloat("CurrentExp", currentExperience);
        PlayerPrefs.SetFloat("ExpNextLevel", experienceToNextLvl);
    }

    private void getStats()
    {
        playerLevel = PlayerPrefs.GetInt("Level");
        currentHealth = PlayerPrefs.GetFloat("Health");
        mana = PlayerPrefs.GetFloat("Mana");
        playerStrength = PlayerPrefs.GetInt("Strength");
        playerArmor = PlayerPrefs.GetInt("Armor");
        playerIntelligence = PlayerPrefs.GetInt("Intelligence");
        attributePoints = PlayerPrefs.GetInt("AttributePoints");
        specialAttributePoints = PlayerPrefs.GetInt("SpecialAttributePoints");
        maxHealth = PlayerPrefs.GetFloat("MaxHealth");
        maxMana = PlayerPrefs.GetFloat("MaxMana");
        currentExperience = PlayerPrefs.GetFloat("CurrentExp");
        experienceToNextLvl = PlayerPrefs.GetFloat("ExpNextLevel");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Bomb")
        {
            bombCount++;
            updateStats();
            Destroy(collision.gameObject);
        }
    }
}
