using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceManager : MonoBehaviour
{
    private Image experienceBar;
    // Start is called before the first frame update
    void Start()
    {
        experienceBar = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        experienceBar.fillAmount = GameObject.Find("Player").GetComponent<PlayerStats>().currentExperience / GameObject.Find("Player").GetComponent<PlayerStats>().experienceToNextLvl;
    }
}
