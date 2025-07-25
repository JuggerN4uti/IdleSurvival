using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Perks : MonoBehaviour
{
    [Header("Scripts")]
    public Player PlayerScript;
    public SkillTree[] SkillTreeScript;

    [Header("Stats")]
    public int Class;
    public int PointsSpent;

    [Header("UI")]
    public GameObject ClassSelectScreen;
    public GameObject[] ClassSkillTree;
    public TMPro.TextMeshProUGUI SPCountText;

    [Header("Passives - Warrior")]
    public bool crushingBlow;
    public bool DamageFromRegen;

    public void ChooseClass(int id) // 0 - Warrior
    {
        Class = id;
        switch (id)
        {
            case 0:
                PlayerScript.GainHP(40);
                PlayerScript.damageIncrease += 0.02f;
                break;
        }
        ClassSelectScreen.SetActive(false);
        ClassSkillTree[id].SetActive(true);
    }

    public void LearnPerk(int perkID)
    {
        PlayerScript.SpendSP(1);
        PointsSpent++;

        switch (Class, perkID)
        {
            case (0, 0):
                PlayerScript.attackDamage[0] += 1f;
                PlayerScript.attackDamage[1] += 1.2f;
                break;
            case (0, 1):
                PlayerScript.GainHP(20);
                PlayerScript.GainRegen(0.1f);
                break;
            case (0, 2):
                PlayerScript.attackRate += 0.005f;
                break;
            case (0, 3):
                crushingBlow = true;
                break;
            case (0, 4):
                PlayerScript.GainRegen(0.4f);
                DamageFromRegen = true;
                PlayerScript.attackDamage[0] += PlayerScript.regeneration;
                PlayerScript.attackDamage[1] += PlayerScript.regeneration;
                break;
            case (0, 5):
                PlayerScript.damageIncrease += 0.012f;
                PlayerScript.lifeSteal += 0.012f;
                break;
        }

        Check();
    }

    public void Check()
    {
        SPCountText.text = PlayerScript.skillPoints.ToString("0");

        for (int i = 0; i < SkillTreeScript[Class].perks; i++)
        {
            if (SkillTreeScript[Class].possible[i])
            {
                if (PlayerScript.skillPoints >= 1)
                    SkillTreeScript[Class].LearnButton[i].interactable = true;
                else SkillTreeScript[Class].LearnButton[i].interactable = false;
            }
            else SkillTreeScript[Class].LearnButton[i].interactable = false;
        }
    }

    public void CrushingBlowCooldown()
    {
        crushingBlow = true;
    }
}
