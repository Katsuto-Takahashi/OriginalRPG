using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OldSkillData", menuName = "OldSkillData")]
public class OldSkillData : ScriptableObject
{
    [SerializeField]
    [Tooltip("スキルの名前")]
    string skillName = "";
    /// <summary>スキルの名前</summary>
    public string SkillName => skillName;

    [SerializeField]
    [Tooltip("スキルの情報")]
    string skillInformation = "";
    /// <summary>スキルの情報</summary>
    public string SkillInformation => skillInformation;

    [SerializeField]
    [Tooltip("スキルの威力")]
    [Range(0, 1000)]
    int skillPower = 0;
    /// <summary>スキルの威力</summary>
    public int SkillPower => skillPower;

    [SerializeField]
    [Tooltip("スキル倍率")]
    [Range(0.0f, 2.0f)]
    float skillMagnification = 0.0f;
    /// <summary>スキル倍率</summary>
    public float SkillMagnification => skillMagnification;

    [SerializeField]
    [Tooltip("必要AP")]
    [Range(1, 1000)]
    int requiredAP = 1;
    /// <summary>必要AP</summary>
    public int RequiredAP => requiredAP;

    [SerializeField]
    [Tooltip("攻撃の範囲")]
    [Range(1f, 15f)]
    float attackRange = 1.0f;
    /// <summary>攻撃の範囲</summary>
    public float AttackRange => attackRange;

    /// <summary>攻撃の属性</summary>
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

    /// <summary>攻撃のタイプ</summary>
    public enum AttackType
    {
        physicalAttack,
        magicAttack
    }
    [Tooltip("攻撃のタイプ")]
    public AttackType attackType;

    public enum SubEfect
    {
        マヒ,
        毒
    }
}
