using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Smelting : MonoBehaviour
{
    [Header("Scripts")]
    public Player PlayerScript;
    public Storage StorageScript;
    public ItemsLibrary ILib;

    [Header("Smelting Screen")]
    public GameObject[] SlotObject;
    public Image[] SlotIcon;
    public TMPro.TextMeshProUGUI[] SlotAmountText;
    public Button[] SlotButton;

    [Header("Smelting UI")]
    public Image SmeltingIcon;
    public Image SmeltingProgressImage;
    public TMPro.TextMeshProUGUI SmeltingAmountText, FuelAmountText;
    public Sprite BlankSprite;

    [Header("Stats")]
    public bool smelting;
    public int[] itemsID;
    public int itemsVariety, smeltingID, smeltingCount;
    public float fuel, smeltingProgress, smeltingTime;

    void Update()
    {
        if (smelting)
        {
            if (fuel > 0f)
            {
                fuel -= Time.deltaTime;
                if (fuel < 0f)
                    fuel = 0f;
                FuelAmountText.text = fuel.ToString("0.00");
                smeltingProgress += Time.deltaTime;
                if (smeltingProgress >= smeltingTime)
                    Smelted();
                SmeltingProgressImage.fillAmount = SmeltingPercent();
                SmeltingProgressImage.color = new Color(1f - SmeltingPercent(), SmeltingPercent(), 0f, 1f);
            }
        }
    }

    public void DisplayStorage()
    {
        for (int i = 0; i < SlotObject.Length; i++)
        {
            SlotObject[i].SetActive(false);
        }

        itemsVariety = 0; // smeltable
        for (int i = 0; i < StorageScript.itemsCount.Length; i++)
        {
            if (StorageScript.itemsCount[i] > 0 && ILib.Items[i].smeltable)
            {
                SlotObject[itemsVariety].SetActive(true);
                itemsID[itemsVariety] = i;
                SlotIcon[itemsVariety].sprite = ILib.Items[i].ItemSprite;
                SlotAmountText[itemsVariety].text = StorageScript.itemsCount[i].ToString("0");
                if (smelting && smeltingID != i)
                    SlotButton[itemsVariety].interactable = false;
                else SlotButton[itemsVariety].interactable = true;
                itemsVariety++;
            }
        }

        itemsVariety = 12;
        for (int i = 0; i < StorageScript.itemsCount.Length; i++)
        {
            if (StorageScript.itemsCount[i] > 0 && ILib.Items[i].fuel)
            {
                SlotObject[itemsVariety].SetActive(true);
                itemsID[itemsVariety] = i;
                SlotIcon[itemsVariety].sprite = ILib.Items[i].ItemSprite;
                SlotAmountText[itemsVariety].text = StorageScript.itemsCount[i].ToString("0");
                itemsVariety++;
            }
        }
    }

    public void SlotClicked(int slot)
    {
        if (ILib.Items[itemsID[slot]].fuel)
        {
            GainFuel(ILib.Items[itemsID[slot]].furnaceTimer);
            StorageScript.itemsCount[itemsID[slot]]--;
        }
        else if (ILib.Items[itemsID[slot]].smeltable)
        {
            smelting = true;
            smeltingTime = ILib.Items[itemsID[slot]].furnaceTimer;
            smeltingID = itemsID[slot];
            StorageScript.itemsCount[itemsID[slot]]--;
            smeltingCount++;
            SmeltingIcon.sprite = ILib.Items[itemsID[slot]].ItemSprite;
            SmeltingAmountText.text = smeltingCount.ToString("0");
        }
        DisplayStorage();
    }

    void GainFuel(float fuelGained)
    {
        fuel += fuelGained;
        FuelAmountText.text = fuel.ToString("0.00");
    }

    void Smelted()
    {
        smeltingProgress -= smeltingTime;
        smeltingCount--;
        if (smeltingCount == 0)
        {
            SmeltingAmountText.text = "";
            smelting = false;
            smeltingProgress = 0f;
            SmeltingIcon.sprite = BlankSprite;
        }
        else SmeltingAmountText.text = smeltingCount.ToString("0");
        StorageScript.CollectItem(ILib.Items[smeltingID].smeltedID, 1);
        PlayerScript.GainXP(ILib.Items[smeltingID].smeltedXP);
    }

    float SmeltingPercent()
    {
        return (smeltingProgress / smeltingTime);
    }
}
