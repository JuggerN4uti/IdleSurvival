using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrades : MonoBehaviour
{
    [Header("Scripts")]
    public Player PlayerScript;

    [Header("Stats")]
    public int totalUpgradesBought;
    public int[] upgradeCost, upgradesBought;

    [Header("UI")]
    public Button[] UpgradeButton;
    public TMPro.TextMeshProUGUI[] UpgradeCostText, UpgradeEffectText;

    public void BuyUpgrade(int id)
    {
        PlayerScript.SpendGold(upgradeCost[id]);
        totalUpgradesBought++;
        upgradesBought[id]++;
        UpgradeEffectText[id].text = upgradesBought[id].ToString("0");

        PlayerScript.GainAttribute(id, 1);

        SetCosts();
    }

    void SetCosts()
    {
        for (int i = 0; i < upgradeCost.Length; i++)
        {
            upgradeCost[i] = 10 + totalUpgradesBought * 4 + upgradesBought[i] * (upgradesBought[i] + 8) / 2;
            UpgradeCostText[i].text = upgradeCost[i].ToString("0");
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
