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
    List<ISkillEffectable> m_effects = new List<ISkillEffectable>();

    /// <summary>スキルデータ</summary>
    public SkillData SkillParameter => m_skillParameter;
    /// <summary>効果のList</summary>
    public List<ISkillEffectable> Effects => m_effects;
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

    [SerializeField]
    AnimationClip m_skillAnimationClip;
    /// <summary>スキルのanimationclip</summary>
    public AnimationClip SkillAnimationClip => m_skillAnimationClip;
}
public interface ISkillEffectable
{
    void Effect(GameObject attacker, GameObject defender, Skill skill);
}

//それぞれのスキルの効果
[System.Serializable]
public class Attack : ISkillEffectable
{
    public void Effect(GameObject attacker, GameObject defender, Skill skill)
    {
        Debug.Log($"{skill.SkillParameter.SkillName}は{ skill.SkillParameter.SkillInformation }");
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
        Debug.Log($"{skill.SkillParameter.SkillName}は{time}秒間拘束");

        BattleManager.Instance.StartCoroutine(BindCharacter(time));
    }

    public IEnumerator BindCharacter(float bindTime, Character character = null, Enemy enemy = null)
    {
        Debug.Log("止めます");
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
        Debug.Log($"{skill.SkillParameter.SkillName}は{timer}秒間に{time}秒間隔で{damage}ダメージ");
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
            Debug.Log($"{skill.SkillParameter.SkillName}は{ skill.SkillParameter.SkillInformation }");
        }
        else
        {
            Debug.Log($"{skill.SkillParameter.SkillName}は{ skill.SkillParameter.SkillInformation }");
        }
    }
}