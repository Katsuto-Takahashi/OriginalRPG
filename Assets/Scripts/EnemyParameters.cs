using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyParameters", menuName = "EnemyParameters")]
public class EnemyParameters : ScriptableObject
{
    [SerializeField]
    [Tooltip("敵キャラクターの名前")]
    private string enemyCharacterName = "";
    /// <summary>敵キャラクターの名前</summary>
    public string EnemyCharacterName => enemyCharacterName;

    [SerializeField]
    [Tooltip("敵キャラクターのID")]
    [Range(1, 10000)]
    private int enemyCharacterID = 1;
    /// <summary>敵キャラクターのID</summary>
    public int EnemyCharacterID => enemyCharacterID;

    [SerializeField]
    [Tooltip("敵キャラクターの最大HP")]
    [Range(1, 10000)]
    private int maxHP = 1;
    /// <summary>敵キャラクターの最大HP</summary>
    public int MaxHP => maxHP;

    [SerializeField]
    [Tooltip("敵キャラクターの最大AP")]
    [Range(0, 10000)]
    private int maxAP = 0;
    /// <summary>敵キャラクターの最大AP</summary>
    public int MaxAP => maxAP;

    [SerializeField]
    [Tooltip("敵キャラクターの物理攻撃力")]
    [Range(1, 10000)]
    private int strength = 1;
    /// <summary>敵キャラクターの物理攻撃力</summary>
    public int Strength => strength;

    [SerializeField]
    [Tooltip("敵キャラクターの物理防御力")]
    [Range(1, 10000)]
    private int defense = 1;
    /// <summary>敵キャラクターの物理防御力</summary>
    public int Defense => defense;

    [SerializeField]
    [Tooltip("敵キャラクターの魔法攻撃力")]
    [Range(1, 10000)]
    private int magicPower = 1;
    /// <summary>敵キャラクターの魔法攻撃力</summary>
    public int MagicPower => magicPower;

    [SerializeField]
    [Tooltip("敵キャラクターの魔法防御力")]
    [Range(1, 10000)]
    private int magicResist = 1;
    /// <summary>敵キャラクターの魔法防御力</summary>
    public int MagicResist => magicResist;

    [SerializeField]
    [Tooltip("敵キャラクターの運")]
    [Range(0, 10000)]
    private int luck = 0;
    /// <summary>敵キャラクターの運</summary>
    public int Luck => luck;

    [SerializeField]
    [Tooltip("敵キャラクターの速さ")]
    [Range(1, 10000)]
    private int speed = 1;
    /// <summary>敵キャラクターの速さ</summary>
    public int Speed=> speed;

    [SerializeField]
    [Tooltip("敵キャラクターの一つ目のドロップアイテム")]
    private GameObject firstDropItem = null;
    /// <summary>敵キャラクターの一つ目のドロップアイテム</summary>
    public GameObject FirstDropItem => firstDropItem;

    [SerializeField]
    [Tooltip("敵キャラクターの二つ目のドロップアイテム")]
    private GameObject secondDropItem = null;
    /// <summary>敵キャラクターの二つ目のドロップアイテム</summary>
    public GameObject SecondDropItem  => secondDropItem;

    [SerializeField]
    [Tooltip("敵キャラクターの二つ目のアイテムのドロップ率")]
    [Range(0, 100)]
    private int dropRate = 0;

    /// <summary>敵キャラクターの二つ目のアイテムのドロップ率</summary>
    public int DropRate => dropRate;

    [SerializeField]
    [Tooltip("敵キャラクターを倒した際の獲得経験値")]
    [Range(1, 10000)]
    private int experiencePoint = 1;
    /// <summary>敵キャラクターを倒した際の獲得経験値</summary>
    public int ExperiencePoint => experiencePoint;

    [SerializeField]
    [Tooltip("敵キャラクターのパーティー構成")]
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
    [Tooltip("攻撃される技の属性耐性")]
    /// <summary>攻撃される技の属性</summary>
    public float[] attackAttributeResistance = new float[9];
    
    public enum AttackTypeResistance
    {
        physicalAttack,
        magicAttack
    }
    [EnumIndex(typeof(AttackTypeResistance))]
    [Tooltip("攻撃されるタイプの耐性")]
    /// <summary>攻撃されるタイプ</summary>
    public float[] attackTypeResistance = new float[2];
}