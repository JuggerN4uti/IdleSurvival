using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Perks : MonoBehaviour
{
    [Header("Scripts")]
    public Player PlayerScript;

    [Header("Stats")]
    public int HeroID;
    public int[] perkCost, perksBought;

    [Header("UI")]
    public Button[] LearnButton;
    public TMPro.TextMeshProUGUI[] PerkEffectText;

    public void LearnPerk(int id)
    {
        PlayerScript.SpendSP(perkCost[id]);
        perksBought[id]++;

        switch (id)
        {
            case 0:
                PlayerScript.HeroScript[HeroID].attackDamage[0] += 1.5f;
                PlayerScript.HeroScript[HeroID].attackDamage[1] += 1.7f;
                PerkEffectText[id].text = PlayerScript.HeroScript[HeroID].attackDamage[0].ToString("0.0") + "-" + PlayerScript.HeroScript[HeroID].attackDamage[1].ToString("0.0");
                break;
            case 1:
                PlayerScript.GainHP(25);
                break;
            case 2:
                PlayerScript.goldIncrease += 0.02f;
                PerkEffectText[id].text = "+" + (perksBought[id] * 2).ToString("0") + "%";
                break;
        }

        Check();
    }

    public void Check()
    {
        PerkEffectText[1].text = PlayerScript.maxHealth.ToString("0");

        for (int i = 0; i < perkCost.Length; i++)
        {
            if (PlayerScript.skillPoints >= perkCost[i])
                LearnButton[i].interactable = true;
            else LearnButton[i].interactable = false;
        }
    }
}
