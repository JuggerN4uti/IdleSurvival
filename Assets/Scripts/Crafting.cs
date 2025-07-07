using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crafting : MonoBehaviour
{
    [Header("Scripts")]
    public Player PlayerScript;
    public Storage StorageScript;
    public CraftingLibrary CLib;
    public ItemsLibrary ILib;
    public EquipmentLibrary ELib;

    [Header("Stats")]
    public int recpie;
    public int differentMaterials;
    public int[] materialID, materialRequired;
    public int timeRequired;
    int minutes;

    [Header("UI")]
    public GameObject[] MaterialObject;
    public Image[] MaterialIcon;
    public Image CraftedIcon;
    public TMPro.TextMeshProUGUI[] MaterialAmountText;
    public TMPro.TextMeshProUGUI TimerText, CraftedAmount;

    public void RecipeSelected(int which)
    {
        recpie = which;
        SetRecipe();
        DisplayRecipe();
    }

    void SetRecipe()
    {
        for (int i = 0; i < 6; i++)
        {
            MaterialObject[i].SetActive(false);
        }

        differentMaterials = CLib.Recipes[recpie].uniqueMaterials;
        timeRequired = CLib.Recipes[recpie].craftDuration;
        TimerText.text = CalculatedTime(timeRequired);
        if (CLib.Recipes[recpie].eqItem)
            CraftedIcon.sprite = ELib.EqItems[CLib.Recipes[recpie].craftedID].EqSprite;
        else CraftedIcon.sprite = ILib.Items[CLib.Recipes[recpie].craftedID].ItemSprite;
        CraftedAmount.text = "x" + CLib.Recipes[recpie].craftedCount.ToString("0");

        for (int i = 0; i < differentMaterials; i++)
        {
            MaterialObject[i].SetActive(true);
            materialID[i] = CLib.Recipes[recpie].materialID[i];
            materialRequired[i] = CLib.Recipes[recpie].materialCount[i];
            MaterialIcon[i].sprite = ILib.Items[materialID[i]].ItemSprite;
        }
    }

    public void DisplayRecipe()
    {
        for (int i = 0; i < differentMaterials; i++)
        {
            MaterialAmountText[i].text = StorageScript.itemsCount[materialID[i]].ToString("0") + "/" + materialRequired[i].ToString("0");
            if (StorageScript.itemsCount[materialID[i]] >= materialRequired[i])
                MaterialAmountText[i].color = new Color(1f, 1f, 1f, 1f);
            else MaterialAmountText[i].color = new Color(1f, 0.25f, 0.25f, 1f);
        }
    }

    string CalculatedTime(int seconds)
    {
        minutes = seconds / 60;
        seconds -= minutes * 60;

        if (minutes > 0)
            return (minutes.ToString("0") + "m\n" + seconds.ToString("0") + "s");
        else return (seconds.ToString("0") + "s");
    }
}
