using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    //[SerializeField] DamageCalculator damageCalculator = null;
    [SerializeField] ContactEnemy contactEnemy = null;
    [SerializeField] EnemyList m_enemyPrefabs = null;
    [SerializeField] PartyManager m_partyManager = null;
    private GameObject m_gameObject;
    private List<GameObject> m_enemyParty = new List<GameObject>();
    private List<bool> m_enemyDeadList = new List<bool>();
    private List<bool> m_characterDeadList = new List<bool>();
    private bool m_isCreated = false;
    private bool m_isChangeState = false;
    private int characterDeadCount = 0;
    private int enemyDeadCount = 0;

    void Update()
    {
        if (contactEnemy.m_isBattle)
        {
            if (contactEnemy.m_isContact && !m_isCreated)
            {
                BattleStanby();
            }
            if (!m_isChangeState)
            {
                for (int i = 0; i < m_partyManager.m_charaParty.Count; i++)
                {
                    m_partyManager.m_charaParty[i].GetComponent<BattleStateMachine>().m_battle = true;
                }
                for (int i = 0; i < m_enemyParty.Count; i++)
                {
                    m_enemyParty[i].GetComponent<BattleStateMachine>().m_battle = true;
                }
                m_isChangeState = true;
            }
            if (WinnerChack() > 1)
            {
                //win
                //contactEnemy.m_isBattle = false;
            }
            else if (WinnerChack() > 0)
            {
                //lose
                //contactEnemy.m_isBattle = false;
            }
            else
            {
                return;
            }
        }
        else
        {
            m_isChangeState = false;
            contactEnemy.DeleteField();
        }
    }
    void CreateEnemy(int num)
    {
        for (int i = 0; i < num; i++)
        {
            m_gameObject = Instantiate(m_enemyPrefabs.m_enemyList[contactEnemy.m_enemyID - 1], contactEnemy.m_contactPosition, Quaternion.identity);
            m_enemyParty.Add(m_gameObject);
            m_enemyParty[i].name = m_enemyParty[i].GetComponent<EnemyManager>().enemyParameters.EnemyCharacterName + $"{i}";
            m_enemyDeadList.Add(m_enemyParty[i].GetComponent<EnemyManager>().IsDeadState);
            m_enemyParty[i].GetComponent<BattleStateMachine>().m_actionTimer = Timer(m_enemyParty[i].GetComponent<EnemyManager>().enemyParameters.Speed);
        }
        m_isCreated = true;
    }
    void BattleStanby()
    {
        if (contactEnemy.m_enemyParty == 0)
        {
            CreateEnemy(1);
        }
        for (int i = 0; i < m_partyManager.m_charaParty.Count; i++)
        {
            m_characterDeadList.Add(m_partyManager.m_charaParty[i].GetComponent<CharacterParameterManager>().IsDeadState);
            m_partyManager.m_charaParty[i].GetComponent<BattleStateMachine>().m_actionTimer = Timer(m_partyManager.m_charaParty[i].GetComponent<CharacterParameterManager>().Speed);
        }
        //敵がボスの時はにげれないようにする
        //if (true)
        //{
        //    contactEnemy.m_isContact = false;
        //}
    }
    float Timer(int speed)
    {
        return (100 - speed) / 10f;
    }
    int WinnerChack()
    {
        characterDeadCount = 0;
        enemyDeadCount = 0;
        for (int i = 0; i < m_partyManager.m_charaParty.Count; i++)
        {
            if (m_characterDeadList[i] == true)
            {
                characterDeadCount++; 
            }
        }
        for (int i = 0; i < m_enemyParty.Count; i++)
        {
            if (m_enemyDeadList[i] == true)
            {
                enemyDeadCount++;
            }
        }
        if (characterDeadCount == m_partyManager.m_charaParty.Count)
        {
            return 1;
        }
        else if (enemyDeadCount == m_enemyParty.Count)
        {
            return 2;
        }
        else
        {
            return 0;
        }
    }
}
