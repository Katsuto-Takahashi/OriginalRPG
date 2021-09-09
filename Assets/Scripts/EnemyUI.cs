using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    private RectTransform m_rectTransform;
    private Text m_text;
    private EnemyManager m_enemyManager;

    void Start()
    {
        m_rectTransform = GetComponent<RectTransform>();
        m_text = GetComponentInChildren<Text>();
        m_enemyManager = GetComponentInParent<EnemyManager>();
        m_text.text = m_enemyManager.enemyParameters.EnemyCharacterName;
    }

    void Update()
    {
        m_rectTransform.LookAt(Camera.main.transform);
        if (m_enemyManager.HP / (float)m_enemyManager.enemyParameters.MaxHP < 0.7f)
        {
            if (m_enemyManager.HP / (float)m_enemyManager.enemyParameters.MaxHP < 0.35f)
            {
                m_text.color = new Color(255, 0, 0);
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
}
