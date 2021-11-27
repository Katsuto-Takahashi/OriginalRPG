using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] DamageCalculator m_damageCalculator = null;
    [SerializeField] ContactEnemy m_contactEnemy = null;
    [SerializeField] PartyManager m_partyManager = null;
    [SerializeField] BattleEnemyList m_battleEnemyList = null;
    [SerializeField] GameObject m_battleInformationUIObject = null;
    GameObject m_gameObject;
    List<GameObject> m_enemyParty = new List<GameObject>();
    List<EnemyManager> m_enemyList = new List<EnemyManager>();
    List<CharacterParameterManager> m_characterList = new List<CharacterParameterManager>();
    bool m_isCreated = false;
    bool m_isChangeState = false;
    bool m_finish = false;
    int characterDeadCount = 0;
    int enemyDeadCount = 0;
    int randam;
    List<GameObject> m_firstDrop = new List<GameObject>();
    List<GameObject> m_secondDrop = new List<GameObject>();
    int m_getExperiencePoint = 0;
    BattleInformationUI m_battleInformationUI;
    enum BattleResults
    {
        Win,
        Lose,
        Escape
    }
    BattleResults m_battleResults = BattleResults.Escape;
    void Start()
    {
        m_contactEnemy.Battle += StartBattle;
    }
    void CreateEnemy(int num)
    {
        for (int i = 0; i < num; i++)
        {
            //m_gameObject = Instantiate(m_enemyPrefabs.m_enemyList[m_contactEnemy.EnemyID - 1], new Vector3(m_contactEnemy.ContactPosition.x + i, m_contactEnemy.ContactPosition.y, m_contactEnemy.ContactPosition.z + i), Quaternion.identity);
            m_gameObject = Instantiate(m_partyManager.EnemyParty[m_contactEnemy.EnemyID - 1], new Vector3((m_contactEnemy.ContactPosition + m_contactEnemy.PlayerTransform.forward * 3).x + i, m_contactEnemy.ContactPosition.y, (m_contactEnemy.ContactPosition + m_contactEnemy.PlayerTransform.forward * 3).z + i), Quaternion.identity);
            m_enemyParty.Add(m_gameObject);
            var em = m_enemyParty[i].GetComponent<EnemyManager>();
            if (num > 1)
            {
                m_enemyParty[i].name = em.EnemyParameters.EnemyCharacterName + $"{i + 1}";
            }
            else
            {
                m_enemyParty[i].name = em.EnemyParameters.EnemyCharacterName;
            }
            m_enemyList.Add(em);
            var ebsm = m_enemyParty[i].GetComponent<BattleCharacterStateMachine>();
            ebsm.enabled = true;
            ebsm.m_actionTimer = Timer(em.EnemyParameters.Speed);
            m_battleEnemyList.AddEnemyList(m_enemyParty[i]);
            m_firstDrop.Add(m_enemyList[i].EnemyParameters.FirstDropItem);
            m_secondDrop.Add(m_enemyList[i].EnemyParameters.SecondDropItem);
            m_getExperiencePoint += m_enemyList[i].EnemyParameters.ExperiencePoint;
            m_enemyParty[i].transform.LookAt(m_contactEnemy.ContactPosition);
        }
        m_battleEnemyList.ChengeBool();
        m_isCreated = true;
    }
    void DestryEnemy(int num)
    {
        for (int i = 0; i < num; i++)
        {
            Destroy(m_enemyParty[i]);
        }
        m_enemyParty.Clear();
        m_enemyList.Clear();
        m_characterList.Clear();
        m_battleEnemyList.ClearEnemyList();
    }
    void BattleStanby()
    {
        for (int i = 0; i < m_partyManager.CharacterParty.Count; i++)
        {
            var cpm = m_partyManager.CharacterParty[i].GetComponent<CharacterParameterManager>();
            m_characterList.Add(cpm);
            var cbsm = m_partyManager.CharacterParty[i].GetComponent<BattleCharacterStateMachine>();
            cbsm.m_actionTimer = Timer(cpm.Speed);
            cbsm.enabled = true;
        }
        if (m_contactEnemy.EnemyParty < 2)
        {
            CreateEnemy(1);
        }
        else if (m_contactEnemy.EnemyParty > 1)
        {
            randam = Random.Range(1, m_contactEnemy.EnemyParty + 1);
            Debug.Log($"出現数{randam}体");
            CreateEnemy(randam);
        }
        //敵がボスの時はにげれないようにする

        StartCoroutine(ChengeActiveUI());
        m_battleInformationUI = m_battleInformationUIObject.GetComponent<BattleInformationUI>();
        StartCoroutine(m_battleInformationUI.BattleStartUI(m_enemyParty[0].GetComponent<EnemyManager>().EnemyParameters.EnemyCharacterName, randam));
        for (int i = 0; i < m_enemyParty.Count; i++)
        {
            var emn = m_enemyParty[i].GetComponent<EnemyManager>().EnemyParameters.EnemyCharacterName;
            if (m_enemyParty.Count > 1)
            {
                emn += $"{i + 1}";
            }
            m_enemyParty[i].GetComponentInChildren<EnemyUI>().ChangeName(emn);
        }
    }
    float Timer(int speed)
    {
        return (1000 - speed) / 100f;
    }
    void WinnerChack()
    {
        characterDeadCount = 0;
        enemyDeadCount = 0;
        for (int i = 0; i < m_partyManager.CharacterParty.Count; i++)
        {
            if (m_characterList[i].IsDeadState == true)
            {
                characterDeadCount++;
            }
        }
        for (int i = 0; i < m_enemyParty.Count; i++)
        {
            if (m_enemyList[i].IsDeadState == true)
            {
                m_enemyParty[i].GetComponent<BattleCharacterStateMachine>().ChangeDead();
                enemyDeadCount++;
            }
        }
        if (characterDeadCount == m_partyManager.CharacterParty.Count)
        {
            m_battleResults = BattleResults.Lose;
            m_contactEnemy.IsBattle = false;
        }
        else if (enemyDeadCount == m_enemyParty.Count)
        {
            m_battleResults = BattleResults.Win;
            m_contactEnemy.IsBattle = false;
        }
    }
    void TargetCharacters()
    {
        for (int n = 0; n < m_enemyParty.Count; n++)
        {
            for (int i = 0; i < m_partyManager.CharacterParty.Count; i++)
            {
                m_enemyParty[n].GetComponent<BattleCharacterStateMachine>().m_targetCharacters.Add(m_partyManager.CharacterParty[i]);
            }
        }
        for (int n = 0; n < m_partyManager.CharacterParty.Count; n++)
        {
            for (int i = 0; i < m_enemyParty.Count; i++)
            {
                m_partyManager.CharacterParty[n].GetComponent<BattleCharacterStateMachine>().m_targetCharacters.Add(m_enemyParty[i]);
            }
        }
    }
    void StateChange()
    {
        for (int i = 0; i < m_partyManager.CharacterParty.Count; i++)
        {
            var cbsm = m_partyManager.CharacterParty[i].GetComponent<BattleCharacterStateMachine>();
            cbsm.m_battle = true;
            cbsm.m_firstAction = true;
        }
        for (int i = 0; i < m_enemyParty.Count; i++)
        {
            var e = m_enemyParty[i]?.GetComponent<BattleCharacterStateMachine>();
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
        return DamageCalculate(attacker, defender, skillData);
    }

    int DamageCalculate(GameObject attacker, GameObject defender, SkillData skillData)
    {
        int damage = 0;
        if (attacker.CompareTag("Player"))
        {
            var parameterManager = attacker.GetComponent<CharacterParameterManager>();
            var enemyParameter = defender.GetComponent<EnemyManager>().EnemyParameters;
            if (skillData.attackType == SkillData.AttackType.physicalAttack)
            {
                if (Random.Range(0, 200) > parameterManager.Luck)
                {
                    damage = m_damageCalculator.EnemyDamage(skillData, enemyParameter, parameterManager.Strength, enemyParameter.Defense, m_battleInformationUI.Critical);
                }
                else
                {
                    m_battleInformationUI.Critical = true;
                    damage = m_damageCalculator.EnemyDamage(skillData, enemyParameter,  parameterManager.Strength, enemyParameter.Defense, m_battleInformationUI.Critical);
                }
            }
            else if (skillData.attackType == SkillData.AttackType.magicAttack)
            {
                if (Random.Range(0, 200) > parameterManager.Luck)
                {
                    damage = m_damageCalculator.EnemyDamage(skillData, enemyParameter, parameterManager.MagicPower, enemyParameter.MagicResist, m_battleInformationUI.Critical);
                }
                else
                {
                    m_battleInformationUI.Critical = true;
                    damage = m_damageCalculator.EnemyDamage(skillData, enemyParameter, parameterManager.MagicPower, enemyParameter.MagicResist, m_battleInformationUI.Critical);
                }
            }
            StartCoroutine(m_battleInformationUI.BattleUIDisplay(damage, defender.name, m_battleInformationUI.Critical));
        }
        else if (attacker.CompareTag("Enemy"))
        {
            var enemyParameter = attacker.GetComponent<EnemyManager>().EnemyParameters;
            var parameterManager = defender.GetComponent<CharacterParameterManager>();
            if (skillData.attackType == SkillData.AttackType.physicalAttack)
            {
                if (Random.Range(0, 200) > enemyParameter.Luck)
                {
                    damage = m_damageCalculator.PlayerDamage(skillData, enemyParameter.Strength, parameterManager.Defense, m_battleInformationUI.Critical);
                }
                else
                {
                    m_battleInformationUI.Critical = true;
                    damage = m_damageCalculator.PlayerDamage(skillData, enemyParameter.Strength, parameterManager.Defense, m_battleInformationUI.Critical);
                }
            }
            else if (skillData.attackType == SkillData.AttackType.magicAttack)
            {
                if (Random.Range(0, 200) > enemyParameter.Luck)
                {
                    damage = m_damageCalculator.PlayerDamage(skillData, enemyParameter.MagicPower, parameterManager.MagicResist, m_battleInformationUI.Critical);
                }
                else
                {
                    m_battleInformationUI.Critical = true;
                    damage = m_damageCalculator.PlayerDamage(skillData, enemyParameter.MagicPower, parameterManager.MagicResist, m_battleInformationUI.Critical);
                }
            }
            StartCoroutine(m_battleInformationUI.BattleUIDisplay(damage, parameterManager.CharacterName, m_battleInformationUI.Critical));
        }
        return damage;
    }
    void FinishBattle()
    {
        for (int i = 0; i < m_partyManager.CharacterParty.Count; i++)
        {
            var cbsm = m_partyManager.CharacterParty[i].GetComponent<BattleCharacterStateMachine>();
            cbsm.m_targetCharacters.Clear();
            cbsm.m_battle = false;
            cbsm.m_open = false;
            cbsm.m_battlePanel.SetActive(false);
            cbsm.enabled = false;
        }
        for (int i = 0; i < m_enemyParty.Count; i++)
        {
            m_enemyParty[i].GetComponent<BattleCharacterStateMachine>().m_battle = false;
        }
        StartCoroutine(BattleData());

        m_finish = true;
    }
    IEnumerator ChengeActiveUI()
    {
        if (m_battleInformationUIObject.activeSelf)
        {
            yield return new WaitForSeconds(2f);
            m_battleInformationUIObject.SetActive(false);
        }
        else
        {
            yield return null;
            m_battleInformationUIObject.SetActive(true);
        }
        yield return null;
    }
    void StartBattle()
    {
        StartCoroutine(BattleUpdate());
    }
    IEnumerator BattleUpdate()
    {
        bool battleNow = true;
        m_battleResults = BattleResults.Escape;
        if (m_contactEnemy.IsContact && !m_isCreated)
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
            if (m_contactEnemy.IsBattle)
            {
                if (m_isChangeState && m_isCreated)
                {
                    WinnerChack();
                }
            }
            else
            {
                m_isChangeState = false;
                m_contactEnemy.DeleteField();
                if (!m_finish)
                {
                    FinishBattle();
                }
                battleNow = m_contactEnemy.IsBattle;
            }
            yield return null;
        }
        if (m_contactEnemy.EnemyParty == 0)
        {
            DestryEnemy(1);
        }
        else if (m_contactEnemy.EnemyParty > 0)
        {
            DestryEnemy(randam);
        }
        m_isCreated = false;
        m_finish = false;
        for (int i = 0; i < m_partyManager.CharacterParty.Count; i++)
        {
            m_partyManager.CharacterParty[i].GetComponent<BattleCharacterStateMachine>().ChangeIdle();
        }
    }
    IEnumerator BattleData()
    {
        if (m_battleResults == BattleResults.Win)
        {
            StartCoroutine(m_battleInformationUI.BattleFinishUI((int)BattleResults.Win));
        }
        else if (m_battleResults == BattleResults.Lose)
        {
            StartCoroutine(m_battleInformationUI.BattleFinishUI((int)BattleResults.Lose));
        }
        else if (m_battleResults == BattleResults.Escape)
        {
            StartCoroutine(m_battleInformationUI.BattleFinishUI((int)BattleResults.Escape));
        }
        StartCoroutine(ChengeActiveUI());
        yield return null;
        if (m_battleResults == BattleResults.Win)
        {
            for (int i = 0; i < m_characterList.Count; i++)
            {
                m_characterList[i].GetExp(m_getExperiencePoint);
                StartCoroutine(m_battleInformationUI.GetExpUI(m_getExperiencePoint, m_characterList[i].CharacterName, m_characterList[i].Level, m_characterList[i].LevelUP));
                m_characterList[i].LevelUP = false;
            }
        }
        else if (m_battleResults == BattleResults.Lose)
        {
            
        }
        m_getExperiencePoint = 0;
    }
}
