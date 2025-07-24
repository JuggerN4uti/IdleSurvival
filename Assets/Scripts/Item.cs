using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Info")]
    public Sprite ItemSprite;
    public bool consumable, smeltable, fuel;
    public int smeltedID, smeltedXP;
    public float furnaceTimer;
    public string itemTooltip;
}
