using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class SkillPanel : MonoBehaviour
{
    Character m_character;

    List<Skill> skills = new List<Skill>();

    void Start()
    {
        m_character = GetComponent<Character>();
    }

    void Update()
    {
    }

    void SkillUpdate()
    {
        
    }

    void GetSkill(int index)
    {
        m_character.GetSkill(skills[index].GetSkill());
    }

    void GotSkill()
    {
    }
}