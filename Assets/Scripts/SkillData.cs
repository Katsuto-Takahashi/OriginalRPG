using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "SkillData")]
public class SkillData : ScriptableObject
{
    [SerializeField, Tooltip("スキルの名前")]
    string skillName = "";
    public string SkillName => skillName;
    [SerializeField, Tooltip("スキルの情報")]
    string skillInformation = "";
    public string SkillInformation => skillInformation;
    [SerializeField, Tooltip("スキルの威力"), Range(0, 1000)]
    int skillPower = 0;
    public int SkillPower => skillPower;
    [SerializeField, Tooltip("スキル倍率"), Range(0.0f, 2.0f)]
    float skillMagnification = 0.0f;
    public float SkillMagnification => skillMagnification;
    [SerializeField, Tooltip("必要AP"), Range(1, 1000)]
    int requiredAP = 1;
    public int RequiredAP => requiredAP;
    [SerializeField, Tooltip("攻撃の範囲"), Range(1, 15)]
    int attackRange = 1;
    public int AttackRange => attackRange;
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
