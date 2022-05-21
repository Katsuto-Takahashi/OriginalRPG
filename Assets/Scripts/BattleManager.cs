using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class BattleManager : SingletonMonoBehaviour<BattleManager>
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

    [SerializeField]
    SkillList m_skillList;

    BattleInformationUI m_battleInformationUI;
    GameObject m_instantiateEnemy;
    GameObject m_instantiateBattleFeild;
    List<GameObject> m_enemyObjects = new List<GameObject>();
    List<Enemy> m_enemyList = new List<Enemy>();
    List<Character> m_characterList = new List<Character>();

    Character m_player;

    bool m_isCreated = false;
    bool m_isChangeState = false;
    bool m_finish = false;
    int characterDeadCount = 0;
    int enemyDeadCount = 0;
    int randam;
    List<GameObject> m_firstDrop = new List<GameObject>();
    List<GameObject> m_secondDrop = new List<GameObject>();
    int m_getExperiencePoint = 0;

    bool m_isBattle = false;
    int m_enemyParty = 0;
    int m_enemyID = 0;
    Vector3 m_contactPosition;
    float m_distsnce = 0.0f;
    Transform m_charaTransform;
    /// <summary>オブジェクト同士の中心点の間隔</summary>
    float m_enemyInterval = 0.0f;
    /// <summary>接触地点からの距離</summary>
    [SerializeField]
    float m_enemyDistance = 3.0f;

    const int INTERVAL_FACTOR = 3;

    enum BattleResults
    {
        Win,
        Lose,
        Escape
    }
    BattleResults m_battleResults = BattleResults.Escape;

    void CreateEnemy(int num)
    {
        var half = num / 2f;
        var distanceFromCenter = (num - 1) / 2f * m_enemyInterval;
        var ep = PartyManager.Instance.EnemyParty;

        for (int i = 0; i < num; i++)
        {
            if (i < half)
            {
                m_instantiateEnemy = Instantiate(ep[m_enemyID - 1].gameObject,
                    new Vector3((m_contactPosition + m_charaTransform.forward * m_enemyDistance).x - distanceFromCenter + m_enemyInterval * i,
                    m_contactPosition.y,
                    (m_contactPosition + m_charaTransform.forward * m_enemyDistance).z - distanceFromCenter + m_enemyInterval * i),
                    Quaternion.identity);
            }
            else
            {
                m_instantiateEnemy = Instantiate(ep[m_enemyID - 1].gameObject,
                    new Vector3((m_contactPosition + m_charaTransform.forward * m_enemyDistance).x - distanceFromCenter + m_enemyInterval * i,
                    m_contactPosition.y,
                    (m_contactPosition + m_charaTransform.forward * m_enemyDistance).z + distanceFromCenter - m_enemyInterval * i),
                    Quaternion.identity);
            }

            m_enemyObjects.Add(m_instantiateEnemy);
            var enemy = m_enemyObjects[i].GetComponent<Enemy>();

            if (num > 1)
            {
                m_enemyObjects[i].name = enemy.Name.Value + $"{i + 1}";
            }
            else
            {
                m_enemyObjects[i].name = enemy.Name.Value;
            }

            m_enemyList.Add(enemy);
            m_battleEnemyList.AddEnemyList(m_enemyObjects[i]);
            m_firstDrop.Add(enemy.FirstDropItem);
            m_secondDrop.Add(enemy.SecondDropItem);
            m_getExperiencePoint += enemy.ExperiencePoint;
            m_enemyList[i].MESM.SetLookPosition(m_contactPosition);
            m_enemyList[i].ChengeKinematic(true);//仮置き
        }

        m_battleEnemyList.Create();
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
        var cp = PartyManager.Instance.CharacterParty;

        for (int i = 0; i < cp.Count; i++)
        {
            var cpm = cp[i].GetComponent<Character>();
            m_characterList.Add(cpm);
        }

        m_player = GameManager.Instance.Player;

        randam = Random.Range(1, m_enemyParty + 1);
        Debug.Log($"出現数{randam}体");
        CreateEnemy(randam);

        //敵がボスの時はにげれないようにする

        StartCoroutine(ChengeActiveUI());
        m_battleInformationUI = m_battleInformationUIObject.GetComponent<BattleInformationUI>();
        StartCoroutine(m_battleInformationUI.BattleStartUI(m_enemyList[0].Name.Value, randam));

        for (int i = 0; i < m_enemyObjects.Count; i++)
        {
            var enemyName = m_enemyList[i].Name.Value;

            if (m_enemyObjects.Count > 1)
            {
                enemyName += $"{i + 1}";
            }
            m_enemyObjects[i].GetComponentInChildren<EnemyUI>().ChangeName(enemyName);
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
        if (characterDeadCount == m_characterList.Count)
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

    /// <summary>ダメージを計算して返す</summary>
    /// <param name="attacker">攻撃側の能力</param>
    /// <param name="defender">防御側の能力</param>
    /// <param name="skillData">攻撃に使用する技のデータ</param>
    /// <returns>計算後のダメージ</returns>
    public int Damage(GameObject attacker, GameObject defender, SkillData skillData)
    {
        return DamageCalculate(attacker, defender, skillData);
    }

    int DamageCalculate(GameObject attacker, GameObject defender, SkillData skillData)
    {
        var damage = 0;
        CriticalCheck check = CriticalCheck.normal;

        if (((1 << attacker.layer) & LayerMask.NameToLayer("Character")) != 0)
        {
            var characterParameter = attacker.GetComponent<Character>();
            var enemyParameter = defender.GetComponent<Enemy>();

            if (Random.Range(0, 200) < characterParameter.Luck.Value)
            {
                check = CriticalCheck.critical;
            }

            damage = m_damageCalculator.Damage(characterParameter, enemyParameter, skillData, Attacker.character, check);
            /*
            if (skillData.attackType == SkillData.AttackType.physicalAttack)
            {
                damage = m_damageCalculator.EnemyDamage(skillData, enemyParameter, characterParameter.Strength.Value, enemyParameter.Defense.Value, check);
            }
            else if (skillData.attackType == SkillData.AttackType.magicAttack)
            {
                damage = m_damageCalculator.EnemyDamage(skillData, enemyParameter, characterParameter.MagicPower.Value, enemyParameter.MagicResist.Value, check);
            }
            */
            StartCoroutine(m_battleInformationUI.BattleUIDisplay(damage, enemyParameter.Name.Value, check));
        }
        else if (((1 << attacker.layer) & LayerMask.NameToLayer("Enemy")) != 0)
        {
            var enemyParameter = attacker.GetComponent<Enemy>();
            var characterParameter = defender.GetComponent<Character>();

            if (Random.Range(0, 200) < enemyParameter.Luck.Value)
            {
                check = CriticalCheck.critical;
            }

            damage = m_damageCalculator.Damage(characterParameter, enemyParameter, skillData, Attacker.enemy, check);
            /*
            if (skillData.attackType == SkillData.AttackType.physicalAttack)
            {
                damage = m_damageCalculator.PlayerDamage(skillData, enemyParameter.Strength.Value, characterParameter.Defense.Value, check);
            }
            else if (skillData.attackType == SkillData.AttackType.magicAttack)
            {
                damage = m_damageCalculator.PlayerDamage(skillData, enemyParameter.MagicPower.Value, characterParameter.MagicResist.Value, check);
            }
            */
            StartCoroutine(m_battleInformationUI.BattleUIDisplay(damage, characterParameter.Name.Value, check));
        }

        return damage;
    }

    void FinishBattle()
    {
        for (int i = 0; i < m_characterList.Count; i++)
        {
            m_characterList[i].IsContact = false;
            m_characterList[i].BCSM.IsBattle = false;
            m_characterList[i].BCSM.Targets.Clear();
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

    /// <summary>バトルを開始するときに呼ぶ</summary>
    /// <param name="character">バトルを開始するキャラクター</param>
    /// <param name="enemyObject">バトルする敵</param>
    public void SetBattle(Character character, GameObject enemyObject)
    {
        Contact(character, enemyObject);
    }

    void Contact(Character character, GameObject enemyObject)
    {
        m_charaTransform = character.gameObject.transform;
        m_contactPosition = enemyObject.transform.position;
        CreateField(m_contactPosition);
        var enemy = enemyObject.GetComponentInParent<Enemy>();
        m_enemyParty = enemy.EnemyPartyNumber;
        m_enemyID = enemy.ID.Value;
        m_enemyInterval = enemyObject.GetComponentInParent<CapsuleCollider>().radius * INTERVAL_FACTOR; //オブジェクト同士を半径の分離す為
        m_isBattle = true;
        Destroy(enemyObject.transform.parent.gameObject);
        BattleStart();
        StartCoroutine(ContactUpdate(character));
    }

    IEnumerator ContactUpdate(Character character)
    {
        if (character.IsContact)
        {
            var limit = m_battleFeildPrefab.transform.localScale.x;
            while (m_isBattle)
            {
                m_distsnce = (m_contactPosition.x - character.transform.position.x) * (m_contactPosition.x - character.transform.position.x) + (m_contactPosition.z - character.transform.position.z) * (m_contactPosition.z - character.transform.position.z);
                
                if (Mathf.Sqrt(m_distsnce) > limit)
                {
                    m_isBattle = false;
                    m_finish = true;
                }

                yield return null;
            }
        }
    }

    void CreateField(Vector3 contactPos)
    {
        m_instantiateBattleFeild = Instantiate(m_battleFeildPrefab, contactPos, Quaternion.identity);
    }

    void DeleteField()
    {
        Destroy(m_instantiateBattleFeild);
        m_contactPosition = Vector3.zero;
        m_distsnce = 0f;
    }

    Enemy m_enemy;
    public Enemy SetEnemy => m_enemy;
    public void SelectEnemy(int id)
    {
        m_enemy = m_enemyList[id];
        m_player.BCSM.CanSelect.Value = false;
    }

    Character m_character;
    public Character SetCharacter => m_character;
    public void SelectCharacter(int id)
    {
        m_character = m_characterList[id];
    }

    SkillData m_skill;
    public SkillData SetSkill => m_skill;
    public void SelectSkill(int id, SkillData.SkillType skillType)
    {
        if (skillType == SkillData.SkillType.physicalAttack)
        {
            m_skill = m_skillList.PhysicalSkills[id].SkillParameter;
        }
        else
        {
            m_skill = m_skillList.MagicSkills[id].SkillParameter;
        }
    }
}