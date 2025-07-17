using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crafting : MonoBehaviour
{
    [Header("Scripts")]
    public Storage StorageScript;
    public CraftingLibrary CLib;
    public ItemsLibrary ILib;
    public EquipmentLibrary ELib;

    [Header("Stats")]
    public int recpie;
    public int differentMaterials;
    public bool[] eqMaterial;
    public int[] materialID, materialRequired;
    public int timeRequired;
    public int number;
    int minutes;
    bool enough;

    [Header("Crafting Progress")]
    public int recipeCrafted;
    public int craftedNumber;
    public float duration, timeLeft;
    bool craftingInProgress;

    [Header("UI")]
    public GameObject[] MaterialObject;
    public Image[] MaterialIcon;
    public Image CraftedIcon, CraftingProgress;
    public Button CraftButton;
    public TMPro.TextMeshProUGUI[] MaterialAmountText, LinesText;
    public TMPro.TextMeshProUGUI TimerText, CraftedAmount, TooltipText;

    void Update()
    {
        if (craftingInProgress)
        {
            timeLeft -= Time.deltaTime;
            CraftingProgress.fillAmount = timeLeft / duration;
            if (timeLeft <= 0f)
                CraftCompleted();
        }
    }

    public void RecipeSelected(int which)
    {
        recpie = which;
        SetRecipe();
        DisplayRecipe();
    }

    void SetRecipe()
    {
        number = 1;
        for (int i = 0; i < 6; i++)
        {
            MaterialObject[i].SetActive(false);
            if (i != 5)
                LinesText[i].text = "";
        }

        differentMaterials = CLib.Recipes[recpie].uniqueMaterials;
        timeRequired = CLib.Recipes[recpie].craftDuration * number;
        TimerText.text = CalculatedTime(timeRequired);
        if (CLib.Recipes[recpie].eqItem)
        {
            CraftedIcon.sprite = ELib.EqItems[CLib.Recipes[recpie].craftedID].EqSprite;
            TooltipText.text = ELib.EqItems[CLib.Recipes[recpie].craftedID].EqTooltip;
            for (int i = 0; i < ELib.EqItems[CLib.Recipes[recpie].craftedID].lines; i++)
            {
                LinesText[i].text = ELib.EqItems[CLib.Recipes[recpie].craftedID].lineText[i];
                LinesText[i].color = ELib.EqItems[CLib.Recipes[recpie].craftedID].lineColor[i];
            }
        }
        else
        {
            CraftedIcon.sprite = ILib.Items[CLib.Recipes[recpie].craftedID].ItemSprite;
            TooltipText.text = ILib.Items[CLib.Recipes[recpie].craftedID].itemTooltip;
        }
        CraftedAmount.text = "x" + CLib.Recipes[recpie].craftedCount.ToString("0");

        for (int i = 0; i < differentMaterials; i++)
        {
            MaterialObject[i].SetActive(true);
            eqMaterial[i] = CLib.Recipes[recpie].eqMaterial[i];
            materialID[i] = CLib.Recipes[recpie].materialID[i];
            materialRequired[i] = CLib.Recipes[recpie].materialCount[i];
            if (eqMaterial[i])
                MaterialIcon[i].sprite = ELib.EqItems[materialID[i]].EqSprite;
            else MaterialIcon[i].sprite = ILib.Items[materialID[i]].ItemSprite;
        }
    }

    public void DisplayRecipe()
    {
        enough = true;
        for (int i = 0; i < differentMaterials; i++)
        {
            if (eqMaterial[i])
            {
                MaterialAmountText[i].text = StorageScript.eqCount[materialID[i]].ToString("0") + "/" + materialRequired[i].ToString("0");
                if (StorageScript.eqCount[materialID[i]] >= materialRequired[i])
                    MaterialAmountText[i].color = new Color(1f, 1f, 1f, 1f);
                else
                {
                    MaterialAmountText[i].color = new Color(1f, 0.25f, 0.25f, 1f);
                    enough = false;
                }
            }
            else
            {
                MaterialAmountText[i].text = StorageScript.itemsCount[materialID[i]].ToString("0") + "/" + MaterialRequired(i).ToString("0");
                if (StorageScript.itemsCount[materialID[i]] >= MaterialRequired(i))
                    MaterialAmountText[i].color = new Color(1f, 1f, 1f, 1f);
                else
                {
                    MaterialAmountText[i].color = new Color(1f, 0.25f, 0.25f, 1f);
                    enough = false;
                }
            }
        }
        if (craftingInProgress)
            CraftButton.interactable = false;
        else CraftButton.interactable = enough;
    }

    string CalculatedTime(int seconds)
    {
        minutes = seconds / 60;
        seconds -= minutes * 60;

        if (minutes > 0)
            return (minutes.ToString("0") + "m\n" + seconds.ToString("0") + "s");
        else return (seconds.ToString("0") + "s");
    }

    int MaterialRequired(int id)
    {
        return materialRequired[id] * number;
    }

    public void Craft()
    {
        for (int i = 0; i < differentMaterials; i++)
        {
            if (eqMaterial[i])
                StorageScript.UseEq(materialID[i], MaterialRequired(i));
            else StorageScript.UseItem(materialID[i], MaterialRequired(i));
        }
        recipeCrafted = recpie;
        craftedNumber = number;
        duration = timeRequired;
        timeLeft = duration;
        CraftingProgress.fillAmount = 1f;
        craftingInProgress = true;
        DisplayRecipe();
    }

    public void UpTheNumber()
    {
        number++;

        timeRequired = CLib.Recipes[recpie].craftDuration * number;
        TimerText.text = CalculatedTime(timeRequired);
        CraftedAmount.text = "x" + (CLib.Recipes[recpie].craftedCount * number).ToString("0");

        DisplayRecipe();
    }

    public void DownTheNumber()
    {
        if (number > 1)
            number--;

        timeRequired = CLib.Recipes[recpie].craftDuration * number;
        TimerText.text = CalculatedTime(timeRequired);
        CraftedAmount.text = "x" + (CLib.Recipes[recpie].craftedCount * number).ToString("0");

        DisplayRecipe();
    }

    void CraftCompleted()
    {
        CraftingProgress.fillAmount = 0f;
        craftingInProgress = false;
        DisplayRecipe();

        if (CLib.Recipes[recipeCrafted].eqItem)
            StorageScript.CollectEq(CLib.Recipes[recipeCrafted].craftedID, CLib.Recipes[recipeCrafted].craftedCount * craftedNumber);
        else StorageScript.CollectItem(CLib.Recipes[recipeCrafted].craftedID, CLib.Recipes[recipeCrafted].craftedCount * craftedNumber);
    }
}
