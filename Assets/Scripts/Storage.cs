using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Storage : MonoBehaviour
{
    [Header("Stats")]
    public Player PlayerScript;
    public Smelting SmeltingScript;
    public ItemsLibrary ILib;
    public EquipmentLibrary ELib;
    public Crafting CraftingScript;
    public int[] itemsCount, itemsID, eqCount, eqID;
    int itemsVariety, armor;

    [Header("UI")]
    public GameObject DisplayObject;
    public Transform Origin;
    private TextPop Displayed;

    [Header("Storage Screen")]
    public GameObject[] SlotObject;
    public Image[] SlotIcon, EqSlotIcon, WornIcon;
    public TMPro.TextMeshProUGUI[] SlotAmountText, EqSlotAmountText, LinesText;
    public TMPro.TextMeshProUGUI TooltipText, EqTooltipText;
    public Button[] SlotButton;

    [Header("Equipment")]
    public bool[] gearEquiped;
    public int[] gearWornID;
    public int eqSlot, eqUnequipped;

    public void CollectItem(int itemID, int amount)
    {
        itemsCount[itemID] += amount;
        Display(itemID, amount);
        if (PlayerScript.windowOpened[2])
            DisplayStorage();
        if (PlayerScript.windowOpened[3])
            CraftingScript.DisplayRecipe();
        if (PlayerScript.windowOpened[5])
            SmeltingScript.DisplayStorage();
    }

    public void UseItem(int itemID, int amount)
    {
        itemsCount[itemID] -= amount;
    }

    public void CollectEq(int eqID, int amount, bool display = true)
    {
        eqCount[eqID] += amount;
        if (display)
            DisplayEq(eqID, amount);
        if (PlayerScript.windowOpened[4])
            DisplayEquipment();
    }

    public void UseEq(int eqID, int amount)
    {
        eqCount[eqID] -= amount;
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
                EqSlotIcon[itemsVariety].enabled = true;
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
                PlayerScript.RestoreHealth(15);
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
            eqUnequipped = gearWornID[eqSlot];
            gearWornID[eqSlot] = eqID[slot];
            eqCount[eqID[slot]]--;
            CollectEq(eqUnequipped, 1, false);
            LoseStats(eqUnequipped);
            GainStats(eqID[slot]);
        }
        else
        {
            gearEquiped[eqSlot] = true;
            gearWornID[eqSlot] = eqID[slot];
            eqCount[eqID[slot]]--;
            GainStats(eqID[slot]);
        }
        DisplayEquipment();
        DisplayGear();
    }

    public void UnEquipGear(int slot)
    {
        CollectEq(gearWornID[slot], 1, false);
        gearEquiped[slot] = false;
        DisplayEquipment();
        DisplayGear();
        LoseStats(gearWornID[slot]);
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

    void GainStats(int eqID)
    {
        if (ELib.EqItems[eqID].eqType == 0)
        {
            PlayerScript.weaponDamage = ELib.EqItems[eqID].DamageMultiplier;
            PlayerScript.weaponRate = ELib.EqItems[eqID].SpeedMultiplier;
        }
        else if (ELib.EqItems[eqID].eqType == 0)
        {
            // potem
        }
        else
        {
            PlayerScript.armor += ELib.EqItems[eqID].Armor;
            PlayerScript.GainHP(ELib.EqItems[eqID].BonusHealth);
            PlayerScript.regeneration += ELib.EqItems[eqID].BonusRegen;
        }
    }

    void LoseStats(int eqID)
    {
        if (ELib.EqItems[eqID].eqType == 0)
        {
            PlayerScript.weaponDamage = 1.0f;
            PlayerScript.weaponRate = 1.0f;
        }
        else if (ELib.EqItems[eqID].eqType == 0)
        {
            // potem
        }
        else
        {
            PlayerScript.armor -= ELib.EqItems[eqID].Armor;
            PlayerScript.LoseHP(ELib.EqItems[eqID].BonusHealth);
            PlayerScript.regeneration -= ELib.EqItems[eqID].BonusRegen;
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
        armor = 0;
        for (int i = 2; i < 5; i++)
        {
            if (gearEquiped[i])
                armor += ELib.EqItems[gearWornID[i]].Armor;
        }
        PlayerScript.armor = armor;
    }

    public void SlotHovered(int slot, bool eq, bool worn)
    {
        if (eq)
        {
            if (worn)
            {
                EqTooltipText.text = ELib.EqItems[gearWornID[slot]].EqTooltip;
                for (int i = 0; i < ELib.EqItems[gearWornID[slot]].lines; i++)
                {
                    LinesText[i].text = ELib.EqItems[gearWornID[slot]].lineText[i];
                    LinesText[i].color = ELib.EqItems[gearWornID[slot]].lineColor[i];
                }
            }
            else
            {
                EqTooltipText.text = ELib.EqItems[eqID[slot]].EqTooltip;
                for (int i = 0; i < ELib.EqItems[eqID[slot]].lines; i++)
                {
                    LinesText[i].text = ELib.EqItems[eqID[slot]].lineText[i];
                    LinesText[i].color = ELib.EqItems[eqID[slot]].lineColor[i];
                }
            }
        }
        else TooltipText.text = ILib.Items[itemsID[slot]].itemTooltip;
    }

    public void Unhovered()
    {
        TooltipText.text = "";
        EqTooltipText.text = "";
        for (int i = 0; i < LinesText.Length; i++)
        {
            LinesText[i].text = "";
        }
    }
}
