using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Woodcutting : MonoBehaviour
{
    [Header("Scripts")]
    public Player PlayerScript;

    [Header("Stats")]
    public int level;
    public int experience, expRequired;
    public float cutRate, cutCharge;
    public float[] cutStrengthRange;

    [Header("Tree")]
    public float durability;

    [Header("UI")]
    public Image ExperienceBarFill;
    public Image ProgressBarFill;
    public TMPro.TextMeshProUGUI ExperienceText, LevelText;
    public GameObject DisplayObject;
    public Transform Origin;
    private TextPop Displayed;

    void Start()
    {
        expRequired = PlayerScript.CalculateExpReq(level);
        ExperienceBarFill.fillAmount = (experience * 1f) / (expRequired * 1f);
        ExperienceText.text = experience.ToString("0") + "/" + expRequired.ToString("0");
    }

    void Update()
    {
        if (PlayerScript.task == 1)
        {
            cutCharge += cutRate * Time.deltaTime * PlayerScript.speedIncrease;
            if (cutCharge >= 1f)
                Strike();
        }
    }

    void Strike()
    {
        cutCharge -= 1f;
        durability -= Random.Range(cutStrengthRange[0], cutStrengthRange[1]);
        if (durability <= 0f)
            TreeCut();
        ProgressBarFill.fillAmount = durability / 8f;
    }

    void TreeCut()
    {
        durability += 8f;
        GainXP(Random.Range(3, 5));
        DropWood();
        if (Random.Range(0f, 100f + 1f * level) < 25f + 1f * level)
            Invoke("DropApple", 0.75f);
    }

    void GainXP(int xp)
    {
        experience += xp;
        Display(xp);
        PlayerScript.GainXP(xp * level);
        if (experience >= expRequired)
            LevelUp();
        ExperienceBarFill.fillAmount = (experience * 1f) / (expRequired * 1f);
        ExperienceText.text = experience.ToString("0") + "/" + expRequired.ToString("0");
    }

    void Display(int amount)
    {
        //Origin[type].rotation = Quaternion.Euler(Origin[type].rotation.x, Origin[type].rotation.y, Origin[type].rotation.z);
        GameObject display = Instantiate(DisplayObject, Origin.position, transform.rotation);
        Displayed = display.GetComponent(typeof(TextPop)) as TextPop;
        Displayed.SetText(amount, 0);
        Rigidbody2D display_body = display.GetComponent<Rigidbody2D>();
        display_body.AddForce(Origin.up * 0.44f, ForceMode2D.Impulse);
    }

    void LevelUp()
    {
        experience -= expRequired;
        level++;
        LevelText.text = level.ToString("0");
        cutRate += 0.081f;
        cutStrengthRange[0] += 0.016f;
        cutStrengthRange[1] += 0.02f;
        expRequired = PlayerScript.CalculateExpReq(level);
    }

    void DropWood()
    {
        PlayerScript.StorageScript.CollectItem(0, Random.Range(3 + level / 4, 5 + level / 2));
    }

    void DropApple()
    {
        PlayerScript.StorageScript.CollectItem(1, 1);
        /*if (Random.Range(0f, 100f + 1f * level) < 25f + 1f * level)
            return true;
        else return false;*/
    }
}
