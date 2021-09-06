using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "SkillData")]
public class SkillData : ScriptableObject
{
    /// <summary>スキルの名前</summary>
    [SerializeField]
    private string skillName;
    public string SkillName
    {
        get { return skillName; }
        set { skillName = value; }
    }
    /// <summary>スキルの情報</summary>
    [SerializeField]
    private string skillInformation;
    public string SkillInformation
    {
        get { return skillInformation; }
        set { skillInformation = value; }
    }
    /// <summary>スキルの威力</summary>
    [SerializeField]
    [Range(0, 1000)] private int skillPower = default;
    public int SkillPower { get => skillPower; }
    /// <summary>スキル倍率</summary>
    [SerializeField]
    [Range(0.0f, 2.0f)] private float skillMagnification = default;
    public float SkillMagnification { get => skillMagnification; }
    /// <summary>必要AP</summary>
    [SerializeField]
    [Range(1,1000)] private int requiredAP = default;
    public int RequiredAP { get => requiredAP; }
    /// <summary>攻撃の範囲</summary>
    [SerializeField]
    [Range(1, 15)] private int attackRange = default;
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
    /// <summary>攻撃の属性</summary>
    public AttackAttributes attackAttributes;

    public enum AttackType
    {
        physicalAttack,
        magicAttack
    }
    /// <summary>攻撃のタイプ</summary>
    public AttackType attackType;
}
