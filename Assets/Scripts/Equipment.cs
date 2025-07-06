using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    [Header("Info")]
    public int eqType; // 0 - Weapon,
    public Sprite EqSprite;

    [Header("Weapon")]
    public float DamageMultiplier;
    public float SpeedMultiplier;

    [Header("Tooltip")]
    public int lines;
    public string[] lineText;
    public Color[] lineColor;
}
