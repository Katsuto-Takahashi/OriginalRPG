using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] DamageCalculator damageCalculator = null;
    [SerializeField] ContactEnemy contactEnemy = null;
    [SerializeField] EnemyList m_enemyPrefabs = null;
    [SerializeField] PartyManager m_partyManager = null;
    [SerializeField] BattleEnemyList m_battleEnemyList = null;
    private GameObject m_gameObject;
    private List<GameObject> m_enemyParty = new List<GameObject>();
    private List<EnemyManager> m_enemyDeadList = new List<EnemyManager>();
    private List<CharacterParameterManager> m_characterDeadList = new List<CharacterParameterManager>();
    private bool m_isCreated = false;
    private bool m_isChangeState = false;
    private bool m_finish = false;
    private int characterDeadCount = 0;
    private int enemyDeadCount = 0;

    private void Start()
    {
        contactEnemy.Battle += StartBattle;
    }
    void CreateEnemy(int num)
    {
        for (int i = 0; i < num; i++)
        {
            m_gameObject = Instantiate(m_enemyPrefabs.m_enemyList[contactEnemy.m_enemyID - 1], contactEnemy.m_contactPosition, Quaternion.identity);
            m_enemyParty.Add(m_gameObject);
            m_enemyParty[i].name = m_enemyParty[i].GetComponent<EnemyManager>().enemyParameters.EnemyCharacterName + $"{i}";
            m_enemyDeadList.Add(m_enemyParty[i].GetComponent<EnemyManager>());
            m_enemyParty[i].GetComponent<BattleStateMachine>().enabled = true;
            m_enemyParty[i].GetComponent<BattleStateMachine>().m_actionTimer = Timer(m_enemyParty[i].GetComponent<EnemyManager>().enemyParameters.Speed);
            m_battleEnemyList.m_battleEnemys.Add(m_enemyParty[i]);
        }
        m_isCreated = true;
    }
    void DestryEnemy(int num)
    {
        for (int i = 0; i < num; i++)
        {
            Destroy(m_enemyParty[i]);
        }
        m_enemyParty.Clear();
        m_enemyDeadList.Clear();
        m_characterDeadList.Clear();
        m_battleEnemyList.m_battleEnemys.Clear();
    }
    void BattleStanby()
    {
        if (contactEnemy.m_enemyParty == 0)
        {
            CreateEnemy(1);
        }
        for (int i = 0; i < m_partyManager.m_charaParty.Count; i++)
        {
            m_characterDeadList.Add(m_partyManager.m_charaParty[i].GetComponent<CharacterParameterManager>());
            m_partyManager.m_charaParty[i].GetComponent<BattleStateMachine>().m_actionTimer = Timer(m_partyManager.m_charaParty[i].GetComponent<CharacterParameterManager>().Speed);
            m_partyManager.m_charaParty[i].GetComponent<BattleStateMachine>().enabled = true;
        }
        //敵がボスの時はにげれないようにする
        //if (true)
        //{
        //    contactEnemy.m_isContact = false;
        //}
    }
    float Timer(int speed)
    {
        return (1000 - speed) / 100f;
    }
    void WinnerChack()
    {
        characterDeadCount = 0;
        enemyDeadCount = 0;
        for (int i = 0; i < m_partyManager.m_charaParty.Count; i++)
        {
            if (m_characterDeadList[i].IsDeadState == true)
            {
                characterDeadCount++;
            }
        }
        for (int i = 0; i < m_enemyParty.Count; i++)
        {
            if (m_enemyDeadList[i].IsDeadState == true)
            {
                enemyDeadCount++;
            }
        }
        if (characterDeadCount == m_partyManager.m_charaParty.Count)
        {
            Debug.Log("lose");
            contactEnemy.m_isBattle = false;
        }
        else if (enemyDeadCount == m_enemyParty.Count)
        {
            Debug.Log("win");
            contactEnemy.m_isBattle = false;
        }
    }
    void TargetCharacters()
    {
        for (int n = 0; n < m_enemyParty.Count; n++)
        {
            for (int i = 0; i < m_partyManager.m_charaParty.Count; i++)
            {
                m_enemyParty[n].GetComponent<BattleStateMachine>().m_targetCharacters.Add(m_partyManager.m_charaParty[i]);
            }
        }
        for (int n = 0; n < m_partyManager.m_charaParty.Count; n++)
        {
            for (int i = 0; i < m_enemyParty.Count; i++)
            {
                m_partyManager.m_charaParty[n].GetComponent<BattleStateMachine>().m_targetCharacters.Add(m_enemyParty[i]);
            }
        }
    }
    void StateChange()
    {
        for (int i = 0; i < m_partyManager.m_charaParty.Count; i++)
        {
            m_partyManager.m_charaParty[i].GetComponent<BattleStateMachine>().m_battle = true;
            m_partyManager.m_charaParty[i].GetComponent<BattleStateMachine>().m_firstAction = true;
        }
        for (int i = 0; i < m_enemyParty.Count; i++)
        {
            var e = m_enemyParty[i]?.GetComponent<BattleStateMachine>();
            if (e != null)
            {
                e.m_firstAction = true;
                e.m_battle = true;
            }
        }
        m_isChangeState = true;
    }
    public int Damage(GameObject attacker, GameObject defender, SkillData skillData)
    {
        int damage = 0;
        if (attacker.CompareTag("Player"))
        {
            var parameterManager = attacker.GetComponent<CharacterParameterManager>();
            var enemyParameter = defender.GetComponent<EnemyManager>().enemyParameters;
            if (skillData.attackType == SkillData.AttackType.physicalAttack)
            {
                if (Random.Range(0, 200) > parameterManager.Luck)
                {
                    damage = damageCalculator.DecideEnemyDamege(skillData, damageCalculator.CalculateNormalDamage(skillData, enemyParameter.Defense, parameterManager.Strength), enemyParameter);
                }
                else
                {
                    damage = damageCalculator.DecideEnemyDamege(skillData, damageCalculator.CalculateCriticalDamage(skillData, parameterManager.Strength), enemyParameter);
                }
            }
            else if (skillData.attackType == SkillData.AttackType.magicAttack)
            {
                if (Random.Range(0, 200) > parameterManager.Luck)
                {
                    damage = damageCalculator.DecideEnemyDamege(skillData, damageCalculator.CalculateNormalDamage(skillData, enemyParameter.MagicResist, parameterManager.MagicPower), enemyParameter);
                }
                else
                {
                    damage = damageCalculator.DecideEnemyDamege(skillData, damageCalculator.CalculateCriticalDamage(skillData, parameterManager.MagicPower), enemyParameter);
                }
            }
        }
        else if (attacker.CompareTag("Enemy"))
        {
            var enemyParameter = attacker.GetComponent<EnemyManager>().enemyParameters;
            var parameterManager = defender.GetComponent<CharacterParameterManager>();
            if (skillData.attackType == SkillData.AttackType.physicalAttack)
            {
                if (Random.Range(0, 200) > enemyParameter.Luck)
                {
                    damage = damageCalculator.DecidePlayerDamege(damageCalculator.CalculateNormalDamage(skillData, parameterManager.Defense, enemyParameter.Strength));
                }
                else
                {
                    damage = damageCalculator.DecidePlayerDamege(damageCalculator.CalculateCriticalDamage(skillData, enemyParameter.Strength));
                }
            }
            else if (skillData.attackType == SkillData.AttackType.magicAttack)
            {
                if (Random.Range(0, 200) > enemyParameter.Luck)
                {
                    damage = damageCalculator.DecidePlayerDamege(damageCalculator.CalculateNormalDamage(skillData, parameterManager.MagicResist, enemyParameter.MagicPower));
                }
                else
                {
                    damage = damageCalculator.DecidePlayerDamege(damageCalculator.CalculateCriticalDamage(skillData, enemyParameter.MagicPower));
                }
            }
        }
        return damage;
    }
    void FinishBattle()
    {
        for (int i = 0; i < m_partyManager.m_charaParty.Count; i++)
        {
            m_partyManager.m_charaParty[i].GetComponent<BattleStateMachine>().m_targetCharacters.Clear();
            m_partyManager.m_charaParty[i].GetComponent<BattleStateMachine>().m_battle = false;
            m_partyManager.m_charaParty[i].GetComponent<BattleStateMachine>().m_open = false;
            m_partyManager.m_charaParty[i].GetComponent<BattleStateMachine>().m_battlePanel.SetActive(false);
            m_partyManager.m_charaParty[i].GetComponent<BattleStateMachine>().enabled = false;
        }
        for (int i = 0; i < m_enemyParty.Count; i++)
        {
            m_enemyParty[i].GetComponent<BattleStateMachine>().m_battle = false;
        }
        m_finish = true;
    }
    void StartBattle()
    {
        StartCoroutine(BattleUpdate());
    }
    IEnumerator BattleUpdate()
    {
        bool battleNow = true;
        if (contactEnemy.m_isContact && !m_isCreated)
        {
            BattleStanby();
            TargetCharacters();
        }
        if (!m_isChangeState)
        {
            StateChange();
        }
        while (battleNow)
        {
            if (contactEnemy.m_isBattle)
            {
               
                if (m_isChangeState && m_isCreated)
                {
                    WinnerChack();
                }
            }
            else
            {
                m_isChangeState = false;
                contactEnemy.DeleteField();
                if (!m_finish)
                {
                    FinishBattle();
                }
                battleNow = contactEnemy.m_isBattle;
            }
            yield return null;
        }
        DestryEnemy(1);
        m_isCreated = false;
        m_finish = false;
        for (int i = 0; i < m_partyManager.m_charaParty.Count; i++)
        {
            m_partyManager.m_charaParty[i].GetComponent<BattleStateMachine>().ChangeIdle();
        }
    }
}
