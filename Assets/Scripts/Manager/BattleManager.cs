using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public enum BattleResults
{
    Win,
    Lose,
    Escape
}

public class BattleManager : SingletonMonoBehaviour<BattleManager>
{
    DamageCalculator m_damageCalculator = new DamageCalculator();

    RecoveryCalculator m_recoveryCalculator = new RecoveryCalculator();

    [SerializeField]
    BattleEnemyList m_battleEnemyList = null;

    [SerializeField]
    List<GameObject> m_battleFeildList = new List<GameObject>();

    [SerializeField]
    [EnumIndex(typeof(SkillType))]
    List<SkillList> m_skillsList = null;

    GameObject m_instantiateEnemy;
    GameObject m_instantiateBattleFeild;
    List<GameObject> m_enemyObjects = new List<GameObject>();
    List<Enemy> m_enemyList = new List<Enemy>();
    List<Character> m_characterList = new List<Character>();

    Character m_player;

    /// <summary>バトル中の全てのcharacterを保持するList</summary>
    List<GameObject> m_battleCharacters = new List<GameObject>();
    /// <summary>選択されたスキルを保持するList</summary>
    List<Skill> m_selectSkillList = new List<Skill>();

    /// <summary>選択されたスキルを保持するList</summary>
    List<GameObject> m_selectTargetList = new List<GameObject>();

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

    BattleResults m_battleResults = BattleResults.Escape;

