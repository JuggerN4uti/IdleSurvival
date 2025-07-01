using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    [Header("Stats")]
    public ItemsLibrary ILib;
    public int[] itemsCount;

    [Header("UI")]
    public GameObject DisplayObject;
    public Transform Origin;
    private TextPop Displayed;

    [Header("Screen")]
    public GameObject[] SlotObject;

    public void CollectItem(int itemID, int amount)
    {
        itemsCount[itemID] += amount;
        Display(itemID, amount);
    }

    void Display(int itemID, int amount)
    {
        GameObject display = Instantiate(DisplayObject, Origin.position, transform.rotation);
        Displayed = display.GetComponent(typeof(TextPop)) as TextPop;
        Displayed.SetItemText(amount, ILib.Items[itemID].ItemSprite);
        Rigidbody2D display_body = display.GetComponent<Rigidbody2D>();
        display_body.AddForce(Origin.up * 0.44f, ForceMode2D.Impulse);
    }
}
