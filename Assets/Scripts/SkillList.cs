using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

[System.Serializable]
[CreateAssetMenu(fileName = "Skills", menuName = "Skills")]
public class SkillList : ScriptableObject
{
    [SerializeField]
    List<SkillDataTest> m_physicalSkills = new List<SkillDataTest>();
    [SerializeField]
    List<SkillDataTest> m_magicSkills = new List<SkillDataTest>();

    /// <summary>�����U���X�L��List</summary>
    public List<SkillDataTest> PhysicalSkills { get => m_physicalSkills; set => m_physicalSkills = value; }
    /// <summary>���@�U���X�L��List</summary>
    public List<SkillDataTest> MagicSkills { get => m_magicSkills; set => m_magicSkills = value; }
}

//�X�L�����
[System.Serializable]
public class SkillDataTest
{
    [SerializeField]
    Skill m_skillParameter = new Skill();
    [SerializeReference, SubclassSelector]
    List<ISkillEfectable> m_efect = new List<ISkillEfectable>();

    /// <summary>�X�L���f�[�^</summary>
    public Skill SkillParameter => m_skillParameter;
    /// <summary>���ʂ�List</summary>
    public List<ISkillEfectable> Efect => m_efect;
}

[System.Serializable]
public class Skill
{
    [SerializeField]
    [Tooltip("�X�L���̖��O")]
    string m_skillName = "";
    /// <summary>�X�L���̖��O</summary>
    public string SkillName => m_skillName;

    [SerializeField]
    [Tooltip("�X�L���̏��")]
    string m_skillInformation = "";
    /// <summary>�X�L���̏��</summary>
    public string SkillInformation => m_skillInformation;

    [SerializeField]
    [Tooltip("�X�L���̈З�")]
    [Range(0, 1000)]
    int m_skillPower = 0;
    /// <summary>�X�L���̈З�</summary>
    public int SkillPower => m_skillPower;

    [SerializeField]
    [Tooltip("�X�L���{��")]
    [Range(0.0f, 2.0f)]
    float m_skillMagnification = 0.0f;
    /// <summary>�X�L���{��</summary>
    public float SkillMagnification => m_skillMagnification;

    [SerializeField]
    [Tooltip("�K�vAP")]
    [Range(1, 1000)]
    int m_requiredAP = 1;
    /// <summary>�K�vAP</summary>
    public int RequiredAP => m_requiredAP;

    [SerializeField]
    [Tooltip("�X�L���͈̔�")]
    [Range(1.0f, 15.0f)]
    float m_skillRange = 1.0f;
    /// <summary>�X�L���͈̔�</summary>
    public float SkillRange => m_skillRange;

    [SerializeField]
    [Tooltip("���̎g�p�\�܂ł̎���")]
    [Range(0.0f, 60.0f)]
    float m_coolTime = 0.0f;
    /// <summary>���̎g�p�\�܂ł̎���</summary>
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
    [Tooltip("�X�L���̑���")]
    [SerializeField]
    private SkillAttributes m_skillAttributes;
    /// <summary>�X�L���̑���</summary>
    public SkillAttributes Attributes => m_skillAttributes;

    public enum SkillType
    {
        physicalAttack,
        magicAttack
    }
    [Tooltip("�X�L���̃^�C�v")]
    [SerializeField]
    private SkillType m_skillType;
    /// <summary>�X�L���̃^�C�v</summary>
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
    [Tooltip("�X�L���̑Ώ�")]
    [SerializeField]
    private SkillTarget m_skillTarget;
    /// <summary>�X�L���̑Ώ�</summary>
    public SkillTarget Target => m_skillTarget;
}
public interface ISkillEfectable
{
    void Efect(Skill skill);
}

//���ꂼ��̃X�L���̌���
[System.Serializable]
public class Attack : ISkillEfectable
{
    public void Efect(Skill skill)
    {
        Debug.Log($"{skill.SkillName}��{ skill.SkillInformation }");
    }
}

public class Bind : ISkillEfectable
{
    [SerializeField]
    float time = 0.0f;

    public void Efect(Skill skill)
    {
        Debug.Log($"{skill.SkillName}��{time}�b�ԍS��");
    }
}

public class ContinuationDamage : ISkillEfectable
{
    [SerializeField]
    float time = 0.0f;
    [SerializeField]
    int damage = 0;

    public void Efect(Skill skill)
    {
        Debug.Log($"{skill.SkillName}��{time}�b��{damage}�_���[�W");
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

    public void Efect(Skill skill)
    {
        if (healPoint == HealPoint.HP)
        {
            Debug.Log($"{skill.SkillName}��{ skill.SkillInformation }");
        }
        else
        {
            Debug.Log($"{skill.SkillName}��{ skill.SkillInformation }");
        }
    }
}