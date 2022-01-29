using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    private RectTransform m_rectTransform;
    private Text m_text;
    private Enemy m_enemyManager;

    void Start()
    {
        m_rectTransform = GetComponent<RectTransform>();
        m_text = GetComponentInChildren<Text>();
        m_enemyManager = GetComponentInParent<Enemy>();
        if (m_text.text == "")
        {
            m_text.text = m_enemyManager.Name.Value;
        }
    }

    void Update()
    {
        m_rectTransform.LookAt(Camera.main.transform);
        ChangeColor();
    }

    void ChangeColor()
    {
        if (m_enemyManager.HP.Value / (float)m_enemyManager.MaxHP.Value < 0.7f)
        {
            if (m_enemyManager.HP.Value / (float)m_enemyManager.MaxHP.Value < 0.35f)
            {
                if (m_enemyManager.HP.Value <= 0)
                {
                    m_text.color = new Color(0, 0, 0);
                }
                else
                {
                    m_text.color = new Color(255, 0, 0);
                }
            }
            else
            {
                m_text.color = new Color(255, 255, 0);
            }
        }
        else
        {
            m_text.color = new Color(0, 255, 0);
        }
    }

    public void ChangeName(string enemyName)
    {
        SetName(enemyName);
    }

    void SetName(string enemyName)
    {
        if (m_text == null)
        {
            m_text = GetComponentInChildren<Text>();
        }
        m_text.text = "";
        m_text.text = $"{enemyName}";
    }
}
