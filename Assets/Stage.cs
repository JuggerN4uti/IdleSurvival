using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    [Header("Mob")]
    public Sprite MobSprite;
    public int MobHealth;
    public int[] Xp, Gold;

    [Header("Boss")]
    public Sprite BossSprite;
    public int BossHealth;
    public float BossAttackRate;
    public int[] BossAttackDamage;
    public int BossXp, BossGold;
}
