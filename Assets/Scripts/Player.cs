using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Scripts")]
    public Storage StorageScript;
    public Smelting SmeltingScript;
    public Milestones MilestonesScript;
    public Upgrades UpgradesScript;
    public Perks PerksScript;
    public Combat CombatScript;
    public Hero[] HeroScript;
    public WorldMap WorldMapScript;

    [Header("Movement")]
    public Rigidbody2D Body;
    public float movementSpeed;
    public Transform MoveTowards;
    public Vector2 move;
    public Vector3 mousePos, movePos;
    bool moving;

    [Header("Combat")]
    public Mobile MobileTargeted;
    public bool fighting;
    public float attackRange;
    public float[] attackDamage;
    public float attackRate, attackCharge, dodgeChance, critChance, critDamage, lifeSteal;
    int taken;
    float damage;
    bool crited;

    [Header("Task")]
    public Resource ResourceTargeted;
    public bool collecting, automation;
    public int island;
    public int task, auto; // 0 - woodcutting,
    public float taskProgress, inCombat;
    public float[] collectingSpeed;
    public GameObject[] TaskScreens;
    public GameObject FinderPrefab;
    private Finder FinderScript;

    [Header("Stats")]
    public int gold;
    public int skillPoints, totalSkillPoints;
    public int maxHealth, health;
    public float regeneration, damageIncrease, speedIncrease, goldIncrease;
    int tempi;
    float temp;

    [Header("Attributes")]
    public int vitality;
    public int strength, dexterity, resilience, luck; 

    [Header("Levels")]
    public int level;
    public int experience, expRequired;
    public int[] taskLevel, taskExperience, taskExpRequired;

    [Header("Equipment Stats")]
    public float weaponDamage;
    public float weaponRate, weaponCrit, weaponRange;
    public int armor;

    [Header("UI")]
    public Image ExperienceBarFill;
    public Image HealthBarFill;
    public Image[] TaskExperienceBarFill, AutomationImage;
    public TMPro.TextMeshProUGUI ExperienceText, LevelText, GoldText, HealthText;
    public TMPro.TextMeshProUGUI[] TaskExperienceText, TaskLevelText;
    public GameObject DisplayObject;
    public Transform[] Origin;
    private TextPop Displayed;

    [Header("Windowns")]
    public GameObject WorldScreenObject;
    public GameObject[] WindowObject;
    public bool[] windowOpened;

    [Header("Warrior")]
    public GameObject RageBarObject;
    public Image RageBarFill;
    public float rage, maxRage;

    [Header("Bonus")]
    public bool bonusActive;
    public int bonusXP, bonusGold;

    void Start()
    {
        expRequired = CalculateExpReq(level);
        ExperienceBarFill.fillAmount = (experience * 1f) / (expRequired * 1f);
        ExperienceText.text = experience.ToString("0") + "/" + expRequired.ToString("0");
        for (int i = 0; i < taskLevel.Length; i++)
        {
            taskExpRequired[i] = CalculateExpReq(taskLevel[i]);
            TaskExperienceBarFill[i].fillAmount = (taskExperience[i] * 1f) / (taskExpRequired[i] * 1f);
            TaskExperienceText[i].text = taskExperience[i].ToString("0") + "/" + taskExpRequired[i].ToString("0");
        }
        Invoke("Regen", 1f);
        if (bonusActive)
        {
            GainXP(bonusXP / 5);
            GainGold(bonusGold / 5);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            //MoveTowards = Input.mousePosition;
            mousePos = Input.mousePosition;
            movePos = Camera.main.ScreenToWorldPoint(mousePos);
            ChangeTask();
            moving = true;
            //MoveTowards = mousePos;
        }
        if (fighting)
        {
            if (Vector3.Distance(transform.position, MobileTargeted.transform.position) > AttackRange())
                transform.position = Vector2.MoveTowards(transform.position, MobileTargeted.transform.position, movementSpeed * Time.deltaTime);
            else
            {
                attackCharge += attackRate * Time.deltaTime * speedIncrease * weaponRate;
                if (attackCharge >= 1f)
                    Attack();
                //AttackBarFill.fillAmount = attackCharge / 1f;
            }
        }
        else if (collecting)
        {
            if (Vector3.Distance(transform.position, ResourceTargeted.transform.position) > 0.3f)
                transform.position = Vector2.MoveTowards(transform.position, ResourceTargeted.transform.position, movementSpeed * Time.deltaTime);
            else
            {
                taskProgress += Time.deltaTime * speedIncrease * collectingSpeed[task];
                if (taskProgress >= 1f)
                    DoTask();
                //AttackBarFill.fillAmount = attackCharge / 1f;
            }
        }
        else if (moving)
        {
            if (Vector3.Distance(transform.position, mousePos) > 0.003f)
                transform.position = Vector2.MoveTowards(transform.position, movePos, movementSpeed * Time.deltaTime);
            else moving = false;
        }

        if (inCombat > 0f)
        {
            inCombat -= Time.deltaTime;
        }
        else
        {
            if (PerksScript.rage && rage > 0f)
            {
                rage -= 5 * Time.deltaTime;
                if (rage < 0f)
                    rage = 0f;
                RageBarFill.fillAmount = rage / 100f;
            }
        }
        /*move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (move[0] != 0 || move[1] != 0)
        {
            MoveTowards.position = new Vector3(transform.position.x + move[0], transform.position.y + move[1], transform.position.z);
            transform.position = Vector2.MoveTowards(transform.position, MoveTowards.position, movementSpeed * Time.deltaTime);
        }*/
    }

    void Attack()
    {
        inCombat = 8f;
        attackCharge -= 1f;
        moving = false;
        movePos = transform.position;

        damage = Random.Range(attackDamage[0], attackDamage[1]);
        damage *= damageIncrease;
        damage *= weaponDamage;
        damage *= 1f + rage * 0.001f;
        if (PerksScript.crushingBlow)
        {
            damage *= 1.42f;
            PerksScript.crushingBlow = false;
            PerksScript.Invoke("CrushingBlowCooldown", 8f);
        }
        if (CritChance() >= Random.Range(0f, 1f + CritChance()))
        {
            damage *= critDamage;
            crited = true;
        }
        else crited = false;

        MobileTargeted.DamageMob(damage, crited);
        tempi = Mathf.RoundToInt(damage * lifeSteal);
        if (tempi > 0)
            RestoreHealth(tempi);

        if (PerksScript.rage)
        {
            GainRage(6f / weaponRate);
        }
    }

    void DoTask()
    {
        taskProgress -= 1f;
        ResourceTargeted.Collect();
    }

    void Regen()
    {
        //if (CombatScript.)
        temp = 1f / regeneration;
        if (temp > 0.4f)
        {
            Invoke("Regen", temp);
            RestoreHealth(1);
        }
        else
        {
            temp *= 2;
            Invoke("Regen", temp);
            RestoreHealth(2);
        }
        //Invoke("Regen", 0.8f);
    }

    public void GainXP(int xp)
    {
        experience += xp;
        if (xp > 0)
            Display(xp, 0);
        if (experience >= expRequired)
            LevelUp();
        ExperienceBarFill.fillAmount = (experience * 1f) / (expRequired * 1f);
        ExperienceText.text = experience.ToString("0") + "/" + expRequired.ToString("0");
    }

    public void GainTaskXP(int xp, int which)
    {
        GainXP(xp * taskLevel[which]);
        taskExperience[which] += xp;
        /*if (xp > 0)
            Display(xp, 0);*/
        if (taskExperience[which] >= taskExpRequired[which])
            LevelUpTask(which);
        TaskExperienceBarFill[which].fillAmount = (taskExperience[which] * 1f) / (taskExpRequired[which] * 1f);
        TaskExperienceText[which].text = taskExperience[which].ToString("0") + "/" + taskExpRequired[which].ToString("0");
    }

    public void GainGold(int amount)
    {
        amount = Mathf.RoundToInt(amount * goldIncrease);
        gold += amount;
        if (amount > 0)
            Display(amount, 1);
        MilestonesScript.ProgressMilestone(0, amount);
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
        GainRegen(0.1f);
        expRequired = CalculateExpReq(level);
        GainXP(0);
    }

    void LevelUpTask(int which)
    {
        taskExperience[which] -= taskExpRequired[which];
        taskLevel[which]++;
        TaskLevelText[which].text = taskLevel[which].ToString("0");
        collectingSpeed[which] += 0.03f;
        // switch task bla bla
        taskExpRequired[which] = CalculateExpReq(taskLevel[which]);
    }

    public void GainAttribute(int id, int amount)
    {
        switch (id)
        {
            case 0:
                vitality += amount;
                GainHP(20 * amount);
                GainRegen(0.1f * amount);
                break;
            case 1:
                strength += amount;
                attackDamage[0] += amount;
                attackDamage[1] += amount;
                break;
            case 2:
                dexterity += amount;
                attackRate += 0.005f * amount;
                dodgeChance += 0.01f * amount;
                break;
            case 3:
                resilience += amount;
                armor += 2 * amount;
                break;
            case 4:
                luck += amount;
                critChance += 0.01f * amount;
                critDamage += 0.01f * amount;
                break;
        }
    }

    public void LoseAttribute(int id, int amount)
    {
        switch (id)
        {
            case 0:
                vitality -= amount;
                LoseHP(20 * amount);
                LoseRegen(0.1f * amount);
                break;
            case 1:
                strength -= amount;
                attackDamage[0] -= amount;
                attackDamage[1] -= amount;
                break;
            case 2:
                dexterity -= amount;
                attackRate -= 0.005f * amount;
                dodgeChance -= 0.01f * amount;
                break;
            case 3:
                resilience -= amount;
                armor -= 2 * amount;
                break;
            case 4:
                luck -= amount;
                critChance -= 0.01f * amount;
                critDamage -= 0.01f * amount;
                break;
        }
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

        HealthBarFill.fillAmount = HealthPercent();
        HealthText.text = health.ToString("0") + "/" + maxHealth.ToString("0");
    }

    public void LoseHP(int amount)
    {
        maxHealth -= amount;
        health -= amount;

        if (health <= 0)
            health = 1;

        HealthBarFill.fillAmount = HealthPercent();
        HealthText.text = health.ToString("0") + "/" + maxHealth.ToString("0");
    }

    public void GainRegen(float amount)
    {
        regeneration += amount;
        if (PerksScript.DamageFromRegen)
        {
            attackDamage[0] += amount;
            attackDamage[1] += amount;
        }
    }

    public void LoseRegen(float amount)
    {
        regeneration -= amount;
        if (PerksScript.DamageFromRegen)
        {
            attackDamage[0] -= amount;
            attackDamage[1] -= amount;
        }
    }

    public int CalculateExpReq(int level)
    {
        return level * (level + 1) * 20 + level * 60;
    }

    public bool Dodge()
    {
        if (dodgeChance >= Random.Range(0f, 1f + dodgeChance))
            return true; // potem efekt dodga
        else return false;
    }

    public void TakeDamage(int amount)
    {
        inCombat = 8f;
        taken = Mathf.RoundToInt(amount / (1f + armor * 0.01f));
        health -= taken;
        if (health <= 0)
        {
            health = 1;
        }
        HealthBarFill.fillAmount = HealthPercent();
        HealthText.text = health.ToString("0") + "/" + maxHealth.ToString("0");
    }

    public void RestoreHealth(int amount)
    {
        health += amount;
        if (health > maxHealth)
            health = maxHealth;

        HealthBarFill.fillAmount = HealthPercent();
        HealthText.text = health.ToString("0") + "/" + maxHealth.ToString("0");
    }

    public void GainRage(float amount)
    {
        rage += amount;
        if (rage > maxRage)
            rage = maxRage;
        RageBarFill.fillAmount = rage / maxRage;
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

            switch (which)
            {
                case 0:
                    UpgradesScript.Check();
                    break;
                case 1:
                    PerksScript.Check();
                    break;
                case 2:
                    StorageScript.DisplayStorage();
                    break;
                case 3:
                    StorageScript.CraftingScript.DisplayRecipe();
                    break;
                case 4:
                    StorageScript.DisplayEquipment();
                    break;
                case 5:
                    SmeltingScript.DisplayStorage();
                    break;
            }
        }
    }

    public void SelectAutomation(int autoID)
    {
        if (automation)
        {
            if (autoID == auto)
            {
                automation = false;
                AutomationImage[auto].color = new Color(0f, 0f, 0f, 0.49f);
            }
            else
            {
                AutomationImage[auto].color = new Color(0f, 0f, 0f, 0.49f);
                auto = autoID;
                AutomationImage[auto].color = new Color(0.18f, 0.18f, 0.57f, 0.57f);
                if (auto == 0)
                    AutoCombat();
                else AutoCollect();
            }
        }
        else
        {
            automation = true;
            auto = autoID;
            AutomationImage[auto].color = new Color(0.18f, 0.18f, 0.57f, 0.57f);
            if (auto == 0)
                AutoCombat();
            else AutoCollect();
        }
    }

    public void WorldView()
    {
        WorldScreenObject.SetActive(true);
        WorldMapScript.Set(island);
    }

    public void SelectNewTask(int which)
    {
        task = which;
        
        TaskScreens[task].SetActive(true);

        switch (task)
        {
            case 0:
                break;
            case 1:
                break;
        }
    }

    public void ChangeTask()
    {
        if (MobileTargeted)
            MobileTargeted.Shadow.color = new Color(0f, 0f, 0f, 0.49f);
        if (ResourceTargeted)
            ResourceTargeted.Shadow.color = new Color(0f, 0f, 0f, 0.49f);
        for (int i = 0; i < TaskScreens.Length; i++)
        {
            TaskScreens[i].SetActive(false);
        }
        fighting = false;
        if (attackCharge > 0.1f)
            attackCharge = 0.1f;
        collecting = false;
        if (taskProgress > 0.1f)
            taskProgress = 0.1f;
        moving = false;
    }

    void AutoCombat()
    {
        if (automation && auto == 0)
        {
            if (HealthPercent() >= 0.3f && !fighting)
            {
                GameObject finder = Instantiate(FinderPrefab, transform.position, transform.rotation);
                FinderScript = finder.GetComponent(typeof(Finder)) as Finder;
                FinderScript.PlayerScript = this;
                FinderScript.taskFinderID = 0;
            }
            Invoke("AutoCombat", 2f);
        }
    }

    void AutoCollect()
    {
        if (automation && auto == 1)
        {
            if (!collecting)
            {
                GameObject finder = Instantiate(FinderPrefab, transform.position, transform.rotation);
                FinderScript = finder.GetComponent(typeof(Finder)) as Finder;
                FinderScript.PlayerScript = this;
                FinderScript.taskFinderID = 1;
            }
            Invoke("AutoCollect", 2f);
        }
    }

    public void UnlockRage()
    {
        RageBarObject.SetActive(true);
        RageBarFill.fillAmount = 0f;
    }

    float HealthPercent()
    {
        return (health * 1f) / (maxHealth * 1f);
    }

    float CritChance()
    {
        return critChance + weaponCrit;
    }

    float AttackRange()
    {
        return attackRange * weaponRange;
    }

    /*private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Tree")
            ChangeTask(1);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        ChangeTask(2);
    }*/
}
