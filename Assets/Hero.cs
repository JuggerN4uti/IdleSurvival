using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hero : MonoBehaviour
{
    [Header("Scripts")]
    public Player PlayerScript;
    public Combat CombatScript;

    [Header("Stats")]
    public float[] attackDamage;
    public float attackRate, attackCharge, critChance, critDamage;
    float damage;

    [Header("UI")]
    public Image AttackBarFill;

    [Header("Perks")]
    public int skillPoints;

    public void Update()
    {
        attackCharge += attackRate * Time.deltaTime * PlayerScript.speedIncrease;
        if (attackCharge >= 1f)
            Attack();
        AttackBarFill.fillAmount = attackCharge / 1f;
    }

    void Attack()
    {
        attackCharge -= 1f;
        damage = Random.Range(attackDamage[0] + PlayerScript.minDamageBonus, attackDamage[1] + PlayerScript.maxDamageBonus);
        if (critChance >= Random.Range(0f, 1f))
        {
            damage *= critDamage;
            CombatScript.DamageMob(damage, true);
        }
        else CombatScript.DamageMob(damage, false);
    }
}
