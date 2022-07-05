using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolTimeChecker
{
    float m_currentTimer = 0.0f;
    public float CurrentTimer => m_currentTimer;

    bool m_canUse = true;
    public bool CanUse
    {
        get => m_canUse;
        set
        {
            if (m_currentTimer > 0.0f)
            {
                m_canUse = false;
            }
            else
            {
                m_canUse = true;
            }
        }
    }

    public void SkillCoolTimerSet(Skill skill)
    {
        m_currentTimer = skill.SkillParameter.CoolTime;
    }

    public IEnumerator Timer(Skill skill)
    {
        Debug.Log("CoolTimeräJén");
        while (m_currentTimer > 0.0f)
        {
            m_currentTimer -= Time.deltaTime;
            yield return null;
        }
        Debug.Log("CoolTimerèIóπ");
    }
}