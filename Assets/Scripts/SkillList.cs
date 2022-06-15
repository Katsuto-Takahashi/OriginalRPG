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

    /// <summary>�����U���X�L��List</summary>
    public List<Skill> PhysicalSkills => m_physicalSkills;
    /// <summary>���@�U���X�L��List</summary>
    public List<Skill> MagicSkills => m_magicSkills;
}

//�X�L�����
[System.Serializable]
public class Skill
{
    [SerializeField]
    SkillData m_skillParameter = new SkillData();
    [SerializeReference, SubclassSelector]
    List<ISkillEffectable> m_effects = new List<ISkillEffectable>();

    /// <summary>�X�L���f�[�^</summary>
    public SkillData SkillParameter => m_skillParameter;
    /// <summary>���ʂ�List</summary>
    public List<ISkillEffectable> Effects => m_effects;
}

[System.Serializable]
public class SkillData
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

    [SerializeField]
    AnimationClip m_skillAnimationClip;
    /// <summary>�X�L����animationclip</summary>
    public AnimationClip SkillAnimationClip => m_skillAnimationClip;
}
public interface ISkillEffectable
{
    void Effect(GameObject attacker, GameObject defender, Skill skill);
}

//���ꂼ��̃X�L���̌���
[System.Serializable]
public class Attack : ISkillEffectable
{
    public void Effect(GameObject attacker, GameObject defender, Skill skill)
    {
        Debug.Log($"{skill.SkillParameter.SkillName}��{ skill.SkillParameter.SkillInformation }");
        //int d = BattleManager.Instance.Damage(attacker, defender, skill.SkillParameter);
        attacker.GetComponentInChildren<AttackChecker>().SetAttackParam(attacker, skill, defender.layer);
    }
}

public class Bind : ISkillEffectable
{
    [SerializeField]
    float time = 0.0f;

    public void Effect(GameObject attacker, GameObject defender, Skill skill)
    {
        Debug.Log($"{skill.SkillParameter.SkillName}��{time}�b�ԍS��");

        BattleManager.Instance.StartCoroutine(BindCharacter(time));
    }

    public IEnumerator BindCharacter(float bindTime, Character character = null, Enemy enemy = null)
    {
        Debug.Log("�~�߂܂�");
        if (character != null) character.BCSM.IsBind = true;
        else if (enemy != null) enemy.BESM.IsBind = true;

        yield return new WaitForSeconds(bindTime);

        if (character != null) character.BCSM.IsBind = false;
        else if (enemy != null) enemy.BESM.IsBind = false;
    }
}

public class ContinuationDamage : ISkillEffectable
{
    [SerializeField]
    float timer = 0.0f;
    [SerializeField]
    float time = 0.0f;
    [SerializeField]
    int damage = 0;

    public void Effect(GameObject attacker, GameObject defender, Skill skill)
    {
        Debug.Log($"{skill.SkillParameter.SkillName}��{timer}�b�Ԃ�{time}�b�Ԋu��{damage}�_���[�W");
    }

    public IEnumerator Continuation()
    {
        while (timer < 0.0f)
        {
            yield return null;
        }
    }
}

public class Heal : ISkillEffectable
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

    public void Effect(GameObject attacker, GameObject defender, Skill skill)
    {
        if (healPoint == HealPoint.HP)
        {
            Debug.Log($"{skill.SkillParameter.SkillName}��{ skill.SkillParameter.SkillInformation }");
        }
        else
        {
            Debug.Log($"{skill.SkillParameter.SkillName}��{ skill.SkillParameter.SkillInformation }");
        }
    }
}