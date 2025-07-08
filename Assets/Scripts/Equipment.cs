using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    [Header("Info")]
    public int eqType; // 0 - Main Hand, 1 - Upper Body, 2 - Off Hand, 3 - Head, 4 - Lower Body
    public Sprite EqSprite;
    public string EqTooltip;

    [Header("Weapon")]
    public float DamageMultiplier;
    public float SpeedMultiplier;

    [Header("Tooltip")]
    public int lines;
    public string[] lineText;
    public Color[] lineColor;
}
