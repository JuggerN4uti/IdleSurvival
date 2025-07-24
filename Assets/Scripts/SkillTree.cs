using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillTree : MonoBehaviour
{
    [Header("Scripts")]
    public Player PlayerScript;
    public Perks PerksScript;

    [Header("UI")]
    public int perks;
    public int[] perksBought, maxPerks;
    public bool[] possible;
    public Button[] LearnButton;
    public TMPro.TextMeshProUGUI[] PerkBoughtText;

    [Header("Info")]
    public TMPro.TextMeshProUGUI PerkTooltip;
    public string[] perkEffectString;

    public void LearnPerk(int perkID)
    {
        perksBought[perkID]++;
        if (perksBought[perkID] == maxPerks[perkID])
        {
            possible[perkID] = false;
            if (perkID < 3)
                possible[perkID + 3] = true;
        }
        PerkBoughtText[perkID].text = perksBought[perkID].ToString("0") + "/" + maxPerks[perkID].ToString("0");
        PerksScript.LearnPerk(perkID);
    }

    public void PerkHovered(int perkID)
    {
        PerkTooltip.text = perkEffectString[perkID];
    }

    public void Unhovered()
    {
        PerkTooltip.text = "";
    }
}
