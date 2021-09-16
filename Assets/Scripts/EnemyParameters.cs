using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyParameters", menuName = "EnemyParameters")]
public class EnemyParameters : ScriptableObject
{
    /// <summary>敵キャラクターの名前</summary>
    [SerializeField]
    private string enemyCharacterName;
    public string EnemyCharacterName
    {
        get { return enemyCharacterName; }
        set { enemyCharacterName = value; }
    }
    /// <summary>敵キャラクターのID</summary>
    [SerializeField]
    [Range(1, 10000)]
    private int enemyCharacterID = 1;
    public int EnemyCharacterID
    {
        get { return enemyCharacterID; }
        set { enemyCharacterID = value; }
    }
    /// <summary>敵キャラクターの最大HP</summary>
    [SerializeField]
    [Range(1, 10000)]
    private int maxHP = 1;
    public int MaxHP
    {
        get { return maxHP; }
        set { maxHP = value; }
    }
    /// <summary>敵キャラクターの最大AP</summary>
    [SerializeField]
    [Range(0, 10000)]
    private int maxAP = 0;
    public int MaxAP
    {
        get { return maxAP; }
        set { maxAP = value; }
    }
    /// <summary>敵キャラクターの物理攻撃力</summary>
    [SerializeField]
    [Range(1, 10000)]
    private int strength = 1;
    public int Strength
    {
        get { return strength; }
        set { strength = value; }
    }
    /// <summary>敵キャラクターの物理防御力</summary>
    [SerializeField]
    [Range(1, 10000)]
    private int defense = 1;
    public int Defense
    {
        get { return defense; }
        set { defense = value; }
    }
    /// <summary>敵キャラクターの魔法攻撃力</summary>
    [SerializeField]
    [Range(1, 10000)]
    private int magicPower = 1;
    public int MagicPower
    {
        get { return magicPower; }
        set { magicPower = value; }
    }
    /// <summary>敵キャラクターの魔法防御力</summary>
    [SerializeField]
    [Range(1, 10000)]
    private int magicResist = 1;
    public int MagicResist
    {
        get { return magicResist; }
        set { magicResist = value; }
    }
    /// <summary>敵キャラクターの運</summary>
    [SerializeField]
    [Range(0, 10000)]
    private int luck = 0;
    public int Luck
    {
        get { return luck; }
        set { luck = value; }
    }
    /// <summary>敵キャラクターの速さ</summary>
    [SerializeField]
    [Range(1, 10000)]
    private int speed = 1;
    public int Speed
    {
        get { return speed; }
        set { speed = value; }
    }
    /// <summary>敵キャラクターの一つ目のドロップアイテム</summary>
    public ItemData firstDropItem;
    /// <summary>敵キャラクターの二つ目のドロップアイテム</summary>
    public ItemData secondDropItem;
    /// <summary>敵キャラクターの二つ目のアイテムのドロップ率</summary>
    [SerializeField]
    [Range(0, 100)]
    private int dropRate = 0;
    public int DropRate
    {
        get { return dropRate; }
        set { dropRate = value; }
    }
    /// <summary>敵キャラクターを倒した際の獲得経験値</summary>
    [SerializeField]
    [Range(1, 10000)] 
    private int experiencePoint = 1;
    public int ExperiencePoint
    {
        get { return experiencePoint; }
        set { experiencePoint = value; }
    }
    /// <summary>敵キャラクターのパーティー構成</summary>
    [SerializeField]
    [Range(0, 6)]
    private int enemyPartyNumber = 0;
    public int EnemyPartyNumber
    {
        get { return enemyPartyNumber; }
        set { enemyPartyNumber = value; }
    }
    /// <summary>攻撃される技の属性耐性</summary>
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
    [SerializeField, EnumIndex(typeof(AttackAttributesResistance))]
    public float[] attackAttributeResistance = new float[9];
    /// <summary>攻撃されるタイプの耐性</summary>
    public enum AttackTypeResistance
    {
        physicalAttack,
        magicAttack
    }
    [SerializeField, EnumIndex(typeof(AttackTypeResistance))]
    public float[] attackTypeResistance = new float[2];
}