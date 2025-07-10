using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    [Header("Info")]
    public int eqType; // 0 - Main Hand, 1 - Off Hand, 2 - Upper Body, 3 - Head, 4 - Lower Body
    public Sprite EqSprite;
    public string EqTooltip;

    [Header("Weapon")]
    public float DamageMultiplier;
    public float SpeedMultiplier;

    [Header("Armor")]
    public int Armor;

    [Header("Tooltip")]
    public int lines;
    public string[] lineText;
    public Color[] lineColor;
}
