using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

[System.Serializable]
[CreateAssetMenu(fileName = "Skills", menuName = "Skills")]
public class SkillList : ScriptableObject
{
    [SerializeField]
    List<Skill> m_skills = new List<Skill>();

    /// <summary>�X�L��List</summary>
    public List<Skill> Skills => m_skills;
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
    /// <summary>���ʔ���</summary>
    public void PlayEffect(GameObject user, GameObject target, Skill skill)
    {
        user.GetComponentInChildren<SkillEffectController>().SetSkill(skill, user, target);
    }
}

public enum SkillAttributes
{
    Non,
    Fire,
    Water,
    Thunder,
    Ground,
    Wind,
    Plant,
    Dark,
    Light
}

public enum SkillType
{
    Physical,
    Magical
}

public enum SkillTarget
{
    All,
    OneEnemy,
    EnemyOnly,
    OneAlly,
    AllyOnly,
    Myself
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
    int m_skillPower = 100;
    /// <summary>�X�L���̈З�</summary>
    public int SkillPower => m_skillPower;

    [SerializeField]
    [Tooltip("�X�L���{��")]
    [Range(0.0f, 2.0f)]
    float m_skillMagnification = 1.0f;
    /// <summary>�X�L���{��</summary>
    public float SkillMagnification => m_skillMagnification;

    [SerializeField]
    [Tooltip("�K�vAP")]
    [Range(1, 1000)]
    int m_requiredAP = 1;
    /// <summary>�K�vAP</summary>
    public int RequiredAP => m_requiredAP;

    [SerializeField]
    [Tooltip("�X�L���̎˒�")]
    [Range(1.0f, 15.0f)]
    float m_skillRange = 1.0f;
    /// <summary>�X�L���̎˒�</summary>
    public float SkillRange => m_skillRange;
    
    [SerializeField]
    [Tooltip("���ʂ͈̔�")]
    [Range(0.5f, 15.0f)]
    float m_effectRange = 1.0f;
    /// <summary>���ʂ͈̔�</summary>
    public float EffectRange => m_effectRange;

    [SerializeField]
    [Tooltip("���̎g�p�\�܂ł̎���")]
    [Range(0.0f, 60.0f)]
    float m_coolTime = 0.0f;
    /// <summary>���̎g�p�\�܂ł̎���</summary>
    public float CoolTime => m_coolTime;

    [Tooltip("�X�L���̑���")]
    [SerializeField]
    private SkillAttributes m_skillAttributes;
    /// <summary>�X�L���̑���</summary>
    public SkillAttributes Attributes => m_skillAttributes;

    [Tooltip("�X�L���̃^�C�v")]
    [SerializeField]
    private SkillType m_skillType;
    /// <summary>�X�L���̃^�C�v</summary>
    public SkillType Type => m_skillType;

    [Tooltip("�X�L���̑Ώ�")]
    [SerializeField]
    private SkillTarget m_skillTarget;
    /// <summary>�X�L���̑Ώ�</summary>
    public SkillTarget Target => m_skillTarget;

    [SerializeField]
    GameObject m_skillEffect;
    /// <summary>�X�L����animationclip</summary>
    public GameObject SkillEffect => m_skillEffect;

    [SerializeField]
    GameObject m_skillFinishEffect;
    /// <summary>�X�L����ParticleSystem</summary>
    public GameObject SkillFinishEffect => m_skillFinishEffect;
}
public interface ISkillEffectable
{
    void Effect(GameObject user, GameObject target, Skill skill);
}

public class Attack : ISkillEffectable
{
    public void Effect(GameObject user, GameObject target, Skill skill)
    {
        Debug.Log($"{skill.SkillParameter.SkillName}��{skill.SkillParameter.SkillInformation}");
        target.GetComponent<ITakableDamage>().TakeDamage(BattleManager.Instance.Damage(user, target, skill.SkillParameter));
    }
}

public class Bind : ISkillEffectable
{
    [SerializeField]
    float time = 0.0f;

    public void Effect(GameObject user, GameObject target, Skill skill)
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
    float maxTime = 0.0f;
    [SerializeField]
    float delayTime = 0.0f;
    [SerializeField]
    int damage = 0;

    public void Effect(GameObject user, GameObject target, Skill skill)
    {
        Debug.Log($"{skill.SkillParameter.SkillName}��{maxTime}�b�Ԃ�{delayTime}�b�Ԋu��{damage}�_���[�W");
        BattleManager.Instance.EffectCoroutine(Continuation(user, target, skill.SkillParameter));
    }

    public IEnumerator Continuation(GameObject user, GameObject target, SkillData skilldata)
    {
        var damage = target.GetComponent<ITakableDamage>();
        float count = maxTime;
        float delay = delayTime;
        while (count >= 0.0f)
        {
            count -= Time.deltaTime;
            delay -= Time.deltaTime;
            if (delay <= 0.0f)
            {
                damage.TakeDamage(BattleManager.Instance.Damage(user, target, skilldata));
                delay += delayTime;
            }
            yield return null;
        }
    }
}

public class Heal : ISkillEffectable
{
    [SerializeField]
    int value = 0;
    enum HealPoint
    {
        HP,
        AP
    }
    [SerializeField]
    HealPoint healPoint = HealPoint.HP;

    public void Effect(GameObject user, GameObject target, Skill skill)
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