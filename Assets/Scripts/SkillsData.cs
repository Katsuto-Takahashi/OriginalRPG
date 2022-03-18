using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Skills", menuName = "Skills")]
public class SkillsData : ScriptableObject
{
    [SerializeField]
    List<SkillDataTest> m_physicalSkills = new List<SkillDataTest>();
    [SerializeField]
    List<SkillDataTest> m_magicSkills = new List<SkillDataTest>();

    public List<SkillDataTest> PhysicalSkills { get => m_physicalSkills; set => m_physicalSkills = value; }
    public List<SkillDataTest> MagicSkills { get => m_magicSkills; set => m_magicSkills = value; }
}

//スキル情報
[System.Serializable]
public class SkillDataTest
{
    [SerializeField]
    Skill m_skillParameter = new Skill();
    [SerializeReference, SubclassSelector]
    List<SkillEfect> m_efect = new List<SkillEfect>();

    public Skill SkillParameter => m_skillParameter;
    public List<SkillEfect> Efect => m_efect;
}

[System.Serializable]
public class Skill
{
    [SerializeField]
    [Tooltip("スキルの名前")]
    string m_skillName = "";
    /// <summary>スキルの名前</summary>
    public string SkillName => m_skillName;

    [SerializeField]
    [Tooltip("スキルの情報")]
    string m_skillInformation = "";
    /// <summary>スキルの情報</summary>
    public string SkillInformation => m_skillInformation;

    [SerializeField]
    [Tooltip("スキルの威力")]
    [Range(0, 1000)]
    int m_skillPower = 0;
    /// <summary>スキルの威力</summary>
    public int SkillPower => m_skillPower;

    [SerializeField]
    [Tooltip("スキル倍率")]
    [Range(0.0f, 2.0f)]
    float m_skillMagnification = 0.0f;
    /// <summary>スキル倍率</summary>
    public float SkillMagnification => m_skillMagnification;

    [SerializeField]
    [Tooltip("必要AP")]
    [Range(1, 1000)]
    int m_requiredAP = 1;
    /// <summary>必要AP</summary>
    public int RequiredAP => m_requiredAP;

    [SerializeField]
    [Tooltip("攻撃の範囲")]
    [Range(1.0f, 15.0f)]
    float m_attackRange = 1.0f;
    /// <summary>攻撃の範囲</summary>
    public float AttackRange => m_attackRange;

    [SerializeField]
    [Tooltip("次に攻撃できるまでの時間")]
    [Range(0.0f, 60.0f)]
    float m_coolTime = 0.0f;
    /// <summary>次に攻撃できるまでの時間</summary>
    public float CoolTime => m_coolTime;

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
    public AttackAttributes m_attackAttributes;

    /// <summary>攻撃のタイプ</summary>
    public enum AttackType
    {
        physicalAttack,
        magicAttack
    }
    [Tooltip("攻撃のタイプ")]
    public AttackType m_attackType;
}
public interface SkillEfect
{
    void Efect(Skill skill);
}

//それぞれのスキルの効果
[System.Serializable]
public class Attack : SkillEfect
{
    [SerializeField]
    float time = 0.0f;

    public void Efect(Skill skill)
    {
        throw new System.NotImplementedException();
    }
}