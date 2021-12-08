using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasSkillList : MonoBehaviour
{
    [SerializeField, Tooltip("通常攻撃")]
    List<SkillData> m_normalSkill = new List<SkillData>();
    [SerializeField, Tooltip("スキル")]
    List<SkillData> m_skillDatas = new List<SkillData>();
    [SerializeField, Tooltip("魔法")]
    List<SkillData> m_magicDatas = new List<SkillData>();

    public List<SkillData> NormalSkill 
    {
        get { return m_normalSkill; }
        set { m_normalSkill = value; }
    }
    public List<SkillData> SkillDatas 
    {
        get { return m_skillDatas; }
        set { m_skillDatas = value; }
    }
    public List<SkillData> MagicDatas
    {
        get { return m_magicDatas; }
        set { m_magicDatas = value; }
    }
}
