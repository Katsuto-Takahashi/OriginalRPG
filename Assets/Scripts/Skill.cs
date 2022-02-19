using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class Skill : MonoBehaviour
{
    [SerializeField]
    SkillData m_skillData = null;

    void Start()
    {
    }

    void Update()
    {
    }

    public SkillData GetSkill()
    {
        return m_skillData;
    }

    void GotSkill()
    {
    }
}
