using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCalculator : MonoBehaviour
{
    readonly float[] normalDamegeRandom = new float[] { 0.890f, 0.891f, 0.892f, 0.893f, 0.894f, 0.895f, 0.896f, 0.897f, 0.898f, 0.899f, 0.990f, 0.991f, 0.992f, 0.993f, 0.994f, 0.995f, 0.996f, 0.997f, 0.998f, 0.999f, 1.000f};
    readonly float[] criticalDamegeRandam = new float[] { 0.95f, 0.96f, 0.97f, 0.98f, 0.99f, 1.00f, 1.01f, 1.02f, 1.03f, 1.04f, 1.05f};
    readonly int[] minDamage = new int[] { 0, 1 };
    int decideDamage;

    internal T GetRandom<T>(params T[] Params)
    {
        return Params[Random.Range(0, Params.Length)];
    }
    
    int CalculateNormalDamage(SkillData skillData, int defender, int attacker)
    {
        float power = skillData.SkillPower / 100;
        int baseDamage = attacker / 4 - defender / 3;
        float normalDamege = baseDamage * GetRandom(normalDamegeRandom) * skillData.SkillMagnification * power;
        return (int)normalDamege;
    }
    
    int CalculateCriticalDamage(SkillData skillData, int attacker)
    {
        float power = skillData.SkillPower / 100;
        float criticalDamege = attacker * GetRandom(criticalDamegeRandam) * skillData.SkillMagnification * power;
        return (int)criticalDamege;
    }
    
    int DecideEnemyDamage(SkillData skillData, int damage, Enemy enemy)
    {
        if (damage <= 0)
        {
            decideDamage = GetRandom(minDamage);
        }
        else
        {
            decideDamage = damage;
        }
        if (decideDamage != 0)
        {
            if ((int)(decideDamage * enemy.attackTypeResistance[(int)skillData.attackType]) != 0)
            {
                decideDamage = (int)(decideDamage * enemy.attackTypeResistance[(int)skillData.attackType]);
            }
        }
        if (decideDamage != 0)
        {
            if ((int)(decideDamage * enemy.attackAttributeResistance[(int)skillData.attackAttributes]) != 0)
            {
                decideDamage = (int)(decideDamage * enemy.attackAttributeResistance[(int)skillData.attackAttributes]);
            }
        }
        return decideDamage;
    }

    int DecidePlayerDamage(int damage)
    {
        if (damage <= 0)
        {
            decideDamage = GetRandom(minDamage);
        }
        else
        {
            decideDamage = damage;
        }
        return decideDamage;
    }

    /// <summary>敵に与えるダメージの計算</summary>
    /// <param name="skillData">技データ</param>
    /// <param name="enemy">敵の情報</param>
    /// <param name="attacker">攻撃する側の能力</param>
    /// <param name="defender">攻撃される側の能力</param>
    /// <param name="critical">クリティカルかどうか</param>
    /// <returns>敵へのダメージ</returns>
    public int EnemyDamage(SkillData skillData, Enemy enemy, int attacker, int defender, bool critical)
    {
        int damage;
        if (critical)
        {
            damage = DecideEnemyDamage(skillData, CalculateCriticalDamage(skillData, attacker), enemy);
        }
        else
        {
            damage = DecideEnemyDamage(skillData, CalculateNormalDamage(skillData, defender, attacker), enemy);
        }
        return damage;
    }

    /// <summary>プレイヤーに与えるダメージの計算</summary>
    /// <param name="skillData">技データ</param>
    /// <param name="attacker">攻撃する側の能力</param>
    /// <param name="defender">攻撃される側の能力</param>
    /// <param name="critical">クリティカルかどうか</param>
    /// <returns>プレイヤーに与えるダメージ</returns>
    public int PlayerDamage(SkillData skillData, int attacker, int defender, bool critical)
    {
        int damage;
        if (critical)
        {
            damage = DecidePlayerDamage(CalculateCriticalDamage(skillData, attacker));
        }
        else
        {
            damage = DecidePlayerDamage(CalculateNormalDamage(skillData, defender, attacker));
        }
        return damage;
    }
}
