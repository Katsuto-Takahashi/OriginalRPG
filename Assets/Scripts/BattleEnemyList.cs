using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleEnemyList : MonoBehaviour
{
    [SerializeField] List<GameObject> m_target = new List<GameObject>();
    public List<GameObject> m_battleEnemys = new List<GameObject>();
    void Start()
    {
        for (int i = 0; i < m_battleEnemys.Count; i++)
        {
            m_target[i].GetComponentInChildren<CharacterParameterUI>().CreateName(m_battleEnemys[i].GetComponent<EnemyManager>().enemyParameters.EnemyCharacterName);
            m_target[i].SetActive(true);
        }
    }

    void Update()
    {
        
    }
}
