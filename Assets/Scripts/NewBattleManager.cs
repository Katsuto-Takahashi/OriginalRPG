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

    bool m_isContact = false;
    bool m_isBattle = false;
    int m_enemyParty = 0;
    int m_enemyID = 0;
    Vector3 m_contactPosition;
    float m_distsnce = 0f;
    Transform m_charaTransform;

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
            m_instantiateEnemy = Instantiate(m_partyManager.EnemyParty[m_enemyID - 1], new Vector3((m_contactPosition + m_charaTransform.forward * 3).x + i, m_contactPosition.y, (m_contactPosition + m_charaTransform.forward * 3).z + i), Quaternion.identity);
            m_enemyObjects.Add(m_instantiateEnemy);
            var em = m_enemyObjects[i].GetComponent<EnemyManager>();
            var ep = em.EnemyParameters;
            if (num > 1)
            {
                m_enemyObjects[i].name = ep.EnemyCharacterName + $"{i + 1}";
            }
            else
            {
                m_enemyObjects[i].name = ep.EnemyCharacterName;
            }
            m_enemyList.Add(em);
            var ebsm = m_enemyObjects[i].GetComponent<BCharacterStateMachine>();
            ebsm.enabled = true;
            ebsm.m_actionTimer = Timer(ep.Speed);
            m_battleEnemyList.AddEnemyList(m_enemyObjects[i]);
            m_firstDrop.Add(ep.FirstDropItem);
            m_secondDrop.Add(ep.SecondDropItem);
            m_getExperiencePoint += ep.ExperiencePoint;
            m_enemyObjects[i].transform.LookAt(m_contactPosition);
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

        randam = Random.Range(1, m_enemyParty + 1);
        Debug.Log($"出現数{randam}体");
        CreateEnemy(randam);

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
            m_finish = true;
        }
        else if (enemyDeadCount == m_enemyObjects.Count)
        {
            m_battleResults = BattleResults.Win;
            //m_contactEnemy.IsBattle = false;
            m_finish = true;
        }
    }
    void TargetCharacters()
    {
        for (int n = 0; n < m_enemyObjects.Count; n++)
        {
            for (int i = 0; i < m_characterList.Count; i++)
            {
                m_enemyObjects[n].GetComponent<BCharacterStateMachine>().m_targetCharacters.Add(m_characterList[i].gameObject);
            }
        }
        for (int n = 0; n < m_characterList.Count; n++)
        {
            for (int i = 0; i < m_enemyObjects.Count; i++)
            {
                m_characterList[n].GetComponent<BCharacterStateMachine>().m_targetCharacters.Add(m_enemyObjects[i]);
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
        //m_isChangeState = true;
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

        //m_finish = true;
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
        m_battleResults = BattleResults.Escape;

        BattleStanby();
        TargetCharacters();

        WinnerChack();

        yield return new WaitUntil(() => m_finish);
        FinishBattle();

        DestryEnemy(randam);

        m_finish = false;
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
                StartCoroutine(m_battleInformationUI.GetExpUI(m_getExperiencePoint, m_characterList[i].Name.Value, m_characterList[i].Level.Value, m_characterList[i].LevelUP.Value));
                //m_characterList[i].LevelUP.Value = false;
            }
        }
        else if (m_battleResults == BattleResults.Lose)
        {

        }
        m_getExperiencePoint = 0;
    }

    public void SetBattle(GameObject character, GameObject enemy)
    {
        Contact(character, enemy);
    }

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
        ContactUpdate(character.GetComponent<Character>());
    }

    void ContactUpdate(Character character)
    {
        if (character.IsContact.HasValue)
        {
            while (m_isBattle)
            {
                m_distsnce = (m_contactPosition.x - m_charaTransform.position.x) * (m_contactPosition.x - m_charaTransform.position.x) + (m_contactPosition.z - m_charaTransform.position.z) * (m_contactPosition.z - m_charaTransform.position.z);
                if (Mathf.Sqrt(m_distsnce) > 15f)
                {
                    m_isBattle = false;
                    DeleteField(character);
                }
            }
        }
    }
    void CreateField(Vector3 contactPos)
    {
        m_instantiateBattleFeild = Instantiate(m_battleFeildPrefab, contactPos, Quaternion.identity);
    }
    public void DeleteField(Character character)
    {
        Destroy(m_instantiateBattleFeild);
        m_contactPosition = Vector3.zero;
        m_distsnce = 0f;
    }

}