using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Combat : MonoBehaviour
{
    [Header("Scripts")]
    public Player PlayerScript;

    [Header("Stage")]
    public CombatLibrary CLib;
    public int stageID, wave, slained;
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
    public GameObject DisplayObject, FightBossObject;
    public Rigidbody2D Body;
    public Transform Origin;
    private TextPop Displayed;
    public Button FightBossButton;

    void Start()
    {
        SetMobile();
    }

    void SetMobile()
    {
        UnitImage.sprite = CLib.StageLibrary[stageID].MobSprite[Random.Range(0, CLib.StageLibrary[stageID].MobSprite.Length)];
        maxHealth = CLib.StageLibrary[stageID].MobHealth[wave];
        health = maxHealth;
        HealthBarFill.fillAmount = 1f;
        catchUp = maxHealth;
        CatchUpFill.fillAmount = 1f;
        HealthText.text = health.ToString("0") + "/" + maxHealth.ToString("0");
        expRange[0] = CLib.StageLibrary[stageID].minXP[wave];
        expRange[1] = CLib.StageLibrary[stageID].maxXP[wave];
        goldRange[0] = CLib.StageLibrary[stageID].minGold[wave];
        goldRange[1] = CLib.StageLibrary[stageID].maxGold[wave];
    }

    void Update()
    {
        if (catchUp > health)
        {
            catchUp -= maxHealth * 0.16f * Time.deltaTime;
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
            Death();
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

    void Death()
    {
        PlayerScript.GainXP(Random.Range(expRange[0], expRange[1] + 1));
        PlayerScript.GainGold(Random.Range(goldRange[0], goldRange[1] + 1));
        if (!bossReady)
        {
            slained++;
            SlainedText.text = slained.ToString("0") + "/10";
        }
        if (slained >= 10)
            NextWave();
        else SetMobile();
    }

    void NextWave()
    {
        slained -= 10;
        SlainedText.text = slained.ToString("0") + "/10";
        if (wave == CLib.StageLibrary[stageID].stages - 1)
        {
            bossReady = true;
            FightBossObject.SetActive(true);
            bossHealth = CLib.StageLibrary[stageID].BossHealth;
            SetBoss();
        }
        else
        {
            wave++;
            SetMobile();
        }
    }

    public void SetBoss()
    {
        bossFight = true;
        FightBossButton.interactable = false;
        UnitImage.sprite = CLib.StageLibrary[stageID].BossSprite;
        maxHealth = CLib.StageLibrary[stageID].BossHealth;
        health = bossHealth;
        HealthBarFill.fillAmount = (health * 1f) / (maxHealth * 1f);
        catchUp = health;
        CatchUpFill.fillAmount = catchUp / (maxHealth * 1f);
        HealthText.text = health.ToString("0") + "/" + maxHealth.ToString("0");
        expRange[0] = CLib.StageLibrary[stageID].xpDropRange[0];
        expRange[1] = CLib.StageLibrary[stageID].xpDropRange[1];
        goldRange[0] = CLib.StageLibrary[stageID].goldDropRange[0];
        goldRange[1] = CLib.StageLibrary[stageID].goldDropRange[1];
        Invoke("Attack", 2.5f);
    }

    void Attack()
    {
        damage = Random.Range(CLib.StageLibrary[stageID].BossAttackDamage[0], CLib.StageLibrary[stageID].BossAttackDamage[1] + 1);
        PlayerScript.TakeDamage(damage);
        if (bossFight)
            Invoke("Attack", 2.5f);
    }

    public void Fallen()
    {
        damage = bossHealth - health;
        bossHealth -= Mathf.RoundToInt(damage * 0.04f + (maxHealth - health) * 0.008f);
        bossFight = false;
        SetMobile();
    }
}
