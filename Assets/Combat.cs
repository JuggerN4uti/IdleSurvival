using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Combat : MonoBehaviour
{
    [Header("Scripts")]
    public Player PlayerScript;
    public Mobile[] MobileScript;

    [Header("Stage")]
    public CombatLibrary CLib;
    public int stageID, wave, slained, killsRequired;
    public bool bossReady, bossFight;

    [Header("Stats")]
    public int maxHealth;
    public int health, bossHealth;
    public int[] expRange, goldRange;
    public float catchUp;
    int damage;

    [Header("UI")]
    public Image UnitImage;
    public Image HealthBarFill, CatchUpFill;
    public TMPro.TextMeshProUGUI HealthText, SlainedText;
    public GameObject DisplayObject, FightBossObject, MobsScreen, BossScreen;
    public Rigidbody2D Body;
    public Transform Origin;
    private TextPop Displayed;
    public Button FightBossButton;

    void Update()
    {
        if (catchUp > health)
        {
            catchUp -= maxHealth * 0.05f * Time.deltaTime;
            CatchUpFill.fillAmount = catchUp / (maxHealth * 1f);
        }
    }

    public void DamageMob(float amount, bool crit = false)
    {
        amount *= PlayerScript.damageIncrease;
        damage = Mathf.RoundToInt(amount);
        health -= damage;
        Display(damage, crit);
        if (health <= 0f)
            BossDefeated();
        else
        {
            HealthBarFill.fillAmount = (health * 1f) / (maxHealth * 1f);
            HealthText.text = health.ToString("0") + "/" + maxHealth.ToString("0");
        }
    }

    void Display(int amount, bool crit)
    {
        Origin.rotation = Quaternion.Euler(Origin.rotation.x, Origin.rotation.y, Body.rotation + Random.Range(-75f, 75f));
        GameObject display = Instantiate(DisplayObject, Origin.position, transform.rotation);
        Displayed = display.GetComponent(typeof(TextPop)) as TextPop;
        Displayed.SetDamageText(amount, crit);
        Rigidbody2D display_body = display.GetComponent<Rigidbody2D>();
        display_body.AddForce(Origin.up * Random.Range(0.66f, 1.1f), ForceMode2D.Impulse);
    }

    public void MobSlained()
    {
        if (!bossReady)
        {
            slained++;
            SlainedText.text = slained.ToString("0") + "/" + killsRequired.ToString("0");
        }
        if (slained >= killsRequired)
            BossReached();
    }

    public void BossDefeated()
    {
        bossFight = false;
        FightBossObject.SetActive(false);
        MobsScreen.SetActive(true);
        BossScreen.SetActive(false);
        PlayerScript.GainXP(CLib.StageLibrary[stageID].BossXp);
        PlayerScript.GainGold(CLib.StageLibrary[stageID].BossGold);
    }

    void BossReached()
    {
        slained = 0;
        SlainedText.text = "";
        SetBoss();
        bossReady = true;
        FightBossObject.SetActive(true);
    }

    void SetBoss()
    {
        bossHealth = CLib.StageLibrary[stageID].BossHealth;
        UnitImage.sprite = CLib.StageLibrary[stageID].BossSprite;
        maxHealth = CLib.StageLibrary[stageID].BossHealth;
        health = bossHealth;
        HealthBarFill.fillAmount = 1f;
        catchUp = health;
        CatchUpFill.fillAmount = 1f;
        HealthText.text = health.ToString("0") + "/" + maxHealth.ToString("0");
    }

    public void FightBoss()
    {
        bossFight = true;
        FightBossButton.interactable = false;
        maxHealth = CLib.StageLibrary[stageID].BossHealth;
        health = bossHealth;
        HealthBarFill.fillAmount = (health * 1f) / (maxHealth * 1f);
        catchUp = health;
        CatchUpFill.fillAmount = catchUp / (maxHealth * 1f);
        HealthText.text = health.ToString("0") + "/" + maxHealth.ToString("0");
        Invoke("Attack", 2.6f);
        MobsScreen.SetActive(false);
        BossScreen.SetActive(true);
    }

    void Attack()
    {
        damage = Random.Range(CLib.StageLibrary[stageID].BossAttackDamage[0], CLib.StageLibrary[stageID].BossAttackDamage[1] + 1);
        PlayerScript.TakeDamage(damage);
        if (bossFight)
            Invoke("Attack", 2.6f);
    }

    public void Fallen()
    {
        damage = bossHealth - health;
        bossHealth -= Mathf.RoundToInt(damage * 0.04f + (maxHealth - health) * 0.008f);
        bossFight = false;
        MobsScreen.SetActive(true);
        BossScreen.SetActive(false);
    }
}
