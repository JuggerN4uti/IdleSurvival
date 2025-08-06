using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hero : MonoBehaviour
{
    [Header("Scripts")]
    public Player PlayerScript;
    public Combat CombatScript;
    public Mobile[] MobileScript;

    [Header("Stats")]
    public int focused;
    public float[] attackDamage;
    public float attackRate, attackCharge, critChance, critDamage;
    float damage;
    bool crited;

    [Header("UI")]
    public Image AttackBarFill;

    [Header("Perks")]
    public int skillPoints;

    public void Update()
    {
        if (PlayerScript.task == 0)
        {
            attackCharge += attackRate * Time.deltaTime * PlayerScript.speedIncrease;
            if (attackCharge >= 1f)
                Attack();
            AttackBarFill.fillAmount = attackCharge / 1f;
        }
    }

    void Attack()
    {
        attackCharge -= 1f;
        damage = Random.Range(attackDamage[0], attackDamage[1]);
        if (critChance >= Random.Range(0f, 1f))
        {
            damage *= critDamage;
            crited = true;
        }
        else crited = false;

        if (CombatScript.bossFight)
            CombatScript.DamageMob(damage, crited);
        else
        {
            MobileScript[focused].DamageMob(damage, crited);
            if (!MobileScript[focused].alive)
                ChangeFocus();
        }
    }

    void ChangeFocus()
    {
        do
        {
            focused = Random.Range(0, 5);
        } while (!MobileScript[focused].alive);
    }
}
