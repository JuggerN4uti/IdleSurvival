using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    [Header("Waves")]
    public int stages;
    public Sprite[] MobSprite;
    public int[] MobHealth, minXP, maxXP, minGold, maxGold;

    [Header("Boss")]
    public Sprite BossSprite;
    public int BossHealth;
    public float BossAttackRate;
    public int[] BossAttackDamage, xpDropRange, goldDropRange;
}
