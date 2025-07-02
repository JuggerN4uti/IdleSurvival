using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Storage : MonoBehaviour
{
    [Header("Stats")]
    public Player PlayerScript;
    public ItemsLibrary ILib;
    public int[] itemsCount, itemsID;
    int itemsVariety;

    [Header("UI")]
    public GameObject DisplayObject;
    public Transform Origin;
    private TextPop Displayed;

    [Header("Screen")]
    public GameObject[] SlotObject;
    public Image[] SlotIcon;
    public TMPro.TextMeshProUGUI[] SlotAmountText;
    public Button[] SlotButton;

    public void CollectItem(int itemID, int amount)
    {
        itemsCount[itemID] += amount;
        Display(itemID, amount);
        if (PlayerScript.windowOpened[2])
            DisplayStorage();
    }

    void Display(int itemID, int amount)
    {
        GameObject display = Instantiate(DisplayObject, Origin.position, transform.rotation);
        Displayed = display.GetComponent(typeof(TextPop)) as TextPop;
        Displayed.SetItemText(amount, ILib.Items[itemID].ItemSprite);
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
}
