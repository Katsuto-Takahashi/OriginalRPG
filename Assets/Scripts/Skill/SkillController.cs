using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController
{
    List<Skill> m_SkillList;
    List<CoolTimeChecker> m_CoolTimeCheckerList;

    public SkillController()
    {
        m_SkillList = new List<Skill>();
        m_CoolTimeCheckerList = new List<CoolTimeChecker>();
    }

    public void SetSkill(Skill skill, int id)
    {
        m_SkillList.Add(skill);
        m_CoolTimeCheckerList.Add(new CoolTimeChecker());
        m_CoolTimeCheckerList[id].SkillCoolTimerSet(skill);
    }

    public IEnumerator UseSkill(int id)
    {
        return m_CoolTimeCheckerList[id].Timer();
    }

    public bool CanUse(int id)
    {
        return m_CoolTimeCheckerList[id].CanUse;
    }
}
