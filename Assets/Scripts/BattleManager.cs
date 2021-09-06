using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] DamageCalculator damageCalculator;
    [SerializeField] ContactEnemy contactEnemy;
    [SerializeField] EnemyList m_enemyPrefabs;
    [SerializeField] PartyManager m_partyManager;
    private GameObject m_gameObject;
    private List<GameObject> m_enemyParty = new List<GameObject>();
    public List<bool> m_enemyDeadList = new List<bool>();
    public List<bool> m_characterDeadList = new List<bool>();
    private bool m_isCreated = false;
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
            if (m_isCreated)
            {
                for (int i = 0; i < m_partyManager.m_charaParty.Count; i++)
                {
                    m_partyManager.m_charaParty[i].GetComponent<BattleStateMachine>().m_actionTimer = Timer(m_partyManager.m_charaParty[i].GetComponent<CharacterParameterManager>().Speed);
                }
                for (int i = 0; i < m_enemyParty.Count; i++)
                {
                    m_enemyParty[i].GetComponent<BattleStateMachine>().m_actionTimer = Timer(m_enemyParty[i].GetComponent<EnemyManager>().enemyParameters.Speed);
                }
            }
            if (WinnerChack() == 1)
            {
                //lose
            }
            else if (WinnerChack() == 2)
            {
                //win
            }
            else
            {
                return;
            }
            //各キャラにあるstatemachineを切り替える
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
