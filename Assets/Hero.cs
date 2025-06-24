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
        damage = Random.Range(attackDamage[0] + PlayerScript.baseDamageBonus, attackDamage[1] + PlayerScript.baseDamageBonus);
        if (critChance >= Random.Range(0f, 1f))
        {
            damage *= critDamage;
            CombatScript.DamageMob(damage, true);
        }
        else CombatScript.DamageMob(damage, false);
    }
}
