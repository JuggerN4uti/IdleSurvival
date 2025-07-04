using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Scripts")]
    public Storage StorageScript;
    public Upgrades UpgradesScript;
    public Perks PerksScript;
    public Combat CombatScript;
    public Hero[] HeroScript;

    [Header("Movement")]
    public Rigidbody2D Body;
    public float movementSpeed;
    public Transform MoveTowards;
    public Vector2 move;

    [Header("Task")]
    public int island;
    public int task; // 0 - combat, 1 - woodcutting
    public GameObject[] TaskScreens;

    [Header("Stats")]
    public int level;
    public int experience, expRequired, gold, skillPoints, totalSkillPoints;
    public int maxHealth, health, regeneration;
    public float minDamageBonus, maxDamageBonus, damageIncrease, speedIncrease, goldIncrease;

    [Header("UI")]
    public Image ExperienceBarFill;
    public Image HealthBarFill;
    public TMPro.TextMeshProUGUI ExperienceText, LevelText, GoldText, HealthText;
    public GameObject DisplayObject;
    public Transform[] Origin;
    private TextPop Displayed;

    [Header("Windowns")]
    public GameObject[] WindowObject;
    public bool[] windowOpened;

    [Header("Bonus")]
    public bool bonusActive;
    public int bonusXP, bonusGold;

    void Start()
    {
        expRequired = CalculateExpReq(level);
        ExperienceBarFill.fillAmount = (experience * 1f) / (expRequired * 1f);
        ExperienceText.text = experience.ToString("0") + "/" + expRequired.ToString("0");
        Invoke("Regen", 0.8f);
        if (bonusActive)
        {
            GainXP(bonusXP);
            GainGold(bonusGold);
        }
    }

    void Update()
    {
        move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (move[0] != 0 || move[1] != 0)
        {
            MoveTowards.position = new Vector3(transform.position.x + move[0] /*+ momentum[0] * 3f*/, transform.position.y + move[1] /*+ momentum[1] * 3f*/, transform.position.z);
            transform.position = Vector2.MoveTowards(transform.position, MoveTowards.position, movementSpeed * Time.deltaTime);
        }
    }

    void Regen()
    {
        //if (CombatScript.)
        RestoreHealth(regeneration);
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
        amount = Mathf.RoundToInt(amount * goldIncrease);
        gold += amount;
        if (amount > 0)
            Display(amount, 1);
        GoldText.text = gold.ToString("0");
        if (windowOpened[0])
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
        minDamageBonus += 0.1f + level * 0.01f;
        maxDamageBonus += 0.2f + level * 0.01f;
        if (level % 4 == 0)
            regeneration++;
        expRequired = CalculateExpReq(level);
    }

    public void GainSP(int amount)
    {
        skillPoints += amount;
        //HeroScript[0].skillPoints += amount;
        totalSkillPoints += amount;
        if (windowOpened[1])
            PerksScript.Check();
    }

    public void SpendSP(int amount)
    {
        skillPoints -= amount;
        //HeroScript[0].skillPoints += amount;
    }

    public void GainHP(int amount)
    {
        maxHealth += amount;
        health += amount;

        HealthBarFill.fillAmount = (health * 1f) / (maxHealth * 1f);
        HealthText.text = health.ToString("0") + "/" + maxHealth.ToString("0");
    }

    public int CalculateExpReq(int level)
    {
        return level * (level + 1) * 30 + level * 40;
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

    public void RestoreHealth(int amount)
    {
        health += amount;
        if (health > maxHealth)
            health = maxHealth;
        if (!CombatScript.bossFight)
        {
            if (health * 4 > maxHealth * 3)
                CombatScript.FightBossButton.interactable = true;
        }

        HealthBarFill.fillAmount = (health * 1f) / (maxHealth * 1f);
        HealthText.text = health.ToString("0") + "/" + maxHealth.ToString("0");
    }

    public void SelectScreen(int which)
    {
        if (windowOpened[which])
        {
            windowOpened[which] = false;
            WindowObject[which].SetActive(false);
        }
        else
        {
            for (int i = 0; i < WindowObject.Length; i++)
            {
                windowOpened[i] = false;
                WindowObject[i].SetActive(false);
            }
            windowOpened[which] = true;
            WindowObject[which].SetActive(true);

            if (which == 0)
                UpgradesScript.Check();
            if (which == 1)
                PerksScript.Check();
            if (which == 2)
                StorageScript.DisplayStorage();
        }
    }

    public void ChangeTask(int what)
    {
        task = what;

        for (int i = 0; i < TaskScreens.Length; i++)
        {
            TaskScreens[i].SetActive(false);
        }
        if (what != 2)
            TaskScreens[task].SetActive(true);

        switch (task)
        {
            case 0:
                break;
            case 1:
                break;
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Tree")
            ChangeTask(1);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        ChangeTask(2);
    }
}
