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

//�X�L�����
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
    [Tooltip("�U���͈̔�")]
    [Range(1.0f, 15.0f)]
    float m_attackRange = 1.0f;
    /// <summary>�U���͈̔�</summary>
    public float AttackRange => m_attackRange;

    [SerializeField]
    [Tooltip("���ɍU���ł���܂ł̎���")]
    [Range(0.0f, 60.0f)]
    float m_coolTime = 0.0f;
    /// <summary>���ɍU���ł���܂ł̎���</summary>
    public float CoolTime => m_coolTime;

    /// <summary>�U���̑���</summary>
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
    [Tooltip("�U���̑���")]
    public AttackAttributes m_attackAttributes;

    /// <summary>�U���̃^�C�v</summary>
    public enum AttackType
    {
        physicalAttack,
        magicAttack
    }
    [Tooltip("�U���̃^�C�v")]
    public AttackType m_attackType;
}
public interface SkillEfect
{
    void Efect(Skill skill);
}

//���ꂼ��̃X�L���̌���
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