using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [SerializeField]
    GameObject m_menu = null;
    bool m_menuDisplay = false;
    bool m_canDisplay = true;

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
                m_menuDisplay = !m_menuDisplay;
            }
            m_menu.SetActive(m_menuDisplay);
        }
        else
        {
            m_menu.SetActive(false);
        }
    }
}
