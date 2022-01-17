using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class NewBattleManager : SingletonMonoBehaviour<NewBattleManager>
{
    [SerializeField]
    DamageCalculator m_damageCalculator = null;

    [SerializeField]
    PartyManager m_partyManager = null;

    [SerializeField]
    BattleEnemyList m_battleEnemyList = null;

    [SerializeField]
    GameObject m_battleInformationUIObject = null;

    [SerializeField]
    GameObject m_battleFeildPrefab = null;

    BattleInformationUI m_battleInformationUI;
    GameObject m_instantiateEnemy;
    GameObject m_instantiateBattleFeild;
    List<GameObject> m_enemyObjects = new List<GameObject>();
    List<EnemyManager> m_enemyList = new List<EnemyManager>();
    List<Character> m_characterList = new List<Character>();
    bool m_isCreated = false;
    bool m_isChangeState = false;
    bool m_finish = false;
    int characterDeadCount = 0;
    int enemyDeadCount = 0;
    int randam;
    List<GameObject> m_firstDrop = new List<GameObject>();
    List<GameObject> m_secondDrop = new List<GameObject>();
    int m_getExperiencePoint = 0;

    enum BattleResults
    {
        Win,
        Lose,
        Escape
    }
    BattleResults m_battleResults = BattleResults.Escape;
    void Start()
    {
        m_partyManager.CharacterCount.DistinctUntilChanged().Subscribe(_ => PartyNum());
    }

    void PartyNum()
    {
        Debug.Log("Character.IsContact");
        for (int i = 0; i < m_partyManager.CharacterParty.Count; i++)
        {
            var c = m_partyManager.CharacterParty[i].GetComponent<Character>();
            c.IsContact.Where(x => x is true).Subscribe(_ => BattleStart()).AddTo(m_partyManager.CharacterParty[i]);
        }
    }

    void CreateEnemy(int num)
    {
        for (int i = 0; i < num; i++)
        {
            //m_gameObject = Instantiate(m_partyManager.EnemyParty[m_contactEnemy.EnemyID - 1], new Vector3((m_contactEnemy.ContactPosition + m_contactEnemy.PlayerTransform.forward * 3).x + i, m_contactEnemy.ContactPosition.y, (m_contactEnemy.ContactPosition + m_contactEnemy.PlayerTransform.forward * 3).z + i), Quaternion.identity);
            m_enemyObjects.Add(m_instantiateEnemy);
            var em = m_enemyObjects[i].GetComponent<EnemyManager>();
            if (num > 1)
            {
                m_enemyObjects[i].name = em.EnemyParameters.EnemyCharacterName + $"{i + 1}";
            }
            else
            {
                m_enemyObjects[i].name = em.EnemyParameters.EnemyCharacterName;
            }
            m_enemyList.Add(em);
            var ebsm = m_enemyObjects[i].GetComponent<BCharacterStateMachine>();
            ebsm.enabled = true;
            ebsm.m_actionTimer = Timer(em.EnemyParameters.Speed);
            m_battleEnemyList.AddEnemyList(m_enemyObjects[i]);
            m_firstDrop.Add(m_enemyList[i].EnemyParameters.FirstDropItem);
            m_secondDrop.Add(m_enemyList[i].EnemyParameters.SecondDropItem);
            m_getExperiencePoint += m_enemyList[i].EnemyParameters.ExperiencePoint;
            //m_enemyParty[i].transform.LookAt(m_contactEnemy.ContactPosition);
        }
        m_battleEnemyList.ChengeBool();
        m_isCreated = true;
    }
    void DestryEnemy(int num)
    {
        for (int i = 0; i < num; i++)
        {
            Destroy(m_enemyObjects[i]);
        }
        m_enemyObjects.Clear();
        m_enemyList.Clear();
        m_characterList.Clear();
        m_battleEnemyList.ClearEnemyList();
    }
    void BattleStanby()
    {
        for (int i = 0; i < m_partyManager.CharacterParty.Count; i++)
        {
            var cpm = m_partyManager.CharacterParty[i].GetComponent<Character>();
            m_characterList.Add(cpm);
        }
        //if (m_contactEnemy.EnemyParty < 2)
        {
            CreateEnemy(1);
        }
        //else if (m_contactEnemy.EnemyParty > 1)
        {
            //randam = Random.Range(1, m_contactEnemy.EnemyParty + 1);
            Debug.Log($"出現数{randam}体");
            CreateEnemy(randam);
        }
        //敵がボスの時はにげれないようにする

        StartCoroutine(ChengeActiveUI());
        m_battleInformationUI = m_battleInformationUIObject.GetComponent<BattleInformationUI>();
        StartCoroutine(m_battleInformationUI.BattleStartUI(m_enemyObjects[0].GetComponent<EnemyManager>().EnemyParameters.EnemyCharacterName, randam));
        for (int i = 0; i < m_enemyObjects.Count; i++)
        {
            var emn = m_enemyObjects[i].GetComponent<EnemyManager>().EnemyParameters.EnemyCharacterName;
            if (m_enemyObjects.Count > 1)
            {
                emn += $"{i + 1}";
            }
            m_enemyObjects[i].GetComponentInChildren<EnemyUI>().ChangeName(emn);
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
            m_characterList[i].HP.Where(h => h < 1).DistinctUntilChanged().Subscribe(_ => characterDeadCount++).AddTo(m_characterList[i]);
            m_characterList[i].HP.Where(h => h > 0).DistinctUntilChanged().Subscribe(_ => characterDeadCount--).AddTo(m_characterList[i]);
            //if (m_characterList[i].IsDeadState == true)
            //{
            //    characterDeadCount++;
            //}
        }
        for (int i = 0; i < m_enemyObjects.Count; i++)
        {
            if (m_enemyList[i].IsDeadState == true)
            {
                m_enemyObjects[i].GetComponent<BCharacterStateMachine>().ChangeDead();
                enemyDeadCount++;
            }
        }
        if (characterDeadCount == m_partyManager.CharacterParty.Count)
        {
            m_battleResults = BattleResults.Lose;
            //m_contactEnemy.IsBattle = false;
        }
        else if (enemyDeadCount == m_enemyObjects.Count)
        {
            m_battleResults = BattleResults.Win;
            //m_contactEnemy.IsBattle = false;
        }
    }
    void TargetCharacters()
    {
        for (int n = 0; n < m_enemyObjects.Count; n++)
        {
            for (int i = 0; i < m_partyManager.CharacterParty.Count; i++)
            {
                m_enemyObjects[n].GetComponent<BCharacterStateMachine>().m_targetCharacters.Add(m_partyManager.CharacterParty[i]);
            }
        }
        for (int n = 0; n < m_partyManager.CharacterParty.Count; n++)
        {
            for (int i = 0; i < m_enemyObjects.Count; i++)
            {
                m_partyManager.CharacterParty[n].GetComponent<BCharacterStateMachine>().m_targetCharacters.Add(m_enemyObjects[i]);
            }
        }
    }
    void StateChange()
    {
        for (int i = 0; i < m_characterList.Count; i++)
        {
            //var cbsm = m_characterList[i].BattleStateMachine.IsBattle;
            m_characterList[i].BCSM.IsBattle = true;
            //cbsm.m_battle = true;
            //cbsm.m_firstAction = true;
        }
        for (int i = 0; i < m_enemyObjects.Count; i++)
        {
            var e = m_enemyObjects[i]?.GetComponent<BCharacterStateMachine>();
            if (e != null)
            {
                //e.m_firstAction = true;
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
                    damage = m_damageCalculator.EnemyDamage(skillData, enemyParameter, parameterManager.Strength, enemyParameter.Defense, m_battleInformationUI.Critical);
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
        for (int i = 0; i < m_characterList.Count; i++)
        {
            m_characterList[i].BCSM.IsBattle = false;
            //var cbsm = m_partyManager.CharacterParty[i].GetComponent<BCharacterStateMachine>();
            //cbsm.m_targetCharacters.Clear();
            //cbsm.m_battle = false;
            //cbsm.m_open = false;
            //cbsm.m_battlePanel.SetActive(false);
            //cbsm.enabled = false;
        }
        for (int i = 0; i < m_enemyObjects.Count; i++)
        {
            m_enemyObjects[i].GetComponent<BCharacterStateMachine>().m_battle = false;
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
    void BattleStart()
    {
        Debug.Log("戦闘開始");
        StartCoroutine(BattleUpdate());
    }
    IEnumerator BattleUpdate()
    {
        bool battleNow = true;
        m_battleResults = BattleResults.Escape;
        //if (m_contactEnemy.IsContact && !m_isCreated)
        {
            BattleStanby();
            TargetCharacters();
        }
        //if (!m_isChangeState)
        //{
        //    StateChange();
        //}

        WinnerChack();

        while (battleNow)
        {
            //if (m_contactEnemy.IsBattle)
            //{
            //    if (m_isChangeState && m_isCreated)
            //    {
            //        WinnerChack();
            //    }
            //}
            //else
            {
                m_isChangeState = false;
                //m_contactEnemy.DeleteField();
                //if (!m_finish)
                //{
                //    FinishBattle();
                //}
                //battleNow = m_contactEnemy.IsBattle;
            }
            yield return null;
        }

        FinishBattle();

        //if (m_contactEnemy.EnemyParty == 0)
        {
            DestryEnemy(1);
        }
        //else if (m_contactEnemy.EnemyParty > 0)
        {
            DestryEnemy(randam);
        }
        m_isCreated = false;
        m_finish = false;
        //for (int i = 0; i < m_partyManager.CharacterParty.Count; i++)
        //{
        //    m_partyManager.CharacterParty[i].GetComponent<BCharacterStateMachine>().ChangeIdle();
        //}
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
                //StartCoroutine(m_battleInformationUI.GetExpUI(m_getExperiencePoint, m_characterList[i].CharacterName, m_characterList[i].Level, m_characterList[i].LevelUP));
                //m_characterList[i].LevelUP = false;
            }
        }
        else if (m_battleResults == BattleResults.Lose)
        {

        }
        m_getExperiencePoint = 0;
    }

    public void SetBattle(GameObject character, GameObject enemy)
    {
        //Contact(character, enemy);
    }
    /*
    void Contact(GameObject character, GameObject enemy)
    {
        m_charaTransform = character.transform;
        m_contactPosition = enemy.transform.position;
        CreateField(m_contactPosition);
        var em = enemy.GetComponent<EnemyManager>().EnemyParameters;
        m_enemyParty = em.EnemyPartyNumber;
        m_enemyID = em.EnemyCharacterID;
        m_isBattle = true;
        Destroy(enemy);
    }

    void ContactUpdate()
    {
        if (IsContact)
        {
            while (m_isBattle)
            {
                m_distsnce = (m_contactPosition.x - m_charaTransform.position.x) * (m_contactPosition.x - m_charaTransform.position.x) + (m_contactPosition.z - m_charaTransform.position.z) * (m_contactPosition.z - m_charaTransform.position.z);
                if (Mathf.Sqrt(m_distsnce) > 15f)
                {
                    m_isBattle = false;
                    DeleteField();
                }
            }
        }
    }
    void CreateField(Vector3 contactPos)
    {
        //m_battleFeildPrefab.transform.position = enemyPos;
        m_battleFeild = Instantiate(m_battleFeildPrefab, contactPos, Quaternion.identity);
        m_battleFeild.transform.position = contactPos;
    }
    public void DeleteField()
    {
        Destroy(m_battleFeild);
        IsContact = false;
        m_contactPosition = Vector3.zero;
        m_distsnce = 0f;
    }
    */
}

public class ContactManager
{
    [SerializeField]
    GameObject m_battleFeildPrefab = null;
    GameObject m_instantiateBattleFeild;
    bool m_isContact = false;
    bool m_isBattle = false;
    int m_enemyParty;
    int m_enemyID;
    Vector3 m_contactPosition;
    float m_distsnce;
    Transform m_PlayerTransform;

    /// <summary>接敵時のPosition</summary>
    public Vector3 ContactPosition { get => m_contactPosition; }

    /// <summary>接敵した敵のID</summary>
    public int EnemyID { get => m_enemyID; }

    /// <summary>接敵した敵のパーティ構成</summary>
    public int EnemyParty { get => m_enemyParty; }

    /// <summary>バトル中かどうか</summary>
    public bool IsBattle { get => m_isBattle; set => m_isBattle = value; }

    /// <summary>接敵したかどうか</summary>
    public bool IsContact { get => m_isContact; set => m_isContact = value; }

    /// <summary>接敵時のPlayerのTransform</summary>
    public Transform PlayerTransform { get => m_PlayerTransform; }
    /// <summary>中心からの距離</summary>
    public float Distsnce { get => m_distsnce; set => m_distsnce = value; }
}