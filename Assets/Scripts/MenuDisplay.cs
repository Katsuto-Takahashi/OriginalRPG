using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuDisplay : MonoBehaviour
{
    [SerializeField] GameObject m_menu = null;
    private bool m_menuDisplay = false;
    void Start()
    {
        m_menuDisplay = true;
        m_menu.SetActive(m_menuDisplay);
    }

    void Update()
    {
        if (Input.GetButtonDown("Optionsbutton"))
        {
            if (m_menuDisplay)
            {
                m_menuDisplay = false;
                m_menu.SetActive(m_menuDisplay);
            }
            else
            {
                m_menuDisplay = true;
                m_menu.SetActive(m_menuDisplay);
            }
        }
    }
}
