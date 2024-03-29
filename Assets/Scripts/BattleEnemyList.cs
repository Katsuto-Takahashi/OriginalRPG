﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleEnemyList : MonoBehaviour
{
    [SerializeField] List<GameObject> m_target = new List<GameObject>();
    List<Enemy> m_battleEnemys = new List<Enemy>();
    
    //void Start()
    //{
    //    CreateTarget();
    //}

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

    public void Create()
    {
        CreateTarget();
    }

    void CreateTarget()
    {
        for (int i = 0; i < m_battleEnemys.Count; i++)
        {
            var ui = m_target[i].GetComponent<CharacterParameterUI>();
            var enemyName = m_battleEnemys[i].Name.Value;
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

    public void AddEnemyList(Enemy enemy)
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
