using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mobile : MonoBehaviour
{
    [Header("Scripts")]
    public Player PlayerScript;
    public Combat CombatScript;
    public CombatLibrary CLib;

    [Header("Stats")]
    public int unitID;
    public int maxHealth, health;
    public int[] damageRange;
    public int[] expRange, goldRange;
    public float catchUp, fading;
    public bool alive;
    int damage;

    [Header("Movement")]
    public Vector3 CenterPosition;
    public Vector3 MovePosition;
    public float movementSpeed;
    bool moving;

    [Header("UI")]
    public SpriteRenderer UnitImage;
    public Image HealthBarFill, CatchUpFill;
    public TMPro.TextMeshProUGUI HealthText;
    public GameObject DisplayObject;
    public Rigidbody2D Body;
    public Transform Origin;
    private TextPop Displayed;

    [Header("Drops")]
    public int mobDropCount;
    public int[] dropID, maxDrops;
    public float[] dropChance;
    int dropped;

    void Start()
    {
        PlayerScript = GameObject.FindGameObjectWithTag("Player").GetComponent(typeof(Player)) as Player;
        CenterPosition = transform.position;
        Invoke("Wander", Random.Range(4.5f, 7.5f));
        //SetMobile();
    }

    void Update()
    {
        if (catchUp > health)
        {
            catchUp -= maxHealth * 0.15f * Time.deltaTime;
            CatchUpFill.fillAmount = catchUp / (maxHealth * 1f);
        }
        if (!alive)
        {
            fading -= 1.2f * Time.deltaTime;
            UnitImage.color = new Color(fading, fading, fading, fading);
        }
        if (moving)
        {
            transform.position = Vector2.MoveTowards(transform.position, MovePosition, movementSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, MovePosition) <= 0.003f)
                moving = false;
        }
    }

    void Wander()
    {
        MovePosition = new Vector3(CenterPosition.x + Random.Range(-1f, 1f), CenterPosition.y + Random.Range(-1f, 1f), CenterPosition.z);
        moving = true;
        Invoke("Wander", Random.Range(4.5f, 7.5f));
    }

    void SetMobile()
    {
        UnitImage.sprite = CLib.Mobs[unitID].MobSprite;
        maxHealth = CLib.Mobs[unitID].MobHealth;
        health = maxHealth;
        HealthBarFill.fillAmount = 1f;
        catchUp = maxHealth;
        CatchUpFill.fillAmount = 1f;
        HealthText.text = health.ToString("0") + "/" + maxHealth.ToString("0");
        damageRange[0] = CLib.Mobs[unitID].AttackDamage[0];
        damageRange[1] = CLib.Mobs[unitID].AttackDamage[1];
        expRange[0] = CLib.Mobs[unitID].Xp[0];
        expRange[1] = CLib.Mobs[unitID].Xp[1];
        goldRange[0] = CLib.Mobs[unitID].Gold[0];
        goldRange[1] = CLib.Mobs[unitID].Gold[1];

        mobDropCount = CLib.Mobs[unitID].mobDropCount;
        for (int i = 0; i < mobDropCount; i++)
        {
            dropID[i] = CLib.Mobs[unitID].dropID[i];
            maxDrops[i] = CLib.Mobs[unitID].maxDrops[i];
            dropChance[i] = CLib.Mobs[unitID].dropChance[i];
        }
    }

    void ResetMobile()
    {
        alive = true;
        fading = 1f;
        UnitImage.color = new Color(fading, fading, fading, fading);
        health = maxHealth;
        HealthBarFill.fillAmount = 1f;
        catchUp = maxHealth;
        CatchUpFill.fillAmount = 1f;
        HealthText.text = health.ToString("0") + "/" + maxHealth.ToString("0");
    }

    public void DamageMob(float amount, bool crit = false)
    {
        amount *= PlayerScript.damageIncrease;
        PlayerScript.GainGold(Mathf.RoundToInt(amount * 0.0075f));
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
        alive = false;

        PlayerScript.GainXP(Random.Range(expRange[0], expRange[1] + 1));
        PlayerScript.GainGold(Random.Range(goldRange[0], goldRange[1] + 1));
        Drops();

        HealthBarFill.fillAmount = 0f;
        HealthText.text = "0/" + maxHealth.ToString("0");

        CombatScript.MobSlained();
        Invoke("ResetMobile", 0.75f);
    }

    void Drops()
    {
        for (int i = 0; i < mobDropCount; i++)
        {
            dropped = 0;
            for (int j = 0; j < maxDrops[i]; j++)
            {
                if (dropChance[i] >= Random.Range(0f, 1f))
                    dropped++;
            }
            if (dropped > 0)
                PlayerScript.StorageScript.CollectItem(dropID[i], dropped);
        }
    }
}
