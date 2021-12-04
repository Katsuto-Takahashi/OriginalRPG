using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasSkillList : MonoBehaviour
{
    [SerializeField, Tooltip("通常攻撃")]
    private List<SkillData> m_normalSkill = new List<SkillData>();
    [SerializeField, Tooltip("スキル")]
    private List<SkillData> m_skillDatas = new List<SkillData>();

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
}
