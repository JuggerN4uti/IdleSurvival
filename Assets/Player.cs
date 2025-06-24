using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Scripts")]
    public Upgrades UpgradesScript;
    public Combat CombatScript;

    [Header("Stats")]
    public int level;
    public int experience, expRequired, gold, skillPoints, totalSkillPoints;
    public int maxHealth, health;
    public float baseDamageBonus, damageIncrease, speedIncrease;
    public bool upgradeScreenOpen;

    [Header("UI")]
    public Image ExperienceBarFill;
    public Image HealthBarFill;
    public TMPro.TextMeshProUGUI ExperienceText, LevelText, GoldText, HealthText;
    public GameObject DisplayObject;
    public Transform[] Origin;
    private TextPop Displayed;

    [Header("Bonus")]
    public bool bonusActive;
    public int bonusXP, bonusGold;

    void Start()
    {
        CalculateExpReq();
        ExperienceBarFill.fillAmount = (experience * 1f) / (expRequired * 1f);
        ExperienceText.text = experience.ToString("0") + "/" + expRequired.ToString("0");
        Invoke("Regen", 0.8f);
        if (bonusActive)
        {
            GainXP(bonusXP);
            GainGold(bonusGold);
        }
    }

    void Regen()
    {
        //if (CombatScript.)
        RestoreHealth(1);
        Invoke("Regen", 0.8f);
    }

    public void GainXP(int xp)
    {
        experience += xp;
        Display(xp, 0);
        if (experience >= expRequired)
            LevelUp();
        ExperienceBarFill.fillAmount = (experience * 1f) / (expRequired * 1f);
        ExperienceText.text = experience.ToString("0") + "/" + expRequired.ToString("0");
    }

    public void GainGold(int amount)
    {
        gold += amount;
        if (amount > 0)
            Display(amount, 1);
        GoldText.text = gold.ToString("0");
        if (upgradeScreenOpen)
            UpgradesScript.Check();
    }

    public void SpendGold(int amount)
    {
        gold -= amount;
        GoldText.text = gold.ToString("0");
    }

    void Display(int amount, int type)
    {
        //Origin[type].rotation = Quaternion.Euler(Origin[type].rotation.x, Origin[type].rotation.y, Origin[type].rotation.z);
        GameObject display = Instantiate(DisplayObject, Origin[type].position, transform.rotation);
        Displayed = display.GetComponent(typeof(TextPop)) as TextPop;
        Displayed.SetText(amount, type);
        Rigidbody2D display_body = display.GetComponent<Rigidbody2D>();
        display_body.AddForce(Origin[type].up * 0.44f, ForceMode2D.Impulse);
    }

    void LevelUp()
    {
        experience -= expRequired;
        level++;
        LevelText.text = level.ToString("0");
        GainSP(1);
        GainHP(10);
        CalculateExpReq();
    }

    public void GainSP(int amount)
    {
        skillPoints += amount;
        totalSkillPoints += amount;
    }

    void GainHP(int amount)
    {
        maxHealth += amount;
        health += amount;

        HealthBarFill.fillAmount = (health * 1f) / (maxHealth * 1f);
        HealthText.text = health.ToString("0") + "/" + maxHealth.ToString("0");
    }

    void CalculateExpReq()
    {
        expRequired = level * (level + 1) * 40 + level * 20;
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            CombatScript.Fallen();
            health = 1;
        }
        HealthBarFill.fillAmount = (health * 1f) / (maxHealth * 1f);
        HealthText.text = health.ToString("0") + "/" + maxHealth.ToString("0");
    }

    void RestoreHealth(int amount)
    {
        health += amount;
        if (health > maxHealth)
            health = maxHealth;
        if (!CombatScript.bossFight)
        {
            if (health * 10 > maxHealth * 7)
                CombatScript.FightBossButton.interactable = true;
        }

        HealthBarFill.fillAmount = (health * 1f) / (maxHealth * 1f);
        HealthText.text = health.ToString("0") + "/" + maxHealth.ToString("0");
    }
}
