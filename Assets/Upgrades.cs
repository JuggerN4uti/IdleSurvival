using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrades : MonoBehaviour
{
    [Header("Scripts")]
    public Player PlayerScript;

    [Header("Stats")]
    public int[] upgradeCost, upgradesBought;
    public float[] upgradeCostRise, upgradeCostIncrease;

    [Header("UI")]
    public Button[] UpgradeButton;
    public TMPro.TextMeshProUGUI[] UpgradeCostText, UpgradeEffectText;

    public void BuyUpgrade(int id)
    {
        PlayerScript.SpendGold(upgradeCost[id]);
        upgradeCost[id] = Mathf.RoundToInt(upgradeCost[id] * upgradeCostRise[id] + upgradeCostIncrease[id]);
        UpgradeCostText[id].text = upgradeCost[id].ToString("0");
        upgradesBought[id]++;

        switch (id)
        {
            case 0:
                //PlayerScript.damageIncrease += 0.01f;
                PlayerScript.minDamageBonus += 0.9f;
                PlayerScript.maxDamageBonus += 1.4f;
                UpgradeEffectText[id].text = "+" + PlayerScript.minDamageBonus.ToString("0.0") + "-" + PlayerScript.maxDamageBonus.ToString("0.0");
                break;
            case 1:
                PlayerScript.speedIncrease += 0.02f;
                UpgradeEffectText[id].text = "+" + (upgradesBought[id] * 2).ToString("0") + "%";
                break;
            case 2:
                PlayerScript.GainSP(1);
                UpgradeEffectText[id].text = "+" + upgradesBought[id].ToString("0");
                break;
        }

        Check();
    }

    public void Check()
    {
        for (int i = 0; i < upgradeCost.Length; i++)
        {
            if (PlayerScript.gold >= upgradeCost[i])
                UpgradeButton[i].interactable = true;
            else UpgradeButton[i].interactable = false;
        }
    }
}
