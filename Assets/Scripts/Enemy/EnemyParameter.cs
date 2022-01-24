using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class EnemyParameter : MonoBehaviour
{
    [SerializeField]
    StringReactiveProperty m_name = new StringReactiveProperty();
    /// <summary>キャラクターの名前</summary>
    public IReadOnlyReactiveProperty<string> Name => m_name;
    
    [SerializeField]
    IntReactiveProperty m_id = new IntReactiveProperty(1);
    /// <summary>キャラクターのID</summary>
    public IReadOnlyReactiveProperty<int> ID => m_id;

    [SerializeField]
    IntReactiveProperty m_hp = new IntReactiveProperty(1);
    /// <summary>キャラクターの現在HP</summary>
    public IReactiveProperty<int> HP => m_hp;

    [SerializeField]
    IntReactiveProperty m_maxHp = new IntReactiveProperty(1);
    /// <summary>キャラクターの最大HP</summary>
    public IReadOnlyReactiveProperty<int> MaxHP => m_maxHp;

    [SerializeField]
    IntReactiveProperty m_ap = new IntReactiveProperty(0);
    /// <summary>キャラクターの現在AP</summary>
    public IReactiveProperty<int> AP => m_ap;

    [SerializeField]
    IntReactiveProperty m_maxAp = new IntReactiveProperty(0);
    /// <summary>キャラクターの最大AP</summary>
    public IReadOnlyReactiveProperty<int> MaxAP => m_maxAp;

    [SerializeField]
    IntReactiveProperty m_strength = new IntReactiveProperty(1);
    /// <summary>キャラクターの攻撃力</summary>
    public IReadOnlyReactiveProperty<int> Strength => m_strength;

    [SerializeField]
    IntReactiveProperty m_defense = new IntReactiveProperty(1);
    /// <summary>キャラクターの防御力</summary>
    public IReadOnlyReactiveProperty<int> Defense => m_defense;

    [SerializeField]
    IntReactiveProperty m_magicPower = new IntReactiveProperty(1);
    /// <summary>キャラクターの魔法攻撃力</summary>
    public IReadOnlyReactiveProperty<int> MagicPower => m_magicPower;

    [SerializeField]
    IntReactiveProperty m_magicResist = new IntReactiveProperty(1);
    /// <summary>キャラクターの魔法抵抗力</summary>
    public IReadOnlyReactiveProperty<int> MagicResist => m_magicResist;

    [SerializeField]
    IntReactiveProperty m_luck = new IntReactiveProperty(1);
    /// <summary>キャラクターの運</summary>
    public IReadOnlyReactiveProperty<int> Luck => m_luck;

    [SerializeField]
    IntReactiveProperty m_speed = new IntReactiveProperty(1);
    /// <summary>キャラクターの速さ</summary>
    public IReadOnlyReactiveProperty<int> Speed => m_speed;

    [SerializeField]
    private GameObject firstDropItem = null;
    /// <summary>敵キャラクターの一つ目のドロップアイテム</summary>
    public GameObject FirstDropItem => firstDropItem;

    [SerializeField]
    private GameObject secondDropItem = null;
    /// <summary>敵キャラクターの二つ目のドロップアイテム</summary>
    public GameObject SecondDropItem => secondDropItem;

    [SerializeField]
    [Range(0, 100)]
    private int dropRate = 0;
    /// <summary>敵キャラクターの二つ目のアイテムのドロップ率</summary>
    public int DropRate => dropRate;

    [SerializeField]
    [Range(1, 10000)]
    private int experiencePoint = 1;
    /// <summary>敵キャラクターを倒した際の獲得経験値</summary>
    public int ExperiencePoint => experiencePoint;

    [SerializeField]
    [Range(1, 6)]
    private int enemyPartyNumber = 1;
    /// <summary>敵キャラクターのパーティー構成</summary>
    public int EnemyPartyNumber => enemyPartyNumber;

    public enum AttackAttributesResistance
    {
        non,
        fire,
        water,
        thunder,
        ground,
        wind,
        plant,
        dark,
        light
    }
    [EnumIndex(typeof(AttackAttributesResistance))]
    /// <summary>攻撃される技の属性</summary>
    public float[] attackAttributeResistance = new float[9];

    public enum AttackTypeResistance
    {
        physicalAttack,
        magicAttack
    }
    [EnumIndex(typeof(AttackTypeResistance))]
    /// <summary>攻撃されるタイプ</summary>
    public float[] attackTypeResistance = new float[2];

    [SerializeField]
    [Range(1, 5)]
    int m_maxActionCount = 3;
    /// <summary>蓄積可能な行動回数</summary>
    public int MaxActionCount => m_maxActionCount;
}
