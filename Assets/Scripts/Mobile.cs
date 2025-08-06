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
    public Island IslandScript;

    [Header("Stats")]
    public int unitID;
    public int maxHealth, health, regeneration;
    public int[] damageRange;
    public int[] expRange, goldRange;
    public float attackRange, attackCharge, attackRate, catchUp, fading;
    public bool alive, fighting;
    public bool perishing;
    int damage;

    [Header("Spawner")]
    public bool spawner;
    public Transform DistancePoint, DistanceRotation;
    public int spawnPerDamage, nextSpawn;
    public GameObject SpawnObject;
    private Mobile MobileSpawned;

    [Header("Movement")]
    public Vector3 CenterPosition;
    public Vector3 MovePosition;
    public float movementSpeed;
    bool moving;

    [Header("UI")]
    public SpriteRenderer UnitImage;
    public SpriteRenderer Shadow;
    public Image HealthBarFill, CatchUpFill;
    public TMPro.TextMeshProUGUI HealthText;
    public GameObject DisplayObject;
    public Rigidbody2D Body;
    public Transform Origin;
    private TextPop Displayed;
    public Collider2D collide;

    [Header("Drops")]
    public int mobDropCount;
    public int[] dropID, maxDrops;
    public float[] dropChance;
    int dropped;

    void Start()
    {
        PlayerScript = GameObject.FindGameObjectWithTag("Player").GetComponent(typeof(Player)) as Player;
        CenterPosition = transform.position;
        if (!spawner)
            Invoke("Wander", Random.Range(4.5f, 7.5f));
        else nextSpawn = maxHealth - spawnPerDamage / 4;
        SetMobile();
        if (regeneration > 0)
            Invoke("Regen", 0.8f);
    }

    void SetMobile()
    {
        health = maxHealth;
        catchUp = maxHealth;
        HealthBarFill.fillAmount = 1f;
        CatchUpFill.fillAmount = 1f;
        HealthText.text = health.ToString("0") + "/" + maxHealth.ToString("0");
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
            if (fading <= 0f)
                Destroy(gameObject);
        }
        else if (!spawner)
        {
            if (fighting)
            {
                if (Vector3.Distance(transform.position, PlayerScript.transform.position) > attackRange)
                    transform.position = Vector2.MoveTowards(transform.position, PlayerScript.transform.position, movementSpeed * 1.3f * Time.deltaTime);
                else
                {
                    attackCharge += attackRate * Time.deltaTime;
                    if (attackCharge >= 1f)
                        Attack();
                    //AttackBarFill.fillAmount = attackCharge / 1f;
                }
            }
            else if (moving)
            {
                transform.position = Vector2.MoveTowards(transform.position, MovePosition, movementSpeed * Time.deltaTime);
                if (Vector3.Distance(transform.position, MovePosition) <= 0.003f)
                    moving = false;
            }
        }
    }

    void Regen()
    {
        //if (CombatScript.)
        RestoreHealth(regeneration);
        Invoke("Regen", 0.8f);
    }

    void Attack()
    {
        attackCharge -= 1f;

        damage = Random.Range(damageRange[0], damageRange[1] + 1);
        if (!PlayerScript.Dodge())
            PlayerScript.TakeDamage(damage);
    }

    void Wander()
    {
        MovePosition = new Vector3(CenterPosition.x + Random.Range(-1f, 1f), CenterPosition.y + Random.Range(-1f, 1f), CenterPosition.z);
        moving = true;
        Invoke("Wander", Random.Range(4.5f, 7.5f));
    }

    public void DamageMob(float amount, bool crit = false)
    {
        if (perishing)
            perishing = false;
        fighting = true;
        damage = Mathf.RoundToInt(amount);
        health -= damage;
        Display(damage, crit);
        if (health <= 0f && alive)
            Death(true);
        else
        {
            HealthBarFill.fillAmount = (health * 1f) / (maxHealth * 1f);
            HealthText.text = health.ToString("0") + "/" + maxHealth.ToString("0");
            if (spawner)
            {
                if (health < nextSpawn)
                    Spawn();
            }
        }
    }

    public void RestoreHealth(int amount)
    {
        health += amount;
        if (health > maxHealth)
            health = maxHealth;
        if (health > catchUp)
            catchUp = health;

        HealthBarFill.fillAmount = (health * 1f) / (maxHealth * 1f);
        CatchUpFill.fillAmount = catchUp / (maxHealth * 1f);
        HealthText.text = health.ToString("0") + "/" + maxHealth.ToString("0");
    }

    void Display(int amount, bool crit)
    {
        Origin.rotation = Quaternion.Euler(Origin.rotation.x, Origin.rotation.y, Body.rotation + Random.Range(-60f, 60f));
        GameObject display = Instantiate(DisplayObject, Origin.position, transform.rotation);
        Displayed = display.GetComponent(typeof(TextPop)) as TextPop;
        Displayed.SetDamageText(amount, crit);
        Rigidbody2D display_body = display.GetComponent<Rigidbody2D>();
        display_body.AddForce(Origin.up * Random.Range(0.6f, 1f), ForceMode2D.Impulse);
    }

    void Spawn()
    {
        nextSpawn -= spawnPerDamage;

        DistancePoint.position = new Vector2(DistanceRotation.position.x, Random.Range(0.25f, 0.75f) + DistanceRotation.position.y);
        DistanceRotation.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));

        GameObject mob = Instantiate(SpawnObject, DistancePoint.position, transform.rotation);
        MobileSpawned = mob.GetComponent(typeof(Mobile)) as Mobile;
        MobileSpawned.PlayerScript = PlayerScript;
        MobileSpawned.fighting = true;

        if (health < nextSpawn)
            Spawn();
    }

    void Death(bool killed)
    {
        fading = 1f;
        alive = false;
        PlayerScript.MobileTargeted = null;
        PlayerScript.fighting = false;
        Shadow.color = new Color(0f, 0f, 0f, 0.49f);
        collide.enabled = false;

        HealthBarFill.fillAmount = 0f;
        HealthText.text = "0/" + maxHealth.ToString("0");

        if (killed)
        {
            if (IslandScript)
                IslandScript.MobSlained();
            PlayerScript.GainXP(Random.Range(expRange[0], expRange[1] + 1));
            PlayerScript.GainGold(Random.Range(goldRange[0], goldRange[1] + 1));
            Drops();
        }
        //CombatScript.MobSlained();
        //Invoke("ResetMobile", 0.75f);
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

    public void TargetThis()
    {
        PlayerScript.ChangeTask();
        Shadow.color = new Color(1f, 0f, 0f, 0.49f);
        PlayerScript.MobileTargeted = this;
        PlayerScript.fighting = true;
    }

    public void SetExpire(float timer)
    {
        perishing = true;
        Invoke("Perish", timer);
    }

    void Perish()
    {
        if (perishing)
            Death(false);
        else
        {
            perishing = true;
            Invoke("Perish", 50f);
        }
    }
}
