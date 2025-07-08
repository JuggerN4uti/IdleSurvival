using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Storage : MonoBehaviour
{
    [Header("Stats")]
    public Player PlayerScript;
    public Woodcutting WoodcuttingScript;
    public ItemsLibrary ILib;
    public EquipmentLibrary ELib;
    public Crafting CraftingScript;
    public int[] itemsCount, itemsID, eqCount, eqID;
    int itemsVariety;

    [Header("UI")]
    public GameObject DisplayObject;
    public Transform Origin;
    private TextPop Displayed;

    [Header("Storage Screen")]
    public GameObject[] SlotObject;
    public Image[] SlotIcon, EqSlotIcon, WornIcon;
    public TMPro.TextMeshProUGUI[] SlotAmountText, EqSlotAmountText;
    public TMPro.TextMeshProUGUI TooltipText, EqTooltipText;
    public Button[] SlotButton;

    [Header("Equipment")]
    public bool[] gearEquiped;
    public int[] gearWornID;
    int eqSlot;

    public void CollectItem(int itemID, int amount)
    {
        itemsCount[itemID] += amount;
        Display(itemID, amount);
        if (PlayerScript.windowOpened[2])
            DisplayStorage();
        if (PlayerScript.windowOpened[3])
            CraftingScript.DisplayRecipe();
    }

    public void UseItem(int itemID, int amount)
    {
        itemsCount[itemID] -= amount;
    }

    public void CollectEq(int eqID, bool display = true)
    {
        eqCount[eqID]++;
        if (display)
            DisplayEq(eqID, 1);
        if (PlayerScript.windowOpened[4])
            DisplayEquipment();
    }

    void Display(int itemID, int amount)
    {
        GameObject display = Instantiate(DisplayObject, Origin.position, transform.rotation);
        Displayed = display.GetComponent(typeof(TextPop)) as TextPop;
        Displayed.SetItemText(amount, ILib.Items[itemID].ItemSprite);
        Rigidbody2D display_body = display.GetComponent<Rigidbody2D>();
        display_body.AddForce(Origin.up * 0.44f, ForceMode2D.Impulse);
    }

    void DisplayEq(int eqID, int amount)
    {
        GameObject display = Instantiate(DisplayObject, Origin.position, transform.rotation);
        Displayed = display.GetComponent(typeof(TextPop)) as TextPop;
        Displayed.SetItemText(amount, ELib.EqItems[eqID].EqSprite);
        Rigidbody2D display_body = display.GetComponent<Rigidbody2D>();
        display_body.AddForce(Origin.up * 0.44f, ForceMode2D.Impulse);
    }

    public void DisplayStorage()
    {
        for (int i = 0; i < SlotObject.Length; i++)
        {
            SlotObject[i].SetActive(false);
        }
        itemsVariety = 0;
        for (int i = 0; i < itemsCount.Length; i++)
        {
            if (itemsCount[i] > 0)
            {
                SlotObject[itemsVariety].SetActive(true);
                itemsID[itemsVariety] = i;
                SlotIcon[itemsVariety].sprite = ILib.Items[i].ItemSprite;
                if (ILib.Items[i].consumable)
                    SlotButton[itemsVariety].interactable = true;
                else SlotButton[itemsVariety].interactable = false;
                SlotAmountText[itemsVariety].text = itemsCount[i].ToString("0");
                itemsVariety++;
            }
        }
    }

    public void DisplayEquipment()
    {
        for (int i = 0; i < EqSlotIcon.Length; i++)
        {
            EqSlotIcon[i].enabled = false;
            EqSlotAmountText[i].text = "";
        }
        itemsVariety = 0;
        for (int i = 0; i < eqCount.Length; i++)
        {
            if (eqCount[i] > 0)
            {
                EqSlotIcon[i].enabled = true;
                eqID[itemsVariety] = i;
                EqSlotIcon[itemsVariety].sprite = ELib.EqItems[i].EqSprite;
                EqSlotAmountText[itemsVariety].text = eqCount[i].ToString("0");
                itemsVariety++;
            }
        }
    }

    public void SlotClicked(int slot)
    {
        switch (itemsID[slot])
        {
            case 1:
                PlayerScript.RestoreHealth(10);
                break;
            case 3:
                WoodcuttingScript.GainXP(6, false);
                break;
        }

        itemsCount[itemsID[slot]]--;
        DisplayStorage();
    }

    public void EqSlotClicked(int slot)
    {
        eqSlot = ELib.EqItems[eqID[slot]].eqType;
        if (gearEquiped[eqSlot])
        {
            CollectEq(gearWornID[eqSlot], false);
            gearWornID[eqSlot] = eqID[slot];
        }
        else
        {
            gearEquiped[eqSlot] = true;
            gearWornID[eqSlot] = eqID[slot];
        }
        eqCount[eqID[slot]]--;
        DisplayEquipment();
        DisplayGear();
        SetGearStats();
    }

    public void UnEquipGear(int slot)
    {
        CollectEq(gearWornID[slot], false);
        gearEquiped[slot] = false;
        DisplayEquipment();
        DisplayGear();
        SetGearStats();
    }

    void DisplayGear()
    {
        for (int i = 0; i < 5; i++)
        {
            if (gearEquiped[i])
            {
                WornIcon[i].enabled = true;
                WornIcon[i].sprite = ELib.EqItems[gearWornID[i]].EqSprite;
            }
            else WornIcon[i].enabled = false;
        }
    }

    void SetGearStats()
    {
        if (gearEquiped[0])
        {
            PlayerScript.weaponDamage = ELib.EqItems[gearWornID[0]].DamageMultiplier;
            PlayerScript.weaponRate = ELib.EqItems[gearWornID[0]].SpeedMultiplier;
        }
        else
        {
            PlayerScript.weaponDamage = 1.0f;
            PlayerScript.weaponRate = 1.0f;
        }
    }

    public void SlotHovered(int slot, bool eq, bool worn)
    {
        if (eq)
        {
            if (worn)
                EqTooltipText.text = ELib.EqItems[gearWornID[slot]].EqTooltip; //potem reszte stat
            else EqTooltipText.text = ELib.EqItems[itemsID[slot]].EqTooltip; //potem reszte stat
        }
        else TooltipText.text = ILib.Items[itemsID[slot]].itemTooltip;
    }

    public void Unhovered()
    {
        TooltipText.text = "";
        EqTooltipText.text = "";
    }
}
