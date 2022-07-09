using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class EnemyParameter : MonoBehaviour
{
    [SerializeField]
    StringReactiveProperty m_name = new StringReactiveProperty();
    /// <summary>名前</summary>
    public IReadOnlyReactiveProperty<string> Name => m_name;
    
    [SerializeField]
    IntReactiveProperty m_id = new IntReactiveProperty(1);
    /// <summary>ID</summary>
    public IReadOnlyReactiveProperty<int> ID => m_id;

    [SerializeField]
    IntReactiveProperty m_hp = new IntReactiveProperty(1);
    /// <summary>現在HP</summary>
    public IntReactiveProperty HP => m_hp;

    [SerializeField]
    IntReactiveProperty m_maxHp = new IntReactiveProperty(1);
    /// <summary>最大HP</summary>
    public IReadOnlyReactiveProperty<int> MaxHP => m_maxHp;

    [SerializeField]
    IntReactiveProperty m_ap = new IntReactiveProperty(0);
    /// <summary>現在AP</summary>
    public IntReactiveProperty AP => m_ap;

    [SerializeField]
    IntReactiveProperty m_maxAp = new IntReactiveProperty(0);
    /// <summary>最大AP</summary>
    public IReadOnlyReactiveProperty<int> MaxAP => m_maxAp;

    [SerializeField]
    IntReactiveProperty m_strength = new IntReactiveProperty(1);
    /// <summary>攻撃力</summary>
    public IReadOnlyReactiveProperty<int> Strength => m_strength;

    [SerializeField]
    IntReactiveProperty m_defense = new IntReactiveProperty(1);
    /// <summary>防御力</summary>
    public IReadOnlyReactiveProperty<int> Defense => m_defense;

    [SerializeField]
    IntReactiveProperty m_magicPower = new IntReactiveProperty(1);
    /// <summary>魔法攻撃力</summary>
    public IReadOnlyReactiveProperty<int> MagicPower => m_magicPower;

    [SerializeField]
    IntReactiveProperty m_magicResist = new IntReactiveProperty(1);
    /// <summary>魔法抵抗力</summary>
    public IReadOnlyReactiveProperty<int> MagicResist => m_magicResist;

    [SerializeField]
    IntReactiveProperty m_luck = new IntReactiveProperty(1);
    /// <summary>運</summary>
    public IReadOnlyReactiveProperty<int> Luck => m_luck;

    [SerializeField]
    IntReactiveProperty m_intelligence = new IntReactiveProperty(1);
    /// <summary>賢さ</summary>
    public IReadOnlyReactiveProperty<int> Intelligence => m_intelligence;

    [SerializeField]
    IntReactiveProperty m_speed = new IntReactiveProperty(1);
    /// <summary>速さ</summary>
    public IReadOnlyReactiveProperty<int> Speed => m_speed;

    [SerializeField]
    GameObject m_firstDropItem = null;
    /// <summary>一つ目のドロップアイテム</summary>
    public GameObject FirstDropItem => m_firstDropItem;

    [SerializeField]
    GameObject m_secondDropItem = null;
    /// <summary>二つ目のドロップアイテム</summary>
    public GameObject SecondDropItem => m_secondDropItem;

    [SerializeField]
    [Range(0, 100)]
    int m_dropRate = 0;
    /// <summary>二つ目のアイテムのドロップ率</summary>
    public int DropRate => m_dropRate;

    [SerializeField]
    [Range(1, 10000)]
    int m_experiencePoint = 1;
    /// <summary>倒された際の経験値</summary>
    public int ExperiencePoint => m_experiencePoint;

    [SerializeField]
    [Range(1, 6)]
    int m_enemyPartyNumber = 1;
    /// <summary>パーティー構成</summary>
    public int EnemyPartyNumber => m_enemyPartyNumber;

    [SerializeField]
    [Range(1, 5)]
    int m_maxActionCount = 3;
    /// <summary>蓄積可能な行動回数</summary>
    public int MaxActionCount => m_maxActionCount;

    [EnumIndex(typeof(SkillAttributes))]
    [SerializeField]
    float[] m_attackAttributesResistance = new float[9];

    /// <summary>攻撃される技の属性の配列</summary>
    public float[] AttackAttributesResistance => m_attackAttributesResistance;

    [EnumIndex(typeof(SkillType))]
    [SerializeField]
    float[] m_attackTypesResistance = new float[2];

    /// <summary>攻撃されるタイプの配列</summary>
    public float[] AttackTypesResistance => m_attackTypesResistance;

    [SerializeField]
    SkillIndex m_hasSkillIndex;
    /// <summary>持っているスキルのインデックス</summary>
    public SkillIndex HasSkillIndex { get => m_hasSkillIndex; set => m_hasSkillIndex = value; }
}
