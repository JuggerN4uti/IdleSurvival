using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : MonoBehaviour
{
    [Header("Stats")]
    public Sprite MobSprite;
    public int MobHealth;
    public int[] AttackDamage;
    public int[] Xp, Gold;

    [Header("Drops")]
    public int mobDropCount;
    public int[] dropID, maxDrops;
    public float[] dropChance;
}
