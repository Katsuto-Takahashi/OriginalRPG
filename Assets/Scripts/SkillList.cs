using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

[System.Serializable]
[CreateAssetMenu(fileName = "Skills", menuName = "Skills")]
public class SkillList : ScriptableObject
{
    [SerializeField]
    List<Skill> m_physicalSkills = new List<Skill>();
    [SerializeField]
    List<Skill> m_magicSkills = new List<Skill>();

    /// <summary>物理攻撃スキルList</summary>
    public List<Skill> PhysicalSkills => m_physicalSkills;
    /// <summary>魔法攻撃スキルList</summary>
    public List<Skill> MagicSkills => m_magicSkills;
}

//スキル情報
[System.Serializable]
public class Skill
{
    [SerializeField]
    SkillData m_skillParameter = new SkillData();
    [SerializeReference, SubclassSelector]
    List<ISkillEfectable> m_efect = new List<ISkillEfectable>();

    /// <summary>スキルデータ</summary>
    public SkillData SkillParameter => m_skillParameter;
    /// <summary>効果のList</summary>
    public List<ISkillEfectable> Efect => m_efect;
}

[System.Serializable]
public class SkillData
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
    [Tooltip("スキルの範囲")]
    [Range(1.0f, 15.0f)]
    float m_skillRange = 1.0f;
    /// <summary>スキルの範囲</summary>
    public float SkillRange => m_skillRange;

    [SerializeField]
    [Tooltip("次の使用可能までの時間")]
    [Range(0.0f, 60.0f)]
    float m_coolTime = 0.0f;
    /// <summary>次の使用可能までの時間</summary>
    public float CoolTime => m_coolTime;

    public enum SkillAttributes
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
    [Tooltip("スキルの属性")]
    [SerializeField]
    private SkillAttributes m_skillAttributes;
    /// <summary>スキルの属性</summary>
    public SkillAttributes Attributes => m_skillAttributes;

    public enum SkillType
    {
        physicalAttack,
        magicAttack
    }
    [Tooltip("スキルのタイプ")]
    [SerializeField]
    private SkillType m_skillType;
    /// <summary>スキルのタイプ</summary>
    public SkillType Type => m_skillType;

    public enum SkillTarget
    {
        all,
        oneEnemy,
        enemyOnly,
        oneAlly,
        allyOnly,
        myself
    }
    [Tooltip("スキルの対象")]
    [SerializeField]
    private SkillTarget m_skillTarget;
    /// <summary>スキルの対象</summary>
    public SkillTarget Target => m_skillTarget;
}
public interface ISkillEfectable
{
    void Efect(SkillData skill);
}

//それぞれのスキルの効果
[System.Serializable]
public class Attack : ISkillEfectable
{
    public void Efect(SkillData skill)
    {
        Debug.Log($"{skill.SkillName}は{ skill.SkillInformation }");
    }
}

public class Bind : ISkillEfectable
{
    [SerializeField]
    float time = 0.0f;

    public void Efect(SkillData skill)
    {
        Debug.Log($"{skill.SkillName}は{time}秒間拘束");
    }
}

public class ContinuationDamage : ISkillEfectable
{
    [SerializeField]
    float time = 0.0f;
    [SerializeField]
    int damage = 0;

    public void Efect(SkillData skill)
    {
        Debug.Log($"{skill.SkillName}は{time}秒間{damage}ダメージ");
    }
}

public class Heal : ISkillEfectable
{
    [SerializeField]
    int value = 0;
    public enum HealPoint
    {
        HP,
        AP
    }
    [SerializeField]
    HealPoint healPoint = HealPoint.HP;

    public void Efect(SkillData skill)
    {
        if (healPoint == HealPoint.HP)
        {
            Debug.Log($"{skill.SkillName}は{ skill.SkillInformation }");
        }
        else
        {
            Debug.Log($"{skill.SkillName}は{ skill.SkillInformation }");
        }
    }
}