    void CreateEnemy(int num)
    {
        var half = num / 2f;
        var distanceFromCenter = (num - 1) / 2f * m_enemyInterval;
        var ep = CharactersManager.Instance.Enemies;

        for (int i = 0; i < num; i++)
        {
            if (i == 0)
            {
                m_instantiateEnemy = Instantiate(ep[m_enemyID - 1].gameObject,
                        new Vector3((m_contactPosition + m_charaTransform.forward * m_enemyDistance).x,
                        m_contactPosition.y,
                        (m_contactPosition + m_charaTransform.forward * m_enemyDistance).z),
                        Quaternion.identity);
            }
            else
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
            m_battleEnemyList.AddEnemyList(enemy);
            m_firstDrop.Add(enemy.FirstDropItem);
            m_secondDrop.Add(enemy.SecondDropItem);
            m_getExperiencePoint += enemy.ExperiencePoint;
            m_enemyList[i].MESM.SetLookPosition(m_contactPosition);
            m_enemyList[i].ChengeKinematic(true);//仮置き
        }

        m_battleEnemyList.Create();
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
        var cp = PartyManager.Instance.Party;

        for (int i = 0; i < cp.Count; i++)
        {
            m_characterList.Add(cp[i]);
        }

        m_player = GameManager.Instance.Player;

        randam = Random.Range(1, m_enemyParty + 1);
        Debug.Log($"出現数{randam}体");
        CreateEnemy(randam);

        //敵がボスの時はにげれないようにする

        UIManager.Instance.ChangeInformationActive(0.0f);
        UIManager.Instance.BattleStart(m_enemyList[0].Name.Value, randam);

        for (int i = 0; i < m_characterList.Count + m_enemyList.Count; i++)//m_battleCharactersの順にbattleIDを設定する
        {
            if (i < m_characterList.Count)
            {
                m_characterList[i].BCSM.BattleID.Value = i;
                m_battleCharacters.Add(m_characterList[i].gameObject);
            }
            else if (i - m_characterList.Count < m_enemyList.Count)
            {
                m_enemyList[i - m_characterList.Count].BESM.BattleID.Value = i;
                m_battleCharacters.Add(m_enemyList[i - m_characterList.Count].gameObject);
            }
            m_selectSkillList.Add(default);//キャラ数の分だけからの入れ物を作る
            m_selectTargetList.Add(default);//キャラ数の分だけからの入れ物を作る
        }

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

        //for (int n = 0; n < m_characterList.Count; n++)
        //{
        //    for (int i = 0; i < m_enemyObjects.Count; i++)
        //    {
        //        m_characterList[n].BCSM.Targets.Add(m_enemyObjects[i]);
        //    }
        //}
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

    public void PlaySkillEffect(GameObject user, GameObject target, Skill skill)
    {
        skill.PlayEffect(user, target, skill);
    }

    public void PlayAdditionalSkillEffect(GameObject user, GameObject target, Skill skill)
    {
        for (int i = 0; i < skill.Effects.Count; i++)
        {
            skill.Effects[i].Effect(user, target, skill);
        }
    }

    public void EffectCoroutine(IEnumerator enumerator)
    {
        StartCoroutine(enumerator);
    }

    /// <summary>ダメージを計算して返す</summary>
    /// <param name="attacker">攻撃側</param>
    /// <param name="defender">防御側</param>
    /// <param name="skillData">攻撃に使用する技のデータ</param>
    /// <returns>計算後のダメージ</returns>
    public int Damage(GameObject attacker, GameObject defender, SkillData skillData)
    {
        return DamageCalculate(attacker, defender, skillData);
    }

    int DamageCalculate(GameObject attacker, GameObject defender, SkillData skillData)
    {
        int damage;
        CriticalCheck check = default;
        Character character = default;
        Enemy enemy = default;
        Attacker attackCharacter = default;
        string defenderName = "";

        if (((1 << attacker.layer) & LayerMask.NameToLayer("Character")) != 0)
        {
            character = attacker.GetComponent<Character>();
            enemy = defender.GetComponent<Enemy>();
            check = Random.Range(0, 200) < character.Luck.Value ? CriticalCheck.Critical : CriticalCheck.Normal;
            attackCharacter = Attacker.Character;
            defenderName = enemy.Name.Value;
        }
        else if (((1 << attacker.layer) & LayerMask.NameToLayer("Enemy")) != 0)
        {
            enemy = attacker.GetComponent<Enemy>();
            character = defender.GetComponent<Character>();
            check = Random.Range(0, 200) < enemy.Luck.Value ? CriticalCheck.Critical : CriticalCheck.Normal;
            attackCharacter = Attacker.Enemy;
            defenderName = character.Name.Value;
        }

        damage = m_damageCalculator.Damage(character, enemy, skillData, attackCharacter, check);
        if (check == CriticalCheck.Critical)
        {
            UIManager.Instance.CriticalHit();
        }
        //StartCoroutine(m_battleInformationUI.DamageUIDisplay(damage, defenderName));
        return damage;
    }

    /// <summary>回復量を計算して返す</summary>
    /// <param name="user">使用者</param>
    /// <param name="skillData">技のデータ</param>
    /// <param name="standardValue">基準値</param>
    /// <returns>計算後の回復量</returns>
    public int Recovery(GameObject user, SkillData skillData, int standardValue)
    {
        return RecoveryCalculate(user, skillData, standardValue);
    }

    int RecoveryCalculate(GameObject user, SkillData skillData, int standardValue)
    {
        var heal = 0;
        var characterName = "";

        if (((1 << user.layer) & LayerMask.NameToLayer("Character")) != 0)
        {
            var characterParameter = user.GetComponent<Character>();
            heal = m_recoveryCalculator.Recovery(characterParameter, skillData, standardValue);
            characterName = characterParameter.Name.Value;
        }
        else if (((1 << user.layer) & LayerMask.NameToLayer("Enemy")) != 0)
        {
            var enemyParameter = user.GetComponent<Enemy>();
            heal = m_recoveryCalculator.Recovery(enemyParameter, skillData, standardValue);
            characterName = enemyParameter.Name.Value;
        }
        //StartCoroutine(m_battleInformationUI.RecoveryUIDisplay(heal, characterName));
        return heal;
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
        UIManager.Instance.CloseBattleCommandPanel(1.0f);
        StartCoroutine(BattleData());
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
        UIManager.Instance.BattleFinish(m_battleResults);

        UIManager.Instance.ChangeInformationActive(1.0f);
        yield return null;

        if (m_battleResults == BattleResults.Win)
        {
            for (int i = 0; i < m_characterList.Count; i++)
            {
                m_characterList[i].GetExp(m_getExperiencePoint);
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
        CreateField(m_contactPosition, enemyObject);
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
            var limit = m_instantiateBattleFeild.transform.localScale.x;
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

    void CreateField(Vector3 contactPos, GameObject enemyObject)
    {
        if (enemyObject.CompareTag("Boss"))
        {
            m_instantiateBattleFeild = Instantiate(m_battleFeildList[0], contactPos, Quaternion.identity);
        }
        else
        {
            m_instantiateBattleFeild = Instantiate(m_battleFeildList[1], contactPos, Quaternion.identity);
        }
    }

    void DeleteField()
    {
        Destroy(m_instantiateBattleFeild);
        m_contactPosition = Vector3.zero;
        m_distsnce = 0f;
    }

    public Skill GetSkill(SkillType skillType, int skillID)
    {
        return m_skillsList[(int)skillType].Skills[skillID];
    }

    public Skill GetSelectSkill(int battleID)
    {
        return m_selectSkillList[battleID];
    }

    public void SetSelectSkill(int skillID, SkillType skillType, SkillIndex skillIndex, int battleID)
    {
        if (skillType == SkillType.Physical)
        {
            m_selectSkillList[battleID] = m_skillsList[(int)skillType].Skills[skillIndex.Physicals[skillID]];
        }
        else
        {
            m_selectSkillList[battleID] = m_skillsList[(int)skillType].Skills[skillIndex.Magicals[skillID]];
        }
    }

    public GameObject GetSelectTarget(int battleID)
    {
        return m_selectTargetList[battleID];
    }

    public void SetSelectTarget(int targetID, int battleID)
    {
        m_selectTargetList[battleID] = m_battleCharacters[targetID];
        if (battleID < m_characterList.Count)
        {
            m_characterList[battleID].BCSM.CanSelect.Value = false;
        }
        //else if (battleID - m_characterList.Count < m_enemyList.Count)
        //{
        //    m_enemyList[battleID - m_characterList.Count].BESM.CanSelect.Value = false;
        //}
    }

    public int CharacterCount => m_characterList.Count;
    public int EnemyCount => m_enemyList.Count;
}
