using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "SkillData")]
public class SkillData : ScriptableObject
{
    [SerializeField, Tooltip("スキルの名前")]
    string skillName = "";
    public string SkillName { get => skillName; }
    [SerializeField, Tooltip("スキルの情報")]
    string skillInformation = "";
    public string SkillInformation { get => skillInformation; }
    /// <summary>スキルの威力</summary>
    [SerializeField, Tooltip("スキルの威力"), Range(0, 1000)]
    int skillPower = 0;
    public int SkillPower { get => skillPower; }
    /// <summary>スキル倍率</summary>
    [SerializeField, Tooltip("スキル倍率"), Range(0.0f, 2.0f)]
    float skillMagnification = 0.0f;
    public float SkillMagnification { get => skillMagnification; }
    /// <summary>必要AP</summary>
    [SerializeField, Tooltip("必要AP"), Range(1, 1000)]
    int requiredAP = 1;
    public int RequiredAP { get => requiredAP; }
    /// <summary>攻撃の範囲</summary>
    [SerializeField, Tooltip("攻撃の範囲"), Range(1, 15)]
    int attackRange = 1;
    public int AttackRange { get => attackRange; }
    public enum AttackAttributes
    {
        non,
        fire,
        water,
        thunder,
        ground,
        wind,
        plant,
        dark,
        light
    }
    [Tooltip("攻撃の属性")]
    public AttackAttributes attackAttributes;

    public enum AttackType
    {
        physicalAttack,
        magicAttack
    }
    [Tooltip("攻撃のタイプ")]
    public AttackType attackType;
}
