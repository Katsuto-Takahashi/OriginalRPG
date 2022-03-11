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
    List<Enemy> m_enemyList = new List<Enemy>();
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
            var em = m_enemyObjects[i].GetComponent<Enemy>();
            if (num > 1)
            {
                m_enemyObjects[i].name = em.Name.Value + $"{i + 1}";
            }
            else
            {
                m_enemyObjects[i].name = em.Name.Value;
            }
            m_enemyList.Add(em);
            m_battleEnemyList.AddEnemyList(m_enemyObjects[i]);
            m_firstDrop.Add(em.FirstDropItem);
            m_secondDrop.Add(em.SecondDropItem);
            m_getExperiencePoint += em.ExperiencePoint;
            m_enemyList[i].MESM.SetLookPosition(m_contactPosition);
            m_enemyList[i].ChengeKinematic(true);
            //em.transform.LookAt(m_contactPosition);
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
        characterDeadCount = 0;
        enemyDeadCount = 0;
        m_battleResults = BattleResults.Escape;

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
        StartCoroutine(m_battleInformationUI.BattleStartUI(m_enemyList[0].Name.Value, randam));

        for (int i = 0; i < m_enemyObjects.Count; i++)
        {
            var emn = m_enemyList[i].Name.Value;
            if (m_enemyObjects.Count > 1)
            {
                emn += $"{i + 1}";
            }
            m_enemyObjects[i].GetComponentInChildren<EnemyUI>().ChangeName(emn);
        }
    }

    void WinnerChack()
    {
        characterDeadCount = m_characterList.Count;
        enemyDeadCount = m_enemyList.Count;
        for (int i = 0; i < m_characterList.Count; i++)
        {
            m_characterList[i].HP.DistinctUntilChanged().Where(h => h < 1).Subscribe(_ => characterDeadCount++).AddTo(m_characterList[i]);
            m_characterList[i].HP.DistinctUntilChanged().Where(h => h > 0).Subscribe(_ => characterDeadCount--).AddTo(m_characterList[i]);
        }
        for (int i = 0; i < m_enemyList.Count; i++)
        {
            m_enemyList[i].HP.DistinctUntilChanged().Where(h => h < 1).Subscribe(_ => enemyDeadCount++).AddTo(m_enemyList[i]);
            m_enemyList[i].HP.DistinctUntilChanged().Where(h => h > 0).Subscribe(_ => enemyDeadCount--).AddTo(m_enemyList[i]);
        }
    }

    void DeadCheck()
    {
        if (characterDeadCount == m_partyManager.CharacterParty.Count)
        {
            m_battleResults = BattleResults.Lose;
            m_finish = true;
        }
        else if (enemyDeadCount == m_enemyObjects.Count)
        {
            m_battleResults = BattleResults.Win;
            m_finish = true;
        }
    }

    void TargetCharacters()
    {
        for (int n = 0; n < m_enemyObjects.Count; n++)
        {
            for (int i = 0; i < m_characterList.Count; i++)
            {
                m_enemyList[n].BESM.Targets.Add(m_characterList[i].gameObject);
            }
        }
        for (int n = 0; n < m_characterList.Count; n++)
        {
            for (int i = 0; i < m_enemyObjects.Count; i++)
            {
                m_characterList[n].BCSM.Targets.Add(m_enemyObjects[i]);
            }
        }
    }
    void StateChange()
    {
        for (int i = 0; i < m_characterList.Count; i++)
        {
            m_characterList[i].BCSM.IsBattle = true;
        }
        for (int i = 0; i < m_enemyObjects.Count; i++)
        {
            m_enemyList[i].BESM.IsBattle = true;
        }
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
            var characterParameter = attacker.GetComponent<Character>();
            var enemyParameter = defender.GetComponent<Enemy>();
            if (skillData.attackType == SkillData.AttackType.physicalAttack)
            {
                if (Random.Range(0, 200) > characterParameter.Luck.Value)
                {
                    damage = m_damageCalculator.EnemyDamage(skillData, enemyParameter, characterParameter.Strength.Value, enemyParameter.Defense.Value, m_battleInformationUI.Critical);
                }
                else
                {
                    m_battleInformationUI.Critical = true;
                    damage = m_damageCalculator.EnemyDamage(skillData, enemyParameter, characterParameter.Strength.Value, enemyParameter.Defense.Value, m_battleInformationUI.Critical);
                }
            }
            else if (skillData.attackType == SkillData.AttackType.magicAttack)
            {
                if (Random.Range(0, 200) > characterParameter.Luck.Value)
                {
                    damage = m_damageCalculator.EnemyDamage(skillData, enemyParameter, characterParameter.MagicPower.Value, enemyParameter.MagicResist.Value, m_battleInformationUI.Critical);
                }
                else
                {
                    m_battleInformationUI.Critical = true;
                    damage = m_damageCalculator.EnemyDamage(skillData, enemyParameter, characterParameter.MagicPower.Value, enemyParameter.MagicResist.Value, m_battleInformationUI.Critical);
                }
            }
            StartCoroutine(m_battleInformationUI.BattleUIDisplay(damage, defender.name, m_battleInformationUI.Critical));
        }
        else if (attacker.CompareTag("Enemy"))
        {
            var enemyParameter = attacker.GetComponent<Enemy>();
            var characterParameter = defender.GetComponent<Character>();
            if (skillData.attackType == SkillData.AttackType.physicalAttack)
            {
                if (Random.Range(0, 200) > enemyParameter.Luck.Value)
                {
                    damage = m_damageCalculator.PlayerDamage(skillData, enemyParameter.Strength.Value, characterParameter.Defense.Value, m_battleInformationUI.Critical);
                }
                else
                {
                    m_battleInformationUI.Critical = true;
                    damage = m_damageCalculator.PlayerDamage(skillData, enemyParameter.Strength.Value, characterParameter.Defense.Value, m_battleInformationUI.Critical);
                }
            }
            else if (skillData.attackType == SkillData.AttackType.magicAttack)
            {
                if (Random.Range(0, 200) > enemyParameter.Luck.Value)
                {
                    damage = m_damageCalculator.PlayerDamage(skillData, enemyParameter.MagicPower.Value, characterParameter.MagicResist.Value, m_battleInformationUI.Critical);
                }
                else
                {
                    m_battleInformationUI.Critical = true;
                    damage = m_damageCalculator.PlayerDamage(skillData, enemyParameter.MagicPower.Value, characterParameter.MagicResist.Value, m_battleInformationUI.Critical);
                }
            }
            StartCoroutine(m_battleInformationUI.BattleUIDisplay(damage, characterParameter.Name.Value, m_battleInformationUI.Critical));
        }
        return damage;
    }
    void FinishBattle()
    {
        for (int i = 0; i < m_characterList.Count; i++)
        {
            m_characterList[i].BCSM.IsBattle = false;
            m_characterList[i].BCSM.Targets.Clear();
            //cbsm.m_open = false;
            //cbsm.m_battlePanel.SetActive(false);
        }
        for (int i = 0; i < m_enemyObjects.Count; i++)
        {
            m_enemyList[i].BESM.IsBattle = false;
            m_enemyList[i].BESM.Targets.Clear();
        }
        StartCoroutine(BattleData());
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
        yield return new WaitUntil(() => m_isBattle);

        BattleStanby();
        TargetCharacters();
        yield return null;
        StateChange();
        WinnerChack();

        while (!m_finish)
        {
            DeadCheck();
            yield return null;
        }
        //yield return new WaitUntil(() => m_finish);
        FinishBattle();
        DestryEnemy(randam);
        DeleteField();
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
                m_characterList[i].LevelUP.Value = false;
            }
        }
        else if (m_battleResults == BattleResults.Lose)
        {

        }
        m_getExperiencePoint = 0;
    }

    public void SetBattle(Character character, GameObject enemy)
    {
        Contact(character, enemy);
    }

    void Contact(Character character, GameObject enemy)
    {
        m_charaTransform = character.gameObject.transform;
        m_contactPosition = enemy.transform.position;
        CreateField(m_contactPosition);
        var em = enemy.GetComponentInParent<Enemy>();
        m_enemyParty = em.EnemyPartyNumber;
        m_enemyID = em.ID.Value;
        m_isBattle = true;
        Destroy(enemy.transform.parent.gameObject);
        StartCoroutine(ContactUpdate(character));
    }

    IEnumerator ContactUpdate(Character character)
    {
        if (character.IsContact.HasValue)
        {
            while (m_isBattle)
            {
                m_distsnce = (m_contactPosition.x - character.transform.position.x) * (m_contactPosition.x - character.transform.position.x) + (m_contactPosition.z - character.transform.position.z) * (m_contactPosition.z - character.transform.position.z);
                if (Mathf.Sqrt(m_distsnce) > 15f)
                {
                    m_isBattle = false;
                    m_finish = true;
                    DeleteField();
                }
                yield return null;
            }
        }
    }
    void CreateField(Vector3 contactPos)
    {
        m_instantiateBattleFeild = Instantiate(m_battleFeildPrefab, contactPos, Quaternion.identity);
    }
    public void DeleteField()
    {
        Destroy(m_instantiateBattleFeild);
        m_contactPosition = Vector3.zero;
        m_distsnce = 0f;
    }

}