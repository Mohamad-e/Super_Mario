using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class HealthManager : MonoBehaviour
{
    private Image healthBar;
    // Start is called before the first frame update
    void Start()
    {
        healthBar = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = GameObject.Find("Player").GetComponent<PlayerStats>().currentHealth / GameObject.Find("Player").GetComponent<PlayerStats>().maxHealth;
    }
}
