using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    bool m_isDisplay = false;
    bool m_canDisplay = true;

    [SerializeField]
    SkillList skillsData = null;

    Character m_player;
    public Character Player => m_player;

    protected override void Awake()
    {
        base.Awake();
        m_player = PartyManager.Instance.CharacterParty[0];
    }

    void Start()
    {
        
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
