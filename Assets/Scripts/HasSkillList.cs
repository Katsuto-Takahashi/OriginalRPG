using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasSkillList : MonoBehaviour
{
    [SerializeField, Tooltip("通常攻撃")]
    List<OldSkillData> m_normalSkill = new List<OldSkillData>();
    [SerializeField, Tooltip("スキル")]
    List<OldSkillData> m_skillDatas = new List<OldSkillData>();
    [SerializeField, Tooltip("魔法")]
    List<OldSkillData> m_magicDatas = new List<OldSkillData>();

    public List<OldSkillData> NormalSkill 
    {
        get { return m_normalSkill; }
        set { m_normalSkill = value; }
    }
    public List<OldSkillData> SkillDatas 
    {
        get { return m_skillDatas; }
        set { m_skillDatas = value; }
    }
    public List<OldSkillData> MagicDatas
    {
        get { return m_magicDatas; }
        set { m_magicDatas = value; }
    }
}
