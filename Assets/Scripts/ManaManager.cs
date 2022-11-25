using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ManaManager : MonoBehaviour
{
    private Image manaBar;
    // Start is called before the first frame update
    void Start()
    {
        manaBar = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        manaBar.fillAmount = GameObject.Find("Player").GetComponent<PlayerStats>().mana / PlayerStats.maxMana;
    }
}
