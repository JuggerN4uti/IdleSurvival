using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextPop : MonoBehaviour
{
    public TMPro.TextMeshProUGUI DisplayedText;
    public Image IconImage;
    public float fade, fadeSpeed;
    public float[] colors;

    public void SetText(int amount, int type)
    {
        if (type == 0)
            DisplayedText.text = "+" + amount.ToString("0") + " xp";
        else DisplayedText.text = "+" + amount.ToString("0") + " gold";
    }

    public void SetDamageText(int amount, bool crit)
    {
        if (crit)
            DisplayedText.text = amount.ToString("0") + "!";
        else DisplayedText.text =amount.ToString("0");
    }

    public void SetItemText(int amount, Sprite itemSprite)
    {
        DisplayedText.text = "+" + amount.ToString("0");
        IconImage.sprite = itemSprite;
    }

    void Update()
    {
        fade -= fadeSpeed * Time.deltaTime;
        if (fade <= 0f)
            Destroy(gameObject);
        else DisplayedText.color = new Color(colors[0], colors[1], colors[2], fade);
    }
}
