using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Resource : MonoBehaviour
{
    [Header("Scripts")]
    public Player PlayerScript;

    [Header("Stats")]
    public bool infiniteResource;
    public int resourceType, maxCollects; // 0 - wood,
    public int collectsLeft;
    public float catchUp;
    bool destroyed, perishing;

    [Header("Collecting")]
    public int expPerCollect;
    public int[] expForComplete;
    public int dropsPerCollectCount, dropID, bonusDropID;
    public int[] dropCollectID, dropCompleteRange, increasePerLevel;
    public float bonusDropChance, increaseChancePerLevel;
    public float[] dropsCollectChance;
    int dropped;

    [Header("UI")]
    public SpriteRenderer ResourceSprite;
    public SpriteRenderer Shadow;
    public Image ProgressBarFill, CatchUpFill;
    public TMPro.TextMeshProUGUI ProgressText;
    public Collider2D collide;
    float fading;

    void Start()
    {
        PlayerScript = GameObject.FindGameObjectWithTag("Player").GetComponent(typeof(Player)) as Player;
        SetResource();
    }

    void SetResource()
    {
        collectsLeft = maxCollects;
        catchUp = maxCollects;
        ProgressBarFill.fillAmount = 1f;
        CatchUpFill.fillAmount = 1f;
        ProgressText.text = maxCollects.ToString("0") + "/" + collectsLeft.ToString("0");
    }

    void Reset()
    {

    }

    void Update()
    {
        if (catchUp > collectsLeft)
        {
            catchUp -= 1.6f * Time.deltaTime;
            CatchUpFill.fillAmount = catchUp / (maxCollects * 1f);
        }
        if (destroyed)
        {
            fading -= 1.25f * Time.deltaTime;
            ResourceSprite.color = new Color(fading, fading, fading, fading);
            if (fading <= 0f)
                Destroy(gameObject);
        }
    }

    public void Collect()
    {
        if (perishing)
            perishing = false;
        collectsLeft--;
        if (collectsLeft <= 0)
            FullyCollected();
        else
        {
            ProgressBarFill.fillAmount = (collectsLeft * 1f) / (maxCollects * 1f);
            ProgressText.text = collectsLeft.ToString("0") + "/" + maxCollects.ToString("0");

            PlayerScript.GainTaskXP(expPerCollect, resourceType);

            for (int i = 0; i < dropsPerCollectCount; i++)
            {
                if (dropsCollectChance[i] > Random.Range(0, 1f))
                    PlayerScript.StorageScript.CollectItem(dropCollectID[i], 1);
            }
        }
    }

    void FullyCollected()
    {
        if (infiniteResource)
            SetResource();
        else Fade();

        PlayerScript.GainTaskXP(Random.Range(expForComplete[0], expForComplete[1] + 1), resourceType);

        int level = PlayerScript.taskLevel[resourceType];
        PlayerScript.StorageScript.CollectItem(dropID, Random.Range(dropCompleteRange[0] + level / increasePerLevel[0], dropCompleteRange[1] + level / increasePerLevel[1] + 1));

        if (Random.Range(0f, 100f + increaseChancePerLevel * level) < bonusDropChance + increaseChancePerLevel * level)
            Invoke("BonusDrop", 0.8f);
    }

    void Fade()
    {
        fading = 1f;
        destroyed = true;
        PlayerScript.ResourceTargeted = null;
        PlayerScript.collecting = false;
        Shadow.color = new Color(0f, 0f, 0f, 0.49f);
        collide.enabled = false;

        ProgressBarFill.fillAmount = 0f;
        ProgressText.text = "0/" + maxCollects.ToString("0");
    }

    void BonusDrop()
    {
        PlayerScript.StorageScript.CollectItem(bonusDropID, 1);
    }

    public void TargetThis()
    {
        PlayerScript.ChangeTask();
        Shadow.color = new Color(0f, 1f, 0f, 0.49f);
        PlayerScript.ResourceTargeted = this;
        PlayerScript.SelectNewTask(resourceType);
        PlayerScript.collecting = true;
    }

    public void SetExpire(float timer)
    {
        perishing = true;
        Invoke("Perish", timer);
    }

    void Perish()
    {
        if (perishing)
            Fade();
        else
        {
            perishing = true;
            Invoke("Perish", 50f);
        }
    }
}
