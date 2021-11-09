using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCalculator : MonoBehaviour
{
    private readonly float[] normalDamegeRandom = new float[] { 0.890f, 0.891f, 0.892f, 0.893f, 0.894f, 0.895f, 0.896f, 0.897f, 0.898f, 0.899f, 0.990f, 0.991f, 0.992f, 0.993f, 0.994f, 0.995f, 0.996f, 0.997f, 0.998f, 0.999f, 1.000f};
    private readonly float[] criticalDamegeRandam = new float[] { 0.95f, 0.96f, 0.97f, 0.98f, 0.99f, 1.00f, 1.01f, 1.02f, 1.03f, 1.04f, 1.05f};
    private readonly int[] minDamage = new int[] { 0, 1 };
    private int decideDamege;
    internal T GetRandom<T>(params T[] Params)
    {
        return Params[Random.Range(0, Params.Length)];
    }
    /// <summary>通常時のダメージ計算</summary>
    /// <param name="skillData">技データ</param>
    /// <param name="defender">受ける側の能力</param>
    /// <param name="attacker">与える側の能力</param>
    /// <returns>計算結果</returns>
    public int CalculateNormalDamage(SkillData skillData, int defender, int attacker)
    {
        float power = skillData.SkillPower / 100;
        int baseDamage = attacker / 4 - defender / 3;
        float normalDamege = baseDamage * GetRandom(normalDamegeRandom) * skillData.SkillMagnification * power;
        return (int)normalDamege;
    }
    /// <summary>クリティカル時のダメージ計算</summary>
    /// <param name="skillData">技データ</param>
    /// <param name="attacker">与える側の能力</param>
    /// <returns>計算結果</returns>
    public int CalculateCriticalDamage(SkillData skillData, int attacker)
    {
        float power = skillData.SkillPower / 100;
        float criticalDamege = attacker * GetRandom(criticalDamegeRandam) * skillData.SkillMagnification * power;
        return (int)criticalDamege;
    }
    /// <summary>キャラクターが敵に与えるダメージの決定</summary>
    /// <param name="skillData">技データ</param>
    /// <param name="damage">ダメージの計算結果</param>
    /// <param name="enemy">敵のパラメータ</param>
    /// <returns>キャラクターが敵に与えるダメージ</returns>
    public int DecideEnemyDamege(SkillData skillData, int damage, EnemyParameters enemy)
    {
        if (damage <= 0)
        {
            decideDamege = GetRandom(minDamage);
        }
        else
        {
            decideDamege = damage;
        }
        if (decideDamege != 0)
        {
            if ((int)(decideDamege * enemy.attackTypeResistance[(int)skillData.attackType]) != 0)
            {
                decideDamege = (int)(decideDamege * enemy.attackTypeResistance[(int)skillData.attackType]);
            }
        }
        if (decideDamege != 0)
        {
            if ((int)(decideDamege * enemy.attackAttributeResistance[(int)skillData.attackAttributes]) != 0)
            {
                decideDamege = (int)(decideDamege * enemy.attackAttributeResistance[(int)skillData.attackAttributes]);
            }
        }
        return decideDamege;
    }

    public int DecidePlayerDamege(int damage)
    {
        if (damage <= 0)
        {
            decideDamege = GetRandom(minDamage);
        }
        else
        {
            decideDamege = damage;
        }
        return decideDamege;
    }
}
