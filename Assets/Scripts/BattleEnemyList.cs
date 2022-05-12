using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleEnemyList : MonoBehaviour
{
    [SerializeField] List<GameObject> m_target = new List<GameObject>();
    List<GameObject> m_battleEnemys = new List<GameObject>();
    bool target = false;
    
    void Start()
    {
        CreateTarget();
    }
    void Update()
    {
        if (target)
        {
            CreateTarget();
            target = false;
        }
    }
    public void SetEnemy(List<Image> images)
    {
        Set(images);
    }
    void Set(List<Image> images)
    {
        images.Clear();
        for (int i = 0; i < m_battleEnemys.Count; i++)
        {
            images.Add(m_target[i].GetComponent<Image>());
        }
    }
    void CreateTarget()
    {
        for (int i = 0; i < m_battleEnemys.Count; i++)
        {
            var ui = m_target[i].GetComponent<CharacterParameterUI>();
            var enemyName = m_battleEnemys[i].GetComponent<Enemy>().Name.Value;
            if (m_battleEnemys.Count > 1)
            {
                ui.CreateName($"{enemyName}{i + 1}");
            }
            else
            {
                ui.CreateName(enemyName);
            }
            m_target[i].SetActive(true);
        }
    }
    public void ChengeBool()
    {
        target = true;
    }
    public void AddEnemyList(GameObject enemy)
    {
        m_battleEnemys.Add(enemy);
    }
    public void ClearEnemyList()
    {
        Clear();
    }

    void Clear()
    {
        for (int i = 0; i < m_target.Count; i++)
        {
            m_target[i].SetActive(false);
        }
        m_battleEnemys.Clear();
    }
}
