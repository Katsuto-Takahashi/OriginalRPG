using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    bool m_isDisplay = false;
    bool m_canDisplay = true;

    [SerializeField]
    SkillList skillsData = null;

    void Start()
    {
        for (int n = 0; n < skillsData.MagicSkills.Count; n++)
        {
            for (int i = 0; i < skillsData.MagicSkills[n].Efect.Count; i++)
            {
                skillsData.MagicSkills[n].Efect[i].Efect(skillsData.MagicSkills[n].SkillParameter);
            }
        }
    }

    void Update()
    {
        Display();
    }

    void Display()
    {
        if (m_canDisplay)
        {
            if (InputController.Instance.Menu())
            {
                m_isDisplay = !m_isDisplay;
            }
            UIManager.Instance.DisplayMenu(m_isDisplay);
        }
        else
        {
            UIManager.Instance.DisplayMenu(m_canDisplay);
        }
    }
}
